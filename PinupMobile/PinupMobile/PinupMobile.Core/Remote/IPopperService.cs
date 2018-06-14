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

        Task<bool> SendSelect();

        Task<bool> SendPlayGame(int gameid);

        Task<bool> SendExitEmulator();

        Task<bool> SendHome();

        Task<bool> SendMenuReturn();

        Task<bool> SendShutdown();

        Task<bool> SendSystemMenu();

        Task<bool> SendRestart();

        Task<string> GetDisplay(string display);

        string GetCurrentSavedPopperURL();
    }
}
