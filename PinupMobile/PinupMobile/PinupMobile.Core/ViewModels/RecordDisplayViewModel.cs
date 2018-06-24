using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using PinupMobile.Core.Remote;

namespace PinupMobile.Core.ViewModels
{
    /// <summary>
    /// Main home screen for pinup remote
    /// Shows current playing item
    /// </summary>
    public class RecordDisplayViewModel
        : MvxViewModel
    {
        private readonly IPopperService _server;
        private readonly IMvxNavigationService _navigationService;

        public MvxAsyncCommand CloseCommand => new MvxAsyncCommand(async () => await _navigationService.Close(this));

        public RecordDisplayViewModel(IPopperService server,
                             IMvxNavigationService navigationService)
        {
            _server = server;
            _navigationService = navigationService;
        }
    }
}
