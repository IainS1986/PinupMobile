using System;
namespace PinupMobile.Core.Remote.API
{
    [Route("function/launchgame/{id}")]
    public class LaunchGameRequest
    {
        public int id;
    }
}
