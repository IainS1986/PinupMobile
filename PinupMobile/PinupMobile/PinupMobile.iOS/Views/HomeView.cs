using System;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using PinupMobile.Core.Converters;
using PinupMobile.Core.ViewModels;
using PinupMobile.iOS.Extensions;
using UIKit;

namespace PinupMobile.iOS.Views
{
    [MvxRootPresentation(WrapInNavigationController = true,
                         AnimationDuration = 0.35f,
                         AnimationOptions = UIViewAnimationOptions.TransitionCrossDissolve)]
    public partial class HomeView : MvxViewController<HomeViewModel>
    {
        private UIImage _currentWheelImage;
        private UIBarButtonItem _refreshButton;
        private UIBarButtonItem _recordButton;

        private string _wheelImagePath;
        public string WheelImagePath
        {
            get { return _wheelImagePath; }
            set { _wheelImagePath = value; UpdateWheel(); }
        }

        public int TitleClickCount { get; set; }

        public HomeView() : base("HomeView", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var refreshImg = UIImage.FromBundle("ic_refresh");
            _refreshButton = new UIBarButtonItem(refreshImg, UIBarButtonItemStyle.Plain, null);
            NavigationItem.RightBarButtonItem = _refreshButton;

            var recordImg = UIImage.FromBundle("ic_record");
            _recordButton = new UIBarButtonItem(recordImg, UIBarButtonItemStyle.Plain, null);
            _recordButton.TintColor = UIColor.Red;
            NavigationItem.LeftBarButtonItem = _recordButton;

            //Rotate Wheel Icon image as they will be rotated 90
            var rect = WheeImage.Frame;
            WheeImage.Transform = CoreGraphics.CGAffineTransform.MakeRotation((nfloat)1.5708);
            WheeImage.Frame = rect;

            var set = this.CreateBindingSet<HomeView, HomeViewModel>();
            set.Bind(CurrentItemName).For(v => v.Text).To(vm => vm.CurrentItem).WithConversion<CurrentItemDisplayNameConverter>();
            set.Bind(this).For(v => v.WheelImagePath).To(vm => vm.WheelIconPath);
            set.Bind(_refreshButton).To(vm => vm.OnRefreshCommand);
            set.Bind(_recordButton).To(vm => vm.OnRecordCommand);
            set.Bind(PrevButton).To(vm => vm.OnGamePrevCommand);
            set.Bind(NextButton).To(vm => vm.OnGameNextCommand);
            set.Bind(PrevPageButton).To(vm => vm.OnPagePrevCommand);
            set.Bind(NextPageButton).To(vm => vm.OnPageNextCommand);
            set.Bind(DisplayButton).To(vm => vm.OnShowDisplayViewCommand);
            set.Bind(PlayButton).To(vm => vm.OnGameStartCommand);
            set.Bind(HomeButton).To(vm => vm.OnHomeCommand);
            set.Bind(ExitButton).To(vm => vm.OnExitEmulatorCommand);
            set.Bind(ShutdownButton).To(vm => vm.OnSystemMenuCommand);
            set.Bind(SelectButton).To(vm => vm.OnSelectCommand);
            set.Bind(BackButton).To(vm => vm.OnMenuReturnCommand);
            set.Bind(HiddenNameButton).To(vm => vm.OnTitleTappedCommand);
            set.Bind(CurrentItemName).For(v => v.Hidden).To(vm => vm.TitleHidden);
            set.Apply();

            IntroAnimation();
        }

        private void IntroAnimation()
        {
            //Fade in Wheel
            WheeImage.FadeIn();

            if(CurrentItemName.Hidden == false)
            {
                CurrentItemName.FadeIn();
            }
            
            //Animate buttons in sequence
            for (int i = 0; i < RemoteButtons.Length; i++)
            {
                RemoteButtons[i].FadeIn(0.25, 0.1 + (0.1 * i));
            }
        }

        private void UpdateWheel()
        {
            if (string.IsNullOrEmpty(WheelImagePath))
            {
                return;
            }

            if (_currentWheelImage != null)
            {
                _currentWheelImage.Dispose();
                _currentWheelImage = null;
            }

            _currentWheelImage = UIImage.FromFile(WheelImagePath);
            WheeImage.Image = _currentWheelImage;
        }
    }
}
