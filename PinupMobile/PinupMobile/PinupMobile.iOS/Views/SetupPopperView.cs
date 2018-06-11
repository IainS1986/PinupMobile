using System;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using PinupMobile.Core.Converters;
using PinupMobile.Core.ViewModels;
using UIKit;

namespace PinupMobile.iOS.Views
{
    public partial class SetupPopperView : MvxViewController<SetupPopperViewModel>
    {
        public SetupPopperView() : base("SetupPopperView", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            NavigationController.NavigationBar.Translucent = false;

            UrlInput.ShouldReturn = delegate
            {
                UrlInput.ResignFirstResponder();

                //Try and Connect
                if(ViewModel != null)
                {
                    ViewModel.OnConnectCommand.ExecuteAsync();
                }

                return true;
            };

            var set = this.CreateBindingSet<SetupPopperView, SetupPopperViewModel>();
            set.Bind(UrlInput).For(v => v.Text).To(vm => vm.Url).TwoWay();
            set.Bind(UrlInput).For(v => v.Enabled).To(vm => vm.Connecting).WithConversion("IsFalse");
            set.Bind(ConnectingSpinner).For(v => v.Hidden).To(vm => vm.Connecting).WithConversion("IsFalse");
            set.Bind(ErrorLabel).For(v => v.Hidden).To(vm => vm.FailedToConnect).WithConversion("IsFalse");
            set.Bind(ConnectButton).For(v => v.Enabled).To(vm => vm.CanConnect);
            set.Bind(ConnectButton).To(vm => vm.OnConnectCommand);
            set.Apply();
        }
    }
}
