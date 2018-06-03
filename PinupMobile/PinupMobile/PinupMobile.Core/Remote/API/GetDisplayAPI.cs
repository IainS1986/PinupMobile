using System;
namespace PinupMobile.Core.Remote.API
{
    [Route("function/getdisplay/{display}")]
    public class GetDisplayRequest
    {
        public string display;
    }

    //PNG or MP4
    public class GetDisplayResponse
    {
    }
}
