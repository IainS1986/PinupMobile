using Foundation;
using PinupMobile.Core.Strings;

namespace PinupMobile.iOS.Settings
{
    public class Localisation : ILocalisation
    {
        public string GetString(string key)
        {
            var str = NSBundle.MainBundle.GetLocalizedString(key);
            return string.IsNullOrEmpty(str) ? $"!!!{key}!!!" : str;
        }
    }
}
