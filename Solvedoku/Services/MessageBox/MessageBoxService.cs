using System.Windows;

namespace Solvedoku.Services.MessageBox
{
    class MessageBoxService : DependencyObject, IMessageBoxService
    {
        private Style _messageBoxStyle = (Style)Application.Current.Resources["MessageBoxStyle"];

        /// <summary>
        /// Displays a messagebox with the given parameters.
        /// </summary>
        /// <param name="messageText">Text of the messagebox.</param>
        /// <param name="title">Title of the messagebox.</param>
        /// <param name="messageBoxButton">Displayed button(s) in the messagebox. (Choose from the MessageBoxButton enum.).</param>
        /// <param name="messageBoxImage">Displayed icon in the messagebox. (Choose from the MessageBoxImage enum.).</param>
        /// <returns>MessageBoxResult</returns>
        public MessageBoxResult Show(string messageText, string title, MessageBoxButton messageBoxButton, MessageBoxImage messageBoxImage)
        {
            return Xceed.Wpf.Toolkit.MessageBox.Show(Application.Current.MainWindow, messageText, title, messageBoxButton, messageBoxImage, _messageBoxStyle);
        }

        /// <summary>
        /// Displays a messagebox with the given parameters.
        /// </summary>
        /// <param name="messageText">Text of the messagebox.</param>
        /// <param name="title">Title of the messagebox.</param>
        /// <param name="messageBoxButton">Displayed button(s) in the messagebox. (Choose from the MessageBoxButton enum.).</param>
        /// <param name="messageBoxImage">Displayed icon in the messagebox. (Choose from the MessageBoxImage enum.).</param>
        /// <param name="messageBoxStyle">Style what is applied on the MessageBox.</param>
        /// <returns>MessageBoxResult</returns>
        public MessageBoxResult Show(string messageText, string title, MessageBoxButton messageBoxButton, MessageBoxImage messageBoxImage, Style messageBoxStyle)
        {
            return Xceed.Wpf.Toolkit.MessageBox.Show(Application.Current.MainWindow, messageText, title, messageBoxButton, messageBoxImage, messageBoxStyle);
        }
    }
}