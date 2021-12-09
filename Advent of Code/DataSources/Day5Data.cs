using System.IO;

namespace Advent_of_Code.DataSources
{
    public static class Day5Data
    {
        public static string[] GetTestData()
        {
            return new string[]
            {
                "0,9 -> 5,9",
                "8,0 -> 0,8",
                "9,4 -> 3,4",
                "2,2 -> 2,1",
                "7,0 -> 7,4",
                "6,4 -> 2,0",
                "0,9 -> 2,9",
                "3,4 -> 1,4",
                "0,0 -> 8,8",
                "5,5 -> 8,2"
            };
        }

        public static string[] GetRealData()
        {
            return File.ReadAllLines(@"..\..\RawInputs\Day 5\input.mos");
        }
    }
}
