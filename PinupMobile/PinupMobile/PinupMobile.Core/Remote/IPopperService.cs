using System;
using System.Threading.Tasks;
using PinupMobile.Core.Remote.API;
using PinupMobile.Core.Remote.Model;

namespace PinupMobile.Core.Remote
{
    public interface IPopperService
    {
        Task<Item> GetCurrentItem();

        Task<bool> ServerExists();

        Task<bool> ServerExistsWithUrl(string url);

        Task<bool> SendGameNext();

        Task<bool> SendGamePrev();

        Task<bool> SendPageNext();

        Task<bool> SendPagePrev();

        Task<bool> SendPlayGame();

        Task<bool> SendExitEmulator();

        Task<bool> SendHome();

        Task<string> GetDisplay(string display);
    }
}
