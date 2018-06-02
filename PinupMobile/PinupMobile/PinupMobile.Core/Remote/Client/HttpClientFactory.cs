using System.Net.Http;

namespace PinupMobile.Core.Remote.Client
{
    public class HttpClientFactory : IHttpClientFactory
    {
        public HttpClient Create()
        {
            return new HttpClient();
        }
    }
}
