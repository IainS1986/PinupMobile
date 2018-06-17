using System;
using Android.Views;
using Android.Views.Animations;

namespace PinupMobile.Droid.Extensions
{
    public static class ViewExtensions
    {
        public static void FadeIn(this View view,
                                  long durationMS = 1000,
                                  long delayMS = 0)
        {
            //view.Visibility = ViewStates.Visible;

            AlphaAnimation animation = new AlphaAnimation(0, 1);
            animation.Duration = durationMS;
            animation.StartOffset = delayMS;
            //animation.AnimationEnd += (sender, e) => view.Visibility = ViewStates.Visible;

            view.StartAnimation(animation);
        }
    }
}
