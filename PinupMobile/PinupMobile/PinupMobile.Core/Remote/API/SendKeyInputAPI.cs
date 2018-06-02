using System;
namespace PinupMobile.Core.Remote.API
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
