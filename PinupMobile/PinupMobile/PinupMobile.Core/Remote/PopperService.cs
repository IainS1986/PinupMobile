using System;
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
using PinupMobile.Core.Settings;

namespace PinupMobile.Core.Remote
{
    public class PopperService : IPopperService
    {
        private const string PopperURLSettingKey = "popperServerURL";
        
        private readonly IUserSettings _settings;
        private readonly IHttpClientFactory _clientFactory;

        private Uri _baseUri;
        public Uri BaseUri
        {
            get { return _baseUri; }
            set { _baseUri = value; }
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
                url = "http://192.168.0.31/";
            }

            BaseUri = new Uri(url);
        }

        public async Task<bool> ServerExists()
        {
            try
            {
                // I was going to use this code to ping, but this might incorrectly say
                // the host exists, even if popper server is not working! So, unfortunately for now
                // we've got to do a GetItem.
                //var ping = new System.Net.NetworkInformation.Ping();
                //var result = await ping.SendPingAsync(BaseUri.AbsoluteUri);

                //if (result.Status != System.Net.NetworkInformation.IPStatus.Success)
                //return false;

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
            string responseBody = null;
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
                    responseBody = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (typeof(ResponseT) == typeof(string))
                    {
                        response.Data = (ResponseT)(object)responseBody;
                    }
                    else
                    {
                        response.Data = JsonConvert.DeserializeObject<ResponseT>(responseBody);
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

        public async Task<CurrentItem> GetCurrentItem()
        {
            // Popper has no endpoint to just get state, or any authentication to do a keep alive
            // so instead, make a call to "Get current item" which should respond but not alter
            // anything on popper
            var request = new GetCurrentItemRequest();
            var response = await MakeRequest<GetCurrentItemRequest, GetCurrentItemResponse>(request).ConfigureAwait(false);

            if (response?.Success == true &&
                response?.Data != null)
            {
                return new CurrentItem(response.Data);
            }
            else
            {
                //TODO Error handling???
                Logger.Error($"Error requesting GetCurrentItem, responded with {response?.Code} and {response?.Messsage}");
                return null;
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

        /// <summary>
        /// Send pup the given key.
        /// </summary>
        /// <returns>The pup key.</returns>
        /// <param name="keycode">Keycode.</param>
        public async Task<bool> SendPupKey(PopperCommand command)
        {
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