using MvvmCross.Platforms.Ios.Core;
using PinupMobile.Core;
using UIKit;

namespace PinupMobile.iOS
{
    public class Setup : MvxIosSetup<App>
    {
        protected override void InitializeIoC()
        {
            base.InitializeIoC();

            //Mvx.RegisterType<ICameraCompatibility, CameraCompatibility>();
        }
    }
}
