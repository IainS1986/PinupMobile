using System;
using CoreGraphics;
using PinupMobile.Core.Alerts;
using UIKit;

namespace PinupMobile.iOS.Alerts
{
    public class Dialog : IDialog
    {
        public void Show(string title, string message, string cancelText, Action buttonAction = null)
        {
            var alert = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);
            alert.AddAction(UIAlertAction.Create(cancelText, UIAlertActionStyle.Default, (action) => buttonAction?.Invoke()));

            PresentAlert(alert);
        }

        public void Show(string title, string message, string cancelText, System.Collections.Generic.List<(string, Action)> actions)
        {
            var alert = UIAlertController.Create(title, message, UIAlertControllerStyle.ActionSheet);

            foreach (var alertAction in actions)
            {
                alert.AddAction(UIAlertAction.Create(alertAction.Item1, UIAlertActionStyle.Default, (action) => alertAction.Item2?.Invoke()));
            }

            alert.AddAction(UIAlertAction.Create(cancelText, UIAlertActionStyle.Cancel, null));

            var viewController = FindViewControllerForPresentation();

            if (alert.PopoverPresentationController != null)
            {
                alert.PopoverPresentationController.SourceView = viewController.View;
                alert.PopoverPresentationController.SourceRect = new CGRect(viewController.View.Bounds.GetMidX(), viewController.View.Bounds.GetMidY(), 0, 0);
                alert.PopoverPresentationController.PermittedArrowDirections = 0;
            }

            viewController.PresentViewController(alert, true, null);
        }

        private static UIViewController FindViewControllerForPresentation()
        {
            var viewController = UIApplication.SharedApplication.KeyWindow.RootViewController;

            while (viewController.PresentedViewController != null)
            {
                viewController = viewController.PresentedViewController;
            }

            return viewController;
        }

        private void PresentAlert(UIAlertController alert)
        {
            var viewController = FindViewControllerForPresentation();
            viewController.PresentViewController(alert, true, null);
        }
    }
}
