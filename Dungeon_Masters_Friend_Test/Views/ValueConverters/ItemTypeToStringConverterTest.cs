using Avalonia.Data;
using Avalonia.Platform;
using Dungeon_Masters_Friend.Views.ValueConverters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using TreasureType = Dungeon_Masters_Friend.Models.Treasure.Type;

namespace Dungeon_Masters_Friend_Test.Views.ValueConverters
{
    public class ItemTypeToStringConverterTest
    {
        private readonly ItemTypeToStringConverter _converter = new();

        [Fact]
        public void Convert()
        {
            Assert.Equal("Trade Good", _converter.Convert(TreasureType.TradeGood, typeof(string), null, CultureInfo.InvariantCulture));
        }

        [Fact]
        public void Convert_InvalidType()
        {
            var result = _converter.Convert(123, typeof(string), null, CultureInfo.InvariantCulture);

            Assert.IsType<BindingNotification>(result);
            var exception = BindingNotification.ExtractValue(result);
            Assert.IsType<ArgumentException>(exception); 
            Assert.Equal("Expected an object of type Treasure.Type but received System.Int32", ((ArgumentException)exception).Message);
        }

        [Fact]
        public void ConvertBack()
        {
            Assert.Throws<NotImplementedException>(() => _converter.ConvertBack("Trade Good", typeof(TreasureType), null, CultureInfo.InvariantCulture));
        }
    }
}
