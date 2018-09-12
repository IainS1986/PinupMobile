using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using PinupMobile.Core.Alerts;
using PinupMobile.Core.Logging;
using PinupMobile.Core.Remote;
using PinupMobile.Core.Remote.API;
using PinupMobile.Core.Remote.Model;
using PinupMobile.Core.Settings;
using PinupMobile.Core.Strings;

namespace PinupMobile.Core.ViewModels
{
    /// <summary>
    /// Main home screen for pinup remote
    /// Shows current playing item
    /// </summary>
    public class HomeViewModel
        : BaseViewModel
    {
        private const string TITLE_VISIBLE_KEY = "TitleVisible";
        private const int REFRESH_RATE_SECONDS = 5;//seconds

        private readonly IPopperService _server;
        private readonly IMvxNavigationService _navigationService;
        private readonly IUserSettings _userSettings;
        private readonly IDialog _dialogService;

        private Timer _refreshTimer;

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

        private bool _titleVisible;
        public bool TitleHidden
        {
            get { return _titleVisible; }
            set { _titleVisible = value; RaisePropertyChanged(() => TitleHidden);  }
        }

        public MvxAsyncCommand OnGameNextCommand => new MvxAsyncCommand(OnGameNext);

        public MvxAsyncCommand OnGamePrevCommand => new MvxAsyncCommand(OnGamePrev);

        public MvxAsyncCommand OnPageNextCommand => new MvxAsyncCommand(OnPageNext);

        public MvxAsyncCommand OnPagePrevCommand => new MvxAsyncCommand(OnPagePrev);

        public MvxAsyncCommand OnPlayCommand => new MvxAsyncCommand(OnPlay);

        public MvxAsyncCommand OnSelectCommand => new MvxAsyncCommand(OnSelect);

        public MvxAsyncCommand OnMenuReturnCommand => new MvxAsyncCommand(OnMenuReturn);

        public MvxAsyncCommand OnSystemMenuCommand => new MvxAsyncCommand(OnSystemMenu);

        public MvxAsyncCommand OnHomeCommand => new MvxAsyncCommand(OnHome);

        public MvxAsyncCommand OnExitEmulatorCommand => new MvxAsyncCommand(OnExitEmulator);

        public MvxAsyncCommand OnShowDisplayViewCommand => new MvxAsyncCommand(OnDisplayView);

        public MvxAsyncCommand OnRefreshCommand => new MvxAsyncCommand(Refresh);

        public MvxAsyncCommand OnRecordCommand => new MvxAsyncCommand(OnRecordMenu);

        public MvxAsyncCommand OnGameStartCommand => new MvxAsyncCommand(OnGameStart);

        public MvxAsyncCommand OnRecordStartCommand => new MvxAsyncCommand(OnRecordStart);

        public MvxCommand OnTitleTappedCommand => new MvxCommand(OnTitleTapped);

        public HomeViewModel(IPopperService server,
                             IMvxNavigationService navigationService,
                             IUserSettings userSettings,
                             IDialog dialogService)
        {
            _server = server;
            _navigationService = navigationService;
            _userSettings = userSettings;
            _dialogService = dialogService;

            _refreshTimer = new Timer(REFRESH_RATE_SECONDS * 1000);
            _refreshTimer.Elapsed += OnRefreshTimerElapsed;
            _refreshTimer.AutoReset = true;
        }

        public override async void ViewAppearing()
        {
            base.ViewAppearing();

            TitleHidden = _userSettings.GetBool(TITLE_VISIBLE_KEY);

            await Refresh();

            _refreshTimer.Start();
        }

        public override void ViewDisappearing()
        {
            base.ViewDisappearing();

            _refreshTimer.Stop();
        }

        public async Task OnDisplayView()
        {
            _dialogService.Show(Translation.alert_display_title, Translation.alert_display_body, Translation.general_cancel, new List<(string, Action)>
            {
                (Translation.general_display_playfield, async () => await _navigationService.Navigate<string>(typeof(DisplayViewModel), PopperDisplayConstants.POPPER_DISPLAY_PLAYFIELD)),
                (Translation.general_display_backglass, async () => await _navigationService.Navigate<string>(typeof(DisplayViewModel), PopperDisplayConstants.POPPER_DISPLAY_BACKGLASS)),
                (Translation.general_display_dmd, async () => await _navigationService.Navigate<string>(typeof(DisplayViewModel), PopperDisplayConstants.POPPER_DISPLAY_DMD)),
                (Translation.general_display_topper, async () => await _navigationService.Navigate<string>(typeof(DisplayViewModel), PopperDisplayConstants.POPPER_DISPLAY_TOPPER)),
            });

            await Task.CompletedTask;
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

        public async Task OnExitEmulator()
        {
            await Task.Run(() => ExecuteCommand(_server.SendExitEmulator)).ConfigureAwait(false);
        }

        public async Task OnHome()
        {
            await Task.Run(() => ExecuteCommand(_server.SendHome)).ConfigureAwait(false);
        }

        public async Task OnSelect()
        {
            await Task.Run(() => ExecuteCommand(_server.SendSelect)).ConfigureAwait(false);
        }

        public async Task OnMenuReturn()
        {
            await Task.Run(() => ExecuteCommand(_server.SendMenuReturn)).ConfigureAwait(false);
        }

        public async Task OnSystemMenu()
        {
            // Show dialog asking if we want to , Quit, Shutdown, Reboot or Cancel
            _dialogService.Show(Translation.alert_display_power_title, Translation.alert_display_power_body, Translation.general_cancel, new List<(string, Action)>
            {
                (Translation.alert_display_power_option_shut_down, async () => await OnShutdown()),
                (Translation.alert_display_power_option_reboot, async () => await OnRestart()),
                (Translation.alert_display_power_option_exit, async () => await OnExitPopper()),
            });

            await Task.CompletedTask;
        }

        public async Task OnRestart()
        {
            await Task.Run(() => ExecuteCommand(_server.SendRestart)).ConfigureAwait(false);
        }

        public async Task OnShutdown()
        {
            await Task.Run(() => ExecuteCommand(_server.SendShutdown)).ConfigureAwait(false);
        }

        public async Task OnExitPopper()
        {
            await Task.Run(() => ExecuteCommand(_server.SendExitPopper)).ConfigureAwait(false);
        }

        public async Task OnGameStart()
        {
            await Task.Run(() => ExecuteCommand(_server.SendGameStart)).ConfigureAwait(false);
        }

        public async Task OnRecordStart()
        {
            await Task.Run(async () =>
            {

                bool success = await _server.SendRecordGame(CurrentItem.GameID);

                if (success)
                {
                    // It appears popper needs "some" time to move onto a new game
                    await Task.Delay(500);
                    await Refresh();
                }
            }).ConfigureAwait(false);
        }

        public async Task OnRecordMenu()
        {
            _dialogService.Show(Translation.alert_record_menu_title, Translation.alert_record_menu_body, Translation.general_cancel, new List<(string, Action)>
            {
                (Translation.alert_record_menu_option_running, async () => await _navigationService.Navigate<RecordDisplayViewModel>()),
                (Translation.alert_record_menu_option_launch_record, async () => { await OnRecordStart(); await _navigationService.Navigate<RecordDisplayViewModel>(); }),
                (Translation.alert_record_menu_option_launch_normal, async () => { await OnGameStart(); await _navigationService.Navigate<RecordDisplayViewModel>(); }),
            });

            await Task.CompletedTask;
        }

        private async Task Refresh()
        {
            await Task.Run(async () =>
            {
                var itemReq = _server.GetCurrentItem();
                var itemRes = await itemReq;

                // Check if things have actually changed before going off to get more display details
                if (!string.Equals(itemRes.DisplayName, CurrentItem?.DisplayName, StringComparison.CurrentCultureIgnoreCase))
                {
                    var wheelRes = await _server.GetDisplay(PopperDisplayConstants.POPPER_DISPLAY_WHEEL);
                    
                    CurrentItem = itemRes;
                    WheelIconPath = wheelRes.MediaUrl;
                }

            }).ConfigureAwait(false);
        }

        public async Task OnPlay()
        {
            await Task.Run(async () =>
            {
                bool success = await _server.SendPlayGame(CurrentItem.GameID);

                if (success)
                {
                    // It appears popper needs "some" time to move onto a new game
                    await Task.Delay(500);
                    await Refresh();
                }
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

        private void OnTitleTapped()
        {
            TitleHidden = !TitleHidden;

            _userSettings.SetBool(TITLE_VISIBLE_KEY, TitleHidden);
        }

        private void OnRefreshTimerElapsed(object sender, ElapsedEventArgs eventArgs)
        {
            Task.Run(async () => await Refresh()).ConfigureAwait(false);
        }
    }
}
