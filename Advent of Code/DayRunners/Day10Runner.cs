using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using Advent_of_Code.DataSources;

namespace Advent_of_Code.DayRunners
{
    public enum LineParseStatus
    {
        Valid,
        Incomplete,
        Corrupted
    }

    public class LineParseResult
    {
        public string Line { get; set;}
        public LineParseStatus Status { get; set; }
        public char? CorruptedCharacter { get; set; }
        public Stack<char> RemainingStartStack { get; set; }

        public int GetScore()
        {
            if (Status == LineParseStatus.Corrupted)
            {
                switch (CorruptedCharacter)
                {
                    case ')':
                        return 3;
                    case ']':
                        return 57;
                    case '}':
                        return 1197;
                    case '>':
                        return 25137;
                    default:
                        break;
                }
            }

            return 0;
        }


    }

    public class Day10Runner : DayRunner
    {
        private List<char> ValidStartChars { get; } = new List<char> { '(', '[', '{', '<' };
        private List<char> ValidEndChars { get; } = new List<char> { ')', ']', '}', '>' };

        public Day10Runner(DayData data) : base(data)
        {
        }

        protected override void SolveDay(string[] data)
        {
            var lineParseResults = GetParsedLines(data);
            var corruptionScore = lineParseResults
                .Where(r => r.Status == LineParseStatus.Corrupted)
                .Sum(r => r.GetScore());
            OutputWriter.WriteResult(1, $"Score of corrupted lines: {corruptionScore}");

            var autocompleteScores = lineParseResults
                .Where(r => r.Status == LineParseStatus.Incomplete)
                .Select(r => GetAutocompleteString(r.RemainingStartStack))
                .Select(GetAutoCompleteScore)
                .ToList();

            autocompleteScores.Sort();
            var middleScore = autocompleteScores[(autocompleteScores.Count - 1) / 2];
            OutputWriter.WriteResult(1, $"Middle Autcomplete Score: {middleScore}");

        }

        private List<char> GetAutocompleteString(Stack<char> stack)
        {
            var remainingEndChars = new List<char>();
            while (stack.Any())
            {
                var startChar = stack.Pop();
                remainingEndChars.Add(GetMatchingEndChar(startChar));
            }

            return remainingEndChars;
        }

        private long GetAutoCompleteScore(List<char> newChars)
        {
            long score = 0;
            foreach (var newChar in newChars)
            {
                score *= 5;
                score += GetCharacterPointValue(newChar);
            }

            return score;
        }

        private int GetCharacterPointValue(char c)
        {
            return ValidEndChars.IndexOf(c) + 1;
        }

        private List<LineParseResult> GetParsedLines(string[] data)
        {
            var results = new List<LineParseResult>();
            foreach (var line in data)
            {
                results.Add(ParseLine(line));
            }
            return results;
        }

        private LineParseResult ParseLine(string line)
        {
            var result = new LineParseResult {Line = line};
            var blockStarts = new Stack<char>();
            var endChar = ParseChars(line.Select(c => c).ToList(), blockStarts);
            if (endChar != null)
            {
                result.Status = LineParseStatus.Corrupted;
                result.CorruptedCharacter = endChar;
            }
            else if (blockStarts.Any())
            {
                result.Status = LineParseStatus.Incomplete;
                result.RemainingStartStack = blockStarts;

            }
            else
            {
                result.Status = LineParseStatus.Valid;
            }

            return result;
        }

        private char? ParseChars(List<char> characters, Stack<char> blockStarts)
        {
            if (characters.Any())
            {
                var currentChar = characters.First();
                if (ValidStartChars.Contains(currentChar))
                {
                    blockStarts.Push(currentChar);
                    return ParseChars(characters.Skip(1).ToList(), blockStarts);
                }

                if (ValidEndChars.Contains(currentChar))
                {
                    var expectedEndChar = GetMatchingEndChar(blockStarts.Pop());
                    if (expectedEndChar != currentChar)
                    {
                        return currentChar;
                    }
                    return ParseChars(characters.Skip(1).ToList(), blockStarts);
                }
            }

            return null;
        }

        private char GetMatchingEndChar(char startChar)
        {
            var index = ValidStartChars.IndexOf(startChar);
            return ValidEndChars[index];
        }
    }
}
