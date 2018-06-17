using System;
using UIKit;

namespace PinupMobile.iOS.Extensions
{
    public static class UIViewExtensions
    {
        public static void FadeIn(this UIView view,
                                  double duration = 1,
                                  double delay = 0)
        {
            view.Alpha = 0;
            UIView.Animate(duration,
                           delay,
                           UIViewAnimationOptions.CurveEaseInOut | UIViewAnimationOptions.BeginFromCurrentState,
                           () => view.Alpha = 1,
                           null);
        }

        public static void FadeOut(this UIView view,
                                   double duration = 1,
                                   double delay = 0)
        {
            view.Alpha = 1;
            UIView.Animate(duration,
                           delay,
                           UIViewAnimationOptions.CurveEaseInOut | UIViewAnimationOptions.BeginFromCurrentState,
                           () => view.Alpha = 0,
                           null);
        }
    }
}
