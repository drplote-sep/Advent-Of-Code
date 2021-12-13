using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using Advent_of_Code.DataSources;

namespace Advent_of_Code.DayRunners
{
    public class DotGrid
    {
        public HashSet<Point> Dots { get; private set; }

        public DotGrid(HashSet<Point> initialDots)
        {
            Dots = initialDots;
        }

        public void Fold(FoldInstruction instruction)
        {
            Dots = instruction.Fold(Dots);
        }

        public override string ToString()
        {
            var maxX = Dots.Max(d => d.X);
            var maxY = Dots.Max(d => d.Y);

            var sb = new StringBuilder();
            for (int y = 0; y <= maxY; y++)
            {
                for (int x = 0; x <= maxX; x++)
                {
                    if (Dots.Any(d => d.X == x && d.Y == y))
                    {
                        sb.Append("#");
                    }
                    else
                    {
                        sb.Append(" ");
                    }
                }

                sb.Append(System.Environment.NewLine);
            }
            return sb.ToString();
        }
    }

    public abstract class FoldInstruction
    {
        public static FoldInstructionX CreateXFold(int coordinate)
        {
            return new FoldInstructionX(coordinate);
        }

        public static FoldInstructionY CreateYFold(int coordinate)
        {
            return new FoldInstructionY(coordinate);
        }

        public virtual HashSet<Point> Fold(HashSet<Point> dots)
        {
            var newPoints = new HashSet<Point>();
            foreach (var dot in dots)
            {
                newPoints.Add(FoldPoint(dot));
            }

            return newPoints;
        }

        protected abstract Point FoldPoint(Point dot);

        public int FoldCoordinate { get; set; }
    }

    public class FoldInstructionX : FoldInstruction
    {
        public FoldInstructionX(int coordinate)
        {
            FoldCoordinate = coordinate;
        }

        protected override Point FoldPoint(Point dot)
        {
            var difference = dot.X - FoldCoordinate;
            if (difference < 0)
            {
                return dot;
            }

            return new Point(FoldCoordinate - difference, dot.Y);
        }

        public override string ToString()
        {
            return $"fold along x={FoldCoordinate}";
        }
    }

    public class FoldInstructionY : FoldInstruction
    {
        public FoldInstructionY(int coordinate)
        {
            FoldCoordinate = coordinate;
        }

        protected override Point FoldPoint(Point dot)
        {
            var difference = dot.Y - FoldCoordinate;
            if (difference < 0)
            {
                return dot;
            }

            return new Point(dot.X, FoldCoordinate - difference);
        }

        public override string ToString()
        {
            return $"fold along y={FoldCoordinate}";
        }
    }

    public class Day13Runner : DayRunner
    {
        public DotGrid Grid { get; private set; }
        public List<FoldInstruction> Folds { get; private set; }

        public HashSet<Point> Points { get; private set; }

        public Day13Runner(DayData data) : base(data)
        {
        }

        protected override void SolveDay(string[] data)
        {
            ParseInput(data);
            Grid = new DotGrid(Points);
            Grid.Fold(Folds.First());

            OutputWriter.WriteResult(1, $"Dots after first fold: {Grid.Dots.Count}");

            var remainingFolds = Folds.Skip(1);
            foreach (var fold in Folds.Skip(1))
            {
                Grid.Fold(fold);
            }

            OutputWriter.WriteResult(2, "Grid appearance after folds");
            OutputWriter.WriteLine(Grid.ToString());

        }

        private void ParseInput(string[] data)
        {
            bool isParsingPoints = true;
            Points = new HashSet<Point>();
            Folds = new List<FoldInstruction>();
            foreach (var input in data)
            {
                if (string.IsNullOrWhiteSpace(input))
                {
                    isParsingPoints = false;
                }
                else if (isParsingPoints)
                {
                    var pieces = input.Split(',').Select(int.Parse).ToList();
                    Points.Add(new Point(pieces[0], pieces[1]));
                }
                else
                {
                    var pieces = input.Split(' ').Last().Split('=');
                    var coordinate = int.Parse(pieces[1]);
                    if (pieces[0] == "x")
                    {
                        Folds.Add(FoldInstructionX.CreateXFold(coordinate));
                    }
                    else
                    {
                        Folds.Add(FoldInstruction.CreateYFold(coordinate));
                    }
                }
            }
            

        }

    }
}
