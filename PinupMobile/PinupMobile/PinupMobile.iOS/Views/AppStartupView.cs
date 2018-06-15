using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using PinupMobile.Core.ViewModels;
using UIKit;

namespace PinupMobile.iOS.Views
{
    [MvxRootPresentation(WrapInNavigationController = false, 
                         AnimationDuration = 0.35f,
                         AnimationOptions = UIViewAnimationOptions.TransitionCrossDissolve)]
    public partial class AppStartupView : MvxViewController
    {

        public AppStartupView() : base("AppStartupView", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            //Fade spinner in
            LoadingSpinner.Alpha = 0;
            UIView.Animate(0.5,
                           0,
                           UIViewAnimationOptions.CurveEaseInOut | UIViewAnimationOptions.BeginFromCurrentState,
                           () => { LoadingSpinner.Alpha = 1; },
                           null);
          
            var set = this.CreateBindingSet<AppStartupView, AppStartupViewModel>();
            set.Apply();
        }
    }
}
