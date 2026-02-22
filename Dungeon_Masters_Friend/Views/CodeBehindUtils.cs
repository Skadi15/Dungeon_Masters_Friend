using Avalonia.Controls;
using Avalonia.Threading;

namespace Dungeon_Masters_Friend.Views
{
    /// <summary>
    /// Utility class with methods for code-behind files to use.
    /// </summary>
    internal class CodeBehindUtils
    {
        // Prevent instantiation
        private CodeBehindUtils() { }

        /// <summary>
        /// Selects all text in a TextBox control when it receives focus.
        /// </summary>
        /// <remarks>
        /// This method is intended to be used as an event handler for the GotFocus event of a TextBox.
        /// </remarks>
        /// <param name="sender">The control where the event originated</param>
        public static void TextBox_SelectContents(object? sender)
        {
            if (sender is not TextBox textBox)
            {
                return;
            }

            Dispatcher.UIThread.Post(textBox.SelectAll);
        }
    }
}