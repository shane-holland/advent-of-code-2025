using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions.Days;

/// <summary>
/// Solution for Day 3: XXXXXXX
/// </summary>
public partial class Day03 : ISolution
{

    [GeneratedRegex(@"")]
    private static partial Regex InputParser();

    public string SolveLevel1(string input)
    {
        // Parse input
        var lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        // Solve Level 1
        var answer = ComputeLevel1(lines);

        return answer.ToString();
    }

    public string SolveLevel2(string input)
    {
        // Parse input
        var lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        // Solve Level 2
        var answer = ComputeLevel2(lines);

        return answer.ToString();
    }

    private static long ComputeLevel1(string[] lines)
    {
        return 0;
    }

    private static long ComputeLevel2(string[] lines)
    {
        return 0;
    }
}