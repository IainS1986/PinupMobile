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

        private string _mediaUrl = string.Empty;
        public string MediaUrl
        {
            get { return _mediaUrl; }
            set { _mediaUrl = value; Play(); }
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
            ImageView.Transform = CoreGraphics.CGAffineTransform.MakeRotation((nfloat)1.5708);
            ImageView.Frame = View.Frame;

            _avplayerController.View.Frame = View.Frame;
            _avplayerController.ShowsPlaybackControls = false;

            // Bring spinner back to front
            View.BringSubviewToFront(LoadingSpinner);

            var set = this.CreateBindingSet<DisplayView, DisplayViewModel>();
            set.Bind(this).For(v => v.MediaUrl).To(vm => vm.MediaUrl);
            set.Apply();
        }

        private void Play()
        {
            if (string.IsNullOrEmpty(MediaUrl))
            {
                return;
            }

            if (MediaUrl.EndsWith(".mp4"))
            {
                ImageView.Hidden = true;

                NSUrl url = NSUrl.CreateFileUrl(MediaUrl, null);
                AVPlayerItem item = new AVPlayerItem(url);
                
                _avLooper = new AVPlayerLooper(_avplayer, item, CoreMedia.CMTimeRange.InvalidRange);
                _avplayer.ReplaceCurrentItemWithPlayerItem(item);
                _avplayer.Play();

            }
            else if(MediaUrl.EndsWith(".png"))
            {
                _avplayer.Dispose();
                _avplayer = null;

                _avplayerController.RemoveFromParentViewController();
                _avplayerController.View.RemoveFromSuperview();
                _avplayerController.Dispose();
                _avplayerController = null;

                UIImage image = UIImage.FromFile(MediaUrl);
                ImageView.Image = image;
            }


            LoadingSpinner.Hidden = true;

        }
    }
}
