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
        public SudokuBoardSize BoardSize
        {
            get => new SudokuBoardSize { Height = 6, Width = 6, BoxCountX = 2, BoxCountY = 3 };
        }
        public UcClassicSudoku6x6Table()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(((TextBox)sender).Text, "[^1-6]") || ((TextBox)sender).Text.Length > 1)
            {
                var messageBoxService = new MessageBoxService();
                messageBoxService.Show("Csak egyjegyű számokat üthetsz be 1-től 6 -ig.", "Információ!",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                ((TextBox)sender).Clear();
            }
        }
    }
}