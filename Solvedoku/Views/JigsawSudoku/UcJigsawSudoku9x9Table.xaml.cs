using Solvedoku.Services.MessageBox;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Solvedoku.Views.JigsawSudoku
{
    /// <summary>
    /// Interaction logic for UcJigsawSudoku9x9Table.xaml
    /// </summary>
    public partial class UcJigsawSudoku9x9Table : UserControl
    {
        public UcJigsawSudoku9x9Table()
        {
            InitializeComponent();
        }

        private void PuzzleCell_OnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(((TextBox)sender).Text, "[^1-9]") || ((TextBox)sender).Text.Length > 1)
            {
                var messageBoxService = new MessageBoxService();
                messageBoxService.Show("Csak egyjegyű számokat üthetsz be 1-től 9 -ig.", "Információ!",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                ((TextBox)sender).Clear();
            }
        }
    }
}