using System;
using PinupMobile.Core.Remote.DTO;

namespace PinupMobile.Core.Remote.API
{
    [Route("function/getcuritem")]
    public class GetCurrentItemRequest
    {
    }

    public class GetCurrentItemResponse : CurrentItemDTO
    {
        
    }
}
