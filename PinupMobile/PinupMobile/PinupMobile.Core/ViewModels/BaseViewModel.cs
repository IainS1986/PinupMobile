using System;
using System.Text.RegularExpressions;
using MvvmCross;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using PinupMobile.Core.Analytics;

namespace PinupMobile.Core.ViewModels
{
    public abstract class BaseViewModel<TParameter> : BaseViewModel, IMvxViewModel<TParameter>, IMvxViewModel
    {
        public abstract void Prepare(TParameter parameter);
    }

    public abstract class BaseViewModel : MvxViewModel
    {
        private readonly IAppAnalytics _analytics;

        public IAppAnalytics Analytics => _analytics;

        protected virtual string NavigationAnalyticsPageName => Regex.Replace(GetType().Name, "ViewModel", string.Empty, RegexOptions.IgnoreCase).ToLowerInvariant();
       
        protected BaseViewModel()
        {
            _analytics = Mvx.Resolve<IAppAnalytics>();
        }

        public override void ViewAppeared()
        {
            base.ViewAppeared();
            _analytics.TrackView(NavigationAnalyticsPageName);
        }
    }
}
