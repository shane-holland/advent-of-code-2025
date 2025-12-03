using System.Globalization;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions.Days;

/// <summary>
/// Solution for Day 3: Lobby
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
        long totalJoltage = 0;
        
        foreach( var line in lines)
        {
            var joltage = ComputeLargestJoltage(line, 2);

            totalJoltage += joltage;
        }

        return totalJoltage;
    }

    private static long ComputeLevel2(string[] lines)
    {
        long totalJoltage = 0;
        
        foreach( var line in lines)
        {
            var joltage = ComputeLargestJoltage(line, 12);

            totalJoltage += joltage;
        }

        return totalJoltage;
    }

    private static long ComputeLargestJoltage(string batteryBank, int length)
    {
        string joltage = "";

        int index = 0;

        while(joltage.Length < length)
        {
            var digit = 0;
            for(var i = index; i<batteryBank.Length - (length - joltage.Length - 1); i++)
            {
                if (int.Parse(batteryBank[i].ToString()) > digit)
                {
                    digit = int.Parse(batteryBank[i].ToString());
                    index = i+1;
                }
            }
            joltage += digit.ToString();
        }


        Console.WriteLine($"{batteryBank} <-- {joltage}");

        return long.Parse(joltage);
    }
}