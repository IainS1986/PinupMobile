using System;
using System.Threading.Tasks;

namespace PinupMobile.Core.Remote
{
    public interface IPopperService
    {
        Task<bool> CheckPopperServerBroadcasting();
    }
}
