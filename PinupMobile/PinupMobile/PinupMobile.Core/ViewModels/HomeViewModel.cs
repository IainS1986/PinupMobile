using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
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

        public MvxAsyncCommand OnGameNextCommand => new MvxAsyncCommand(OnGameNext);

        public MvxAsyncCommand OnGamePrevCommand => new MvxAsyncCommand(OnGamePrev);

        public HomeViewModel(IPopperService server,
                             IMvxNavigationService navigationService)
        {
            _server = server;
            _navigationService = navigationService;
        }

        public override async void ViewAppearing()
        {
            base.ViewAppearing();

            await Refresh();
        }

        public async Task OnGameNext()
        {
            await Task.Run(async () =>
            {
                bool success = await _server.SendGameNext();

                if(success)
                {
                    await Task.Delay(500);
                    await Refresh();
                }
            }).ConfigureAwait(false);
        }

        public async Task OnGamePrev()
        {
            await Task.Run(async () =>
            {
                bool success = await _server.SendGamePrev();

                if (success)
                {
                    await Task.Delay(500);
                    await Refresh();
                }
            }).ConfigureAwait(false);
        }

        private async Task Refresh()
        {
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
