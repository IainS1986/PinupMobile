using Android.App;
using Android.OS;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Droid.Support.V7.AppCompat;
using PinupMobile.Core.ViewModels;

namespace PinupMobile.Droid.Views
{
    [Activity(Label = "Setup")]
    public class SetupPopperView : MvxAppCompatActivity<SetupPopperViewModel>
    {
        protected Android.Support.V7.Widget.Toolbar Toolbar { get; set; }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.SetupPopperView);

            Toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            if (Toolbar != null)
            {
                SetSupportActionBar(Toolbar);
                SupportActionBar.Title = "Connect to Popper";
            }

            var set = this.CreateBindingSet<SetupPopperView, SetupPopperViewModel>();
            set.Apply();
        }
    }
}
