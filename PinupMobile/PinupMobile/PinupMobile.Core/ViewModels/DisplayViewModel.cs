using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using PinupMobile.Core.Logging;
using PinupMobile.Core.Remote;
using PinupMobile.Core.Remote.API;
using PinupMobile.Core.Remote.Model;

namespace PinupMobile.Core.ViewModels
{
    /// <summary>
    /// Main home screen for pinup remote
    /// Shows current playing item
    /// </summary>
    public class DisplayViewModel
        : MvxViewModel
    {
        private readonly IPopperService _server;
        private readonly IMvxNavigationService _navigationService;

        private string _mediaUrl;
        public string MediaUrl
        {
            get { return _mediaUrl; }
            set { _mediaUrl = value; RaisePropertyChanged(() => MediaUrl); }
        }

        public DisplayViewModel(IPopperService server,
                             IMvxNavigationService navigationService)
        {
            _server = server;
            _navigationService = navigationService;
        }

        public override async void ViewAppearing()
        {
            base.ViewAppearing();

            MediaUrl = string.Empty;

            // Download the Playfield Display
            await Task.Run(async () =>
            {
                MediaUrl = await _server.GetDisplay(PopperDisplayConstants.POPPER_DISPLAY_PLAYFIELD);
            }).ConfigureAwait(false);
        }
    }
}
