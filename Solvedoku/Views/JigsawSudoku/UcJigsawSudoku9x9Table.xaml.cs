using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Input;
using Solvedoku.ViewModels.JigsawSudoku;
using Solvedoku.Classes;
using Solvedoku.Services.MessageBox;

namespace Solvedoku.Views.JigsawSudoku
{
    /// <summary>
    /// Interaction logic for UcJigsawSudoku9x9Table.xaml
    /// </summary>
    public partial class UcJigsawSudoku9x9Table : UserControl, IJigsawSudokuControl
    {
        public UcJigsawSudoku9x9Table()
        {
            InitializeComponent();
        }

        public SudokuBoardSize BoardSize { get => new SudokuBoardSize { Height = 9, Width = 9, BoxCountX = 3, BoxCountY = 3 }; }

        private void PuzzleCell_OnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            JigsawSudokuViewModel viewModel = JigsawSudokuViewModel.Instance;
            SolidColorBrush brush = new SolidColorBrush(viewModel.SelectedColor.GetValueOrDefault());
            ((TextBox)sender).Background = brush;
        }

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

        private void PuzzleCell_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                PuzzleCell_OnMouseRightButtonUp(sender, null);
            }
        }
    }
}