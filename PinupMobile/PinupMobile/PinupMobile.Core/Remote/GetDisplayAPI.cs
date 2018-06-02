using System;
namespace PinupMobile.Core.Remote
{
    [Route("function/getdisplay/{display}")]
    public class GetDisplayRequest
    {
        public int display; //0-top, 1-dmd, 2-BG, 3-playfield
    }

    //PNG or MP4
    public class GetDisplayResponse
    {
    }
}
