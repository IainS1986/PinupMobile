using System;
using System.Threading.Tasks;
using PinupMobile.Core.Remote.Model;

namespace PinupMobile.Core.Remote
{
    public interface IPopperService
    {
        Task<CurrentItem> GetCurrentItem();

        Task<bool> ServerExists();

        Task<bool> SendGameNext();

        Task<bool> SendGamePrev();
    }
}
