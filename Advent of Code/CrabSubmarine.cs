using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent_of_Code
{
    public class SubmarineFuelResult
    {
        public int Position { get; set; }
        public long FuelUsed { get; set; }
    }

    public class CrabSubmarine
    {
        public CrabSubmarine(List<int> positions)
        {
            Positions = positions;
        }

        private List<int> Positions { get; }

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
            for (var i = closest; i <= furthest; i++)
            {
                var result = useMethod2 ? GetFuelForPosition2(i) : GetFuelForPosition(i);
                if (bestResult == null || result.FuelUsed < bestResult.FuelUsed) bestResult = result;
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

        private int TriangularNumber(int n)
        {
            return n * (n + 1) / 2;
        }

        private int GetChangeCost(int startingPosition, int desiredPosition)
        {
            var steps = Math.Abs(startingPosition - desiredPosition);
            return TriangularNumber(steps);
        }
    }
}