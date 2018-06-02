using System;
namespace PinupMobile.Core.Remote
{
    [Route("function/launchgame/{id}")]
    public class LaunchGameRequest
    {
        public int id;
    }

    public class LaunchGameResponse
    {
    }
}
