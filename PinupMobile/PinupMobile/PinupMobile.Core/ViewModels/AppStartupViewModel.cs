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
        : MvxViewModel
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

        public override async Task Initialize()
        {
            await base.Initialize();

            // TODO First time run to setup Popper Server URL....
            await Task.Run(async () =>
            {
                var currentItem = await _server.GetCurrentItem();

                if(currentItem!=null)
                {
                    // Go to home screen
                    // TODO Pass in the current item, no need to reload in Homescreen
                    await _navigationService.Navigate<HomeViewModel>();
                }
                else
                {
                    // Go to Setup screen
                    Logger.Debug("No Popper Server found and No Setup View made yet...");
                }

            }).ConfigureAwait(false);

            Logger.Debug("AppStartup Complete");
        }
    }
}
