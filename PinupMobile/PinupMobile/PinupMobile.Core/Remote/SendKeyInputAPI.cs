using System;
namespace PinupMobile.Core.Remote
{
    [Route("pupkey/{keyCode}")]
    public class SendKeyInputRequest
    {
        public string keyCode;
    }

    public class SendKeyInputResponse
    {
    }
}
