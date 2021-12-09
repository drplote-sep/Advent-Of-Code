using System;
using System.Collections.Generic;
using System.Linq;
using Advent_of_Code.DataSources;

namespace Advent_of_Code.DayRunners
{
    public class Day2Runner : DayRunner
    {
        public Day2Runner(DayData data) : base(data)
        {
        }

        protected override void SolveDay(string[] data)
        {
            FindPosition(data);
            FindPositionWithAim(data);
        }

        private static void FindPositionWithAim(string[] day2Input)
        {
            var depth = 0;
            var horizontal = 0;
            var aim = 0;

            foreach (var line in day2Input)
            {
                var words = line.Split(' ');
                var direction = words[0];
                var amount = int.Parse(words[1]);
                switch (direction)
                {
                    case "forward":
                        horizontal += amount;
                        depth += aim * amount;
                        break;
                    case "up":
                        aim -= amount;
                        break;
                    case "down":
                        aim += amount;
                        break;
                    default:
                        throw new ArgumentException("Parsed day 2 file incorrectly");
                }
            }

            OutputWriter.WriteResult(2, $"(horizontal * depth) = {horizontal * depth}");
        }

        private static void FindPosition(string[] day2Input)
        {
            var ups = new List<int>();
            var downs = new List<int>();
            var forwards = new List<int>();

            foreach (var line in day2Input)
            {
                var words = line.Split(' ');
                var direction = words[0];
                var amount = int.Parse(words[1]);
                switch (direction)
                {
                    case "forward":
                        forwards.Add(amount);
                        break;
                    case "up":
                        ups.Add(amount);
                        break;
                    case "down":
                        downs.Add(amount);
                        break;
                    default:
                        throw new ArgumentException("Parsed day 2 file incorrectly");
                }
            }

            var depth = 0;
            var horizontal = 0;
            depth += downs.Sum();
            depth -= ups.Sum();
            horizontal = forwards.Sum();
            OutputWriter.WriteResult(1, $"(horizontal * depth) = {horizontal * depth}");
        }
    }
}