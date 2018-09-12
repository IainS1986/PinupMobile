using System;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using MvvmCross;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Droid.Support.V7.AppCompat;
using PinupMobile.Core.Alerts;
using PinupMobile.Core.Logging;
using PinupMobile.Core.Remote;
using PinupMobile.Core.Strings;
using PinupMobile.Core.ViewModels;
using PinupMobile.Droid.Controls;
using static Android.Media.MediaPlayer;

namespace PinupMobile.Droid.Views
{
    [Activity(Label = "Display",
              HardwareAccelerated = true,
              ParentActivity = typeof(HomeView),
              ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class DisplayView : MvxAppCompatActivity<DisplayViewModel>, IOnVideoSizeChangedListener
    {
        protected Android.Support.V7.Widget.Toolbar Toolbar { get; set; }

        private IPopperService _popper;

        private AutoFitTextureView _textureView;
        private ImageView _imageView;
        private Android.Media.MediaPlayer _mediaPlayer; 
        private ProgressBar _loadingSpinner;
        private IDialog _dialogService;

        private bool _videoSizeSetupDone = false;

        private string _mediaUrl = string.Empty;
        public string MediaUrl
        {
            get { return _mediaUrl; }
            set { _mediaUrl = value; Play(); }
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            _popper = Mvx.Resolve<IPopperService>();
            _dialogService = Mvx.Resolve<IDialog>();

            SetContentView(Resource.Layout.DisplayView);

            Toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            if (Toolbar != null)
            {
                SetSupportActionBar(Toolbar);
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                SupportActionBar.SetHomeButtonEnabled(true);
            }

            _textureView = FindViewById<AutoFitTextureView>(Resource.Id.texture_view);
            _imageView = FindViewById<ImageView>(Resource.Id.image_view);
            _loadingSpinner = FindViewById<ProgressBar>(Resource.Id.progressBar1);

            var set = this.CreateBindingSet<DisplayView, DisplayViewModel>();
            set.Bind(this).For(v => v.MediaUrl).To(vm => vm.MediaUrl);
            set.Apply();
        }

        protected override void OnStart()
        {
            base.OnStart();

            if (_textureView != null)
            {
                _textureView.SurfaceTextureAvailable += OnSurfaceTextureAvailable;
            }
        }

        protected override void OnStop()
        {
            base.OnStop();

            if(_mediaPlayer != null)
            {
                _mediaPlayer.Stop();
                _mediaPlayer.Error -= OnMediaPlaybackError;
                _mediaPlayer.Prepared -= OnMediaPlayerPrepared;
                _mediaPlayer.Release();
                _mediaPlayer.Dispose();
                _mediaPlayer = null;
            }

            if(_textureView != null)
            {
                _textureView.SurfaceTextureAvailable -= OnSurfaceTextureAvailable;
            }
        }

        public void OnSurfaceTextureAvailable(object sender, TextureView.SurfaceTextureAvailableEventArgs e)
        {
            Surface surface = new Surface(e.Surface);
            try
            {
                _mediaPlayer = new Android.Media.MediaPlayer();
                _mediaPlayer.Error += OnMediaPlaybackError;
                _mediaPlayer.Prepared += OnMediaPlayerPrepared;
                _mediaPlayer.SetSurface(surface);
                _mediaPlayer.SetOnVideoSizeChangedListener(this);

                //Start Play incase the URL came in before we were ready
                Play();
            }
            catch(Exception ex)
            {
                Logger.Error($"Error setting up surface for video - {ex.Message}", ex);
            }
        }

        private async void Play()
        {
            if (string.IsNullOrEmpty(MediaUrl) ||
                _mediaPlayer == null)
            {
                return;
            }

            try
            {
                // Video Support
                if (MediaUrl.EndsWith(".mp4", StringComparison.CurrentCultureIgnoreCase) ||
                    MediaUrl.EndsWith(".f4v", StringComparison.CurrentCultureIgnoreCase) ||
                    MediaUrl.EndsWith(".m4v", StringComparison.CurrentCultureIgnoreCase))
                {
                    _imageView.Visibility = ViewStates.Invisible;

                    if(_popper.IsDebugMode)
                    {
                        AssetFileDescriptor afd = Assets.OpenFd(MediaUrl);
                        await _mediaPlayer.SetDataSourceAsync(afd);
                    }
                    else
                    {
                        await _mediaPlayer.SetDataSourceAsync(MediaUrl);
                    }

                    _mediaPlayer.Looping = true;
                    _mediaPlayer.Prepare();
                }
                // PNG Support
                else if (MediaUrl.EndsWith(".png", StringComparison.CurrentCultureIgnoreCase))
                {
                    _textureView.Visibility = ViewStates.Invisible;
                    _mediaPlayer.Release();
                    _mediaPlayer.Dispose();
                    _mediaPlayer = null;

                    if (_popper.IsDebugMode)
                    {
                        System.IO.Stream ims = Assets.Open(MediaUrl);
                        Drawable d = Drawable.CreateFromStream(ims, null);
                        _imageView.SetImageDrawable(d);
                    }
                    else
                    {
                        _imageView.SetImageURI(Android.Net.Uri.FromFile(new Java.IO.File(MediaUrl)));
                    }

                    _loadingSpinner.Visibility = ViewStates.Invisible;
                }
            }
            catch(Exception ex)
            {
                Logger.Error($"Error trying to start media player - {ex.Message}", ex);
            }
        }

        public void OnVideoSizeChanged(Android.Media.MediaPlayer mp, int width, int height)
        {
            if (width > 0 && height > 0 && !_videoSizeSetupDone)
            {
                Logger.Debug($"Video size changed: {width} x {height}");
                _loadingSpinner.Visibility = ViewStates.Invisible;
                ChangeVideoSize(width, height);
            }
        }

        private void OnMediaPlayerPrepared(object sender, EventArgs e)
        {
            _mediaPlayer.Start();
        }

        private void OnMediaPlaybackError(object sender, ErrorEventArgs e)
        {
            Logger.Error($"Error playing media {MediaUrl}, {e}");
            _dialogService.Show(Translation.alert_display_failed_title,
                                Translation.alert_display_failed_body,
                                Translation.general_close,
                                async () => { await ViewModel?.CloseCommand?.ExecuteAsync(); });
        }

        private void ChangeVideoSize(int width, int height)
        {
            DisplayMetrics metrics = new DisplayMetrics();
            RelativeLayout.LayoutParams layoutParams;

            IWindowManager manager = ApplicationContext.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();
            manager.DefaultDisplay.GetMetrics(metrics);

            float rotation = 90.0f;
            layoutParams = new RelativeLayout.LayoutParams(metrics.HeightPixels, metrics.WidthPixels);
            float scale = (width * 1.0f) / (height * 1.0f);
            _textureView.Rotation = rotation;
            _textureView.ScaleX = scale;

            layoutParams.AddRule(LayoutRules.CenterInParent, -1);
            _textureView.LayoutParameters = layoutParams;
            _videoSizeSetupDone = true;
        }
    }
}
