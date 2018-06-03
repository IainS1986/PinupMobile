using System;
namespace PinupMobile.Core.Remote.API
{
    [Route("pupkey/{command}")]
    public class SendKeyInputRequest
    {
        public PopperCommand command;
    }
}
