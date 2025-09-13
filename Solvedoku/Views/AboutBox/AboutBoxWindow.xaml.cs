using System.Windows;
using System.Windows.Input;

namespace Solvedoku.Views.AboutBox
{
    /// <summary>
    /// Interaction logic for AboutBox.xaml
    /// </summary>
    public partial class AboutBoxWindow : Window
    {
        public AboutBoxWindow()
        {
            InitializeComponent();
            Owner = Application.Current.MainWindow;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}