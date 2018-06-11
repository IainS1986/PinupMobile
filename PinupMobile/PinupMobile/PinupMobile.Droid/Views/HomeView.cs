using Android.App;
using Android.OS;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Droid.Support.V7.AppCompat;
using PinupMobile.Core.ViewModels;

namespace PinupMobile.Droid.Views
{
    [Activity(Label = "PinupPopper")]
    public class HomeView : MvxAppCompatActivity<HomeViewModel>
    {
        private ImageView _imageView;

        private string _wheelUrl = string.Empty;
        public string WheelImagePath
        {
            get { return _wheelUrl; }
            set { _wheelUrl = value; UpdateWheel(); }
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.HomeView);

            _imageView = FindViewById<ImageView>(Resource.Id.image_view);

            var set = this.CreateBindingSet<HomeView, HomeViewModel>();
            set.Bind(this).For(v => v.WheelImagePath).To(vm => vm.WheelIconPath);
            set.Apply();
        }

        private void UpdateWheel()
        {
            if (string.IsNullOrEmpty(WheelImagePath))
            {
                return;
            }

            //Android is a wee bit annoying. Setting the *same* URI, even if the file it points too
            //has changed, won't refresh the image. So we need ot null then reset for now
            _imageView.SetImageURI(null);
            _imageView.SetImageURI(Android.Net.Uri.FromFile(new Java.IO.File(WheelImagePath)));
        }
    }
}
