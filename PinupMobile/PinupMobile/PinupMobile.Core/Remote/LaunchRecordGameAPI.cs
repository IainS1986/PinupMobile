using System;
namespace PinupMobile.Core.Remote
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
