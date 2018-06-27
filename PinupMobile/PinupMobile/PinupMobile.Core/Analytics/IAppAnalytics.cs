using System;
using System.Collections.Generic;
using PinupMobile.Core.Remote.API;

namespace PinupMobile.Core.Analytics
{
    public interface IAppAnalytics
    {
        void TrackView(string view);

        void TrackEvent(string eventName, IDictionary<string, string> properties = null);

        void TrackPupCommand(PopperCommand key, bool success = true);

        void TrackDisplay(string display);

        void TrackStartRecordDisplay(string display);

        void TrackStopRecordDisplay(string display, int durationInSeconds);

        void TrackServerConnect(bool success = true);

    }
}
