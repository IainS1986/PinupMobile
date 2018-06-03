using Android.App;
using Android.OS;
using MvvmCross.Droid.Support.V7.AppCompat;
using PinupMobile.Core.ViewModels;

namespace PinupMobile.Droid.Views
{
    [Activity(Label = "PinupPopper")]
    public class HomeView : MvxAppCompatActivity<HomeViewModel>
    {

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.HomeView);
        }
    }
}
