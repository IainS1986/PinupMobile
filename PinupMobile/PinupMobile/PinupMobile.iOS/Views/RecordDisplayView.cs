using System;
using System.IO;
using Foundation;
using MvvmCross;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using PinupMobile.Core.ViewModels;
using UIKit;

namespace PinupMobile.iOS.Views
{
    [MvxModalPresentation(Animated = true,
                          WrapInNavigationController = true,
                          ModalTransitionStyle = UIModalTransitionStyle.FlipHorizontal,
                          ModalPresentationStyle = UIModalPresentationStyle.FullScreen)]
    public partial class RecordDisplayView : MvxViewController<RecordDisplayViewModel>
    {
        private UIBarButtonItem _closeButton;

        public RecordDisplayView() : base("RecordDisplayView", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _closeButton = new UIBarButtonItem("Close", UIBarButtonItemStyle.Plain, null);
            NavigationItem.LeftBarButtonItem = _closeButton;

            var set = this.CreateBindingSet<RecordDisplayView, RecordDisplayViewModel>();
            set.Bind(_closeButton).To(vm => vm.CloseCommand);
            set.Apply();
        }
    }
}
