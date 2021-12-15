using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Advent_of_Code.DataSources;

namespace Advent_of_Code.DayRunners
{
    public class CaveTile
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Risk { get; set; }
        public int Cost { get; set; }
        public int Distance { get; set; }
        public int CostDistance => Cost + Distance;
        public CaveTile Parent { get; set; }
        

        public void SetDistance(int targetX, int targetY)
        {
            Distance = Math.Abs(targetX - X) + Math.Abs(targetY - Y);
        }

        public CaveTile CloneForParent(CaveTile parent)
        {
            return new CaveTile
            {
                X = X,
                Y = Y,
                Risk = Risk,
                Cost = parent.Cost + Risk,
                Distance = Distance,
                Parent = parent
            };
        }
    }

    public class Day15Runner : DayRunner
    {
        public List<CaveTile> Tiles = new List<CaveTile>();

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

        public void PrintTiles()
        {
            var sb = new StringBuilder();
            foreach(var tileGroup in Tiles.GroupBy(t => t.Y).OrderBy(g => g.Key))
            {
                foreach(var tile in tileGroup)
                {
                    sb.Append(tile.Risk);
                }
                sb.Append(System.Environment.NewLine);
                
            }
            OutputWriter.WriteLine(sb.ToString());
        }

        private int FindBestPath()
        {
            var start = Tiles.First();
            var finish = Tiles.Last();

            var activeTiles = new List<CaveTile>();
            activeTiles.Add(start);

            var visitedTiles = new List<CaveTile>();

            while (activeTiles.Any())
            {
                var checkTile = activeTiles.OrderBy(x => x.CostDistance).First();

                if (checkTile.X == finish.X && checkTile.Y == finish.Y)
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
                    if (visitedTiles.Any(x => x.X == walkableTile.X && x.Y == walkableTile.Y))
                        continue;

                    //It's already in the active list, but that's OK, maybe this new tile has a better value (e.g. We might zigzag earlier but this is now straighter). 
                    if (activeTiles.Any(x => x.X == walkableTile.X && x.Y == walkableTile.Y))
                    {
                        var existingTile = activeTiles.First(x => x.X == walkableTile.X && x.Y == walkableTile.Y);
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

        public List<CaveTile> GetAdjacentTiles(CaveTile tile)
        {
            var adjacent = new List<CaveTile>
            {
                GetTile(tile.X + 1, tile.Y)?.CloneForParent(tile),
                GetTile(tile.X - 1, tile.Y)?.CloneForParent(tile),
                GetTile(tile.X, tile.Y + 1)?.CloneForParent(tile),
                GetTile(tile.X, tile.Y - 1)?.CloneForParent(tile)
            };
            return adjacent.Where(t => t!= null).ToList();
        }

        public CaveTile GetTile(int x, int y)
        {
            return Tiles.SingleOrDefault(t => t.X == x && t.Y == y);
        }

        private void ParseInputs(string[] data, int inputMultiplier = 1)
        {
            Tiles = new List<CaveTile>();
            var baseWidth = data.First().Length;
            var baseHeight = data.Length;
            var fullWidth = baseWidth * inputMultiplier;
            var fullHeight = baseHeight * inputMultiplier;
            var endPoint = new Point(fullWidth - 1, fullHeight - 1);
            for (int y = 0; y < fullHeight; y++)
            {

                for (int x = 0; x < fullWidth; x++)
                {
                    var tile = new CaveTile
                    {
                        X = x,
                        Y = y,
                        Risk = GetRiskValue(data, x, y)
                    };
                    tile.SetDistance(endPoint.X, endPoint.Y);
                    Tiles.Add(tile);
                }
            }
        }

        private int GetRiskValue(string[] data, int x, int y)
        {
            var baseWidth = data.First().Length;
            var baseHeight = data.Length;

            var fileRisk = Convert.ToInt32(char.GetNumericValue(data[y % baseWidth][x % baseWidth]));

            return WrapRisk(fileRisk + x/baseWidth + y/baseHeight);
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
