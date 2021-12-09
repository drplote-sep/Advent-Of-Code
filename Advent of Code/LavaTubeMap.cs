using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Advent_of_Code
{
    public class LavaTubeMap
    {
        public List<List<int>> Layout;

        public LavaTubeMap(string[] inputs)
        {
            Layout = new List<List<int>>();
            foreach (var s in inputs) Layout.Add(s.Select(c => c.ToString()).Select(int.Parse).ToList());
        }

        public List<Point> GetLowPoints()
        {
            var lowestPoints = new List<Point>();
            var maxX = Layout.First().Count - 1;
            var maxY = Layout.Count - 1;

            for (var x = 0; x <= maxX; x++)
            for (var y = 0; y <= maxY; y++)
            {
                var point = new Point(x, y);
                var height = GetHeightAtPoint(point);
                var isLowerThanNeighbor = true;

                if (isLowerThanNeighbor && x != 0) isLowerThanNeighbor &= height < GetHeightAtPoint(x - 1, y);

                if (isLowerThanNeighbor && x != maxX) isLowerThanNeighbor &= height < GetHeightAtPoint(x + 1, y);

                if (isLowerThanNeighbor && y != 0) isLowerThanNeighbor &= height < GetHeightAtPoint(x, y - 1);

                if (isLowerThanNeighbor && y != maxY) isLowerThanNeighbor &= height < GetHeightAtPoint(x, y + 1);

                if (isLowerThanNeighbor) lowestPoints.Add(point);
            }

            return lowestPoints;
        }

        public int GetRisk(List<Point> points)
        {
            return points.Sum(GetHeightAtPoint) + points.Count;
        }

        public int GetHeightAtPoint(Point p)
        {
            var maxX = Layout.First().Count - 1;
            var maxY = Layout.Count - 1;

            if (p.Y < 0 || p.Y > maxY || p.X < 0 || p.X > maxX) return 0;
            return Layout[p.Y][p.X];
        }

        public int GetHeightAtPoint(int x, int y)
        {
            return GetHeightAtPoint(new Point(x, y));
        }

        public int GetProductOfSizeOfThreeLargestBasins(List<Point> lowestPoints)
        {
            var basins = new List<HashSet<Point>>();
            foreach (var point in lowestPoints) basins.Add(GetPointsInBasin(point));

            return basins.OrderByDescending(b => b.Count).Take(3).Select(b => b.Count).Aggregate((a, x) => a * x);
        }

        private HashSet<Point> GetPointsInBasin(Point point)
        {
            var pointsInBasin = new HashSet<Point>();

            var height = GetHeightAtPoint(point);
            if (height < 9)
            {
                pointsInBasin.Add(point);
                var adjacentPoints = new List<Point>
                {
                    new Point(point.X, point.Y + 1),
                    new Point(point.X, point.Y - 1),
                    new Point(point.X - 1, point.Y),
                    new Point(point.X + 1, point.Y)
                };

                foreach (var adjacentPoint in adjacentPoints)
                    if (GetHeightAtPoint(adjacentPoint) > height)
                        pointsInBasin.UnionWith(GetPointsInBasin(adjacentPoint));
            }

            return pointsInBasin;
        }
    }
}