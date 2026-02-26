using Avalonia.Data;
using Dungeon_Masters_Friend.Models.Treasure;
using Dungeon_Masters_Friend.Views.ValueConverters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Dungeon_Masters_Friend_Test.Views.ValueConverters
{
    public class ItemRarityToStringConverterTest
    {
        private readonly ItemRarityToStringConverter _converter = new();

        [Fact]
        public void Convert()
        {
            Assert.Equal("Very Rare", _converter.Convert(Rarity.VeryRare, typeof(string), null, CultureInfo.InvariantCulture));
        }

        [Fact]
        public void Convert_InvalidType()
        {
            var result = _converter.Convert(123, typeof(string), null, CultureInfo.InvariantCulture);

            Assert.IsType<BindingNotification>(result);
            var exception = BindingNotification.ExtractValue(result);
            Assert.IsType<ArgumentException>(exception);
            Assert.Equal("Expected an object of type Rarity but received System.Int32", ((ArgumentException)exception).Message);
        }

        [Fact]
        public void ConvertBack()
        {
            Assert.Throws<NotImplementedException>(() => _converter.ConvertBack("Very Rare", typeof(Rarity), null, CultureInfo.InvariantCulture));
        }
    }
}
