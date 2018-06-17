using System;
using System.IO;
using AVFoundation;
using AVKit;
using Foundation;
using MvvmCross;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using PinupMobile.Core.Converters;
using PinupMobile.Core.Alerts;
using PinupMobile.Core.ViewModels;
using UIKit;

namespace PinupMobile.iOS.Views
{
    [MvxModalPresentation(Animated = true,
                          WrapInNavigationController = true,
                          ModalTransitionStyle = UIModalTransitionStyle.FlipHorizontal,
                          ModalPresentationStyle = UIModalPresentationStyle.FullScreen)]
    public partial class DisplayView : MvxViewController<DisplayViewModel>
    {
        private IDialog _dialog;

        private AVQueuePlayer _avplayer;
        private AVPlayerViewController _avplayerController;
        private AVPlayerLooper _avLooper;

        private UIBarButtonItem _closeButton;

        private string _mediaUrl = string.Empty;
        public string MediaUrl
        {
            get { return _mediaUrl; }
            set { _mediaUrl = value; Play(); }
        }

        public DisplayView() : base("DisplayView", null)
        {
            _dialog = Mvx.Resolve<IDialog>();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _closeButton = new UIBarButtonItem("Close", UIBarButtonItemStyle.Plain, null);
            NavigationItem.LeftBarButtonItem = _closeButton;

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
            set.Bind(_closeButton).To(vm => vm.CloseCommand);
            set.Apply();
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
        }

        private void Play()
        {
            if (string.IsNullOrEmpty(MediaUrl))
            {
                LoadingSpinner.Hidden = true;
                return;
            }

            //f4v is not supported on iOS (thanks Apple)
            if(MediaUrl.EndsWith(".f4v", StringComparison.CurrentCultureIgnoreCase))
            {
                //Show alert
                LoadingSpinner.Hidden = true;
                _dialog.Show("Playback Failed",
                             "Sorry, f4v format is not supported on iOS devices. If you autorecord new videos with Popper they will be in mp4 format.", 
                             "Close",
                             async () => { await ViewModel?.CloseCommand?.ExecuteAsync(); });
                return;
            }

            if (MediaUrl.EndsWith(".mp4", StringComparison.CurrentCultureIgnoreCase) ||
                MediaUrl.EndsWith(".m4v", StringComparison.CurrentCultureIgnoreCase))
            {
                ImageView.Hidden = true;

                NSUrl url = NSUrl.CreateFileUrl(MediaUrl, null);
                AVPlayerItem item = new AVPlayerItem(url);
                
                _avLooper = new AVPlayerLooper(_avplayer, item, CoreMedia.CMTimeRange.InvalidRange);
                _avplayer.ReplaceCurrentItemWithPlayerItem(item);
                _avplayer.Play();

            }
            else if(MediaUrl.EndsWith(".png", StringComparison.CurrentCultureIgnoreCase))
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
