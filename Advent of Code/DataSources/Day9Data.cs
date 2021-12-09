namespace Advent_of_Code.DataSources
{
    public class Day9Data : DayData
    {
        public Day9Data() : base(9)
        {
        }

        public override string[] GetTestData()
        {
            return new[]
            {
                "2199943210",
                "3987894921",
                "9856789892",
                "8767896789",
                "9899965678"
            };
        }
    }
}