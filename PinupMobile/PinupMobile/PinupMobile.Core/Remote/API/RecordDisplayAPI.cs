using System;
namespace PinupMobile.Core.Remote.API
{
    [Route("function/record/{display}")]
    public class RecordDisplayRequest
    {
        public string display; //0-top, 1-dmd, 2-BG, 3-playfield
    }
}
