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
        public SudokuBoardSize BoardSize
        {
            get => new SudokuBoardSize { Height = 9, Width = 9, BoxCountX = 3, BoxCountY = 3 };
        }

        public UcClassicSudoku9x9Table()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(((TextBox)sender).Text, "[^1-9]") || ((TextBox)sender).Text.Length > 1)
            {
                var messageBoxService = new MessageBoxService();
                messageBoxService.Show("Csak egyjegyű számokat üthetsz be 1-től 9-ig.", "Információ!",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                ((TextBox)sender).Clear();
            }
        }
    }
}