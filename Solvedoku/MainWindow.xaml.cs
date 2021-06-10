using System.Windows;
using Solvedoku.Views.AboutBox;
using Solvedoku.Views.OptionsWindow;

namespace Solvedoku
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructor

        public MainWindow()
        {
            InitializeComponent();
            MinHeight = Height;
            MinWidth = Width;
        }

        #endregion

        #region Events
        private void Options_Click(object sender, RoutedEventArgs e)
        {
            OptionsWindow optionsWindow = new OptionsWindow();
            optionsWindow.ShowDialog();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }

        #endregion
    }
}