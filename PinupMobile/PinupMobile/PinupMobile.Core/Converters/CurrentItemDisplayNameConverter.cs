using System;
using System.Globalization;
using MvvmCross.Converters;
using PinupMobile.Core.Remote.Model;

namespace PinupMobile.Core.Converters
{
    public class CurrentItemDisplayNameConverter : MvxValueConverter<Item, string>
    {
        protected override string Convert(Item value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null ||
                string.IsNullOrEmpty(value.DisplayName))
                return "None";

            return value.DisplayName;
        }
    }
}
