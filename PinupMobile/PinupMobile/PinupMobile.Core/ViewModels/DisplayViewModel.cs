﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using PinupMobile.Core.Analytics;
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
    public class DisplayViewModel
        : BaseViewModel<string>
    {
        private readonly IPopperService _server;
        private readonly IMvxNavigationService _navigationService;

        private string _display = PopperDisplayConstants.POPPER_DISPLAY_PLAYFIELD;

        public MvxAsyncCommand CloseCommand => new MvxAsyncCommand(async () => await _navigationService.Close(this));

        private string _mediaUrl;
        public string MediaUrl
        {
            get { return _mediaUrl; }
            set { _mediaUrl = value; RaisePropertyChanged(() => MediaUrl); }
        }

        public DisplayViewModel(IPopperService server,
                             IMvxNavigationService navigationService)
        {
            _server = server;
            _navigationService = navigationService;
        }

        public override void Prepare(string parameter)
        {
            _display = parameter;

            Analytics.TrackDisplay(_display);
        }

        public override async void ViewAppearing()
        {
            base.ViewAppearing();

            MediaUrl = string.Empty;

            // Download the Playfield Display
            await Task.Run(async () =>
            {
                MediaUrl = await _server.GetDisplay(_display);
            }).ConfigureAwait(false);
        }
    }
}
