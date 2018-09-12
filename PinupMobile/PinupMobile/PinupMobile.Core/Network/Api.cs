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
using PinupMobile.Core.Remote.Model;
using PinupMobile.Core.Settings;

namespace PinupMobile.Core.Network
{
    public class Api : IApi
    {
        private const string PopperURLSettingKey = "popperServerURL";
        private const string DebugPopperURL = "http://debug/";

        private readonly IHttpClientFactory _clientFactory;
        private readonly IUserSettings _settings;

        private Uri _baseUri;
        public Uri BaseUri
        {
            get { return _baseUri; }
            set { _baseUri = value; }
        }

        public Api(IHttpClientFactory clientFactory,
                   IUserSettings settings)
        {
            _clientFactory = clientFactory;
            _settings = settings;
        }


        public async Task<PopperResponse<ResponseT>> MakeRequest<RequestT, ResponseT>(RequestT request)
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

                    if (contentType.MediaType == "application/json")
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
                    else if (contentType.MediaType.StartsWith("image", StringComparison.CurrentCultureIgnoreCase))
                    {
                        byte[] responseBody = await httpResponse.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                        response.Data = (ResponseT)(object)contentType.MediaType;
                        response.Raw = responseBody;
                    }
                    else if (contentType.MediaType == "text/html")
                    {
                        string responseBody = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

                        //HACK - Backglass GetDisplay is sometimes returning text/html instead of video???
                        if (responseBody.Contains("ftypf4v"))
                        {
                            response.Data = (ResponseT)(object)"video/f4v";
                            response.Raw = await httpResponse.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                        }
                        else
                        {
                            response.Data = (ResponseT)(object)responseBody;
                        }
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
                Logger.Error($"Error fetching {url}", ex);
                response.Success = false;
                response.Messsage = ex.Message;
            }

            return response;
        }
    }
}
