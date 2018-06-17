using Android.App;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Widget;
using MvvmCross;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Droid.Support.V7.AppCompat;
using PinupMobile.Core.Remote;
using PinupMobile.Core.ViewModels;

namespace PinupMobile.Droid.Views
{
    [Activity(Label = "PinupPopper Remote")]
    public class HomeView : MvxAppCompatActivity<HomeViewModel>
    {
        protected Android.Support.V7.Widget.Toolbar Toolbar { get; set; }

        private IPopperService _popper;

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

            _popper = Mvx.Resolve<IPopperService>();

            SetContentView(Resource.Layout.HomeView);

            Toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            if (Toolbar != null)
            {
                SetSupportActionBar(Toolbar);
                SupportActionBar.Title = string.Empty;
            }

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

            if (_popper.IsDebugMode)
            {
                System.IO.Stream ims = Assets.Open(WheelImagePath);
                Drawable d = Drawable.CreateFromStream(ims, null);
                _imageView.SetImageDrawable(d);
            }
            else
            {
                //Android is a wee bit annoying. Setting the *same* URI, even if the file it points too
                //has changed, won't refresh the image. So we need ot null then reset for now
                _imageView.SetImageURI(null);
                _imageView.SetImageURI(Android.Net.Uri.FromFile(new Java.IO.File(WheelImagePath)));
            }

        }
    }
}
