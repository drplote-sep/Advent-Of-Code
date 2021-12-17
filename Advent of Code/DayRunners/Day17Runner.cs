using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security;
using Advent_of_Code.DataSources;

namespace Advent_of_Code.DayRunners
{
    public class Day17Runner : DayRunner
    {
        public int MinX { get; set; }
        public int MaxX { get; set; }
        public int MinY { get; set; }
        public int MaxY { get; set; }
        public Day17Runner(DayData data) : base(data)
        {
        }

        protected override void SolveDay(string[] data)
        {
            ParseInputs(data);

            var highestY = 0;
            var validInitialXVelocities = GetValidInitialXVelocities();
            foreach (var currentVelocityX in validInitialXVelocities)
            {
                // Going to 1000 is dumb, but trying to be smarter wasn't working
                for (int currentVelocityY = 0; currentVelocityY < Math.Abs(MinY); currentVelocityY++)
                {
                    var height = FireProbe(currentVelocityX, currentVelocityY);
                    if (height != null)
                    {
                        highestY = Math.Max(highestY, height.Value);
                    }
                }
            }
            
            OutputWriter.WriteResult(1, $"Max Height Reached: {highestY}");

            var validVelocities = new HashSet<Point>();
            foreach (var currentVelocityX in validInitialXVelocities)
            {
                // Going to 1000 is dumb, but trying to be smarter wasn't working
                for (int currentVelocityY = MinY; currentVelocityY < Math.Abs(MinY); currentVelocityY++)
                {
                    if (FireProbe(currentVelocityX, currentVelocityY) != null)
                    {
                        validVelocities.Add(new Point(currentVelocityX, currentVelocityY));
                    };
                }
            }
            
            OutputWriter.WriteResult(2, $"Number of valid initial velocities: {validVelocities.Count}");
            
        } 

        private List<int> GetValidInitialXVelocities()
        {
            int? min = null;
            var i = 0;
            while (min == null)
            {
                var eventualX = (i * (i + 1)) / 2;
                if (eventualX > MinX)
                {
                    min = i;
                }

                i++;
            }

            var retVal = new List<int>();

            for (int j = min.Value; j <= MaxX; j++)
            {
                retVal.Add(j);
            }
            
            return retVal;
        }

        private void ParseInputs(string[] data)
        {
            var input = data.First();
            var pieces = input.Split(' ');
            var xPiece = pieces[2];
            var yPiece = pieces[3];

            var xMinMax = xPiece.Split(new[] { ".." }, StringSplitOptions.None);
            MinX = Convert.ToInt32(xMinMax.First().Substring(2, xMinMax.First().Length - 2));
            MaxX = Convert.ToInt32(xMinMax.Last().Substring(0, xMinMax.Last().Length - 1));
            
            var yMinMax = yPiece.Split(new[] { ".." }, StringSplitOptions.None);
            MinY = Convert.ToInt32(yMinMax.First().Substring(2, yMinMax.First().Length - 2));
            MaxY = Convert.ToInt32(yMinMax.Last());
        }

        private int? FireProbe(int forwardVelocity, int verticalVelocity)
        {
            var stepPositions = new List<Point>();
            if (FireProbe(new Point(0, 0), forwardVelocity, verticalVelocity, stepPositions))
            {
                return stepPositions.Max(p => p.Y);
            }

            return null;
        }
        
        private bool FireProbe(Point currentPosition, int forwardVelocity, int verticalVelocity, List<Point> stepPositions)
        {
            if (currentPosition.X > MaxX)
            {
                return false;
            }

            if (forwardVelocity == 0)
            {
                if (currentPosition.X > MaxX || currentPosition.X < MinX)
                {
                    // Can't reach the X coordinate because we aren't moving forwards or backward anymore
                    return false;
                }

                if (verticalVelocity <= 0 && currentPosition.Y < MinY)
                {
                    // Already beneath the target area and descending further
                    return false;
                }
            }
            
            
            var stepPosition = new Point(currentPosition.X + forwardVelocity, currentPosition.Y + verticalVelocity);
            stepPositions.Add(stepPosition);
            if (IsInTargetArea(stepPosition))
            {
                return true;
            }

            if (forwardVelocity != 0)
            {
                forwardVelocity = forwardVelocity > 0 ? forwardVelocity - 1 : forwardVelocity + 1;
            }

            verticalVelocity--;

            return FireProbe(stepPosition, forwardVelocity, verticalVelocity, stepPositions);
        }
       
        private bool IsInTargetArea(Point p)
        {
            return p.X >= MinX && p.X <= MaxX && p.Y >= MinY && p.Y <= MaxY;
        }
    }
}