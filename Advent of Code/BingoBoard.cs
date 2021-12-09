using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent_of_Code
{
    public class BingoBoard
    {
        private readonly List<List<bool>> _isMarked = new List<List<bool>>
        {
            new List<bool> {false, false, false, false, false},
            new List<bool> {false, false, false, false, false},
            new List<bool> {false, false, false, false, false},
            new List<bool> {false, false, false, false, false},
            new List<bool> {false, false, false, false, false}
        };

        private readonly List<List<int>> _rows;

        public BingoBoard(List<List<int>> rows)
        {
            if (rows.Count != 5 || rows.Any(r => r.Count != 5)) throw new ArgumentException("Card must be 5x5");

            _rows = rows;
        }

        public bool HasWon { get; set; }

        public bool HandleNumberSelected(int num)
        {
            if (HasWon) return true;
            for (var rowNum = 0; rowNum < 5; rowNum++)
            {
                var row = _rows[rowNum];
                for (var colNum = 0; colNum < 5; colNum++)
                {
                    var colValue = row[colNum];
                    if (colValue == num) _isMarked[rowNum][colNum] = true;
                }
            }

            return IsWinner();
        }

        public bool IsWinner()
        {
            for (var rowNum = 0; rowNum < 5; rowNum++)
                if (IsWholeRowMarked(rowNum))
                    return true;

            for (var colNum = 0; colNum < 5; colNum++)
                if (IsWholeColumnMarked(colNum))
                    return true;

            return false;
        }

        private bool IsWholeRowMarked(int rowNum)
        {
            return _isMarked[rowNum].All(m => m);
        }

        private bool IsWholeColumnMarked(int column)
        {
            return _isMarked.Select(r => r[column]).All(m => m);
        }

        private bool IsDiagonalMarked()
        {
            if (_isMarked[2][2])
                return _isMarked[0][0] && _isMarked[1][1] && _isMarked[3][3] && _isMarked[4][4]
                       || _isMarked[0][4] && _isMarked[1][3] && _isMarked[3][1] && _isMarked[4][0];

            return false;
        }

        public int GetSumUnmarked()
        {
            var sum = 0;
            for (var rowNum = 0; rowNum < 5; rowNum++)
            for (var colNum = 0; colNum < 5; colNum++)
                if (!_isMarked[rowNum][colNum])
                    sum += _rows[rowNum][colNum];

            return sum;
        }
    }
}