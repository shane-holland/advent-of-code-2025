using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

namespace AdventOfCode.Solutions.Days;

/// <summary>
/// Solution for Day 5: Cafeteria
/// </summary>
public partial class Day05 : ISolution
{

    [GeneratedRegex(@"")]
    private static partial Regex InputParser();

    public string SolveLevel1(string input)
    {
        // Parse input
        var lines = input.Split('\n');

        // Solve Level 1
        var answer = ComputeLevel1(lines);

        return answer.ToString();
    }

    public string SolveLevel2(string input)
    {
        // Parse input
        var lines = input.Split('\n');

        // Solve Level 2
        var answer = ComputeLevel2(lines);

        return answer.ToString();
    }

    private static long ComputeLevel1(string[] lines)
    {
        (var ranges, var ingredients) = ParsePuzzleInput(lines);

        return ingredients.Where(i => ranges.Any(r => i >= r.Item1 && i <= r.Item2)).Count();
    }

    private static long ComputeLevel2(string[] lines)
    {
        (var ranges, var ingredients) = ParsePuzzleInput(lines);
        List<(long, long)> consolidated = new List<(long,long)>();

        bool done = false;
        while (!done)
        {
            done = true;
            for (var i=0; i < ranges.Count; i++)
            {
                var joined = ranges[i];
                for (int j = i+1; j < ranges.Count; j++)
                {
                    if (ranges[j].Item1 <= joined.Item2 && ranges[j].Item2 >= joined.Item1)
                    {
                        joined.Item1 = Math.Min(joined.Item1, ranges[j].Item1);
                        joined.Item2 = Math.Max(joined.Item2, ranges[j].Item2);
                        ranges.RemoveAt(j);
                        j--;
                        done = false;
                    }
                }
                ranges[i]= joined;
            }
        }

        return ranges.Aggregate(0L, (result, value) => result + value.Item2 - value.Item1 + 1);

    }

    private static (List<(long, long)>, List<long>) ParsePuzzleInput(string[] lines)
    {
        var divider = Array.IndexOf(lines, string.Empty);

        var ranges = lines[..divider].Select(i =>
        {
            var nums = i.Split("-");
            return (long.Parse(nums[0]), long.Parse(nums[1]));
        }).ToList();

        var ingredients = lines[(divider+1)..].Select(i => long.Parse(i)).ToList();

        return (ranges, ingredients);
    }
}