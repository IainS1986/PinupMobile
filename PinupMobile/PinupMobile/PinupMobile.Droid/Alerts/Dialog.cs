using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Widget;
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
            InvokeOnMainThread(() =>
            {
                var alertDialog = new AlertDialog.Builder(_topActivity.Activity).Create();
                alertDialog.SetTitle(title);
                alertDialog.SetMessage(message);
                alertDialog.SetButton((int)Android.Content.DialogButtonType.Positive,
                                      cancelText,
                                      (sender, e) => buttonAction?.Invoke());

                alertDialog.Show();
            });
        }

        public void Show(string title, string message, string cancelText, List<(string, Action)> actions)
        {
            InvokeOnMainThread(() =>
            {
                // TODO: this doesn't properly support multiple actions - needs custom layout
                var alertDialog = new AlertDialog.Builder(_topActivity.Activity);//.Create();
                alertDialog.SetTitle(message);
                //alertDialog.SetMessage(message); //Thanks android...if you SetMEssage it will block SetItems...
                alertDialog.SetNegativeButton(cancelText, (sender, e) => { });
                alertDialog.SetItems(actions.Select(x => x.Item1).ToArray(),
                                     (object sender, Android.Content.DialogClickEventArgs e) =>
                {
                    actions[e.Which].Item2?.Invoke();
                });

                var dialog = alertDialog.Create();
                dialog.Show();
            });
        }
    }
}
