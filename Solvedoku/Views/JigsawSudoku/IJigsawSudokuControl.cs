﻿using Solvedoku.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solvedoku.Views.JigsawSudoku
{
    interface IJigsawSudokuControl
    {
        SudokuBoardSize BoardSize { get; set; }
    }
}