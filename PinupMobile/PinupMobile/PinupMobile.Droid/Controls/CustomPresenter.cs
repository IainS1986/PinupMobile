using System;
using System.Collections.Generic;
using System.Reflection;
using Android.App;
using Android.Content;
using Android.OS;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platforms.Android.Presenters;

namespace PinupMobile.Droid.Controls
{
    public class CustomPresenter : MvxAppCompatViewPresenter
    {
        public CustomPresenter(IEnumerable<Assembly> androidViewAssemblies) : base(androidViewAssemblies)
        {
        }

        protected override void ShowIntent(Intent intent, Bundle bundle)
        {
            CurrentActivity.StartActivity(intent);
            CurrentActivity.OverridePendingTransition(Resource.Animator.flip_left_in, Resource.Animator.flip_left_out);
        }
    }
}
