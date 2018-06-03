using System;
using System.IO;
using AVFoundation;
using AVKit;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using PinupMobile.Core.Converters;
using PinupMobile.Core.ViewModels;
using UIKit;

namespace PinupMobile.iOS.Views
{
    public partial class DisplayView : MvxViewController<DisplayViewModel>
    {
        private AVQueuePlayer _avplayer;
        private AVPlayerViewController _avplayerController;
        private AVPlayerLooper _avLooper;

        private string _videoUrl = string.Empty;
        public string VideoUrl
        {
            get { return _videoUrl; }
            set { _videoUrl = value; Play(); }
        }

        public DisplayView() : base("DisplayView", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _avplayer = new AVQueuePlayer();
            _avplayerController = new AVPlayerViewController();
            _avplayerController.Player = _avplayer;

            AddChildViewController(_avplayerController);
            View.AddSubview(_avplayerController.View);

            _avplayerController.View.Transform = CoreGraphics.CGAffineTransform.MakeRotation((nfloat)1.5708);
            _avplayerController.View.Frame = View.Frame;
            _avplayerController.ShowsPlaybackControls = false;

            // Bring spinner back to front
            View.BringSubviewToFront(LoadingSpinner);

            var set = this.CreateBindingSet<DisplayView, DisplayViewModel>();
            set.Bind(this).For(v => v.VideoUrl).To(vm => vm.VideoUrl);
            set.Apply();
        }

        private void Play()
        {
            if (string.IsNullOrEmpty(VideoUrl))
            {
                return;
            }

            NSUrl url = NSUrl.CreateFileUrl(VideoUrl, null);
            AVPlayerItem item = new AVPlayerItem(url);

            _avLooper = new AVPlayerLooper(_avplayer, item, CoreMedia.CMTimeRange.InvalidRange);
            _avplayer.ReplaceCurrentItemWithPlayerItem(item);
            _avplayer.Play();

            LoadingSpinner.Hidden = true;
        }
    }
}
