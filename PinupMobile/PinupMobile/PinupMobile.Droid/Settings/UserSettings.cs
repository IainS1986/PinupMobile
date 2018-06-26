using System;
using Android.Content;
using MvvmCross.Platforms.Android;
using Newtonsoft.Json;
using PinupMobile.Core.Logging;
using PinupMobile.Core.Settings;

namespace PinupMobile.Droid.Settings
{
    public class UserSettings : IUserSettings
    {
        private readonly ISharedPreferences _preferences;
        private readonly IMvxAndroidCurrentTopActivity _topActivity;

        public UserSettings(IMvxAndroidCurrentTopActivity topActivity)
        {
            _topActivity = topActivity;
            _preferences = topActivity.Activity.GetSharedPreferences(topActivity.Activity.PackageName, Android.Content.FileCreationMode.Private);
        }

        public float GetFloat(string key)
        {
            try
            {
                return _preferences.GetFloat(key, 0);
            }
            catch (Exception e)
            {
                Logger.Error($"Error Getting Float UserSetting {key} - {e.Message}", e);
            }

            return 0;
        }

        public int GetInt(string key)
        {
            try
            {
                return _preferences.GetInt(key, 0);
            }
            catch (Exception e)
            {
                Logger.Error($"Error Getting Int UserSetting {key} - {e.Message}", e);
            }

            return 0;
        }

        public long GetLong(string key)
        {
            try
            {
                return _preferences.GetLong(key, 0);
            }
            catch (Exception e)
            {
                Logger.Error($"Error Getting Long UserSetting {key} - {e.Message}", e);
            }

            return 0;
        }

        public bool GetBool(string key)
        {
            try
            {
                return _preferences.GetBoolean(key, false);
            }
            catch (Exception e)
            {
                Logger.Error($"Error Getting Bool UserSetting {key} - {e.Message}", e);
            }

            return false;
        }

        public string GetString(string key)
        {
            try
            {
                return _preferences.GetString(key, string.Empty);
            }
            catch (Exception e)
            {
                Logger.Error($"Error Getting String UserSetting {key} - {e.Message}", e);
            }

            return string.Empty;
        }

        public T GetObject<T>(string key) where T : class
        {
            try
            {
                string json = _preferences.GetString(key, string.Empty);
                T obj = JsonConvert.DeserializeObject<T>(json);
                return obj;
            }
            catch (Exception e)
            {
                Logger.Error($"Error Getting Object UserSetting {key} - {e.Message}", e);
            }

            return null;
        }

        public bool SetFloat(string key, float val)
        {
            try
            {
                bool result = _preferences.Edit().PutFloat(key, val).Commit();
                if (!result)
                {
                    throw new ArgumentException($"Failed to set user float preference with key {key} and value {val}");
                }

                return result;
            }
            catch (Exception e)
            {
                Logger.Error($"Error Setting Float UserSetting {key} - {e.Message}", e);
            }

            return false;
        }

        public bool SetInt(string key, int val)
        {
            try
            {
                bool result = _preferences.Edit().PutInt(key, val).Commit();
                if (!result)
                {
                    throw new ArgumentException($"Failed to set user int preference with key {key} and value {val}");
                }

                return result;
            }
            catch (Exception e)
            {
                Logger.Error($"Error Setting Int UserSetting {key} - {e.Message}", e);
            }

            return false;
        }

        public bool SetLong(string key, long val)
        {
            try
            {
                bool result = _preferences.Edit().PutLong(key, val).Commit();
                if (!result)
                {
                    throw new ArgumentException($"Failed to set user long preference with key {key} and value {val}");
                }

                return result;
            }
            catch (Exception e)
            {
                Logger.Error($"Error Setting Long UserSetting {key} - {e.Message}", e);
            }

            return false;
        }

        public bool SetBool(string key, bool val)
        {
            try
            {
                bool result = _preferences.Edit().PutBoolean(key, val).Commit();
                if (!result)
                {
                    throw new ArgumentException($"Failed to set user bool preference with key {key} and value {val}");
                }

                return result;
            }
            catch (Exception e)
            {
                Logger.Error($"Error Setting Bool UserSetting {key} - {e.Message}", e);
            }

            return false;
        }

        public bool SetString(string key, string val)
        {
            try
            {
                bool result = _preferences.Edit().PutString(key, val).Commit();
                if (!result)
                {
                    throw new ArgumentException($"Failed to set user string preference with key {key} and value {val}");
                }

                return result;
            }
            catch (Exception e)
            {
                Logger.Error($"Error Setting String UserSetting {key} - {e.Message}", e);
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
                    bool result = _preferences.Edit().PutString(key, json).Commit();
                    if (!result)
                    {
                        throw new ArgumentException($"Failed to set user string preference with key {key} and value {val}");
                    }

                    return result;
                }
            }
            catch (Exception e)
            {
                Logger.Error($"Error Setting Object UserSetting {key} - {e.Message}", e);
            }

            return false;
        }
    }
}
