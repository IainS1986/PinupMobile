using System;
using MvvmCross;
using PinupMobile.Core.Remote;
using PinupMobile.Core.Remote.Client;

namespace PinupMobile.Core
{
    public static class Setup
    {
        public static void Register()
        {
            Mvx.LazyConstructAndRegisterSingleton<IHttpClientFactory, HttpClientFactory>();
            Mvx.LazyConstructAndRegisterSingleton<IPopperService, PopperService>();
        }
    }
}
