using System;

namespace Solvedoku.Classes
{
    [Serializable]
    public class SudokuBoardSize
    {
        public int Height { get; set; }

        public int Width { get; set; }

        public int BoxCountY { get; set; }

        public int BoxCountX { get; set; }

        public override string ToString() => $"{Width}x{Height} ({BoxCountY}x{BoxCountX})";
    }
}