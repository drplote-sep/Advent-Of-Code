using System;
using System.Collections.Generic;
using System.Drawing;

namespace Advent_of_Code.Parsers
{
    public class Day5InputLine
    {
        public Point A { get; private set; }
        public Point B { get; private set; }

        public Day5InputLine(string input)
        {
            var pieces = input.Split(' ');
            A = CreatePointFromInputString(pieces[0]);
            B = CreatePointFromInputString(pieces[2]);
        }

        private Point CreatePointFromInputString(string pointText)
        {
            var coords = pointText.Split(',');

            return new Point
            {
                X = int.Parse(coords[0]),
                Y = int.Parse(coords[1])
            };
        }
    }

    public static class Day5Parser
    {
        public static IEnumerable<Day5InputLine> Parse(string[] inputs)
        {
            var lines = new List<Day5InputLine>();
            foreach (var input in inputs)
            {
                lines.Add(new Day5InputLine(input));
            }
            return lines;
        }
    }
}
