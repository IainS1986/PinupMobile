using System;
using MvvmCross;
using PinupMobile.Core.Remote;

namespace PinupMobile.Core
{
    public static class Setup
    {
        public static void Register()
        {
            Mvx.RegisterType<IPopperService, PopperService>();
        }
    }
}
