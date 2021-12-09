using System.IO;

namespace Advent_of_Code.DataSources
{
    public static class Day7Data
    {
        public static string[] GetTestData()
        {
            return new string[]
            {
                "16,1,2,0,4,2,7,1,2,14"
            };
        }

        public static string[] GetRealData()
        {
            return File.ReadAllLines(@"..\..\RawInputs\Day 7\input.mos");
        }
    }
}