using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Advent_of_Code.DataSources;

namespace Advent_of_Code.DayRunners
{
    public class CaveTile
    {
        public Point Point { get; set; }
        public int Cost { get; set; }
        public int Distance { get; set; }
        public int CostDistance => Cost + Distance;


        public void SetDistance(int targetX, int targetY)
        {
            Distance = Math.Abs(targetX - Point.X) + Math.Abs(targetY - Point.Y);
        }

        public void SetDistance(Point p)
        {
            SetDistance(p.X, p.Y);
        }
    }

    public class Day15Runner : DayRunner
    {
        public Dictionary<Point, int> gScore { get; set; } = new Dictionary<Point, int>();
        public Dictionary<Point, int> fScore { get; set; } = new Dictionary<Point, int>();
        public Dictionary<Point, int> Risks { get; set; }

        public Day15Runner(DayData data) : base(data)
        {
        }

        protected override void SolveDay(string[] data)
        {
            ParseInputs(data);

            OutputWriter.WriteResult(1, $"Lowest Risk Path: {FindBestPath()}");
            ParseInputs(data, 5);
            OutputWriter.WriteResult(2, $"Lowest Risk Path: {FindBestPath()}");
        }

        // private int FindBestPath2()
        // {
        //     var start = new Point(0, 0);
        //     var openSet = new List<Point> { start };
        //
        //     var cameFrom = new List<Point>();
        //
        //     var gScore = new Dictionary<Point, int>
        //     {
        //         [start] = 0
        //     };
        //
        //     var fScore = new Dictionary<Point, int>
        //     {
        //         [start] = GetDistanceToEnd(start)
        //     };
        //
        //     while (openSet.Any()) // Optimize by making openSet an openHeap or priority queue
        //     {
        //         var current = openSet.OrderBy()
        //     }
        // }

        private int GetDistanceToEnd(Point p)
        {
            return Math.Abs(EndPoint.X - p.X) + Math.Abs(EndPoint.Y - p.Y);
        }

        private int GetG(Point p)
        {
            if (!gScore.ContainsKey(p))
            {
                return int.MaxValue;
            }

            return gScore[p];
        }

        private int GetF(Point p)
        {
            if (!fScore.ContainsKey(p))
            {
                return int.MaxValue;
            }

            return fScore[p];
        }

        private int FindBestPath()
        {
            gScore = new Dictionary<Point, int>();
            fScore = new Dictionary<Point, int>();
            
            var start = new Point (0, 0);
            
            var openSet = new HashSet<Point> { start };

            gScore[start] = 0;
            fScore[start] = GetDistanceToEnd(start);

            while (openSet.Any())
            {
                var current = openSet.OrderBy(GetF).First();

                if (current == EndPoint)
                {
                    return GetG(current);
                }
                
                openSet.Remove(current);

                var neighbors = GetNeighbors(current);

                foreach (var neighbor in neighbors)
                {
                    var tentativeG = GetG(current) + Risks[neighbor];
                    if (tentativeG < GetG(neighbor))
                    {
                        gScore[neighbor] = tentativeG;
                        fScore[neighbor] = tentativeG + GetDistanceToEnd(neighbor);
                        openSet.Add(neighbor);
                    }
                }
            }

            return -1;
        }

        public List<Point> GetNeighbors(Point p)
        {
            var adjacentPoints = new List<Point>
            {
                new Point(p.X, p.Y - 1),
                new Point(p.X, p.Y + 1),
                new Point(p.X - 1, p.Y),
                new Point(p.X + 1, p.Y),
            };

            return adjacentPoints.Where(Risks.ContainsKey).ToList();
        }

        private void ParseInputs(string[] data, int inputMultiplier = 1)
        {
            Risks = new Dictionary<Point, int>();
            var baseWidth = data.First().Length;
            var baseHeight = data.Length;
            var fullWidth = baseWidth * inputMultiplier;
            var fullHeight = baseHeight * inputMultiplier;
            for (int y = 0; y < fullHeight; y++)
            {
                for (int x = 0; x < fullWidth; x++)
                {
                    Risks[new Point(x, y)] = GetRiskValue(data, x, y);
                }
            }

            EndPoint = new Point(fullWidth - 1, fullHeight - 1);
        }

        public Point EndPoint { get; set; }

        private int GetRiskValue(string[] data, int x, int y)
        {
            var baseWidth = data.First().Length;
            var baseHeight = data.Length;

            var fileRisk = Convert.ToInt32(char.GetNumericValue(data[y % baseWidth][x % baseWidth]));

            return WrapRisk(fileRisk + x / baseWidth + y / baseHeight);
        }

        private int WrapRisk(int risk)
        {
            if (risk < 10)
            {
                return risk;
            }

            return WrapRisk(risk - 9);
        }
    }
}