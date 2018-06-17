using Android.App;
using Android.OS;
using MvvmCross.Droid.Support.V7.AppCompat;
using PinupMobile.Core.ViewModels;

namespace PinupMobile.Droid.Views
{
    [Activity(NoHistory = true,
              ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class AppStartupView : MvxAppCompatActivity<AppStartupViewModel>
    {

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.AppStartupView);
        }
    }
}
