using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code
{
    public class SubmarineFuelResult
    {
        public int Position { get; set; }
        public long FuelUsed { get; set; }
    }

    public class CrabSubmarine
    {
        private List<int> Positions { get; }

        public CrabSubmarine(List<int> positions)
        {
            Positions = positions;
        }

        private SubmarineFuelResult GetFuelForPosition(int position)
        {
            return new SubmarineFuelResult
            {
                Position = position,
                FuelUsed = Positions.Select(p => Math.Abs(p - position)).Sum()
            };
        }

        public SubmarineFuelResult FindLeastFuelPosition(bool useMethod2 = false)
        {
            var closest = Positions.Min();
            var furthest = Positions.Max();

            SubmarineFuelResult bestResult = null;
            for (int i = closest; i <= furthest; i++)
            {
                var result = useMethod2 ? GetFuelForPosition2(i) : GetFuelForPosition(i);
                if (bestResult == null || result.FuelUsed < bestResult.FuelUsed)
                {
                    bestResult = result;
                }
            }

            return bestResult;
        }

        private SubmarineFuelResult GetFuelForPosition2(int position)
        {
            return new SubmarineFuelResult
            {
                Position = position,
                FuelUsed = Positions.Select(p => GetChangeCost(p, position)).Sum()
            };
        }

        private int GetChangeCost(int startingPosition, int desiredPosition)
        {
            var steps = Math.Abs(startingPosition - desiredPosition);
            var extraStepCost = 0;
            for (int i = 2; i <= steps; i++)
            {
                extraStepCost += (i - 1);
            }
            return steps + extraStepCost;
        }
    }
}
