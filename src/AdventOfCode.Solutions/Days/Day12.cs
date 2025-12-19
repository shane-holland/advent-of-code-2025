using System.Drawing;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

namespace AdventOfCode.Solutions.Days;

/// <summary>
/// Solution for Day 12: Christmas Tree Farm
/// </summary>
public partial class Day12 : ISolution
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