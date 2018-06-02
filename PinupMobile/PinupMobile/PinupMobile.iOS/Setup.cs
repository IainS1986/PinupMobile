using MvvmCross;
using MvvmCross.Platforms.Ios.Core;
using PinupMobile.Core;
using PinupMobile.Core.Settings;
using PinupMobile.iOS.Settings;
using UIKit;

namespace PinupMobile.iOS
{
    public class Setup : MvxIosSetup<App>
    {
        protected override void InitializeIoC()
        {
            base.InitializeIoC();

            //Mvx.RegisterType<ICameraCompatibility, CameraCompatibility>();

            Mvx.LazyConstructAndRegisterSingleton<IUserSettings, UserSettings>();
        }
    }
}
