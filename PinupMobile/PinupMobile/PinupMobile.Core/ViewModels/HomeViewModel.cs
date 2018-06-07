﻿using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using PinupMobile.Core.Logging;
using PinupMobile.Core.Remote;
using PinupMobile.Core.Remote.API;
using PinupMobile.Core.Remote.Model;

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

        private Item _currentItem;
        public Item CurrentItem
        {
            get { return _currentItem; }
            set { _currentItem = value; RaisePropertyChanged(() => CurrentItem); }
        }

        private string _wheelIconPath;
        public string WheelIconPath
        {
            get { return _wheelIconPath; }
            set { _wheelIconPath = value; RaisePropertyChanged(() => WheelIconPath); }
        }

        public MvxAsyncCommand OnGameNextCommand => new MvxAsyncCommand(OnGameNext);

        public MvxAsyncCommand OnGamePrevCommand => new MvxAsyncCommand(OnGamePrev);

        public MvxAsyncCommand OnPageNextCommand => new MvxAsyncCommand(OnPageNext);

        public MvxAsyncCommand OnPagePrevCommand => new MvxAsyncCommand(OnPagePrev);

        public MvxAsyncCommand OnPlayCommand => new MvxAsyncCommand(OnPlay);

        public MvxAsyncCommand OnShowDisplayViewCommand => new MvxAsyncCommand(OnDisplayView);

        public MvxAsyncCommand OnRefreshCommand => new MvxAsyncCommand(Refresh);

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

        public async Task OnDisplayView()
        {
            await _navigationService.Navigate<DisplayViewModel>();
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

        public async Task OnPlay()
        {
            await Task.Run(() => ExecuteCommand(_server.SendPlayGame)).ConfigureAwait(false);
        }

        public async Task OnExitEmulator()
        {
            await Task.Run(() => ExecuteCommand(_server.SendExitEmulator)).ConfigureAwait(false);
        }

        public async Task OnHome()
        {
            await Task.Run(() => ExecuteCommand(_server.SendHome)).ConfigureAwait(false);
        }

        private async Task Refresh()
        {
            // TODO First time run to setup Popper Server URL....
            await Task.Run(async () =>
            {
                var itemReq = _server.GetCurrentItem();
                var wheelReq = _server.GetDisplay(PopperDisplayConstants.POPPER_DISPLAY_WHEEL);

                var itemRes = await itemReq;
                var wheelRes = await wheelReq;

                CurrentItem = itemRes;
                WheelIconPath = wheelRes;

            }).ConfigureAwait(false);
        }

        private async Task ExecuteCommand(Func<Task<bool>> command)
        {
            await Task.Run(async () =>
            {
                bool success = await command();

                if (success)
                {
                    // It appears popper needs "some" time to move onto a new game
                    await Task.Delay(500);
                    await Refresh();
                }
            });
        }
    }
}
