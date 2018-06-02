using System;
namespace PinupMobile.Core.Settings
{
    /// <summary>
    /// Simple service to interact with SharedPreferences on droid
    /// and NSUserDefaults on iOS. Just a simple Dictionary<string,string>
    /// type of interface
    /// </summary>
    public interface IUserSettings
    {
        string GetString(string key);

        int GetInt(string key);

        long GetLong(string key);

        float GetFloat(string key);

        bool GetBool(string key);

        T GetObject<T>(string key) where T : class;

        bool SetString(string key, string value);

        bool SetInt(string key, int val);

        bool SetFloat(string key, float val);

        bool SetLong(string key, long val);

        bool SetBool(string key, bool val);

        bool SetObject<T>(string key, T val) where T : class;
    }
}
