using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Advent_of_Code
{
    public class VentGrid
    {
        private Dictionary<Point, int> PointCount { get; }

        public VentGrid()
        {
            PointCount = new Dictionary<Point, int>();
        }

        public void AddLine(ThermalVentLine line, bool includeDiagonals = false)
        {
            var points = line.GetPointsInLine(includeDiagonals);
            foreach (var point in points)
            {
                AddPoint(point);
            }
        }

        private void AddPoint(Point point)
        {
            if (!PointCount.ContainsKey(point))
            {
                PointCount[point] = 0;
            }

            PointCount[point] = PointCount[point] + 1;
        }

        public int GetOverlapCount(int minOverlap)
        {
            return PointCount.Count(e => e.Value >= minOverlap);
        }
    }
}
