using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PinupMobile.Core.Strings;

namespace PinupMobile.Droid.Settings
{
    public class Localisation : ILocalisation
    {
        private readonly Dictionary<string, FieldInfo> _fieldInfo;
        private readonly Dictionary<string, string> _strings = new Dictionary<string, string>();

        public Localisation(Type stringResources)
        {
            _fieldInfo = stringResources.GetFields(BindingFlags.Public | BindingFlags.Static).ToDictionary(t => t.Name, t => t);
        }

        public string GetString(string key)
        {
            if (!_strings.ContainsKey(key))
            {
                if (_fieldInfo.ContainsKey(key))
                {
                    // get string from resources
                    _strings.Add(key, Android.App.Application.Context.GetString((int)_fieldInfo[key].GetValue(null)));
                }
                else
                {
                    // This is an error, but we don't want to crash, and we want to show the Key so we can fix the issue.
                    return string.Format(System.Globalization.CultureInfo.InvariantCulture, "~~~ {0} ~~~", key);
                }
            }

#if DEBUG
            // highlight missing strings in debug builds
            var str = _strings[key];
            return string.IsNullOrEmpty(str) ? $"!!{key}!!" : str;
#else
            return _strings[key];
#endif
        }
    }
}
