using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Droid.Support.V7.AppCompat;
using PinupMobile.Core.Logging;
using PinupMobile.Core.ViewModels;
using PinupMobile.Droid.Controls;
using static Android.Media.MediaPlayer;

namespace PinupMobile.Droid.Views
{
    [Activity(Label = "Display", HardwareAccelerated = true, ParentActivity = typeof(HomeView))]
    public class DisplayView : MvxAppCompatActivity<DisplayViewModel>, IOnVideoSizeChangedListener
    {
        protected Android.Support.V7.Widget.Toolbar Toolbar { get; set; }

        private AutoFitTextureView _textureView;
        private MediaPlayer _mediaPlayer;
        private ProgressBar _loadingSpinner;

        private bool _videoSizeSetupDone = false;

        private string _videoUrl = string.Empty;
        public string VideoUrl
        {
            get { return _videoUrl; }
            set { _videoUrl = value; Play(); }
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.DisplayView);

            Toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            if (Toolbar != null)
            {
                SetSupportActionBar(Toolbar);
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                SupportActionBar.SetHomeButtonEnabled(true);
            }

            _textureView = FindViewById<AutoFitTextureView>(Resource.Id.texture_view);
            _loadingSpinner = FindViewById<ProgressBar>(Resource.Id.progressBar1);

            var set = this.CreateBindingSet<DisplayView, DisplayViewModel>();
            set.Bind(this).For(v => v.VideoUrl).To(vm => vm.VideoUrl);
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
                _mediaPlayer = new MediaPlayer();
                _mediaPlayer.SetSurface(surface);
                _mediaPlayer.SetOnVideoSizeChangedListener(this);
            }
            catch(Exception ex)
            {
                Logger.Error($"Error setting up surface for video");
                Logger.Error(ex.Message);
            }
        }

        private async void Play()
        {
            if (string.IsNullOrEmpty(VideoUrl))
            {
                return;
            }

            try
            {
                await _mediaPlayer.SetDataSourceAsync(VideoUrl);
                _mediaPlayer.Prepare();
                _mediaPlayer.Looping = true;
                _mediaPlayer.Start();
            }
            catch(Exception ex)
            {
                Logger.Error($"Error trying to start media player");
                Logger.Error(ex.Message);
            }
        }

        public void OnVideoSizeChanged(MediaPlayer mp, int width, int height)
        {
            if (width > 0 && height > 0 && !_videoSizeSetupDone)
            {
                Logger.Debug($"Video size changed: {width} x {height}");
                _loadingSpinner.Visibility = ViewStates.Invisible;
                ChangeVideoSize(width, height);
            }
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
