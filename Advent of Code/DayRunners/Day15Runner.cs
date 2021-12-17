using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Advent_of_Code.DataSources;

namespace Advent_of_Code.DayRunners
{
    class PriorityQueue<TItem, TPriority> where TPriority : IComparable
    {
        private SortedList<TPriority, Queue<TItem>> pq = new SortedList<TPriority, Queue<TItem>>();
        public int Count { get; private set; }

        public void Enqueue(TItem item, TPriority priority)
        {
            ++Count;
            if (!pq.ContainsKey(priority)) pq[priority] = new Queue<TItem>();
            pq[priority].Enqueue(item);
        }

        public TItem Dequeue()
        {
            --Count;
            var queue = pq.ElementAt(0).Value;
            if (queue.Count == 1) pq.RemoveAt(0);
            return queue.Dequeue();
        }

        public bool Any() => Count > 0;
    }
    
    public class Day15Runner : DayRunner
    {
        public Dictionary<Point, int> gScore { get; set; } = new Dictionary<Point, int>();
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

        private int FindBestPath()
        {
            gScore = new Dictionary<Point, int>();
            
            var start = new Point (0, 0);

            var openSet = new PriorityQueue<Point, int>();
            openSet.Enqueue(start, GetDistanceToEnd(start));

            gScore[start] = 0;

            while (openSet.Any())
            {
                var current = openSet.Dequeue();

                var neighbors = GetNeighbors(current);

                foreach (var neighbor in neighbors)
                {
                    if (neighbor == EndPoint)
                    {
                        return GetG(current) + Risks[EndPoint];
                    }
                    
                    var tentativeG = GetG(current) + Risks[neighbor];
                    if (tentativeG < GetG(neighbor))
                    {
                        gScore[neighbor] = tentativeG;
                        openSet.Enqueue(neighbor, tentativeG + GetDistanceToEnd(neighbor));
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