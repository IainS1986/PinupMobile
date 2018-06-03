using System;
using System.Threading.Tasks;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using PinupMobile.Core.Logging;
using PinupMobile.Core.Remote;

namespace PinupMobile.Core.ViewModels
{
    /// <summary>
    /// Main home screen for pinup remote
    /// Shows current playing item
    /// </summary>
    public class HomeViewModel
        : MvxViewModel
    {
        private readonly IPopperService _server;
        private readonly IMvxNavigationService _navigationService;

        private string _currentItemName;
        public string CurrentItemName
        {
            get { return _currentItemName; }
            set { _currentItemName = value; RaisePropertyChanged(() => CurrentItemName); }
        }

        public HomeViewModel(IPopperService server,
                             IMvxNavigationService navigationService)
        {
            _server = server;
            _navigationService = navigationService;
        }

        public override async Task Initialize()
        {
            await base.Initialize();

            // TODO First time run to setup Popper Server URL....
            await Task.Run(async () =>
            {
                var currentItem = await _server.GetCurrentItem();

                if (currentItem != null)
                {
                    CurrentItemName = currentItem.DisplayName;
                }
                else
                {
                    CurrentItemName = "No Popper Server running";
                }

            }).ConfigureAwait(false);
        }
    }
}
