using System.Windows;
using System.Windows.Controls;
using Solvedoku.Classes;
using Solvedoku.Services.MessageBox;

namespace Solvedoku.Views.ClassicSudoku
{
    /// <summary>
    /// Interaction logic for UcClassicSudoku4x4Table.xaml
    /// </summary>
    public partial class UcClassicSudoku4x4Table : UserControl, IClassicSudokuControl
    { 
        public UcClassicSudoku4x4Table()
        {
            InitializeComponent();
        }

        public SudokuBoardSize BoardSize
        {
            get => new SudokuBoardSize { Height = 4, Width = 4, BoxCountX = 2, BoxCountY = 2 };
        }

        /// <summary>
        /// Displays a messagebox and clears the actual TextBox if you enter any of the not acceptable charcters.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(((TextBox)sender).Text, "[^1-4]") || ((TextBox)sender).Text.Length > 1)
            {
                var messageBoxService = new MessageBoxService();
                messageBoxService.Show(Properties.Resources.MessageBox_OnlyNumbersFrom1To4, Properties.Resources.MessageBox_Information_Title,
                    MessageBoxButton.OK, MessageBoxImage.Information);
                ((TextBox)sender).Clear();
            }
        }
    }
}