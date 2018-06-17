using System;
using System.Collections.Generic;
using Android.Support.V7.App;
using MvvmCross.Base;
using MvvmCross.Platforms.Android;
using PinupMobile.Core.Alerts;

namespace PinupMobile.Droid.Alerts
{
    public class Dialog : MvxMainThreadDispatchingObject, IDialog
    {
        private readonly IMvxAndroidCurrentTopActivity _topActivity;

        public Dialog(IMvxAndroidCurrentTopActivity topActivity)
        {
            _topActivity = topActivity;
        }

        public void Show(string title, string message, string cancelText, Action buttonAction = null)
        {
            var alertDialog = new AlertDialog.Builder(_topActivity.Activity).Create();
            alertDialog.SetTitle(title);
            alertDialog.SetMessage(message);
            alertDialog.SetButton((int)Android.Content.DialogButtonType.Positive,
                                  cancelText,
                                  (sender, e) => buttonAction?.Invoke());

            InvokeOnMainThread(alertDialog.Show);
        }

        public void Show(string title, string message, string cancelText, List<(string, Action)> actions)
        {
            // TODO: this doesn't properly support multiple actions - needs custom layout
            var alertDialog = new AlertDialog.Builder(_topActivity.Activity).Create();
            alertDialog.SetTitle(title);
            alertDialog.SetMessage(message);
            alertDialog.SetButton((int)Android.Content.DialogButtonType.Positive,
                                  actions[0].Item1,
                                  (sender, e) => actions[0].Item2?.Invoke());

            alertDialog.SetButton((int)Android.Content.DialogButtonType.Negative,
                                  cancelText,
                                  (sender, e) => { });

            InvokeOnMainThread(alertDialog.Show);
        }
    }
}
