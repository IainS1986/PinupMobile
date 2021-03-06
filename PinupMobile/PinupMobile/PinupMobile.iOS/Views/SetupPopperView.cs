﻿using System;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using PinupMobile.Core.Converters;
using PinupMobile.Core.Strings;
using PinupMobile.Core.ViewModels;
using PinupMobile.iOS.Extensions;
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

            //Fade all in
            HeaderLabel.FadeIn();
            HttpPrefixLabel.FadeIn();
            UrlInput.FadeIn();
            HelpText.FadeIn();
            ConnectButton.FadeIn();

            HeaderLabel.Text = Translation.setup_top_text;
            HelpText.Text = Translation.setup_footer;
            ErrorLabel.Text = Translation.setup_error;
            ConnectButton.SetTitle(Translation.setup_connect_button, UIControlState.Normal);
        }
    }
}
