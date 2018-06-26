using System;
using System.IO;
using CoreAnimation;
using Foundation;
using MvvmCross;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using PinupMobile.Core.Converters;
using PinupMobile.Core.ViewModels;
using PinupMobile.iOS.Extensions;
using UIKit;

namespace PinupMobile.iOS.Views
{
    [MvxModalPresentation(Animated = true,
                          WrapInNavigationController = true,
                          ModalTransitionStyle = UIModalTransitionStyle.FlipHorizontal,
                          ModalPresentationStyle = UIModalPresentationStyle.FullScreen)]
    public partial class RecordDisplayView : MvxViewController<RecordDisplayViewModel>
    {
        private nfloat _borderWidth = 10;
        private UIBarButtonItem _closeButton;

        private bool _isRecording;
        public bool IsRecording
        {
            get 
            { 
                return _isRecording;
            }

            set 
            { 
                if (_isRecording != value)
                {
                    _isRecording = value;
                    UpdateRecordUI();
                }
            }
        }

        public RecordDisplayView() : base("RecordDisplayView", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            NavigationController.NavigationBar.Translucent = false;

            _closeButton = new UIBarButtonItem("Close", UIBarButtonItemStyle.Plain, null);
            NavigationItem.LeftBarButtonItem = _closeButton;

            RecordButton.SetImage(UIImage.FromBundle("ic_record"), UIControlState.Normal);
            RecordButton.TintColor = UIColor.Red;

            BGView.Layer.BorderColor = UIColor.White.CGColor;
            BGView.Layer.BorderWidth = _borderWidth;

            DurationLabel.Alpha = 0;

            var set = this.CreateBindingSet<RecordDisplayView, RecordDisplayViewModel>();
            set.Bind(this).For(v => v.IsRecording).To(vm => vm.Recording);
            set.Bind(_closeButton).To(vm => vm.CloseCommand);
            set.Bind(RecordButton).To(vm => vm.RecordCommand);
            set.Bind(HelpLabel).To(vm => vm.HelpMessage);
            set.Bind(DurationLabel).To(vm => vm.Time);
            set.Apply();
        }

        private void UpdateRecordUI()
        {
            if (DurationLabel != null)
            {
                if (IsRecording)
                {
                    DurationLabel.FadeIn();
                }
                else
                {
                    DurationLabel.FadeOut(0.5);
                }
            }

            if(BGView != null)
            {
                CABasicAnimation borderCol = CABasicAnimation.FromKeyPath("borderColor");
                borderCol.SetFrom((IsRecording) ? UIColor.White.CGColor : UIColor.Red.CGColor);
                borderCol.SetTo((IsRecording) ? UIColor.Red.CGColor : UIColor.White.CGColor);
                borderCol.FillMode = CAFillMode.Forwards;
                borderCol.TimingFunction = CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseInEaseOut);
                borderCol.Duration = 1;
                BGView.Layer.BorderColor = (IsRecording) ? UIColor.Red.CGColor : UIColor.White.CGColor;

                BGView.Layer.AddAnimation(borderCol, "color");
            }

            if (RecordButton != null)
            {
                RecordButton.FadeOut(0.25, 0, () =>
                {
                    if (RecordButton != null)
                    {
                        string fileName = (IsRecording) ? "ic_stop" : "ic_record";
                        RecordButton.SetImage(UIImage.FromBundle(fileName), UIControlState.Normal);
                        RecordButton.TintColor = (IsRecording) ? UIColor.Black : UIColor.Red;
                        RecordButton.FadeIn(0.25);
                    }
                });
            }
        }
    }
}
