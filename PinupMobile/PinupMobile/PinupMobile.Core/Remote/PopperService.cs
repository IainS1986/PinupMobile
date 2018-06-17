using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PinupMobile.Core.Extensions;
using PinupMobile.Core.Logging;
using PinupMobile.Core.Remote.API;
using PinupMobile.Core.Remote.Client;
using PinupMobile.Core.Remote.DTO;
using PinupMobile.Core.Remote.Model;
using PinupMobile.Core.Remote.Model.Debug;
using PinupMobile.Core.Settings;

namespace PinupMobile.Core.Remote
{
    public class PopperService : IPopperService
    {
        private const string PopperURLSettingKey = "popperServerURL";
        private const string DebugPopperURL = "http://debug/";

        private readonly IUserSettings _settings;
        private readonly IHttpClientFactory _clientFactory;

        private Uri _baseUri;
        public Uri BaseUri
        {
            get { return _baseUri; }
            set { _baseUri = value; }
        }

        public bool IsDebugMode
        {
            get { return BaseUri.AbsoluteUri.Equals(DebugPopperURL, StringComparison.CurrentCultureIgnoreCase); }
        }

        public PopperService(IUserSettings settings,
                             IHttpClientFactory clientFactory)
        {
            _settings = settings;
            _clientFactory = clientFactory;

            Initialize();
        }

        private void Initialize()
        {
            // Check if we've manually set an IP and connect to that if so
            string url = _settings.GetString(PopperURLSettingKey);

            //TODO First time setup...
            if (string.IsNullOrEmpty(url))
            {
                //url = "http://192.168.0.31/";
                url = "http://192.168.0.1/";
            }

            BaseUri = new Uri(url);
        }

        public async Task<bool> ServerExists()
        {
            try
            {
                var item = await GetCurrentItem();

                if (item == null)
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error pinging {BaseUri}");
                Logger.Error(ex.Message);
                return false;
            }
        }

        public async Task<bool> ServerExistsWithUrl(string url)
        {
            BaseUri = new Uri(url);

            bool connected = await ServerExists();

            if (connected && !IsDebugMode)
            {
                //Save off the URL so we autoconnect next time
                _settings.SetString(PopperURLSettingKey, url);
            }

            return connected;
        }

        public string GetCurrentSavedPopperURL()
        {
            return _settings.GetString(PopperURLSettingKey);
        }

        private async Task<PopperResponse<ResponseT>> MakeRequest<RequestT, ResponseT>(RequestT request) 
            where RequestT : class
            where ResponseT : class
        {
            var path = typeof(RequestT).GetAttributeValue((Route ra) => ra.Url);

            // Use Regex to get all (if any) '{property}' instances.
            var pattern = @"\{(.*?)\}";
            var matches = Regex.Matches(path, pattern);

            // For each match, use reflection to get the property valyue from request
            // and replace in the path string
            foreach (var match in matches)
            {
                string replace = match.ToString();
                string property = replace.Substring(1, replace.Length - 2);

                Type rType = typeof(RequestT);
                FieldInfo info = rType.GetField(property);
                var val = info.GetValue(request);
                string val_s = val.ToString();
                if (val.GetType().IsEnum)
                {
                    val_s = ((int)val).ToString();
                }
                path = path.Replace(replace, val_s);
            }

            // HttpClient setup
            Uri url = new Uri(BaseUri, path);
            var client = _clientFactory.Create();
            client.Timeout = TimeSpan.FromSeconds(5);//TODO Configurable in the request object?
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);//TODO HttpMethod in the attribute

            // Repsonse data
            HttpResponseMessage httpResponse = null;
            PopperResponse<ResponseT> response = new PopperResponse<ResponseT>();
            response.Success = false;

