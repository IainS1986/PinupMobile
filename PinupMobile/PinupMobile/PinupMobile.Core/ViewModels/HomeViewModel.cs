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

        public MvxAsyncCommand OnPageNextCommand => new MvxAsyncCommand(OnPageNext);

        public MvxAsyncCommand OnPagePrevCommand => new MvxAsyncCommand(OnPagePrev);

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

        private async Task ExecuteCommand(Func<Task<bool>> command)
        {
            await Task.Run(async () =>
            {
                bool success = await command();

                if (success)
                {
                    await Task.Delay(500);
                    await Refresh();
                }
            });
        }

        public async Task OnGameNext()
        {
            await Task.Run(() => ExecuteCommand(_server.SendGameNext)).ConfigureAwait(false);
        }

        public async Task OnGamePrev()
        {
            await Task.Run(() => ExecuteCommand(_server.SendGamePrev)).ConfigureAwait(false);
        }

        public async Task OnPageNext()
        {
            await Task.Run(() => ExecuteCommand(_server.SendPageNext)).ConfigureAwait(false);
        }

        public async Task OnPagePrev()
        {
            await Task.Run(() => ExecuteCommand(_server.SendPagePrev)).ConfigureAwait(false);
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
