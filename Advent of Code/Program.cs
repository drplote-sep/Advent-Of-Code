using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Advent_of_Code.DataSources;
using Advent_of_Code.Parsers;

namespace Advent_of_Code
{
    class Day1Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2021");
            Console.WriteLine("------");
            var day1Input = File.ReadAllLines(@"..\..\RawInputs\Day 1\input.mos");
            CountIncreases(day1Input);
            CountSlidingIncrease(day1Input);

            var day2Input = File.ReadAllLines(@"..\..\RawInputs\Day 2\input.mos");
            FindPosition(day2Input);
            FindPositionWithAim(day2Input);

            var day3Input = File.ReadAllLines(@"..\..\RawInputs\Day 3\input.mos");
            FindPowerConsumption(day3Input);

            var day4Input = File.ReadAllLines(@"..\..\RawInputs\Day 4\input.mos");
            DoDay4(day4Input);

            DoDay5(Day5Data.GetRealData());

            DoDay6(Day6Data.GetRealData());

            //DoDay7(Day7Data.GetRealData());

            DoDay8(Day8Data.GetRealData());

            DoDay9(Day9Data.GetRealData());
        }

        private static void DoDay9(string[] inputs)
        {
            var lavaTubeMap = new LavaTubeMap(inputs);
            var lowestPoints = lavaTubeMap.GetLowPoints();
            Console.WriteLine($"Day 9.1: Lowest points risk: {lavaTubeMap.GetRisk(lowestPoints)}");
            Console.WriteLine($"Day 9.2: Product of Size of 3 largest basins: {lavaTubeMap.GetProductOfSizeOfThreeLargestBasins(lowestPoints)}");
        }

        private static void DoDay8(string[] inputData)
        {
            var display = new SevenSegmentDisplay(inputData);

            Console.WriteLine($"Day 8.1: Times 1,4,7,or 8 appear in output: {display.GetPartOneAnswer()}");

            Console.WriteLine($"Day 8.2: Sum of decoded outputs: {display.GetPartTwoAnswer()}");
        }

        private static void DoDay7(string[] inputData)
        {
            var data = inputData.Single().Split(',').Select(int.Parse).ToList();
            var crabSubs = new CrabSubmarine(data);

            var result = crabSubs.FindLeastFuelPosition();
            Console.WriteLine($"Day 7.1: Spent {result.FuelUsed} to align to position {result.Position}");

            result = crabSubs.FindLeastFuelPosition(true);
            Console.WriteLine($"Day 7.2: Spent {result.FuelUsed} to align to position {result.Position}");
        }

        private static void DoDay6(string[] inputData)
        {
            var lanternFish = inputData.Single().Split(',').Select(int.Parse).ToList();

            var school = new LanternFishSchool(lanternFish);
            school.SwimForDays(80);
            Console.WriteLine($"Day 6.1: Num fish after 80 days: {school.GetFishTotal()}");

            var school2 = new LanternFishSchool(lanternFish);
            school2.SwimForDays(256);
            Console.WriteLine($"Day 6.2: Num fish after 256 days: {school2.GetFishTotal()}");
        }

        private static void DoDay5(string[] day5Input)
        {
            var inputs = Day5Parser.Parse(day5Input);
            var ventLines = inputs.Select(i => new ThermalVentLine(i.A, i.B)).ToList();

            var ventGrid = new VentGrid();
            var ventGridWithDiagonals = new VentGrid();
            foreach (var ventLine in ventLines)
            {
                ventGrid.AddLine(ventLine);
                ventGridWithDiagonals.AddLine(ventLine, true);
            }

            var atLeastTwoOverlaps = ventGrid.GetOverlapCount(2);
            Console.WriteLine($"Day 5.1: Overlap at least twice: {atLeastTwoOverlaps}");

            var atLeastTwoOverlapsWithDiagonals = ventGridWithDiagonals.GetOverlapCount(2);
            Console.WriteLine($"Day 5.2: Overlap at least twice (with diagonals): {atLeastTwoOverlapsWithDiagonals}");
        }

        private static void DoDay4(string[] day4Input)
        {
            var bingoNumbers = day4Input.First().Split(',').Select(int.Parse);
            var bingoBoards = ParseBingoBoards(day4Input.Skip(1));
            var haveFirstWinner = false;
            foreach (var bingoNum in bingoNumbers)
            {
                foreach (var board in bingoBoards)
                {
                    if (!board.HasWon)
                    {
                        var isWinner = board.HandleNumberSelected(bingoNum);
                        if (isWinner)
                        {
                            if (!haveFirstWinner)
                            {
                                haveFirstWinner = true;
                                var score = board.GetSumUnmarked() * bingoNum;
                                Console.WriteLine($"Day 4.1: Winning bingo card score: {score}");
                            }

                            var unwonBoards = bingoBoards.Count(b => !b.HasWon);
                            if (unwonBoards == 1)
                            {
                                var score = board.GetSumUnmarked() * bingoNum;
                                Console.WriteLine($"Day 4.2: Latest winning bingo card score: {score}");
                            }

                            board.HasWon = true;
                        }
                    }
                }

            }
        }


        private static List<BingoBoard> ParseBingoBoards(IEnumerable<string> lines)
        {
            var boards = new List<BingoBoard>();
            var boardRows = new List<List<int>>();
            foreach (var line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    var lineNums = line.Split(' ').Where(s => !string.IsNullOrWhiteSpace(s)).Select(int.Parse).ToList();
                    boardRows.Add(lineNums);
                }

                if (boardRows.Count == 5)
                {
                    boards.Add(new BingoBoard(boardRows));
                    boardRows = new List<List<int>>();
                }
            }

            return boards;

        }

        private static void FindPowerConsumption(string[] day3Input)
        {
            var bits = new List<List<int>>();
            foreach (var line in day3Input)
            {
                bits.Add(line.ToCharArray().Select(c => Convert.ToInt32(Char.GetNumericValue(c))).ToList());
            }

            var gammaBits = new List<int>();
            var epsilonBits = new List<int>();
            var numBits = bits.First().Count();
            for (int i = 0; i < numBits; i++)
            {
                var mostCommon = GetMostCommonBit(bits.Select(b => b[i]));
                var leastCommon = FlipBit(mostCommon);
                gammaBits.Add(mostCommon);
                epsilonBits.Add(leastCommon);
            }

            var gammaValue = GetDecimalValueOfBits(gammaBits);
            var epsilonValue = GetDecimalValueOfBits(epsilonBits);

            Console.WriteLine($"Day 3.1: Power Consumption = {gammaValue * epsilonValue}");

            var oxygenBits = FilterWithCriteria(bits, true);
            var oxygenValue = GetDecimalValueOfBits(oxygenBits);
            var co2Bits = FilterWithCriteria(bits, false);
            var co2Value = GetDecimalValueOfBits(co2Bits);

            Console.WriteLine($"Day 3.2: Life Support = {co2Value * oxygenValue}");
        }

        private static int FlipBit(int bit)
        {
            return bit == 0 ? 1 : 0;
        }

        private static List<int> FilterWithCriteria(List<List<int>> bitGroups, bool mostFrequent)
        {
            var numBits = bitGroups.First().Count;
            for (int i = 0; i < numBits; i++)
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

            Console.WriteLine($"Day 2.1 (horizontal * depth) = {horizontal * depth}");
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
            Console.WriteLine($"Day 2.1 (horizontal * depth) = {horizontal * depth}");
        }

        private static void CountSlidingIncrease(string[] lines)
        {
            var windows = new Dictionary<int, List<int>>();
            var currentWindow = 0;

            foreach (var line in lines)
            {
                var lineValue = int.Parse(line);
                windows[currentWindow] = new List<int>{lineValue};

                if (currentWindow > 0)
                {
                    windows[currentWindow - 1].Add(lineValue);
                }

                if (currentWindow > 1)
                {
                    windows[currentWindow - 2].Add(lineValue);
                }

                currentWindow++;
            }

            var sums = windows.Values.Select(v => v.Sum());

            Console.WriteLine($"Day 1.2: Total # of Window Increases is {CountIncreases(sums)}");
        }

        private static int CountIncreases(IEnumerable<int> nums)
        {
            int? current = null;
            var increaseCount = 0;

            foreach (var num in nums)
            {
                var previous = current;
                current = num;

                if (previous != null && current > previous)
                {
                    increaseCount++;
                }
            }

            return increaseCount;
        }

        private static void CountIncreases(string[] lines)
        {
            var nums = lines.Select(int.Parse);
            
            Console.WriteLine($"Day 1.1: Total # of Increases is {CountIncreases(nums)}");
        }
    }
}
