using System;
using Foundation;
using Newtonsoft.Json;
using PinupMobile.Core.Logging;
using PinupMobile.Core.Settings;

namespace PinupMobile.iOS.Settings
{
    public class UserSettings : IUserSettings
    {
        private readonly NSUserDefaults _defaults;

        public UserSettings()
        {
            _defaults = NSUserDefaults.StandardUserDefaults;
        }

        public bool GetBool(string key)
        {
            try
            {
                return _defaults.BoolForKey(key);
            }
            catch (Exception e)
            {
                Logger.Error($"Error Getting Bool UserSetting {key}");
                Logger.Error(e.Message);
            }

            return false;
        }

        public float GetFloat(string key)
        {
            try
            {
                return _defaults.FloatForKey(key);
            }
            catch (Exception e)
            {
                Logger.Error($"Error Getting Float UserSetting {key}");
                Logger.Error(e.Message);
            }

            return 0;
        }

        public int GetInt(string key)
        {
            try
            {
                return (int)_defaults.IntForKey(key);
            }
            catch (Exception e)
            {
                Logger.Error($"Error Getting Int UserSetting {key}");
                Logger.Error(e.Message);
            }

            return 0;
        }

        public long GetLong(string key)
        {
            try
            {
                return (long)_defaults.DoubleForKey(key);
            }
            catch (Exception e)
            {
                Logger.Error($"Error Getting Long UserSetting {key}");
                Logger.Error(e.Message);
            }

            return 0;
        }

        public T GetObject<T>(string key) where T : class
        {
            try
            {
                string json = _defaults.StringForKey(key);
                T obj = JsonConvert.DeserializeObject<T>(json);
                return obj;
            }
            catch (Exception e)
            {
                Logger.Error($"Error Getting Object UserSetting {key}");
                Logger.Error(e.Message);
            }

            return null;
        }

        public string GetString(string key)
        {
            try
            {
                return _defaults.StringForKey(key);
            }
            catch (Exception e)
            {
                Logger.Error($"Error Getting String UserSetting {key}");
                Logger.Error(e.Message);
            }

            return string.Empty;
        }

        public bool SetFloat(string key, float val)
        {
            try
            {
                // Apple now recommends you *don't* call Synchronize as it will periodically happen
                // https://docs.microsoft.com/en-us/xamarin/ios/app-fundamentals/user-defaults
                _defaults.SetFloat(val, key);
                return true;
            }
            catch (Exception e)
            {
                Logger.Error($"Error Setting Float UserSetting {key}");
                Logger.Error(e.Message);
            }

            return false;
        }

        public bool SetInt(string key, int val)
        {
            try
            {
                // Apple now recommends you *don't* call Synchronize as it will periodically happen
                // https://docs.microsoft.com/en-us/xamarin/ios/app-fundamentals/user-defaults
                _defaults.SetInt(val, key);
                return true;
            }
            catch (Exception e)
            {
                Logger.Error($"Error Setting Int UserSetting {key}");
                Logger.Error(e.Message);
            }

            return false;
        }

        public bool SetLong(string key, long val)
        {
            try
            {
                // Apple now recommends you *don't* call Synchronize as it will periodically happen
                // https://docs.microsoft.com/en-us/xamarin/ios/app-fundamentals/user-defaults
                _defaults.SetDouble(val, key);
                return true;
            }
            catch (Exception e)
            {
                Logger.Error($"Error Setting Long UserSetting {key}");
                Logger.Error(e.Message);
            }

            return false;
        }

        public bool SetBool(string key, bool val)
        {
            try
            {
                // Apple now recommends you *don't* call Synchronize as it will periodically happen
                // https://docs.microsoft.com/en-us/xamarin/ios/app-fundamentals/user-defaults
                _defaults.SetBool(val, key);
                return true;
            }
            catch (Exception e)
            {
                Logger.Error($"Error Setting Bool UserSetting {key}");
                Logger.Error(e.Message);
            }

            return false;
        }

        public bool SetString(string key, string val)
        {
            try
            {
                // Apple now recommends you *don't* call Synchronize as it will periodically happen
                // https://docs.microsoft.com/en-us/xamarin/ios/app-fundamentals/user-defaults
                _defaults.SetString(val, key);
                return true;
            }
            catch (Exception e)
            {
                Logger.Error($"Error Setting String UserSetting {key}");
                Logger.Error(e.Message);
            }

            return false;
        }

        public bool SetObject<T>(string key, T val) where T : class
        {
            try
            {
                string json = JsonConvert.SerializeObject(val);
                if (!string.IsNullOrEmpty(json))
                {
                    // Apple now recommends you *don't* call Synchronize as it will periodically happen
                    // https://docs.microsoft.com/en-us/xamarin/ios/app-fundamentals/user-defaults
                    _defaults.SetString(json, key);

                    return true;
                }
            }
            catch (Exception e)
            {
                Logger.Error($"Error Setting Object UserSetting {key}");
                Logger.Error(e.Message);
            }

            return false;
        }
    }
}
