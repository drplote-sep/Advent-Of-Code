using System.Linq;
using Advent_of_Code.DataSources;

namespace Advent_of_Code.DayRunners
{
    public class Day6Runner : DayRunner
    {
        public Day6Runner(DayData data) : base(data)
        {
        }

        protected override void SolveDay(string[] data)
        {
            DoDay6(data);
        }

        private static void DoDay6(string[] inputData)
        {
            var lanternFish = inputData.Single().Split(',').Select(int.Parse).ToList();

            var school = new LanternFishSchool(lanternFish);
            school.SwimForDays(80);
            OutputWriter.WriteResult(1, $"Num fish after 80 days: {school.GetFishTotal()}");

            var school2 = new LanternFishSchool(lanternFish);
            school2.SwimForDays(256);
            OutputWriter.WriteResult(2, $"Num fish after 256 days: {school2.GetFishTotal()}");
        }
    }
}