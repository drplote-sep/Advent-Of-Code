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
        public CaveTile Parent { get; set; }


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

        private int FindBestPath()
        {
            var startTile = new CaveTile
            {
                Point = new Point(0, 0),
                Cost = 0
            };
            startTile.SetDistance(EndPoint);

            var endTile = new CaveTile
            {
                Point = EndPoint,
                Cost = Risks[EndPoint]
            };

            var activeTiles = new List<CaveTile>();
            activeTiles.Add(startTile);

            var visitedTiles = new List<CaveTile>();

            while (activeTiles.Any())
            {
                var checkTile = activeTiles.OrderBy(x => x.CostDistance).First();

                if (checkTile.Point == endTile.Point)
                {
                    //We can actually loop through the parents of each tile to find our exact path which we will show shortly. 
                    return checkTile.Cost;
                }

                visitedTiles.Add(checkTile);
                activeTiles.Remove(checkTile);

                var adjacentTiles = GetAdjacentTiles(checkTile);

                foreach (var walkableTile in adjacentTiles)
                {
                    //We have already visited this tile so we don't need to do so again!
                    if (visitedTiles.Any(t => t.Point == walkableTile.Point))
                        continue;

                    //It's already in the active list, but that's OK, maybe this new tile has a better value (e.g. We might zigzag earlier but this is now straighter). 
                    if (activeTiles.Any(t => t.Point == walkableTile.Point))
                    {
                        var existingTile = activeTiles.First(t => t.Point == walkableTile.Point);
                        if (existingTile.CostDistance > walkableTile.CostDistance)
                        {
                            activeTiles.Remove(existingTile);
                            activeTiles.Add(walkableTile);
                        }
                    }
                    else
                    {
                        //We've never seen this tile before so add it to the list. 
                        activeTiles.Add(walkableTile);
                    }
                }
            }

            return 0;
        }

        public List<CaveTile> GetAdjacentTiles(CaveTile currentTile)
        {
            var adjacentPoints = new List<Point>
            {
                new Point(currentTile.Point.X, currentTile.Point.Y - 1),
                new Point(currentTile.Point.X, currentTile.Point.Y + 1),
                new Point(currentTile.Point.X - 1, currentTile.Point.Y),
                new Point(currentTile.Point.X + 1, currentTile.Point.Y),
            };

            var adjacentTiles = adjacentPoints.Where(Risks.ContainsKey).Select(p =>
            {
                var tile = new CaveTile
                {
                    Point = p,
                    Parent = currentTile,
                    Cost = currentTile.Cost + Risks[p]
                };
                tile.SetDistance(EndPoint);
                return tile;
            });

            return adjacentTiles.ToList();
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