using System;
using System.Collections.Generic;
using System.Linq;
using Advent_of_Code.DataSources;

namespace Advent_of_Code.DayRunners
{
    public class Day3Runner : DayRunner
    {
        public Day3Runner(DayData data) : base(data)
        {
        }

        protected override void SolveDay(string[] data)
        {
            FindPowerConsumption(data);
        }

        private static void FindPowerConsumption(string[] day3Input)
        {
            var bits = new List<List<int>>();
            foreach (var line in day3Input)
                bits.Add(line.ToCharArray().Select(c => Convert.ToInt32(char.GetNumericValue(c))).ToList());

            var gammaBits = new List<int>();
            var epsilonBits = new List<int>();
            var numBits = bits.First().Count();
            for (var i = 0; i < numBits; i++)
            {
                var mostCommon = GetMostCommonBit(bits.Select(b => b[i]));
                var leastCommon = FlipBit(mostCommon);
                gammaBits.Add(mostCommon);
                epsilonBits.Add(leastCommon);
            }

            var gammaValue = GetDecimalValueOfBits(gammaBits);
            var epsilonValue = GetDecimalValueOfBits(epsilonBits);

            OutputWriter.WriteResult(1, $"Power Consumption = {gammaValue * epsilonValue}");

            var oxygenBits = FilterWithCriteria(bits, true);
            var oxygenValue = GetDecimalValueOfBits(oxygenBits);
            var co2Bits = FilterWithCriteria(bits, false);
            var co2Value = GetDecimalValueOfBits(co2Bits);

            OutputWriter.WriteResult(2, $"Life Support = {co2Value * oxygenValue}");
        }

        private static int FlipBit(int bit)
        {
            return bit == 0 ? 1 : 0;
        }

        private static List<int> FilterWithCriteria(List<List<int>> bitGroups, bool mostFrequent)
        {
            var numBits = bitGroups.First().Count;
            for (var i = 0; i < numBits; i++)
            {
                var mostCommonBit = GetMostCommonBit(bitGroups.Select(b => b[i]).ToList());
                var filterBit = mostFrequent ? mostCommonBit : FlipBit(mostCommonBit);
                bitGroups = bitGroups.Where(b => b[i] == filterBit).ToList();
                if (bitGroups.Count == 1)
                    return bitGroups.First();
            }

            throw new InvalidOperationException("Didn't get down to one number");
        }

        private static int GetDecimalValueOfBits(List<int> bits)
        {
            var bitString = string.Join(string.Empty, bits);
            return Convert.ToInt32(bitString, 2);
        }

        private static int GetMostCommonBit(IEnumerable<int> bits)
        {
            var zeroes = bits.Count(b => b == 0);
            var ones = bits.Count(b => b == 1);
            return zeroes > ones ? 0 : 1;
        }
    }
}