namespace Solvedoku.ViewModels.ClassicSudoku
{
    abstract class BaseClassicSudokuTableViewModel : BaseSudokuTableViewModel
    {
        public abstract bool AreDiagonalRulesSet { get; set; }
    }
}