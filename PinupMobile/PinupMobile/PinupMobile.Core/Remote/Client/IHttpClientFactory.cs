using System.Net.Http;

namespace PinupMobile.Core.Remote.Client
{
    public interface IHttpClientFactory
    {
        HttpClient Create();
    }
}
