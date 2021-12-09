using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent_of_Code
{
    public enum DisplayLED
    {
        A,
        B,
        C,
        D,
        E,
        F,
        G
    }

    public class SevenSegmentDisplayEntry
    {
        public List<string> Inputs { get; set; }
        public List<string> Outputs { get; set; }
    }

    public class SevenSegmentDisplay
    {
        private static readonly HashSet<DisplayLED> Number0 = new HashSet<DisplayLED>
            {DisplayLED.A, DisplayLED.B, DisplayLED.C, DisplayLED.E, DisplayLED.F, DisplayLED.G};

        private static HashSet<DisplayLED> Number1 = new HashSet<DisplayLED> {DisplayLED.C, DisplayLED.F};

        private static readonly HashSet<DisplayLED> Number2 = new HashSet<DisplayLED>
            {DisplayLED.A, DisplayLED.C, DisplayLED.D, DisplayLED.E, DisplayLED.G};

        private static readonly HashSet<DisplayLED> Number3 = new HashSet<DisplayLED>
            {DisplayLED.A, DisplayLED.C, DisplayLED.D, DisplayLED.F, DisplayLED.G};

        private static HashSet<DisplayLED> Number4 = new HashSet<DisplayLED>
            {DisplayLED.B, DisplayLED.C, DisplayLED.D, DisplayLED.F};

        private static readonly HashSet<DisplayLED> Number5 = new HashSet<DisplayLED>
            {DisplayLED.A, DisplayLED.B, DisplayLED.D, DisplayLED.F, DisplayLED.G};

        private static readonly HashSet<DisplayLED> Number6 = new HashSet<DisplayLED>
            {DisplayLED.A, DisplayLED.B, DisplayLED.D, DisplayLED.E, DisplayLED.F, DisplayLED.G};

        private static HashSet<DisplayLED> Number7 = new HashSet<DisplayLED> {DisplayLED.A, DisplayLED.C, DisplayLED.F};

        private static HashSet<DisplayLED> Number8 = new HashSet<DisplayLED>
            {DisplayLED.A, DisplayLED.B, DisplayLED.C, DisplayLED.D, DisplayLED.E, DisplayLED.F, DisplayLED.G};

        private static readonly HashSet<DisplayLED> Number9 = new HashSet<DisplayLED>
            {DisplayLED.A, DisplayLED.B, DisplayLED.C, DisplayLED.D, DisplayLED.F, DisplayLED.G};

        public SevenSegmentDisplay(string[] inputData)
        {
            ParseInputs(inputData);
        }

        private List<SevenSegmentDisplayEntry> Entries { get; set; }

        private void ParseInputs(string[] inputData)
        {
            Entries = new List<SevenSegmentDisplayEntry>();

            foreach (var line in inputData)
            {
                var inputSplit = line.Split('|').Select(s => s.Trim()).ToList();
                Entries.Add(new SevenSegmentDisplayEntry
                {
                    Inputs = inputSplit[0].Split(' ').ToList(),
                    Outputs = inputSplit[1].Split(' ').ToList()
                });
            }
        }

        public int GetPartOneAnswer()
        {
            // How many output digits contain 1, 4, 7, or 8, which are all uniquely sized (lengths 2, 3, 4, 7)
            var outputDigitLengths = Entries.SelectMany(e => e.Outputs).Select(o => o.Length);

            return outputDigitLengths.Count(l => l == 2 || l == 3 || l == 4 || l == 7);
        }

        public int GetPartTwoAnswer()
        {
            return Entries.Sum(GetDecodedOutputValue);
        }

        private int GetDecodedOutputValue(SevenSegmentDisplayEntry e)
        {
            // 0 #6 (a, b, c, e, f, g)
            // 1 #2 (c, f)
            // 2 #5 (a, c, d, e, g)
            // 3 #5 (a, c, d, f, g)
            // 4 #4 (b, c, d, f)
            // 5 #5 (a, b, d, f, g)
            // 6 #6 (a, b, d, e, f, g)
            // 7 #3 (a, c, f)
            // 8 #7 (a, b, c, d, e, f, g)
            // 9 #6 (a, b, c, d, f, g)
            // A - 0, 2, 3, 5, 6, 7, 8, 9
            // B - 0, 4, 5, 6, 8, 9
            // C - 0, 1, 2, 3, 4, 7, 8, 9
            // D - 2, 3, 4, 5, 6, 8, 9
            // E - 0, 2, 6, 8
            // F - 0, 1, 3, 4, 5, 6, 7, 8, 9
            // G - 0, 2, 3, 5, 6, 8, 9
            var mapping = new Dictionary<DisplayLED, char>();

            var chars1 = e.Inputs.Where(i => i.Length == 2).SelectMany(s => s).Distinct().ToList();
            var chars4 = e.Inputs.Where(i => i.Length == 4).SelectMany(s => s).Distinct().ToList();
            var chars7 = e.Inputs.Where(i => i.Length == 3).SelectMany(s => s).Distinct().ToList();
            var chars8 = e.Inputs.Where(i => i.Length == 7).SelectMany(s => s).Distinct().ToList();

            mapping[DisplayLED.A] = chars7.Except(chars1).Single();

            var chars4Minus1 = chars4.Except(chars1).ToList();
            var chars0 = e.Inputs
                .Where(i => i.Length == 6 && (!i.Contains(chars4Minus1[0]) || !i.Contains(chars4Minus1[1])))
                .SelectMany(s => s).Distinct().ToList();

            mapping[DisplayLED.D] = chars4.Except(chars0).Single();

            var chars6 = e.Inputs
                .Where(i => i.Length == 6 && i.Contains(mapping[DisplayLED.D]))
                .Where(i => !i.Contains(chars1[0]) || !i.Contains(chars1[1]))
                .SelectMany(s => s).Distinct().ToList();

            mapping[DisplayLED.C] = chars1.Except(chars6).Single();
            mapping[DisplayLED.F] = chars1.Except(new[] {mapping[DisplayLED.C]}).Single();

            var chars9 = e.Inputs
                .Where(i => i.Length == 6 && i.Contains(mapping[DisplayLED.D]))
                .Where(i => i.Contains(mapping[DisplayLED.C]))
                .SelectMany(s => s).Distinct().ToList();

            mapping[DisplayLED.E] = chars6.Except(chars9).Single();

            mapping[DisplayLED.B] =
                chars4.Except(new[] {mapping[DisplayLED.C], mapping[DisplayLED.D], mapping[DisplayLED.F]}).Single();

            mapping[DisplayLED.G] = chars8.Except(new[]
            {
                mapping[DisplayLED.A],
                mapping[DisplayLED.B],
                mapping[DisplayLED.C],
                mapping[DisplayLED.D],
                mapping[DisplayLED.E],
                mapping[DisplayLED.F]
            }).Single();

            var reversedMapping = mapping.ToDictionary(x => x.Value, x => x.Key);

            var outputDigits = new List<int>();
            foreach (var digitString in e.Outputs) outputDigits.Add(DecodeDigit(reversedMapping, digitString));

            var outputString = string.Join(string.Empty, outputDigits);
            return Convert.ToInt32(outputString);
        }

        private int DecodeDigit(Dictionary<char, DisplayLED> reversedMapping, string digitString)
        {
            var litLEDs = new HashSet<DisplayLED>();
            foreach (var c in digitString) litLEDs.Add(reversedMapping[c]);

            switch (litLEDs.Count)
            {
                case 2:
                    return 1;
                case 3:
                    return 7;
                case 4:
                    return 4;
                case 5:
                    // 2, 3, or 5
                    if (litLEDs.SetEquals(Number2))
                        return 2;
                    else if (litLEDs.SetEquals(Number3))
                        return 3;
                    else if (litLEDs.SetEquals(Number5))
                        return 5;

                    throw new ArgumentException("unknown 5 length digit");

                case 6:
                    // 0, 6, or 9
                    if (litLEDs.SetEquals(Number0))
                        return 0;
                    else if (litLEDs.SetEquals(Number6))
                        return 6;
                    else if (litLEDs.SetEquals(Number9))
                        return 9;
                    throw new ArgumentException("unknown 6 length digit");
                case 7:
                    return 8;
                default:
                    throw new ArgumentException("unable to decode number");
            }
        }
    }
}