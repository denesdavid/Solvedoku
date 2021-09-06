using System.Windows;

namespace Solvedoku.Services.MessageBox
{
    public interface IMessageBoxService
    {
        /// <summary>
        /// Shows a MessageBox with the given text, title, button(s) and image.
        /// </summary>
        /// <param name="messageText">Text of the MessageBox.</param>
        /// <param name="title">Title of the MessageBox</param>
        /// <param name="messageBoxButton">Button(s) of the MessageBox</param>
        /// <param name="messageBoxImage">Image of the MessageBox</param>
        /// <returns>MessageBoxResult</returns>
        MessageBoxResult Show(string messageText, string title, MessageBoxButton messageBoxButton,
            MessageBoxImage messageBoxImage);

        /// <summary>
        /// Shows a MessageBox with the given text, title, button(s), image and style.
        /// </summary>
        /// <param name="messageText">Text of the MessageBox.</param>
        /// <param name="title">Title of the MessageBox</param>
        /// <param name="messageBoxButton">Button(s) of the MessageBox</param>
        /// <param name="messageBoxImage">Image of the MessageBox</param>
        /// <param name="messageBoxStyle">Style of the MessageBox</param>
        /// <returns>MessageBoxResult</returns>
        MessageBoxResult Show(string messageText, string title, MessageBoxButton messageBoxButton,
            MessageBoxImage messageBoxImage, Style messageBoxStyle);
    }
}