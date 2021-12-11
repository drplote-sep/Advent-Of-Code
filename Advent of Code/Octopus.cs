using System.Drawing;

namespace Advent_of_Code
{
    public class Octopus
    {
        public Point Location { get; }
        public int Energy { get; private set; } = 0;
        public int FlashCount { get; private set; } = 0;
        public bool FlashedThisStep { get; private set; } = false;

        public Octopus(int x, int y, int energy)
        {
            Location = new Point(x, y);
            Energy = energy;
        }

        public void Step()
        {
            FlashedThisStep = false;
            IncreaseEnergy();
        }

        public void IncreaseEnergy()
        {
            if (FlashedThisStep)
            {
                // Can't increase if you already flashed this step
                return;
            }

            Energy++;
            if (Energy == 10)
            {
                Energy = 0;
                FlashedThisStep = true;
                FlashCount++;
            }
        }
    }
}