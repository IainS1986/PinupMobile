using System;
using System.Collections.Generic;
using PinupMobile.Core.Remote.API;

namespace PinupMobile.Core.Analytics
{
    public class AppAnalytics : IAppAnalytics
    {
        public static string ViewEvent = "Screen";
        public static string ViewProperty = "Name";

        public static string DisplayEvent = "View Display";
        public static string DisplayProperty = "Display";

        public static string StartRecordDisplayEvent = "Record Display (Start)";
        public static string EndRecordDisplayEvent = "Record Display (End)";

        public static string RecordDurationProperty = "Duration";

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
            TrackEvent(DisplayEvent, new Dictionary<string, string>()
            {
                {
                    DisplayProperty, AnalyticsDisplayString(display)
                }
            });
        }

        public void TrackStartRecordDisplay(string display)
        {
            TrackEvent(StartRecordDisplayEvent, new Dictionary<string, string>()
            {
                {
                    DisplayProperty, AnalyticsDisplayString(display)
                }
            }); 
        }

        public void TrackStopRecordDisplay(string display, int seconds)
        {
            TrackEvent(EndRecordDisplayEvent, new Dictionary<string, string>()
            {
                {
                    DisplayProperty, AnalyticsDisplayString(display)
                },
                {
                    RecordDurationProperty, seconds.ToString()
                }
            });
        }

        private string AnalyticsDisplayString(string display)
        {
            switch (display)
            {
                case PopperDisplayConstants.POPPER_DISPLAY_BACKGLASS:
                    return "Backglass";
                case PopperDisplayConstants.POPPER_DISPLAY_TOPPER:
                    return "Topper";
                case PopperDisplayConstants.POPPER_DISPLAY_WHEEL:
                    return "Wheel";
                case PopperDisplayConstants.POPPER_DISPLAY_DMD:
                    return "DMD";
                case PopperDisplayConstants.POPPER_DISPLAY_PLAYFIELD:
                    return "Playfield";
                default:
                    return "Unknown";
            }
        }
    }
}
