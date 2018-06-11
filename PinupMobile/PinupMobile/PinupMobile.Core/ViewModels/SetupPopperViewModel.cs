using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using PinupMobile.Core.Logging;
using PinupMobile.Core.Remote;
using PinupMobile.Core.Remote.API;
using PinupMobile.Core.Remote.Model;
using PinupMobile.Core.Settings;

namespace PinupMobile.Core.ViewModels
{
    /// <summary>
    /// Main home screen for pinup remote
    /// Shows current playing item
    /// </summary>
    public class SetupPopperViewModel
        : MvxViewModel
    {
        private readonly IPopperService _server;
        private readonly IMvxNavigationService _navigationService;
        private readonly IUserSettings _userSettings;

        private string _url;
        public string Url
        {
            get { return _url; }
            set { _url = value; RaisePropertyChanged(() => Url); RaisePropertyChanged(() => CanConnect); }
        }

        private bool _connecting;
        public bool Connecting
        {
            get { return _connecting; }
            set { _connecting = value; RaisePropertyChanged(() => Connecting); RaisePropertyChanged(() => CanConnect); }
        }

        private bool _failedToConnect;
        public bool FailedToConnect
        {
            get { return _failedToConnect; }
            set { _failedToConnect = value; RaisePropertyChanged(() => FailedToConnect); }
        }

        public bool CanConnect
        {
            get { return !Connecting && !string.IsNullOrEmpty(Url); }
        }

        public MvxAsyncCommand OnConnectCommand => new MvxAsyncCommand(TryConnect);

        public SetupPopperViewModel(IPopperService server,
                                    IMvxNavigationService navigationService,
                                    IUserSettings userSettings)
        {
            _server = server;
            _navigationService = navigationService;
            _userSettings = userSettings;
        }

        private async Task TryConnect()
        {
            if (!CanConnect)
            {
                return;
            }

            FailedToConnect = false;
            Connecting = true;

            // Try and connect
            // Ensure at least a few seconds so the user knows it actually
            // happened.
            var minTime = Task.Delay(2000);
            var connectReq = _server.ServerExistsWithUrl($"http://{Url}");

            var popperConnected = await connectReq;
            await minTime;

            Connecting = false;

            if (popperConnected)
            {
                //Go to Home
                await _navigationService.Navigate<HomeViewModel>();
            }
            else
            {
                //Display error
                FailedToConnect = true;
            }
        }
    }
}
