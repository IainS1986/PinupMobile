using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Widget;
using MvvmCross;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Droid.Support.V7.AppCompat;
using PinupMobile.Core.Remote;
using PinupMobile.Core.ViewModels;
using PinupMobile.Droid.Extensions;

namespace PinupMobile.Droid.Views
{
    [Activity(Label = "PinupPopper Remote",
              LaunchMode = Android.Content.PM.LaunchMode.SingleInstance,
              ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class HomeView : MvxAppCompatActivity<HomeViewModel>
    {
        protected Android.Support.V7.Widget.Toolbar Toolbar { get; set; }

        private IPopperService _popper;

        private ImageView _imageView;
        private TextView _gameTitle;
        private ImageButton[] _buttons;
        private bool _introPlayed;

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
            _gameTitle = FindViewById<TextView>(Resource.Id.game_title);
            _buttons = new ImageButton[10];
            _buttons[0] = FindViewById<ImageButton>(Resource.Id.button_power);
            _buttons[1] = FindViewById<ImageButton>(Resource.Id.button_select);
            _buttons[2] = FindViewById<ImageButton>(Resource.Id.button_menu_return);
            _buttons[3] = FindViewById<ImageButton>(Resource.Id.button_page_prev);
            _buttons[4] = FindViewById<ImageButton>(Resource.Id.button_skip_prev);
            _buttons[5] = FindViewById<ImageButton>(Resource.Id.button_skip_next);
            _buttons[6] = FindViewById<ImageButton>(Resource.Id.button_page_next);
            _buttons[7] = FindViewById<ImageButton>(Resource.Id.button_home);
            _buttons[8] = FindViewById<ImageButton>(Resource.Id.button_play);
            _buttons[9] = FindViewById<ImageButton>(Resource.Id.button_exit_emulator);

            var set = this.CreateBindingSet<HomeView, HomeViewModel>();
            set.Bind(this).For(v => v.WheelImagePath).To(vm => vm.WheelIconPath);
            set.Apply();

        }

        protected override void OnStart()
        {
            base.OnStart();

            if (!_introPlayed)
            {
                IntroAnimation();
            }
        }

        private void IntroAnimation()
        {
            //Fade in Wheel
            _imageView.FadeIn();

            if (_gameTitle.Visibility == ViewStates.Visible)
            {
                _gameTitle.FadeIn();
            }

            //Animate buttons in sequence
            for (int i = 0; i < _buttons.Length; i++)
            {
                _buttons[i].FadeIn(250, 100 + (100 * i));
            }

            _introPlayed = true;
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
