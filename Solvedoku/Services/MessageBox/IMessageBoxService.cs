using System.Windows;

namespace Solvedoku.Services.MessageBox
{
    interface IMessageBoxService
    {
        /// <summary>
        /// Shows a MessageBox with the given text, title, button(s) and image.
        /// </summary>
        /// <param name="messageText">Text of the MessageBox.</param>
        /// <param name="title">Title of the MessageBox</param>
        /// <param name="messageBoxButton">Button(s) of the MessageBox</param>
        /// <param name="messageBoxImage">image of the MessageBox</param>
        /// <returns></returns>
        MessageBoxResult Show(string messageText, string title, MessageBoxButton messageBoxButton,
            MessageBoxImage messageBoxImage);
    }
}