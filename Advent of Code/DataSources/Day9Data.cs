using System.IO;

namespace Advent_of_Code.DataSources
{
    public static class Day9Data
    {
        public static string[] GetTestData()
        {
            return new string[]
            {
                "2199943210",
                "3987894921",
                "9856789892",
                "8767896789",
                "9899965678"
            };
        }

        public static string[] GetRealData()
        {
            return File.ReadAllLines(@"..\..\RawInputs\Day 9\input.mos");
        }
    }
}