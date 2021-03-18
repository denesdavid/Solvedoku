using System.Windows.Controls;
using Solvedoku.Classes;

namespace Solvedoku.Views.ClassicSudoku
{
    /// <summary>
    /// Interaction logic for UcClassicSudoku4x4Table.xaml
    /// </summary>
    public partial class UcClassicSudoku4x4Table : UserControl, IClassicSudokuControl
    {
        public SudokuBoardSize BoardSize 
        {
            get => new SudokuBoardSize { Height = 4, Width = 4, BoxCountX = 2, BoxCountY = 2 };
        }
        public UcClassicSudoku4x4Table()
        {
            InitializeComponent();
        }
    }
}