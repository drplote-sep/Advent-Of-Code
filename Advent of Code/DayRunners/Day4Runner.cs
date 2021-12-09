using System.Collections.Generic;
using System.Linq;
using Advent_of_Code.DataSources;

namespace Advent_of_Code.DayRunners
{
    public class Day4Runner : DayRunner
    {
        public Day4Runner(DayData data) : base(data)
        {
        }

        protected override void SolveDay(string[] data)
        {
            DoDay4(data);
        }

        private static List<BingoBoard> ParseBingoBoards(IEnumerable<string> lines)
        {
            var boards = new List<BingoBoard>();
            var boardRows = new List<List<int>>();
            foreach (var line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    var lineNums = line.Split(' ').Where(s => !string.IsNullOrWhiteSpace(s)).Select(int.Parse).ToList();
                    boardRows.Add(lineNums);
                }

                if (boardRows.Count == 5)
                {
                    boards.Add(new BingoBoard(boardRows));
                    boardRows = new List<List<int>>();
                }
            }

            return boards;
        }

        private static void DoDay4(string[] day4Input)
        {
            var bingoNumbers = day4Input.First().Split(',').Select(int.Parse);
            var bingoBoards = ParseBingoBoards(day4Input.Skip(1));
            var haveFirstWinner = false;
            foreach (var bingoNum in bingoNumbers)
            foreach (var board in bingoBoards)
                if (!board.HasWon)
                {
                    var isWinner = board.HandleNumberSelected(bingoNum);
                    if (isWinner)
                    {
                        if (!haveFirstWinner)
                        {
                            haveFirstWinner = true;
                            var score = board.GetSumUnmarked() * bingoNum;
                            OutputWriter.WriteResult(1, $"Winning bingo card score: {score}");
                        }

                        var unwonBoards = bingoBoards.Count(b => !b.HasWon);
                        if (unwonBoards == 1)
                        {
                            var score = board.GetSumUnmarked() * bingoNum;
                            OutputWriter.WriteResult(2, $"Latest winning bingo card score: {score}");
                        }

                        board.HasWon = true;
                    }
                }
        }
    }
}