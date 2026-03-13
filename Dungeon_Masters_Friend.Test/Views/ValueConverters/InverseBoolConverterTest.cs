using Dungeon_Masters_Friend.Views.ValueConverters;
using System.Globalization;

namespace Dungeon_Masters_Friend.Test.Views.ValueConverters
{
    public class InverseBoolConverterTest
    {
        private readonly InverseBoolConverter _converter = new();

        [Fact]
        public void Convert()
        {
            Assert.Equal(false, _converter.Convert(true, typeof(bool), null, CultureInfo.InvariantCulture));
            Assert.Equal(true, _converter.Convert(false, typeof(bool), null, CultureInfo.InvariantCulture));
        }

        [Fact]
        public void Convert_InvalidType()
        {
            var result = _converter.Convert(123, typeof(bool), null, CultureInfo.InvariantCulture);
            Assert.IsType<Avalonia.Data.BindingNotification>(result);
            var exception = Avalonia.Data.BindingNotification.ExtractValue(result);
            Assert.IsType<ArgumentException>(exception);
            Assert.Equal("Expected a boolean value but received System.Int32", ((ArgumentException)exception).Message);
        }

        [Fact]
        public void ConvertBack()
        {
            Assert.Equal(false, _converter.ConvertBack(true, typeof(bool), null, CultureInfo.InvariantCulture));
            Assert.Equal(true, _converter.ConvertBack(false, typeof(bool), null, CultureInfo.InvariantCulture));
        }

        [Fact]
        public void ConvertBack_InvalidType()
        {
            var result = _converter.ConvertBack(123, typeof(bool), null, CultureInfo.InvariantCulture);
            Assert.IsType<Avalonia.Data.BindingNotification>(result);
            var exception = Avalonia.Data.BindingNotification.ExtractValue(result);
            Assert.IsType<ArgumentException>(exception);
            Assert.Equal("Expected a boolean value but received System.Int32", ((ArgumentException)exception).Message);
        }
    }
}
