using System.Collections.Generic;
using System.Linq;
using Advent_of_Code.DataSources;

namespace Advent_of_Code.DayRunners
{
    public class PolymerInsertionRule
    {
        public string Pair { get; }
        public string Replacement { get; }

        public string[] ResultingPairs { get; }

        public PolymerInsertionRule(string ruleString)
        {
            var pieces = ruleString.Split(' ');
            Pair = pieces.First();
            Replacement = pieces.Last();

            ResultingPairs = new []
            {
                new string(new[] { Pair[0], Replacement[0] }),
                new string(new[] { Replacement[0], Pair[1] }),
            };
            
        }
    }
    public class Day14Runner : DayRunner
    {
        public string PolymerTemplate { get; private set; }
        public Dictionary<string, long> PairCount = new Dictionary<string, long>();
        public Dictionary<string, string[]> Replacements { get; set; }
        public List<PolymerInsertionRule> Rules { get; private set; }
        public Day14Runner(DayData data) : base(data)
        {
        }

        protected override void SolveDay(string[] data)
        {
            ParseInputs(data);
            
            for (int i = 0; i < 10; i++)
            {
                Step();
            }

            var charCounts = GetCharacterCounts();
            var max  = charCounts.Max(e => e.Value);
            var min  = charCounts.Min(e => e.Value);

            OutputWriter.WriteResult(1, $"After 10 steps: Most Common - Least Common = {max - min}");
            
            for (int i = 10; i < 40; i++)
            {
                Step();
            }
            
            charCounts = GetCharacterCounts();
            max  = charCounts.Max(e => e.Value);
            min  = charCounts.Min(e => e.Value);
            OutputWriter.WriteResult(2, $"After 40 stepsMost Common - Least Common = {max - min}");
        }

        private Dictionary<char, long> GetCharacterCounts()
        {
            var allChars =PairCount.Keys.SelectMany(s => s).Select(c => c).Distinct()
                .ToDictionary(c => c, c => 0L);

            foreach (var entry in PairCount)
            {
                var firstChar = entry.Key.First();
                allChars[firstChar] += entry.Value;
            }

            allChars[PolymerTemplate.Last()] += 1;
            
            return allChars;

        }

        private void Step()
        {
            var oldPairCount = PairCount;
            PairCount = PairCount.ToDictionary(entry => entry.Key, entry => entry.Value);

            foreach (var entry in oldPairCount)
            {
                var pair = entry.Key;
                var oldCount = entry.Value;
                if (oldCount != 0)
                {
                    ChangeCount(pair, -1*oldCount);
                    foreach (var replacement in Replacements[pair])
                    {
                        ChangeCount(replacement, oldCount);
                    }
                }
            }
            
        }

        private List<string> GetPairs(string s)
        {
            var pairs = new List<string>();
            var c = s.ToCharArray();
            for (int i = 1; i < c.Length; i++)
            {
                pairs.Add(new string(new []{c[i-1],c[i]}));
            }
            return pairs;
        }

        private void ChangeCount(string pair, long amount = 1)
        {
            PairCount[pair] += amount;
        }

        private void ParseInputs(string[] data)
        {
            PolymerTemplate = data.First();
            Rules = data.Skip(2).Select(s => new PolymerInsertionRule(s)).ToList();
            Replacements = new Dictionary<string, string[]>();
            var allPossiblePairs = new HashSet<string>();
            foreach (var rule in Rules)
            {
                Replacements[rule.Pair] = rule.ResultingPairs;
                allPossiblePairs.Add(rule.Pair);
                allPossiblePairs.UnionWith(rule.ResultingPairs);
            }

            foreach (var pair in allPossiblePairs)
            {
                PairCount[pair] = 0;
            }

            foreach (var pair in GetPairs(PolymerTemplate))
            {
                if (!PairCount.ContainsKey(pair))
                {
                    PairCount[pair] = 0;
                }

                ChangeCount(pair);
            }
        }


    }
}