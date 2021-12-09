using System.Linq;
using Advent_of_Code.DataSources;

namespace Advent_of_Code.DayRunners
{
    public class Day7Runner : DayRunner
    {
        public Day7Runner(DayData data) : base(data)
        {
        }

        protected override void SolveDay(string[] data)
        {
            DoDay7(data);
        }

        private static void DoDay7(string[] inputData)
        {
            var data = inputData.Single().Split(',').Select(int.Parse).ToList();
            var crabSubs = new CrabSubmarine(data);

            var result = crabSubs.FindLeastFuelPosition();
            OutputWriter.WriteResult(1, $"Spent {result.FuelUsed} to align to position {result.Position}");

            result = crabSubs.FindLeastFuelPosition(true);
            OutputWriter.WriteResult(2, $"Spent {result.FuelUsed} to align to position {result.Position}");
        }
    }
}