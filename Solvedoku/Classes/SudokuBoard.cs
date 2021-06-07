using Solvedoku.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;

namespace Solvedoku.Classes
{
    [Serializable]
    public class SudokuBoard
    {
        #region Fields
        int _row;
        int _maxValue;
        SudokuTile[,] _tiles;
        ISet<SudokuRule> _rules = new HashSet<SudokuRule>();
        public static ObservableCollection<ColorItem> JigsawColors
        {
            get => new ObservableCollection<ColorItem> {
                new ColorItem(Colors.LightBlue, Resources.Color_LightBlue),
                new ColorItem(Colors.CornflowerBlue, Resources.Color_CornflowerBlue),
                new ColorItem(Colors.Magenta, Resources.Color_Magenta),
                new ColorItem(Colors.Red, Resources.Color_Red),
                new ColorItem(Colors.Green, Resources.Color_Green),
                new ColorItem(Colors.Yellow, Resources.Color_Yellow),
                new ColorItem(Colors.RosyBrown, Resources.Color_RosyBrown),
                new ColorItem(Colors.Orange, Resources.Color_Orange),
                new ColorItem(Colors.MediumPurple, Resources.Color_MediumPurple),
                new ColorItem(Colors.LightGray, Resources.Color_LightGray)
            };
        }
        #endregion

        #region Properties
        public SudokuBoardSize BoardSize
        {
            get => new SudokuBoardSize
            {
                Height = Height,
                Width = Width,
                BoxCountX = CountBlockSize(Height).Item1,
                BoxCountY = CountBlockSize(Height).Item2
            };
        }

        public int Width => _tiles.GetLength(0);

        public int Height => _tiles.GetLength(1);

        #endregion

        #region Constructors
        public SudokuBoard(SudokuBoard copy)
        {

            _maxValue = copy._maxValue;
            _tiles = new SudokuTile[copy.Width, copy.Height];
            CreateTiles();
            // Copy the tile values
            foreach (var pos in SudokuFactory.box(Width, Height))
            {
                _tiles[pos.Item1, pos.Item2] = new SudokuTile(pos.Item1, pos.Item2, _maxValue);
                _tiles[pos.Item1, pos.Item2].Value = copy._tiles[pos.Item1, pos.Item2].Value;
            }

            // Copy the rules
            foreach (SudokuRule rule in copy._rules)
            {
                var ruleTiles = new HashSet<SudokuTile>();
                foreach (SudokuTile tile in rule)
                {
                    ruleTiles.Add(_tiles[tile.Row, tile.Column]);
                }
                _rules.Add(new SudokuRule(ruleTiles, rule.Description));
            }
        }

        public SudokuBoard(int width, int height, int maxValue)
        {
            _maxValue = maxValue;
            _tiles = new SudokuTile[width, height];
            CreateTiles();
            if (_maxValue == width || _maxValue == height)// If maxValue is not width or height, then adding line rules would be stupid
            {
                SetupLineRules();
            }
        }

        public SudokuBoard(int width, int height) : this(width, height, Math.Max(width, height)) { }
        #endregion

        #region Functions
        IEnumerable<SudokuTile> GetRow(int row)
        {
            for (int i = 0; i < _tiles.GetLength(0); i++)
            {
                yield return _tiles[i, row];
            }
        }

        IEnumerable<SudokuTile> GetCol(int col)
        {
            for (int i = 0; i < _tiles.GetLength(1); i++)
            {
                yield return _tiles[col, i];
            }
        }

        internal SudokuProgress Simplify()
        {
            SudokuProgress result = SudokuProgress.NO_PROGRESS;
            bool valid = CheckValid();
            if (!valid)
            {
                return SudokuProgress.FAILED;
            }

            foreach (SudokuRule rule in _rules)
            {
                result = SudokuTile.CombineSolvedState(result, rule.Solve());
            }

            return result;
        }

        internal IEnumerable<SudokuTile> TileBox(int startX, int startY, int sizeX, int sizeY)
        {
            return from pos in SudokuFactory.box(sizeX, sizeY) select _tiles[startX + pos.Item1, startY + pos.Item2];
        }

        Tuple<int, int> CountBlockSize(int size)
        {
            Tuple<int, int> blockSize = new Tuple<int, int>(0, 0);
            double sqrtSize = Math.Sqrt(size);

            if (sqrtSize % 1 == 0)
            {
                blockSize = new Tuple<int, int>((int)sqrtSize, (int)sqrtSize);
            }
            else
            {
                blockSize = new Tuple<int, int>((int)sqrtSize + 1, (int)sqrtSize);
            }
            return blockSize;
        }

        public IEnumerable<SudokuBoard> Solve()
        {
            ResetSolutions();
            SudokuProgress simplify = SudokuProgress.PROGRESS;

            while (simplify == SudokuProgress.PROGRESS)
            {
                simplify = Simplify();
            }

            if (simplify == SudokuProgress.FAILED)
            {
                yield break;
            }

            // Find one of the values with the least number of alternatives, but that still has at least 2 alternatives
            var query = from rule in _rules
                        from tile in rule
                        where tile.PossibleCount > 1
                        orderby tile.PossibleCount ascending
                        select tile;

            SudokuTile chosen = query.FirstOrDefault();

            if (chosen == null)
            {
                // The board has been completed, we're done!
                yield return this;
                yield break;
            }

            // Console.WriteLine("SudokuTile: " + chosen.ToString());

            foreach (var value in Enumerable.Range(1, _maxValue))
            {
                // Iterate through all the valid possibles on the chosen square and pick a number for it
                if (!chosen.IsValuePossible(value))
                {
                    continue;
                }

                var copy = new SudokuBoard(this);
                copy.Tile(chosen.Row, chosen.Column).Fix(value);

                foreach (var innerSolution in copy.Solve())
                {
                    yield return innerSolution;
                }
            }
            yield break;
        }

