using System;
using Android.App;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Droid.Support.V7.AppCompat;
using PinupMobile.Core.Logging;
using PinupMobile.Core.ViewModels;
using PinupMobile.Droid.Controls;

namespace PinupMobile.Droid.Views
{
    [Activity(Label = "Display", HardwareAccelerated = true, ParentActivity = typeof(HomeView))]
    public class DisplayView : MvxAppCompatActivity<DisplayViewModel>
    {
        protected Toolbar Toolbar { get; set; }

        private AutoFitTextureView _textureView;

        private MediaPlayer _mediaPlayer;

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

            Toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            if (Toolbar != null)
            {
                SetSupportActionBar(Toolbar);
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                SupportActionBar.SetHomeButtonEnabled(true);
            }

            _textureView = FindViewById<AutoFitTextureView>(Resource.Id.texture_view);

            var set = this.CreateBindingSet<DisplayView, DisplayViewModel>();
            set.Bind(this).For(v => v.VideoUrl).To(vm => vm.VideoUrl);
            set.Apply();
        }

        protected override void OnResume()
        {
            base.OnResume();

            //SetupMatrix(this.Window.DecorView.Width, this.Window.DecorView.Height, 90);
        }

        private void SetupMatrix(int width, int height, int degrees)
        {
            Matrix matrix = new Matrix();
            //The video will be streched if the aspect ratio is in 1,5(recording at 480)
            RectF src;
            src = new RectF(0, 0, _textureView.MeasuredWidth, _textureView.MeasuredHeight);
            RectF dst = new RectF(0, 0, width, height);
            RectF screen = new RectF(dst);
            matrix.PostRotate(degrees, screen.CenterX(), screen.CenterY());
            matrix.MapRect(dst);

            matrix.SetRectToRect(src, dst, Matrix.ScaleToFit.Center);
            matrix.MapRect(src);

            matrix.SetRectToRect(screen, src, Matrix.ScaleToFit.Fill);
            matrix.PostRotate(degrees, screen.CenterX(), screen.CenterY());

            _textureView.SetTransform(matrix);
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
    }
}
