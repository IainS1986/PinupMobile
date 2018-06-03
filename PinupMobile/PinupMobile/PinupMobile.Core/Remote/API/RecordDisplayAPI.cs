﻿using System;
namespace PinupMobile.Core.Remote.API
{
    [Route("function/record/{display}")]
    public class RecordDisplayRequest
    {
        public int display; //0-top, 1-dmd, 2-BG, 3-playfield
    }
}