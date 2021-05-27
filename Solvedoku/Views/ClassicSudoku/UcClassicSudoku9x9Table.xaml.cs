using System.Windows;
using System.Windows.Controls;
using Solvedoku.Classes;
using Solvedoku.Services.MessageBox;

namespace Solvedoku.Views.ClassicSudoku
{
    /// <summary>
    /// Interaction logic for UcClassicSudoku9x9Table.xaml
    /// </summary>
    public partial class UcClassicSudoku9x9Table : UserControl, IClassicSudokuControl
    {
        public UcClassicSudoku9x9Table()
        {
            InitializeComponent();
        }

        public SudokuBoardSize BoardSize
        {
            get => new SudokuBoardSize { Height = 9, Width = 9, BoxCountX = 3, BoxCountY = 3 };
        }

        /// <summary>
        /// Displays a messagebox and clears the actual TextBox if you enter any of the not acceptable charcters.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(((TextBox)sender).Text, "[^1-9]") || ((TextBox)sender).Text.Length > 1)
            {
                var messageBoxService = new MessageBoxService();
                messageBoxService.Show(Properties.Resources.MessageBox_OnlyNumbersFrom1To9, Properties.Resources.MessageBox_Information_Title,
                    MessageBoxButton.OK, MessageBoxImage.Information);
                ((TextBox)sender).Clear();
            }
        }
    }
}