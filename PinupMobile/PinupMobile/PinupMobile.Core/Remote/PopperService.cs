using System;
using System.IO;
using System.Threading.Tasks;
using PinupMobile.Core.Analytics;
using PinupMobile.Core.Network;
using PinupMobile.Core.Logging;
using PinupMobile.Core.Remote.API;
using PinupMobile.Core.Remote.Model;
using PinupMobile.Core.Remote.Model.Mock;
using PinupMobile.Core.Settings;

namespace PinupMobile.Core.Remote
{
    public class PopperService : IPopperService
    {
        private const string DebugPopperURL = "http://debug/";

        private readonly IAppAnalytics _analytics;
        private readonly IApi _api;

        public bool IsDebugMode
        {
            get { return _api.BaseUri.AbsoluteUri.Equals(DebugPopperURL, StringComparison.CurrentCultureIgnoreCase); }
        }

        public PopperService(IAppAnalytics analytics,
                             IApi api)
        {
            _analytics = analytics;
            _api = api;
        }

        public async Task<bool> ServerExists()
        {
            try
            {
                var request = new GetCurrentItemRequest();
                var response = await _api.MakeRequest<GetCurrentItemRequest, GetCurrentItemResponse>(request).ConfigureAwait(false);

                if (response?.Success == true)
                {
                    _analytics.TrackServerConnect(true);
                    return true;
                }
                else
                {
                    _analytics.TrackServerConnect(false);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _analytics.TrackServerConnect(false);
                Logger.Error($"Error pinging {_api.BaseUri} - {ex.Message}", ex);
                return false;
            }
        }

        public async Task<bool> ServerExistsWithUrl(string url)
        {
            _api.BaseUri = new Uri(url);

            bool connected = await ServerExists();

            if (connected && !IsDebugMode)
            {
                _api.SaveUrl();
            }

            return connected;
        }

        public async Task<DisplayResponse> GetDisplay(string display)
        {
            DisplayResponse result = new DisplayResponse();
            result.Success = true;
            if (IsDebugMode)
            {
                Logger.Debug($"Sent popper request for {display} feed");

                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                string localFilename = "test.m4v";
                //Slight hacky fudge
                if (display.Equals("Wheel", StringComparison.CurrentCultureIgnoreCase))
                {
                    localFilename = "test.png";
                }

                result.MediaUrl = localFilename;
            }
            else
            {

                GetDisplayRequest request = new GetDisplayRequest();
                request.display = display;

                var response = await _api.MakeRequest<GetDisplayRequest, string>(request).ConfigureAwait(false);

                if (response.Success == true)
                {
                    try
                    {
                        if (string.Equals(response.Data, "no image found", StringComparison.CurrentCultureIgnoreCase))
                        {
                            Logger.Error($"Error fetching display {display}, no image found returned from Popper");
                            //TODO Change to an actual response object instead of string
                            result.Success = false;
                            result.Error = string.Format($"Error - No image found");
                        }
                        else
                        {
                            // Check Response data to determine what format to save the byte[] too
                            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                            string localFilename = "test.txt";
                            if (response.Data.Contains("video") ||
                                response.Data.Contains("image"))
                            {
                                string format = response.Data.Split('/')[1];
                                localFilename = $"test.{format}";
                            }

                            // Save 
                            string localPath = Path.Combine(documentsPath, localFilename);
                            File.WriteAllBytes(localPath, response.Raw);

                            result.MediaUrl = localPath;
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Error($"Error saving off display data for playback - {ex.Message}", ex);
                        result.Success = false;
                        result.Error = $"Error saving off display data for playback - {ex.Message}";
                    }
                }
                else
                {
                    Logger.Error($"Error requesting Display {display}, responded with {response?.Code} and {response?.Messsage}");
                    result.Success = false;
                    result.Error = $"Error requesting Display { display}, responded with { response?.Code} and { response?.Messsage}";
                }
            }

            return result;
        }

        public async Task<Item> GetCurrentItem()
        {
            if (IsDebugMode)
            {
                return new MockItem();
            }

            // Popper has no endpoint to just get state, or any authentication to do a keep alive
            // so instead, make a call to "Get current item" which should respond but not alter
            // anything on popper
            var request = new GetCurrentItemRequest();
            var response = await _api.MakeRequest<GetCurrentItemRequest, GetCurrentItemResponse>(request).ConfigureAwait(false);

            if (response?.Success == true &&
                response?.Data != null)
            {
                return new Item(response.Data);
            }
            else
            {
                //TODO Error handling???
                Logger.Error($"Error requesting GetCurrentItem, responded with {response?.Code} and {response?.Messsage}");
                return null;
            }
        }

        public async Task<bool> SendPlayGame(int gameid)
        {
            if (IsDebugMode)
            {
                Logger.Debug($"Sent popper command to start playing game with id {gameid}");
                return true;
            }

            var request = new LaunchGameRequest();
            request.id = gameid;

            var response = await _api.MakeRequest<LaunchGameRequest, string>(request).ConfigureAwait(false);

            if (response?.Success == true)
            {
                return true;
            }
            else
            {
                //TODO Error handling???
                Logger.Error($"Error launching game {gameid}, responded with {response?.Code} and {response?.Messsage}");
                return false;
            }
        }

        public async Task<bool> SendRecordGame(int gameid)
        {
            if (IsDebugMode)
            {
                Logger.Debug($"Sent popper command to launch game with id {gameid} in record mode");
                return true;
            }

            var request = new LaunchRecordGameRequest();
            request.id = gameid;

            var response = await _api.MakeRequest<LaunchRecordGameRequest, string>(request).ConfigureAwait(false);

            if (response?.Success == true)
            {
                return true;
            }
            else
            {
                //TODO Error handling???
                Logger.Error($"Error launching game {gameid} in record mode, responded with {response?.Code} and {response?.Messsage}");
                return false;
            }
        }

        public async Task<bool> SendGameNext()
        {
            return await SendPupKey(PopperCommand.KEY_NEXT_GAME);
        }

        public async Task<bool> SendGamePrev()
        {
            return await SendPupKey(PopperCommand.KEY_PREV_GAME);
        }

        public async Task<bool> SendPagePrev()
        {
            return await SendPupKey(PopperCommand.KEY_PREV_PAGE);
        }

        public async Task<bool> SendPageNext()
        {
            return await SendPupKey(PopperCommand.KEY_NEXT_PAGE);
        }

        public async Task<bool> SendExitEmulator()
        {
            return await SendPupKey(PopperCommand.KEY_EXIT_EMULATOR);
        }

        public async Task<bool> SendHome()
        {
            return await SendPupKey(PopperCommand.KEY_HOME);
        }

        public async Task<bool> SendSelect()
        {
            return await SendPupKey(PopperCommand.KEY_SELECT);
        }

        public async Task<bool> SendMenuReturn()
        {
            return await SendPupKey(PopperCommand.KEY_MENU_RETURN);
        }

        public async Task<bool> SendSystemMenu()
        {
            return await SendPupKey(PopperCommand.KEY_SYSTEM_MENU);
        }

        public async Task<bool> SendShutdown()
        {
            return await SendPupKey(PopperCommand.KEY_SHUTDOWN_PC);
        }

        public async Task<bool> SendRestart()
        {
            return await SendPupKey(PopperCommand.KEY_RESTART_PC);
        }

        public async Task<bool> SendExitPopper()
        {
            return await SendPupKey(PopperCommand.KEY_SYSTEM_EXIT);
        }

        public async Task<bool> SendGameStart()
        {
            return await SendPupKey(PopperCommand.KEY_GAME_START);
        }

        public async Task<bool> SendRecordStart()
        {
            return await SendPupKey(PopperCommand.KEY_START_RECORD);
        }

        /// <summary>
        /// Send pup the given key.
        /// </summary>
        /// <returns>The pup key.</returns>
        /// <param name="keycode">Keycode.</param>
        public async Task<bool> SendPupKey(PopperCommand command)
        {
            if (IsDebugMode)
            {
                Logger.Debug($"Sent Pupper keycode {command}");
                return true;
            }

            // TODO Move all this into a handler ...
            SendKeyInputRequest request = new SendKeyInputRequest();
            request.command = command;

            var response = await _api.MakeRequest<SendKeyInputRequest, string>(request).ConfigureAwait(false);

            if (response?.Success == true)
            {
                _analytics.TrackPupCommand(command);
                return true;
            }
            else
            {
                //TODO Error handling???
                Logger.Error($"Error sending pup key {command}, responded with {response?.Code} and {response?.Messsage}");
                _analytics.TrackPupCommand(command, false);
                return false;
            }
        }

        public async Task<bool> SendRecordDisplay(string display)
        {
            if (IsDebugMode)
            {
                Logger.Debug($"Sent popper command to start recording display {display}");
                return true;
            }

            var request = new RecordDisplayRequest();
            request.display = display;

            var response = await _api.MakeRequest<RecordDisplayRequest, string>(request).ConfigureAwait(false);

            if (response?.Success == true)
            {
                return true;
            }
            else
            {
                //TODO Error handling???
                Logger.Error($"Error recording display {display}, responded with {response?.Code} and {response?.Messsage}");
                return false;
            }
        }
    }
}