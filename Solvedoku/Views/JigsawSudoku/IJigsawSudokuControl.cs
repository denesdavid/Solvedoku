using Solvedoku.Classes;

namespace Solvedoku.Views.JigsawSudoku
{
    interface IJigsawSudokuControl
    {
        SudokuBoardSize BoardSize { get; }

        //int[,] GetPuzzleAreas();
    }
}