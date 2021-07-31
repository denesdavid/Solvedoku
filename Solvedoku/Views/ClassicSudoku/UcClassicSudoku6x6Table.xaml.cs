using System.Windows;
using System.Windows.Controls;
using Solvedoku.Classes;
using Solvedoku.Services.MessageBox;

namespace Solvedoku.Views.ClassicSudoku
{
    /// <summary>
    /// Interaction logic for UcClassicSudoku6x6Table.xaml
    /// </summary>
    public partial class UcClassicSudoku6x6Table : UserControl, IClassicSudokuControl
    {
        public UcClassicSudoku6x6Table()
        {
            InitializeComponent();
        }

        public SudokuBoardSize BoardSize
        {
            get => new SudokuBoardSize { Height = 6, Width = 6, BoxCountX = 2, BoxCountY = 3 };
        }

        /// <summary>
        /// Displays a messagebox and clears the actual TextBox if you enter any of the not acceptable charcters.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(((TextBox)sender).Text, "[^1-6]") || ((TextBox)sender).Text.Length > 1)
            {
                var messageBoxService = new MessageBoxService();
                messageBoxService.Show(Properties.Resources.MessageBox_OnlyNumbersFrom1To6, Properties.Resources.MessageBox_Information_Title,
                    MessageBoxButton.OK, MessageBoxImage.Information);
                ((TextBox)sender).Clear();
            }
        }
    }
}