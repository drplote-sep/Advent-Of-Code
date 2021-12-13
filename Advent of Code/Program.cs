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

            foreach (var dayRunner in GetDayRunners())
            {
                dayRunner.Go();
            }
        }

        private static List<DayRunner> GetDayRunners()
        {
            return new List<DayRunner>
            {
                new Day1Runner(new DayData(1)),
                new Day2Runner(new DayData(2)),
                new Day3Runner(new DayData(3)),
                new Day4Runner(new DayData(4)),
                new Day5Runner(new DayData(5)),
                new Day6Runner(new DayData(6)),
                new Day7Runner(new DayData(7)),
                new Day8Runner(new DayData(8)),
                new Day9Runner(new DayData(9)),
                new Day10Runner(new DayData(10)),
                new Day11Runner(new DayData(11)),
                new Day12Runner(new DayData(12)),
                new Day13Runner(new DayData(13)),
				new Day14Runner(new DayData(14))
            };
        }
    }
}