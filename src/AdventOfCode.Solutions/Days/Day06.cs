using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

namespace AdventOfCode.Solutions.Days;

/// <summary>
/// Solution for Day 6: Trash Compactor
/// </summary>
public partial class Day06 : ISolution
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
        return AggregateSolutions(ParsePuzzleInput(lines));
    }

    private static long ComputeLevel2(string[] lines)
    {
        return AggregateSolutions(ParsePuzzleInput2(lines));

    }

    private static long AggregateSolutions(List<(List<long>, string)> problems)
    {
        return problems.Aggregate(0L, (result, problem) =>
        {
            return result + problem.Item1.Aggregate(problem.Item2 == "*"? 1L: 0L, (result, num) =>
            {
                return problem.Item2 == "*"? result * num : result + num;
            });
        });
    }

    private static List<(List<long>, string)> ParsePuzzleInput(string[] lines)
    {
        List<(List<long>, string)> problems = [];
        var operators = lines[^1].Split(" ", StringSplitOptions.TrimEntries)
            .Where(i => !string.IsNullOrWhiteSpace(i))
            .ToList();

        problems = [.. operators.Select(item => (new List<long>(), item))];

        foreach(var line in lines[..^1])
        {
            var numbers = line.Split(" ", StringSplitOptions.TrimEntries)
                .Where(i => ! string.IsNullOrWhiteSpace(i))
                .Select(int.Parse)
                .ToList();

            for (var i=0; i<numbers.Count; i++)
            {
                problems[i].Item1.Add(numbers[i]);
            }
        }

        return problems;
    }

    private static List<(List<long>, string)> ParsePuzzleInput2 (string[] lines)
    {
        List<(List<long>, string)> problems = [];
        var operators = lines[^1].Split(" ", StringSplitOptions.TrimEntries)
            .Where(i => !string.IsNullOrWhiteSpace(i))
            .ToList();

        problems = [.. operators.Select(item => (new List<long>(), item))];

        var index = 0;

        for (var i=0; i<lines.Aggregate(0, (result, line) => Math.Max(result, line.Length)); i++)
        {
            var num = "";
            for (var j=0; j<lines.Length; j++)
            {
                if (lines[j].Length > i && !((List<string>)[" ", "+", "*"]).Contains(lines[j][i].ToString()))
                {
                    num += lines[j][i];
                }
            }

            if (!string.IsNullOrEmpty(num)) 
            {
                problems[index].Item1.Add(long.Parse(num));
            } else
            {
                index++;
            }
        }

        return problems;
    }
}