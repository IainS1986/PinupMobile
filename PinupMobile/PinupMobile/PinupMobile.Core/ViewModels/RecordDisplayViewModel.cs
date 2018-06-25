using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using PinupMobile.Core.Alerts;
using PinupMobile.Core.Remote;
using PinupMobile.Core.Remote.API;
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

            HelpMessage = "Hit record to start";
        }

        private async Task OnRecord()
        {
            if (!Recording)
            {
                Time = 0;
                _dialogService.Show("Record Dispay",
                                    "Which display do you want to record?",
                                    "Cancel",
                                    new List<(string, Action)>()
                {
                    ("Playfield", async () => await OnRecordStart(PopperDisplayConstants.POPPER_DISPLAY_PLAYFIELD)),
                    ("Backglass", async () => await OnRecordStart(PopperDisplayConstants.POPPER_DISPLAY_BACKGLASS)),
                    ("DMD", async () => await OnRecordStart(PopperDisplayConstants.POPPER_DISPLAY_DMD)),
                    ("Topper", async () => await OnRecordStart(PopperDisplayConstants.POPPER_DISPLAY_TOPPER)),
                });
            }
            else
            {
                HelpMessage = "Recording Stopped";
                Recording = false;
                _timer.Stop();
                await _server.SendRecordDisplay(_currentDisplay);
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
                await _server.SendRecordDisplay(_currentDisplay);
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
                HelpMessage = "Recording Failed";
                return;
            }

            //Start timer
            _timer.Start();
            _currentDisplay = display;

            HelpMessage = $"Recording {GetDisplayName(display)}";
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
                    return "Backglass";
                case PopperDisplayConstants.POPPER_DISPLAY_TOPPER:
                    return "Topper";
                case PopperDisplayConstants.POPPER_DISPLAY_PLAYFIELD:
                    return "Playfield";
                case PopperDisplayConstants.POPPER_DISPLAY_DMD:
                    return "DMD";
                default:
                    return "Unknown";
            }
        }
    }
}
