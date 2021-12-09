using System;
using System.Collections.Generic;
using System.Drawing;

namespace Advent_of_Code
{
    public class ThermalVentLine
    {
        private Point A { get; }
        private Point B { get; }

        public ThermalVentLine(Point a, Point b)
        {
            A = a;
            B = b;
        }

        public List<Point> GetPointsInLine(bool includeDiagonals = false)
        {
            if (A.X == B.X)
            {
                return GenerateVerticalLine(A.X, Math.Min(A.Y, B.Y), Math.Max(A.Y, B.Y));
            }
            if (A.Y == B.Y)
            {
                return GenerateHorizontalLine(Math.Min(A.X, B.X), Math.Max(A.X, B.X), A.Y);
            }

            return includeDiagonals ? GenerateDiagonalLine() : new List<Point>();
        }

        private List<Point> GenerateDiagonalLine()
        {
            var points = new List<Point>();

            bool xIncreasing = A.X < B.X;
            bool yIncreasing = A.Y < B.Y;
            var numSteps = Math.Abs(A.X - B.X) + 1;

            for (int i = 0; i < numSteps; i++)
            {
                var newX = A.X + (i * (xIncreasing ? 1 : -1));
                var newY = A.Y + (i * (yIncreasing ? 1 : -1));
                points.Add(new Point(newX, newY));
            }
            
            return points;
        }

        private List<Point> GenerateHorizontalLine(int minX, int maxX, int y)
        {
            var points = new List<Point>();
            for (int i = minX; i <= maxX; i++)
            {
                points.Add(new Point(i, y));
            }
            return points;
        }

        private List<Point> GenerateVerticalLine(int x, int minY, int maxY)
        {
            var points = new List<Point>();
            for (int i = minY; i <= maxY; i++)
            {
                points.Add(new Point(x, i));
            }
            return points;
        }
    }
}
