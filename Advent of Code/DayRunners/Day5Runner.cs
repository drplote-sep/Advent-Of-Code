using System.Linq;
using Advent_of_Code.DataSources;
using Advent_of_Code.Parsers;

namespace Advent_of_Code.DayRunners
{
    public class Day5Runner : DayRunner
    {
        public Day5Runner(DayData data) : base(data)
        {
        }

        protected override void SolveDay(string[] data)
        {
            DoDay5(data);
        }

        private static void DoDay5(string[] day5Input)
        {
            var inputs = Day5Parser.Parse(day5Input);
            var ventLines = inputs.Select(i => new ThermalVentLine(i.A, i.B)).ToList();

            var ventGrid = new VentGrid();
            var ventGridWithDiagonals = new VentGrid();
            foreach (var ventLine in ventLines)
            {
                ventGrid.AddLine(ventLine);
                ventGridWithDiagonals.AddLine(ventLine, true);
            }

            var atLeastTwoOverlaps = ventGrid.GetOverlapCount(2);
            OutputWriter.WriteResult(1, $"Overlap at least twice: {atLeastTwoOverlaps}");

            var atLeastTwoOverlapsWithDiagonals = ventGridWithDiagonals.GetOverlapCount(2);
            OutputWriter.WriteResult(2,
                $"Day 5.2: Overlap at least twice (with diagonals): {atLeastTwoOverlapsWithDiagonals}");
        }
    }
}