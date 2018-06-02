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
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            var requestBody = string.Empty;
            var ContentType = "application/json";
            if (requestBody != null)
            {
                requestMessage.Content = new StringContent(requestBody);
                if (ContentType != null)
                {
                    requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue(ContentType);
                }
            }

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
    }
}
