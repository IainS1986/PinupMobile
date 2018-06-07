using System;
using System.Globalization;
using MvvmCross.Converters;
using PinupMobile.Core.Remote.Model;

namespace PinupMobile.Core.Converters
{
    public class IsFalseConverter : MvxValueConverter<bool, bool>
    {
        protected override bool Convert(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            return !value;
        }

        protected override bool ConvertBack(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            return !value;
        }
    }
}
