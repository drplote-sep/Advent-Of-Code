using System.IO;

namespace Advent_of_Code.DataSources
{
    public class DayData
    {
        public DayData(int dayNumber)
        {
            DayNumber = dayNumber;
        }

        public int DayNumber { get; }

        public virtual string[] GetTestData()
        {
            return new string[] { };
        }

        public virtual string[] GetRealData()
        {
            return File.ReadAllLines($"..\\..\\RawInputs\\Day {DayNumber}\\input.mos");
        }
    }
}