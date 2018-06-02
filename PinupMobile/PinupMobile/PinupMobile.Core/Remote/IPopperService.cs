using System;
using System.Threading.Tasks;
using PinupMobile.Core.Remote.Model;

namespace PinupMobile.Core.Remote
{
    public interface IPopperService
    {
        Task<CurrentItem> GetCurrentItem();
    }
}
