using System.Threading.Tasks;
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

        public AppStartupViewModel(IUserSettings settings,
                                   IPopperService server)
        {
            _settings = settings;
            _server = server;
        }

        public override async Task Initialize()
        {
            await base.Initialize();

            // TODO First time run to setup Popper Server URL....
            await Task.Run(async () =>
            {
                await _server.CheckPopperServerBroadcasting();

            }).ConfigureAwait(false);

            Logger.Debug("AppStartup Complete");
        }
    }
}
