using System;
using System.Collections.Generic;
using PinupMobile.Core.Remote.API;

namespace PinupMobile.Core.Analytics
{
    public class AppAnalytics : IAppAnalytics
    {
        public static string ViewEvent = "Screen";
        public static string ViewProperty = "Name";

        public static string DisplayEvent = "Display";
        public static string DisplayProperty = "Display";

        public static string PupKeySuccessEvent = "Pup Command Success";
        public static string PupKeyFailedEvent = "Pup Command Failed";
        public static string PupKeyProperty = "Command";

        public static string ServerConnectionEvent = "Pup Connection";
        public static string ServerConnectionProperty = "Success";

        public void TrackEvent(string eventName, IDictionary<string, string> properties = null)
        {
            Microsoft.AppCenter.Analytics.Analytics.TrackEvent(eventName, properties);
        }

        public void TrackView(string view)
        {
            TrackEvent(ViewEvent, new Dictionary<string, string>()
            {
                {
                    ViewProperty, view
                }
            });
        }

        public void TrackServerConnect(bool success = true)
        {
            TrackEvent(ServerConnectionEvent, new Dictionary<string, string>()
            {
                {
                    ServerConnectionProperty, success.ToString()
                }
            });
        }

        public void TrackPupCommand(PopperCommand key, bool success = true)
        {
            TrackEvent(success ? PupKeySuccessEvent : PupKeyFailedEvent, new Dictionary<string, string>()
            {
                {
                    PupKeyProperty, key.ToString()
                }
            });
        }

        public void TrackDisplay(string display)
        {
            string displayStr = string.Empty;
            switch(display)
            {
                case PopperDisplayConstants.POPPER_DISPLAY_BACKGLASS:
                    displayStr = "Backglass";
                    break;
                case PopperDisplayConstants.POPPER_DISPLAY_TOPPER:
                    displayStr = "Topper";
                    break;
                case PopperDisplayConstants.POPPER_DISPLAY_WHEEL:
                    displayStr = "Wheel";
                    break;
                case PopperDisplayConstants.POPPER_DISPLAY_DMD:
                    displayStr = "DMD";
                    break;
                case PopperDisplayConstants.POPPER_DISPLAY_PLAYFIELD:
                    displayStr = "Playfield";
                    break;
                default:
                    displayStr = "Unknown";
                    break;
            }
            TrackEvent(DisplayEvent, new Dictionary<string, string>()
            {
                {
                    DisplayProperty, displayStr
                }
            });
        }
    }
}
