using System.IO;

namespace Advent_of_Code.DataSources
{
    public static class Day6Data
    {
        public static string[] GetTestData()
        {
            return new string[]
            {
                "3,4,3,1,2"
            };
        }

        public static string[] GetRealData()
        {
            return File.ReadAllLines(@"..\..\RawInputs\Day 6\input.mos");
        }
    }
}