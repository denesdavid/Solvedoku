using System.Windows;
using System.Windows.Controls;
using Solvedoku.Views.AboutBox;
using Solvedoku.Views.Options;

namespace Solvedoku.Views.TabControlWithRightContentArea
{
    /// <summary>
    /// Interaction logic for UcTabControlWithRightContentArea.xaml
    /// </summary>
    public partial class UcTabControlWithRightContentArea : UserControl
    {
        public UcTabControlWithRightContentArea()
        {
            InitializeComponent();
            DataContext = this;
        }

        #region Events
        private void Options_Click(object sender, RoutedEventArgs e)
        {
            OptionsWindow optionsWindow = new OptionsWindow();
            optionsWindow.ShowDialog();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            AboutBoxWindow aboutBox = new AboutBoxWindow();
            aboutBox.ShowDialog();
        }

        #endregion
    }
}
