using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using PinupMobile.Core.Alerts;
using PinupMobile.Core.Remote;
using PinupMobile.Core.Remote.API;
using PinupMobile.Core.Strings;
using PinupMobile.Core.Timers;

namespace PinupMobile.Core.ViewModels
{
    public class RecordDisplayViewModel
        : BaseViewModel
    {
        private readonly IPopperService _server;
        private readonly IMvxNavigationService _navigationService;
        private readonly IDialog _dialogService;

        private Timer _timer;
        private string _currentDisplay;

        public MvxAsyncCommand CloseCommand => new MvxAsyncCommand(async () => await _navigationService.Close(this));
        public MvxAsyncCommand RecordCommand => new MvxAsyncCommand(async () => await OnRecord());

        private bool _isRecording;
        public bool Recording
        {
            get { return _isRecording; }
            set { _isRecording = value; RaisePropertyChanged(() => Recording); }
        }

        private int _time;
        public int Time
        {
            get { return _time; }
            set { _time = value; RaisePropertyChanged(() => Time); }
        }

        private string _helpMessage;
        public string HelpMessage
        {
            get { return _helpMessage; }
            set { _helpMessage = value; RaisePropertyChanged(() => HelpMessage); }
        }

        public RecordDisplayViewModel(IPopperService server,
                                      IMvxNavigationService navigationService,
                                      IDialog dialog)
        {
            _server = server;
            _navigationService = navigationService;
            _dialogService = dialog;

            _timer = new Timer();
            _timer.TimeElapsed += TimeElapsed;

            HelpMessage = Translation.menu_record_help_intro;
        }

        private async Task OnRecord()
        {
            if (!Recording)
            {
                Time = 0;
                _dialogService.Show(Translation.alert_record_display_title,
                                    Translation.alert_record_display_body,
                                    Translation.general_cancel,
                                    new List<(string, Action)>()
                {
                    (Translation.general_display_playfield, async () => await OnRecordStart(PopperDisplayConstants.POPPER_DISPLAY_PLAYFIELD)),
                    (Translation.general_display_backglass, async () => await OnRecordStart(PopperDisplayConstants.POPPER_DISPLAY_BACKGLASS)),
                    (Translation.general_display_dmd, async () => await OnRecordStart(PopperDisplayConstants.POPPER_DISPLAY_DMD)),
                    (Translation.general_display_topper, async () => await OnRecordStart(PopperDisplayConstants.POPPER_DISPLAY_TOPPER)),
                });
            }
            else
            {
                HelpMessage = Translation.menu_record_help_stopped;
                Recording = false;
                _timer.Stop();
                await _server.SendRecordDisplay(_currentDisplay);
                Analytics.TrackStopRecordDisplay(_currentDisplay, Time);
            }
        }

        public override async void ViewDestroy(bool viewFinishing = true)
        {
            base.ViewDestroy(viewFinishing);

            if (viewFinishing
                && Recording
                && !string.IsNullOrEmpty(_currentDisplay))
            {
                //Kill/Stop the current recording
                _timer.Stop();
                await _server.SendRecordDisplay(_currentDisplay);
                Analytics.TrackStopRecordDisplay(_currentDisplay, Time);
            }
        }

        private async Task OnRecordStart(string display)
        {
            //Bit naff this but go with it for now
            Recording = true;

            //Trigger record on popper 
            bool success = await _server.SendRecordDisplay(display);

            if (!success)
            {
                Recording = false;
                HelpMessage = Translation.menu_record_help_failed;
                Analytics.TrackStartRecordDisplay(display);
                return;
            }

            //Start timer
            _timer.Start();
            _currentDisplay = display;

            HelpMessage = string.Format(Translation.menu_record_help_in_progress, GetDisplayName(display));
        }

        private void TimeElapsed(object sender, int e)
        {
            if (Recording)
            {
                Time = e;
            }
        }

        private string GetDisplayName(string display)
        {
            switch(display)
            {
                case PopperDisplayConstants.POPPER_DISPLAY_BACKGLASS:
                    return Translation.general_display_backglass;
                case PopperDisplayConstants.POPPER_DISPLAY_TOPPER:
                    return Translation.general_display_topper;
                case PopperDisplayConstants.POPPER_DISPLAY_PLAYFIELD:
                    return Translation.general_display_playfield;
                case PopperDisplayConstants.POPPER_DISPLAY_DMD:
                    return Translation.general_display_dmd;
                default:
                    return Translation.general_display_unknown;
            }
        }
    }
}
