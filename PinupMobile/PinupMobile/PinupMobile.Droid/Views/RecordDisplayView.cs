using System;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Droid.Support.V7.AppCompat;
using PinupMobile.Core.ViewModels;
using PinupMobile.Droid.Extensions;

namespace PinupMobile.Droid.Views
{
    [Activity(Label = "Record",
              HardwareAccelerated = true,
              ParentActivity = typeof(HomeView),
              ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class RecordDisplayView : MvxAppCompatActivity<RecordDisplayViewModel>
    {
        protected Android.Support.V7.Widget.Toolbar Toolbar { get; set; }

        private ImageButton _recordButton;
        private TextView _durationLabel;
        private RelativeLayout _root;
        private FrameLayout _border;

        private bool _isRecording;
        public bool IsRecording
        {
            get
            {
                return _isRecording;
            }

            set
            {
                if (_isRecording != value)
                {
                    _isRecording = value;
                    UpdateRecordUI();
                }
            }
        }

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

            _root = FindViewById<RelativeLayout>(Resource.Id.content_frame);
            _durationLabel = FindViewById<TextView>(Resource.Id.duration);
            _recordButton = FindViewById<ImageButton>(Resource.Id.record);
            _border = FindViewById<FrameLayout>(Resource.Id.border);

            _durationLabel.Visibility = ViewStates.Invisible;
            _border.Visibility = ViewStates.Invisible;

            var set = this.CreateBindingSet<RecordDisplayView, RecordDisplayViewModel>();
            set.Bind(this).For(v => v.IsRecording).To(vm => vm.Recording);
            set.Apply();
        }

        private void UpdateRecordUI()
        {
            //Duration Label
            if (IsRecording)
            {
                _durationLabel.FadeIn();
            }
            else
            {
                _durationLabel.FadeOut(500);
            }

            //Border
            if (IsRecording)
            {
                _border.FadeIn();
            }
            else
            {
                _border.FadeOut(500);
            }


            //Record Button
            _recordButton.FadeOut(250, 0, () =>
            {
                //Switch image
                int button = (IsRecording) ? Resource.Drawable.selector_button_stop : Resource.Drawable.selector_button_record;
                _recordButton.SetImageDrawable(ContextCompat.GetDrawable(ApplicationContext, button));
                _recordButton.FadeIn(250);
            });
        }
    }
}
