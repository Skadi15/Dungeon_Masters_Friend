using Avalonia.Data;
using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace Dungeon_Masters_Friend.Views.ValueConverters
{
    /// <summary>
    /// Inverts the value of the given boolean.
    /// </summary>
    public class InverseBoolConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value is bool boolValue ? !boolValue : new BindingNotification(new ArgumentException("Expected a boolean value but received " + value?.GetType()));
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }
    }
}
