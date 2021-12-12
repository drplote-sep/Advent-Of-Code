using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using Advent_of_Code.DataSources;

namespace Advent_of_Code.DayRunners
{
    public class CavePath
    {
        public static bool IsPart2 = false;

        private bool HasDoubledOnSmallCave { get; set; } = false;
    
        public List<Cave> CavesTraveled { get; private set; } = new List<Cave>();

        public Cave LastCave => CavesTraveled.LastOrDefault();
        public bool CanTravelTo(Cave c) => !TraveledToEnd && (c.IsLarge || !ContainsCave(c) || CanReturnToSmallCave(c)) && (LastCave == null || !LastCave.Equals(c));

        public bool CanReturnToSmallCave(Cave c)
        {
            return IsPart2 && !HasDoubledOnSmallCave && !c.IsEnd && !c.IsStart;
        }
        
        public bool TravelTo(Cave c)
        {
            if (CanTravelTo(c))
            {
                if (IsDoublingOnSmallCave(c))
                {
                    HasDoubledOnSmallCave = true;
                }
                CavesTraveled.Add(c);
                return true;
            }

            return false;

        }

        private bool IsDoublingOnSmallCave(Cave cave)
        {
            return !cave.IsLarge && ContainsCave(cave);
        }

        public bool TravelAlongSegment(CaveSegment cs)
        {
            return LastCave.Equals(cs.Cave1) ? TravelTo(cs.Cave2) : TravelTo(cs.Cave1);
        }

        public bool TraveledToEnd => CavesTraveled.Any(c => c.IsEnd);

        public bool ContainsCave(Cave c)
        {
            return CavesTraveled.Contains(c);  
        }

        public override string ToString()
        {
            return string.Join(",", CavesTraveled.Select(c => c.Name));
        }

        public CavePath Clone()
        {
            return new CavePath
            {
                CavesTraveled = this.CavesTraveled.Select(c => c).ToList(),
                HasDoubledOnSmallCave = HasDoubledOnSmallCave
            };
        }
    }
    
    public class CaveSegment
    {
        public Cave Cave1 { get; set; }
        public Cave Cave2 { get; set; }

        public CaveSegment(string cave1, string cave2)
        {
            Cave1 = new Cave(cave1);
            Cave2 = new Cave(cave2);
        }

        public bool HasCave(Cave c)
        {
            return c.Equals(Cave1) || c.Equals(Cave2);
        }

        public bool HasStart => Cave1.IsStart || Cave2.IsStart;

        public bool HasEnd => Cave1.IsEnd || Cave2.IsEnd;

        public override bool Equals(object obj)
        {
            return Equals(obj as CaveSegment);
        }

        public bool Equals(CaveSegment other)
        {
            return other != null && other.Cave1.Equals(Cave1) && other.Cave2.Equals(Cave2);
        }

        public override int GetHashCode()
        {
            return Cave1.GetHashCode() * 17 + Cave2.GetHashCode();
        }

        public override string ToString()
        {
            return $"{Cave1.ToString()} - {Cave2.ToString()}";
        }
    }

    public class Cave
    {
        public string Name { get;  }
        
        public Cave(string name)
        {
            Name = name;
        }

        public bool IsLarge => Name.All(Char.IsUpper);

        public bool IsStart => Name == "start";

        public bool IsEnd => Name == "end";

        public override bool Equals(object obj)
        {
            return Equals(obj as Cave);
        }

        public bool Equals(Cave other)
        {
            return other != null && Name == other.Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }
    }
    
    public class Day12Runner : DayRunner
    {
        public List<CaveSegment> CaveSegments { get; private set; }
        public Day12Runner(DayData data) : base(data)
        {
        }

        protected override void SolveDay(string[] data)
        {
            CaveSegments = ParseInputs(data);
            var startSegments = CaveSegments.Where(s => s.HasStart).ToList();
            var pathsToEnd = new List<CavePath>();
            foreach (var startSegment in startSegments)
            {
                var startPath = new CavePath();
                startPath.TravelTo(new Cave("start"));
                pathsToEnd.AddRange(GetPathsToEnd(startSegment, startPath));
            }
            
            OutputWriter.WriteResult(1, $"Paths from start to end: {pathsToEnd.Count}");

            CavePath.IsPart2 = true;
            pathsToEnd = new List<CavePath>();
            foreach (var startSegment in startSegments)
            {
                var startPath = new CavePath();
                startPath.TravelTo(new Cave("start"));
                pathsToEnd.AddRange(GetPathsToEnd(startSegment, startPath));
            }
            OutputWriter.WriteResult(2, $"Paths from start to end: {pathsToEnd.Count}");
        }

        private List<CavePath> GetPathsToEnd(CaveSegment startSegment, CavePath path)
        {
            var paths = new List<CavePath>();
            var success = path.TravelAlongSegment(startSegment);
            if (success)
            {
                if (path.TraveledToEnd)
                {
                    paths.Add(path);
                }
                else
                {
                    var pathsOut = GetConnectingSegments(path.LastCave);
                    foreach (var pathOut in pathsOut)
                    {
                        paths.AddRange(GetPathsToEnd(pathOut, path.Clone()));
                    }
                }
            }

            return paths;
        }
        
        private List<CaveSegment> GetConnectingSegments(Cave cave)
        {
            if (cave.IsStart || cave.IsEnd)
            {
                return new List<CaveSegment>();
            }
            return CaveSegments.Where(cs => cs.HasCave(cave)).ToList();
        }

        private List<CaveSegment> ParseInputs(string[] data)
        {
            var segments = new List<CaveSegment>();
            foreach (var path in data)
            {
                var parts = path.Split('-');
                segments.Add(new CaveSegment(parts[0], parts[1]));
            }

            return segments;
        }
    }
}