using Avalonia.Data;
using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using TreasureType = Dungeon_Masters_Friend.Models.Treasure.Type;

namespace Dungeon_Masters_Friend.Views.ValueConverters
{
    /// <summary>
    /// Converts item Type values to user-friendly strings for display.
    /// </summary>
    public class ItemTypeToStringConverter : IValueConverter
    {
        private static readonly Dictionary<TreasureType, string> _itemTypeToDisplayString = new()
        {
            { TreasureType.TradeGood, "Trade Good" },
            { TreasureType.Gemstone, "Gemstone" },
            { TreasureType.ArtObject, "Art Object" }
        };

        public object Convert(object? value, System.Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        {
            if (value is TreasureType type)
            {
                return _itemTypeToDisplayString[type];
            }
            return new BindingNotification(new ArgumentException("Expected an object of type Treasure.Type but received " + value?.GetType()));
        }

        public object ConvertBack(object? value, System.Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
