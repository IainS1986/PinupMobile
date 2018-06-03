using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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

        public async Task<CurrentItem> GetCurrentItem()
        {
            // Popper has no endpoint to just get state, or any authentication to do a keep alive
            // so instead, make a call to "Get current item" which should respond but not alter
            // anything on popper
            var request = new GetCurrentItemRequest();

            // TODO Move all this into a handler ...
            var relativePath = typeof(GetCurrentItemRequest).GetAttributeValue((Route ra) => ra.Url);
            Uri url = new Uri(BaseUri, relativePath);
            var client = _clientFactory.Create();
            client.Timeout = TimeSpan.FromSeconds(5);
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            HttpResponseMessage response = null;
            string responseBody = null;
            CurrentItem item = null;

            try
            {
                response = await client.GetAsync(url).ConfigureAwait(false);

                HttpStatusCode code = response.StatusCode;
                string message = response.ReasonPhrase;
                HttpResponseHeaders headers = response.Headers;

                if (response.Content != null)
                {
                    responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    CurrentItemDTO dto = JsonConvert.DeserializeObject<CurrentItemDTO>(responseBody);
                    item = new CurrentItem(dto);
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Error fetching {url}");
                Logger.Error(ex.Message);
            }

            // If no response, the VM/UI should trigger the "please input popper url" for a retry
            return item;
        }

        public async Task<bool> SendGameNext()
        {
            return await SendPupKey("2");
        }

        public async Task<bool> SendGamePrev()
        {
            return await SendPupKey("1");
        }

        /// <summary>
        /// Send pup the given key. Here are key codes..
        /// 
        /// 1 - Game Prior
        /// 2 - Game Next
        /// 14 - Select
        /// 6 - Page Prior
        /// 5 - Page Next
        /// 9 - Home
        /// 15 - Exit Emulator
        /// 11 - Menu System
        /// 56 - Restart PC
        /// 12 - Shut down Windows
        /// </summary>
        /// <returns>The pup key.</returns>
        /// <param name="keycode">Keycode.</param>
        public async Task<bool> SendPupKey(string keycode)
        {
            // TODO Move all this into a handler ...
            SendKeyInputRequest request = new SendKeyInputRequest();
            request.keyCode = keycode;

            var relativePath = typeof(SendKeyInputRequest).GetAttributeValue((Route ra) => ra.Url);
            relativePath = relativePath.Replace("{keyCode}", request.keyCode);

            Uri url = new Uri(BaseUri, relativePath);
            var client = _clientFactory.Create();
            client.Timeout = TimeSpan.FromSeconds(5);
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            HttpResponseMessage response = null;

            try
            {
                response = await client.GetAsync(url).ConfigureAwait(false);

                HttpStatusCode code = response.StatusCode;
                if(code == HttpStatusCode.OK)
                {
                    return true;
                }
                else
                {
                    Logger.Error($"Error sending pup key {keycode}, responded with {code} and {response.ReasonPhrase}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Error fetching {url}");
                Logger.Error(ex.Message);
                return false;
            }
        }
    }
}
