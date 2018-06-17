using System;
using System.Collections.Generic;

namespace PinupMobile.Core.Alerts
{
    public interface IDialog
    {
        void Show(string title, string message, string cancelText, Action buttonAction = null);

        void Show(string title, string message, string cancelText, List<(string, Action)> actions);
    }
}
