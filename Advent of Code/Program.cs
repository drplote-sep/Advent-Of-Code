using System.Collections.Generic;
using Advent_of_Code.DataSources;
using Advent_of_Code.DayRunners;

namespace Advent_of_Code
{
    internal class Day1Program
    {
        private static void Main(string[] args)
        {
            OutputWriter.WriteHeader("Advent of Code 2021");

            foreach (var dayRunner in GetDayRunners()) dayRunner.Go();
        }

        private static List<DayRunner> GetDayRunners()
        {
            return new List<DayRunner>
            {
                new Day1Runner(new DayData(1)),
                new Day2Runner(new DayData(2)),
                new Day3Runner(new DayData(3)),
                new Day4Runner(new DayData(4)),
                new Day5Runner(new Day5Data()),
                new Day6Runner(new Day6Data()),
                new Day7Runner(new Day7Data()),
                new Day8Runner(new Day8Data()),
                new Day9Runner(new Day9Data())
            };
        }
    }
}