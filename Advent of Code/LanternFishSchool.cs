using System.Collections.Generic;

namespace Advent_of_Code
{
    public class LanternFishSchool
    {
        public LanternFishSchool(List<int> initialFish)
        {
            Fish = ParseFishIntoGroups(initialFish);
        }

        public int DaysPassed { get; private set; }
        private List<long> Fish { get; set; }

        private List<long> ParseFishIntoGroups(List<int> initialFish)
        {
            var fishList = new List<long> {0, 0, 0, 0, 0, 0, 0, 0, 0};
            foreach (var fish in initialFish) fishList[fish] = fishList[fish] + 1;

            return fishList;
        }

        public long GetFishTotal()
        {
            long total = 0;
            foreach (var count in Fish) total += count;
            return total;
        }

        public void SwimForDays(int numDays)
        {
            for (var i = 0; i < numDays; i++) NextDay();
        }

        public void NextDay()
        {
            DaysPassed++;

            var newFish = new List<long> {0, 0, 0, 0, 0, 0, 0, 0, 0};
            for (var i = 0; i < 9; i++)
                if (i == 0)
                {
                    newFish[6] = Fish[0];
                    newFish[8] = Fish[0];
                }
                else
                {
                    newFish[i - 1] = newFish[i - 1] + Fish[i];
                }

            Fish = newFish;
        }
    }
}