using System;
using System.Threading.Tasks;
using PinupMobile.Core.Remote.Model;

namespace PinupMobile.Core.Network
{
    public interface IApi
    {
        Uri BaseUri { get; set; }

        Task<PopperResponse<ResponseT>> MakeRequest<RequestT, ResponseT>(RequestT request)
            where RequestT : class
            where ResponseT : class;
    }
}
