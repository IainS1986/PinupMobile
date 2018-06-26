using System;
using Android.Views;
using Android.Views.Animations;

namespace PinupMobile.Droid.Extensions
{
    public static class ViewExtensions
    {
        public static void FadeIn(this View view,
                                  long durationMS = 1000,
                                  long delayMS = 0,
                                  Action onComplete = null)
        {
            view.Visibility = ViewStates.Visible;

            AlphaAnimation animation = new AlphaAnimation(0, 1);
            animation.Duration = durationMS;
            animation.StartOffset = delayMS;
            animation.Interpolator = new AccelerateDecelerateInterpolator();
            animation.AnimationEnd += (sender, e) => { onComplete?.Invoke(); };

            view.StartAnimation(animation);
        }

        public static void FadeOut(this View view,
                                   long durationMS = 1000,
                                   long delayMS = 0,
                                   Action onComplete = null)
        {
            view.Visibility = ViewStates.Visible;

            AlphaAnimation animation = new AlphaAnimation(1, 0);
            animation.Duration = durationMS;
            animation.StartOffset = delayMS;
            animation.Interpolator = new AccelerateDecelerateInterpolator();
            animation.AnimationEnd += (sender, e) => { view.Visibility = ViewStates.Invisible; onComplete?.Invoke(); };

            view.StartAnimation(animation);
        }

    }
}