        public SudokuBoard SolveOnce()
        {
            ResetSolutions();
            SudokuProgress simplify = SudokuProgress.PROGRESS;
            while (simplify == SudokuProgress.PROGRESS)
            {
                simplify = Simplify();
            }

            if (simplify == SudokuProgress.FAILED)
            {
                return null;
            }

            // Find one of the values with the least number of alternatives, but that still has at least 2 alternatives
            var query = from rule in _rules
                        from tile in rule
                        where tile.PossibleCount > 1
                        orderby tile.PossibleCount ascending
                        select tile;

            SudokuTile chosen = query.FirstOrDefault();

            if (chosen == null)
            {
                // The board has been completed, we're done!
                return this;
            }

            // Console.WriteLine("SudokuTile: " + chosen.ToString());

            foreach (var value in Enumerable.Range(1, _maxValue))
            {
                // Iterate through all the valid possibles on the chosen square and pick a number for it
                if (!chosen.IsValuePossible(value))
                {
                    continue;
                }

                var copy = new SudokuBoard(this);
                copy.Tile(chosen.Row, chosen.Column).Fix(value);

                foreach (var innerSolution in copy.Solve())
                {
                    return innerSolution;
                }
            }
            return null;
        }

        public string[,] OutputAsMatrix()
        {
            string[,] output = new string[_tiles.GetLength(0), _tiles.GetLength(1)];
            for (int row = 0; row < _tiles.GetLength(0); row++)
            {
                for (int column = 0; column < _tiles.GetLength(1); column++)
                {
                    output[row, column] = _tiles[row, column].ToStringSimple();
                }
            }
            return output;
        }

        public SudokuTile Tile(int row, int column)
        {
            return _tiles[row, column];
        }
        #endregion

        #region Methods
        void CreateTiles()
        {
            foreach (var pos in SudokuFactory.box(_tiles.GetLength(0), _tiles.GetLength(1)))
            {
                _tiles[pos.Item1, pos.Item2] = new SudokuTile(pos.Item1, pos.Item2, _maxValue);
            }
        }

        void SetupLineRules()
        {
            // Create rules for rows and columns
            for (int x = 0; x < Width; x++)
            {
                IEnumerable<SudokuTile> row = GetCol(x);
                _rules.Add(new SudokuRule(row, "Row " + x.ToString()));
            }
            for (int y = 0; y < Height; y++)
            {
                IEnumerable<SudokuTile> col = GetRow(y);
                _rules.Add(new SudokuRule(col, "Col " + y.ToString()));
            }
        }

        internal void ResetSolutions()
        {
            foreach (SudokuTile tile in _tiles)
            {
                tile.ResetPossibles();
            }
        }

        internal void AddBoxesCount(int boxesX, int boxesY)
        {
            int sizeX = Width / boxesX;
            int sizeY = Height / boxesY;

            var boxes = SudokuFactory.box(sizeX, sizeY);
            foreach (var pos in boxes)
            {
                if ((sizeX * sizeY) != pos.Item1 * sizeX && (sizeX * sizeY) != pos.Item1 * sizeY)
                {
                    IEnumerable<SudokuTile> boxTiles = TileBox(pos.Item1 * sizeX, pos.Item2 * sizeY, sizeX, sizeY);
                    CreateRule("Box at (" + pos.Item1.ToString() + ", " + pos.Item2.ToString() + ")", boxTiles);
                }
            }
        }

        public void CreateRule(string description, params SudokuTile[] tiles)
        {
            _rules.Add(new SudokuRule(tiles, description));
        }

        public void CreateRule(string description, IEnumerable<SudokuTile> tiles)
        {
            _rules.Add(new SudokuRule(tiles, description));
        }

        public bool CheckValid()
        {
            return _rules.All(rule => rule.CheckValid());
        }

        public void AddRow(string rowNumbers)
        {
            // Method for initializing a board from string
            for (int column = 0; column < rowNumbers.Length; column++)
            {
                var tile = _tiles[_row, column];
                if (rowNumbers[column] == '/')
                {
                    tile.Block();
                    continue;
                }
                int value = rowNumbers[column] == '.' ? 0 : (int)Char.GetNumericValue(rowNumbers[column]);
                tile.Value = value;
            }
            _row++;
        }

        public static List<SolidColorBrush> GetPuzzleColorsAsSolidColorBrushes()
        {
            List<SolidColorBrush> puzzleBrushes = new List<SolidColorBrush>();
            foreach (ColorItem cItem in JigsawColors)
            {
                puzzleBrushes.Add(new SolidColorBrush(cItem.Color.GetValueOrDefault()));
            }
            return puzzleBrushes;
        }
        #endregion
    }
}