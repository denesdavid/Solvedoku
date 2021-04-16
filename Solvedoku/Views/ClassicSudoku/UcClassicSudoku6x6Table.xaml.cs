using System.Windows.Controls;
using Solvedoku.Classes;

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
    }
}