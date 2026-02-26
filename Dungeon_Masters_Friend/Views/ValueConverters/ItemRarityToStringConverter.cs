using Avalonia.Data;
using Avalonia.Data.Converters;
using Dungeon_Masters_Friend.Models.Treasure;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Dungeon_Masters_Friend.Views.ValueConverters
{
    /// <summary>
    /// Converts item Rarity values to user-friendly strings for display.
    /// </summary>
    public class ItemRarityToStringConverter : IValueConverter
    {
        private static readonly Dictionary<Rarity, string> _rarityToDisplayString = new()
        {
            { Rarity.Common, "Common" },
            { Rarity.Uncommon, "Uncommon" },
            { Rarity.Rare, "Rare" },
            { Rarity.VeryRare, "Very Rare" },
            { Rarity.Legendary, "Legendary" }
        };

        public object? Convert(object? value, System.Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is Rarity rarity)
            {
                return _rarityToDisplayString[rarity];
            }
            return new BindingNotification(new ArgumentException("Expected an object of type Rarity but received " + value?.GetType()));
        }

        public object? ConvertBack(object? value, System.Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
