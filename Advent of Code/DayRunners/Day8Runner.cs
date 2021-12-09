using Advent_of_Code.DataSources;

namespace Advent_of_Code.DayRunners
{
    public class Day8Runner : DayRunner
    {
        public Day8Runner(DayData data) : base(data)
        {
        }

        protected override void SolveDay(string[] data)
        {
            DoDay8(data);
        }

        private static void DoDay8(string[] inputData)
        {
            var display = new SevenSegmentDisplay(inputData);
            OutputWriter.WriteResult(1, $"Times 1,4,7,or 8 appear in output: {display.GetPartOneAnswer()}");
            OutputWriter.WriteResult(2, $"Sum of decoded outputs: {display.GetPartTwoAnswer()}");
        }
    }
}