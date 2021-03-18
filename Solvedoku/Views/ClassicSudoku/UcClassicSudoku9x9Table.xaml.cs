using System.Windows.Controls;
using Solvedoku.Classes;

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
    }
}