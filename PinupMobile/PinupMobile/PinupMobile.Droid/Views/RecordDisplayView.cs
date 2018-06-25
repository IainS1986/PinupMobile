using System;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.IO;
using MvvmCross;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Droid.Support.V7.AppCompat;
using PinupMobile.Core.Logging;
using PinupMobile.Core.Remote;
using PinupMobile.Core.ViewModels;
using PinupMobile.Droid.Controls;
using static Android.Media.MediaPlayer;

namespace PinupMobile.Droid.Views
{
    [Activity(Label = "Record",
              HardwareAccelerated = true,
              ParentActivity = typeof(HomeView),
              ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class RecordDisplayView : MvxAppCompatActivity<RecordDisplayViewModel>
    {
        protected Android.Support.V7.Widget.Toolbar Toolbar { get; set; }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.RecordDisplayView);

            Toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            if (Toolbar != null)
            {
                SetSupportActionBar(Toolbar);
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                SupportActionBar.SetHomeButtonEnabled(true);
            }

            var set = this.CreateBindingSet<RecordDisplayView, RecordDisplayViewModel>();

            set.Apply();
        }
    }
}
