using System;
using System.Collections.Generic;
using System.Linq;
using Advent_of_Code.DataSources;

namespace Advent_of_Code.DayRunners
{
    public class Day11Runner : DayRunner
    {
        private List<List<Octopus>> Octopuses { get; } = new List<List<Octopus>>();
        public Day11Runner(DayData data) : base(data)
        {
        }

        protected override void SolveDay(string[] data)
        {
            ParseOctopii(data);
            List<int> stepAllFlashed = new List<int>();
            for (int i = 1; i < 100; i++)
            {
                if (PerformStep())
                {
                    stepAllFlashed.Add(i);
                }
                
            }
            var totalFlashes = Octopuses.SelectMany(o => o).Sum(o => o.FlashCount);
            OutputWriter.WriteResult(1, $"Total flashes after 100 steps: {totalFlashes}");

            var currentStep = 100;
            while (!stepAllFlashed.Any())
            {
                if (PerformStep())
                {
                    stepAllFlashed.Add(currentStep);    
                }
                
                currentStep++;
            }
           
            OutputWriter.WriteResult(2, $"First step all flashed: {stepAllFlashed.First() + 1}");
        }

        private bool PerformStep()
        {
            var stepOctopii = Octopuses.SelectMany(o => o).ToList();
            stepOctopii.ForEach(o => o.Step());
            FlashNeighbors(stepOctopii);
            return stepOctopii.All(o => o.FlashedThisStep);
        }

        private void FlashNeighbors(List<Octopus> octopuses)
        {
            var flashingOctopuses = octopuses.Where(o => o.FlashedThisStep).ToList();
            if (flashingOctopuses.Any())
            {
                var unflashingOctopuses = octopuses.Except(flashingOctopuses).ToList();
                flashingOctopuses.ForEach(flasher => GetAdjacent(flasher).ForEach(o => o.IncreaseEnergy()));
                FlashNeighbors(unflashingOctopuses);
            }
        }
        

        private Octopus GetOctopusAtLocation(int x, int y)
        {
            if (x < 0 || x >= Octopuses.First().Count || y < 0 || y >= Octopuses.Count)
            {
                return null;
            }

            return Octopuses[y][x];
        }

        private List<Octopus> GetAdjacent(Octopus o)
        {
            var adjacent = new List<Octopus>
            {
                GetOctopusAtLocation(o.Location.X - 1, o.Location.Y),
                GetOctopusAtLocation(o.Location.X - 1, o.Location.Y - 1),
                GetOctopusAtLocation(o.Location.X - 1, o.Location.Y + 1),
                GetOctopusAtLocation(o.Location.X, o.Location.Y + 1),
                GetOctopusAtLocation(o.Location.X, o.Location.Y - 1),
                GetOctopusAtLocation(o.Location.X + 1, o.Location.Y - 1),
                GetOctopusAtLocation(o.Location.X + 1, o.Location.Y + 1),
                GetOctopusAtLocation(o.Location.X + 1, o.Location.Y)
            };

            adjacent.RemoveAll(a => a == null);
            
            return adjacent;
        }

        private void ParseOctopii(string[] data)
        {
            for (int y = 0; y < data.Length; y++)
            {
                var row = new List<Octopus>();
                var energies = data[y].Select(c => Convert.ToInt32(char.GetNumericValue(c))).ToList();
                for (int x = 0; x < energies.Count; x++)
                {
                    row.Add(new Octopus(x, y, energies[x]));
                }

                Octopuses.Add(row);
            }
        }
    }
}