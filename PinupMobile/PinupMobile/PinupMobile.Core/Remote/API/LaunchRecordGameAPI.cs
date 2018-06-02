using System;
namespace PinupMobile.Core.Remote.API
{
    [Route("function/launchgamerec/{id}")]
    public class LaunchRecordGameRequest
    {
        public int id;
    }

    public class LaunchRecordGameResponse
    {
    }
}
