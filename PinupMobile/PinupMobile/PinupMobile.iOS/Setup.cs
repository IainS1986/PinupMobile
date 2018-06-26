using MvvmCross;
using MvvmCross.Platforms.Ios.Core;
using PinupMobile.Core;
using PinupMobile.Core.Alerts;
using PinupMobile.Core.Settings;
using PinupMobile.Core.Strings;
using PinupMobile.iOS.Alerts;
using PinupMobile.iOS.Settings;
using UIKit;

namespace PinupMobile.iOS
{
    public class Setup : MvxIosSetup<App>
    {
        protected override void InitializeIoC()
        {
            base.InitializeIoC();

            Mvx.LazyConstructAndRegisterSingleton<ILocalisation, Localisation>();
            Mvx.LazyConstructAndRegisterSingleton<IDialog, Dialog>();
            Mvx.LazyConstructAndRegisterSingleton<IUserSettings, UserSettings>();
        }
    }
}
