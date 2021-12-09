namespace Advent_of_Code.DataSources
{
    public class Day5Data : DayData
    {
        public Day5Data() : base(5)
        {
        }

        public override string[] GetTestData()
        {
            return new[]
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
    }
}