using System.Threading.Tasks;
using MvvmCross.ViewModels;
using PinupMobile.Core.Logging;
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
        private const string RunBeforeKey = "run_before";

        private readonly IUserSettings _settings;

        public AppStartupViewModel(IUserSettings settings)
        {
            _settings = settings;
        }

        public override Task Initialize()
        {
            //Test out Mvvm cross IoC working nicely
            bool run_before = _settings.GetBool(RunBeforeKey);
            if (!run_before)
            {
                Logger.Diagnostic("App Not Run before...");
                _settings.SetBool(RunBeforeKey, true);
            }
            else
            {
                Logger.Diagnostic("App Run Before...");
            }

            return base.Initialize();
        }
    }
}
