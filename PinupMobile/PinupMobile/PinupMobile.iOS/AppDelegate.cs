using Foundation;
using UIKit;
using MvvmCross.Platforms.Ios.Core;
using MvvmCross.ViewModels;
using MvvmCross;
using PinupMobile.Core;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace PinupMobile.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : MvxApplicationDelegate<Setup, App>
    {
        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            //AppCenter.Start("YOUR KEY", typeof(Analytics), typeof(Crashes));

            return base.FinishedLaunching(application, launchOptions);
        }
    }
}
