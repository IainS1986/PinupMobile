using System.Threading.Tasks;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using PinupMobile.Core.Logging;
using PinupMobile.Core.Remote;
using PinupMobile.Core.Settings;

namespace PinupMobile.Core.ViewModels
{
    /// <summary>
    /// Handles app initialisation and attempting to connect
    /// to a running PinupServer
    /// </summary>
    public class AppStartupViewModel
        : BaseViewModel
    {
        private readonly IUserSettings _settings;
        private readonly IPopperService _server;
        private readonly IMvxNavigationService _navigationService;

        public AppStartupViewModel(IUserSettings settings,
                                   IPopperService server,
                                   IMvxNavigationService navigationService)
        {
            _settings = settings;
            _server = server;
            _navigationService = navigationService;
        }

        public override async void ViewAppearing()
        {
            base.ViewAppearing();

            // TODO First time run to setup Popper Server URL....
            await Task.Run(async () =>
            {
                var popperConnected = await _server.ServerExists();
                
                if (popperConnected)
                {
                    // Go to home screen
                    // TODO Pass in the current item, no need to reload in Homescreen
                    await _navigationService.Navigate<HomeViewModel>();
                }
                else
                {
                    // Go to Setup screen
                    await _navigationService.Navigate<SetupPopperViewModel>();
                }
                
            }).ConfigureAwait(false);
            
            Logger.Debug("AppStartup Complete");
        }
    }
}
