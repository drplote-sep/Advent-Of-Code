using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Dynamic;
using System.Linq;
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

            var start = GetTile(0, 0);
            var finish = GetTile(data.First().Length - 1, data.Length - 1);

            var activeTiles = new List<CaveTile>();
            activeTiles.Add(start);

            var visitedTiles = new List<CaveTile>();

            while (activeTiles.Any())
            {
                var checkTile = activeTiles.OrderBy(x => x.CostDistance).First();

                if (checkTile.X == finish.X && checkTile.Y == finish.Y)
                {
                    OutputWriter.WriteResult(1, $"Risk: {checkTile.Cost}");
                    //We can actually loop through the parents of each tile to find our exact path which we will show shortly. 
                    return;
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
                        if (existingTile.CostDistance > checkTile.CostDistance)
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

        private void ParseInputs(string[] data)
        {
            Tiles = new List<CaveTile>();
            var endPoint = new Point(data.First().Length - 1, data.Length - 1);
            for (int y = 0; y < data.Length; y++)
            {
                var line = data[y];
                for (int x = 0; x < line.Length; x++)
                {
                    var tile = new CaveTile
                    {
                        X = x,
                        Y = y,
                        Risk = Convert.ToInt32(char.GetNumericValue(data[y][x]))
                    };
                    tile.SetDistance(endPoint.X, endPoint.Y);
                    Tiles.Add(tile);
                }

            }
        }
    }
}
