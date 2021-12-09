using Advent_of_Code.DataSources;

namespace Advent_of_Code.DayRunners
{
    public class Day9Runner : DayRunner
    {
        public Day9Runner(DayData data) : base(data)
        {
        }

        protected override void SolveDay(string[] data)
        {
            DoDay9(data);
        }

        private static void DoDay9(string[] inputs)
        {
            var lavaTubeMap = new LavaTubeMap(inputs);
            var lowestPoints = lavaTubeMap.GetLowPoints();
            OutputWriter.WriteResult(1, $"Lowest points risk: {lavaTubeMap.GetRisk(lowestPoints)}");
            OutputWriter.WriteResult(2,
                $"Product of Size of 3 largest basins: {lavaTubeMap.GetProductOfSizeOfThreeLargestBasins(lowestPoints)}");
        }
    }
}