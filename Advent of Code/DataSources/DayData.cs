using System.Collections.Generic;
using System.IO;

namespace Advent_of_Code.DataSources
{
    public class DayData
    {
        private List<string[]> TestData { get; } = new List<string[]>
        {
            new string[]{},
            new string[]{},
            new string[]{},
            new string[]{},
            new[]
            {
                "0,9 -> 5,9",
                "8,0 -> 0,8",
                "9,4 -> 3,4",
                "2,2 -> 2,1",
                "7,0 -> 7,4",
                "6,4 -> 2,0",
                "0,9 -> 2,9",
                "3,4 -> 1,4",
                "0,0 -> 8,8",
                "5,5 -> 8,2"
            },
            new[]
            {
                "3,4,3,1,2"
            },
            new [] {
                "16,1,2,0,4,2,7,1,2,14"
            },
            new[]
            {
                "be cfbegad cbdgef fgaecd cgeb fdcge agebfd fecdb fabcd edb | fdgacbe cefdb cefbgd gcbe",
                "edbfga begcd cbg gc gcadebf fbgde acbgfd abcde gfcbed gfec | fcgedb cgb dgebacf gc",
                "fgaebd cg bdaec gdafb agbcfd gdcbef bgcad gfac gcb cdgabef | cg cg fdcagb cbg",
                "fbegcd cbd adcefb dageb afcb bc aefdc ecdab fgdeca fcdbega | efabcd cedba gadfec cb",
                "aecbfdg fbg gf bafeg dbefa fcge gcbea fcaegb dgceab fcbdga | gecf egdcabf bgf bfgea",
                "fgeab ca afcebg bdacfeg cfaedg gcfdb baec bfadeg bafgc acf | gebdcfa ecba ca fadegcb",
                "dbcfg fgd bdegcaf fgec aegbdf ecdfab fbedc dacgb gdcebf gf | cefg dcbef fcge gbcadfe",
                "bdfegc cbegaf gecbf dfcage bdacg ed bedf ced adcbefg gebcd | ed bcgafe cdgba cbgef",
                "egadfb cdbfeg cegd fecab cgb gbdefca cg fgcdab egfdb bfceg | gbdfcae bgc cg cgb",
                "gcafb gcf dcaebfg ecagb gf abcdeg gaef cafbge fdbac fegbdc | fgae cfgab fg bagce"
            },
            new[]
            {
                "2199943210",
                "3987894921",
                "9856789892",
                "8767896789",
                "9899965678"
            }
        };
        
        public DayData(int dayNumber)
        {
            DayNumber = dayNumber;
        }

        public int DayNumber { get; }

        public virtual string[] GetTestData()
        {
            return TestData[DayNumber - 1];
        }

        public virtual string[] GetRealData()
        {
            return File.ReadAllLines($"..\\..\\RawInputs\\Day {DayNumber}\\input.mos");
        }
    }
}