            try
            {
                httpResponse = await client.GetAsync(url).ConfigureAwait(false);

                HttpStatusCode code = httpResponse.StatusCode;
                string message = httpResponse.ReasonPhrase;
                HttpResponseHeaders headers = httpResponse.Headers;

                response.Code = (int)code;
                response.Messsage = message;
                response.Success = code == HttpStatusCode.OK;

                if (httpResponse.Content != null)
                {
                    var contentType = httpResponse.Content.Headers.ContentType;

                    if(contentType.MediaType == "application/json")
                    {
                        string responseBody = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                        response.Data = JsonConvert.DeserializeObject<ResponseT>(responseBody);
                    }
                    else if (contentType.MediaType.StartsWith("video", StringComparison.CurrentCultureIgnoreCase))
                    {
                        byte[] responseBody = await httpResponse.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                        response.Data = (ResponseT)(object)contentType.MediaType;
                        response.Raw = responseBody;
                    }
                    else if(contentType.MediaType.StartsWith("image", StringComparison.CurrentCultureIgnoreCase))
                    {
                        byte[] responseBody = await httpResponse.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                        response.Data = (ResponseT)(object)contentType.MediaType;
                        response.Raw = responseBody;
                    }
                    else if(contentType.MediaType == "text/html")
                    {
                        string responseBody = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                        response.Data = (ResponseT)(object) responseBody;
                    }
                    else
                    {
                        Logger.Error($"Recieved MediaType {contentType.MediaType} but have no support for this yet");
                        response.Success = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Error fetching {url}");
                response.Success = false;
                response.Messsage = ex.Message;
            }

            return response;
        }

        public async Task<string> GetDisplay(string display)
        {
            if (IsDebugMode)
            {
                Logger.Debug($"Sent popper request for {display} feed");

                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                string localFilename = "test.f4v";
                //Slight hacky fudge
                if (display.Equals("Wheel", StringComparison.CurrentCultureIgnoreCase))
                {
                    localFilename = "test.png";
                }

                //return Path.Combine(documentsPath, localFilename);
                return localFilename;
            }

            GetDisplayRequest request = new GetDisplayRequest();
            request.display = display;

            var response = await MakeRequest<GetDisplayRequest, string>(request).ConfigureAwait(false);

            if(response.Success == true)
            {
                try
                {
                    // Check Response data to determine what format to save the byte[] too
                    string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    string localFilename = "test.txt";
                    if (response.Data.Contains("video") ||
                        response.Data.Contains("image"))
                    {
                        string format = response.Data.Split('/')[1];
                        localFilename = $"test.{format}";
                    }
                    
                    // Save 
                    string localPath = Path.Combine(documentsPath, localFilename);
                    File.WriteAllBytes(localPath, response.Raw);
                    
                    return localPath;
                    
                }
                catch(Exception ex)
                {
                    Logger.Error($"Error saving off display data for playback");
                    Logger.Error(ex.Message);
                    return string.Empty;
                }
            }
            else
            {
                //TODO Error handling???
                Logger.Error($"Error requesting Display {display}, responded with {response?.Code} and {response?.Messsage}");
                return string.Empty;  
            }
        }

        public async Task<Item> GetCurrentItem()
        {
            if (IsDebugMode)
            {
                return new MockItem();
            }

            // Popper has no endpoint to just get state, or any authentication to do a keep alive
            // so instead, make a call to "Get current item" which should respond but not alter
            // anything on popper
            var request = new GetCurrentItemRequest();
            var response = await MakeRequest<GetCurrentItemRequest, GetCurrentItemResponse>(request).ConfigureAwait(false);

            if (response?.Success == true &&
                response?.Data != null)
            {
                return new Item(response.Data);
            }
            else
            {
                //TODO Error handling???
                Logger.Error($"Error requesting GetCurrentItem, responded with {response?.Code} and {response?.Messsage}");
                return null;
            }
        }

        public async Task<bool> SendPlayGame(int gameid)
        {
            if (IsDebugMode)
            {
                Logger.Debug($"Sent popper command to start playing game with id {gameid}");
                return true;
            }

            var request = new LaunchGameRequest();
            request.id = gameid;

            var response = await MakeRequest<LaunchGameRequest, string>(request).ConfigureAwait(false);

            if (response?.Success == true)
            {
                return true;
            }
            else
            {
                //TODO Error handling???
                Logger.Error($"Error launching game {gameid}, responded with {response?.Code} and {response?.Messsage}");
                return false;
            }
        }

        public async Task<bool> SendGameNext()
        {
            return await SendPupKey(PopperCommand.KEY_NEXT_GAME);
        }

        public async Task<bool> SendGamePrev()
        {
            return await SendPupKey(PopperCommand.KEY_PREV_GAME);
        }

        public async Task<bool> SendPagePrev()
        {
            return await SendPupKey(PopperCommand.KEY_PREV_PAGE);
        }

        public async Task<bool> SendPageNext()
        {
            return await SendPupKey(PopperCommand.KEY_NEXT_PAGE);
        }

        public async Task<bool> SendExitEmulator()
        {
            return await SendPupKey(PopperCommand.KEY_EXIT_EMULATOR);
        }

        public async Task<bool> SendHome()
        {
            return await SendPupKey(PopperCommand.KEY_HOME);
        }

        public async Task<bool> SendSelect()
        {
            return await SendPupKey(PopperCommand.KEY_SELECT);
        }

        public async Task<bool> SendMenuReturn()
        {
            return await SendPupKey(PopperCommand.KEY_MENU_RETURN);
        }

        public async Task<bool> SendSystemMenu()
        {
            return await SendPupKey(PopperCommand.KEY_SYSTEM_MENU);
        }

        public async Task<bool> SendShutdown()
        {
            return await SendPupKey(PopperCommand.KEY_SHUTDOWN_PC);
        }

        public async Task<bool> SendRestart()
        {
            return await SendPupKey(PopperCommand.KEY_RESTART_PC);
        }

        public async Task<bool> SendGameStart()
        {
            return await SendPupKey(PopperCommand.KEY_GAME_START);
        }

        public async Task<bool> SendRecordStart()
        {
            return await SendPupKey(PopperCommand.KEY_START_RECORD);
        }

        /// <summary>
        /// Send pup the given key.
        /// </summary>
        /// <returns>The pup key.</returns>
        /// <param name="keycode">Keycode.</param>
        public async Task<bool> SendPupKey(PopperCommand command)
        {
            if (IsDebugMode)
            {
                Logger.Debug($"Sent Pupper keycode {command}");
                return true;
            }

            // TODO Move all this into a handler ...
            SendKeyInputRequest request = new SendKeyInputRequest();
            request.command = command;

            var response = await MakeRequest<SendKeyInputRequest, string>(request).ConfigureAwait(false);

            if (response?.Success == true)
            {
                return true;
            }
            else
            {
                //TODO Error handling???
                Logger.Error($"Error sending pup key {command}, responded with {response?.Code} and {response?.Messsage}");
                return false;
            }
        }
    }
}