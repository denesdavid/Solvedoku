using System;
using System.Collections.Generic;
using System.Linq;

namespace Solvedoku.Classes
{
    public enum SudokuProgress { FAILED, NO_PROGRESS, PROGRESS }

    [Serializable]
    public class SudokuTile
    {
        #region Fields
        int _maxValue;
        int _value;
        int _row;
        int _column;
        ISet<int> _possibleValues;
        bool _blocked;
        #endregion

        #region Constants
        public const int CLEARED = 0;
        #endregion

        #region Properties
        public int Value
        {
            get => _value;
            set
            {
                if (value > _maxValue)
                {
                    throw new ArgumentOutOfRangeException("SudokuTile Value cannot be greater than " + _maxValue.ToString() + ". Was " + value);
                }

                if (value < CLEARED)
                {
                    throw new ArgumentOutOfRangeException("SudokuTile Value cannot be zero or smaller. Was " + value);
                }

                _value = value;
            }
        }

        public int Row { get => _row; }

        public int Column { get => _column; }

        public bool IsBlocked { get => _blocked; } // A blocked field can not contain a value -- used for creating 'holes' in the map

        public int PossibleCount { get => IsBlocked ? 1 : _possibleValues.Count; }

        #endregion

        #region Constructor
        public SudokuTile(int row, int column, int maxValue)
        {
            _row = row;
            _column = column;
            _blocked = false;
            _maxValue = maxValue;
            _possibleValues = new HashSet<int>();
            _value = 0;
        }
        #endregion

        #region Functions
        internal SudokuProgress RemovePossibles(IEnumerable<int> existingNumbers)
        {
            if (_blocked)
            {
                return SudokuProgress.NO_PROGRESS;
            }

            // Takes the current possible values and removes the ones existing in `existingNumbers`
            _possibleValues = new HashSet<int>(_possibleValues.Where(x => !existingNumbers.Contains(x)));
            SudokuProgress result = SudokuProgress.NO_PROGRESS;
            if (_possibleValues.Count == 1)
            {
                Fix(_possibleValues.First());
                result = SudokuProgress.PROGRESS;
            }
            if (_possibleValues.Count == 0)
            {
                return SudokuProgress.FAILED;
            }

            return result;
        }

        internal static SudokuProgress CombineSolvedState(SudokuProgress a, SudokuProgress b)
        {
            if (a == SudokuProgress.FAILED) { return a; }

            if (a == SudokuProgress.NO_PROGRESS) { return b; }

            if (a == SudokuProgress.PROGRESS)
            { return b == SudokuProgress.FAILED ? b : a; }

            throw new InvalidOperationException("Invalid value for a");
        }

        public void Block() => _blocked = true;

        public bool IsValuePossible(int i) => _possibleValues.Contains(i);

        public bool HasValue => Value != CLEARED;

        public string ToStringSimple() => Value.ToString();

        public override string ToString() =>
            String.Format("Value {0} at pos {1}, {2}. ", Value, _row, _column, _possibleValues.Count);

        #endregion

        #region Methods
        internal void ResetPossibles()
        {
            _possibleValues.Clear();
            foreach (int i in Enumerable.Range(1, _maxValue))
            {
                if (!HasValue || Value == i)
                {
                    _possibleValues.Add(i);
                }
            }
        }

        internal void Fix(int value)
        {
            //Console.WriteLine("Fixing {0} on pos {1}, {2}: {3}", value, _x, _y, reason);
            Value = value;
            ResetPossibles();
        }
        #endregion
    }
}