using System.Windows;

namespace Solvedoku.Services.MessageBox
{
    public class MessageBoxService : IMessageBoxService
    {
        private Style _messageBoxStyle = (Style)Application.Current?.Resources["MessageBoxStyle"];

        /// <inheritdoc />
        public virtual MessageBoxResult Show(string messageText, string title, MessageBoxButton messageBoxButton, MessageBoxImage messageBoxImage)
        {
            return Xceed.Wpf.Toolkit.MessageBox.Show(Application.Current?.MainWindow, messageText, title, messageBoxButton, messageBoxImage, _messageBoxStyle);
        }

        /// <inheritdoc />
        public virtual MessageBoxResult Show(string messageText, string title, MessageBoxButton messageBoxButton, MessageBoxImage messageBoxImage, Style messageBoxStyle)
        {
            return Xceed.Wpf.Toolkit.MessageBox.Show(Application.Current?.MainWindow, messageText, title, messageBoxButton, messageBoxImage, messageBoxStyle);
        }
    }
}