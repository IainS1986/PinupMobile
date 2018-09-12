using System;
using MvvmCross;
using PinupMobile.Core.Analytics;
using PinupMobile.Core.Network;
using PinupMobile.Core.Remote;
using PinupMobile.Core.Remote.Client;

namespace PinupMobile.Core
{
    public static class Setup
    {
        public static void Register()
        {
            Mvx.LazyConstructAndRegisterSingleton<IAppAnalytics, AppAnalytics>();
            Mvx.LazyConstructAndRegisterSingleton<IHttpClientFactory, HttpClientFactory>();
            Mvx.LazyConstructAndRegisterSingleton<IApi, Api>();
            Mvx.LazyConstructAndRegisterSingleton<IPopperService, PopperService>();
        }
    }
}
