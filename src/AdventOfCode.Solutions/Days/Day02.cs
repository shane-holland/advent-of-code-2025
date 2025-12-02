using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions.Days;

/// <summary>
/// Solution for Day 2: Gift Shop
/// </summary>
public partial class Day02 : ISolution
{

    [GeneratedRegex(@"(\d+)-(\d+)")]
    private static partial Regex RangesRegex();

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
        long productIdSum = 0;

        foreach((var start, var end) in ParseRanges(lines))
        {
            productIdSum += BadSolve1(start, end);
        }

        return productIdSum;
    }

    private static long ComputeLevel2(string[] lines)
    {
        long productIdSum = 0;

        foreach((var start, var end) in ParseRanges(lines))
        {
            productIdSum += BadSolve2(start, end);
        }

        return productIdSum;
    }

    private static long BadSolve1(long start, long end)
    {
        long result = 0;

        for (long num = start; num <= end; num++)
        {
            string numStr = num.ToString();
            // Check for odd number of digits and immediately skip to even digits if possible
            if (numStr.Length % 2 != 0)
            {
                long nextAvailable = (long) Math.Pow(10, numStr.Length);
                if (end < nextAvailable)
                {
                    // No need to continue, all other numbers in the range are not possible matches
                    break;
                } 
                num = nextAvailable - 1;
            } else if (Split(numStr, numStr.Length/2).Distinct().Count() == 1)
            {
                Console.WriteLine($"Found match: {num}");
                result += num;
            }
        }
        return result;
    }

    private static long BadSolve2(long start, long end)
    {
        
        
        return Enumerable.Range(0, (int)((int) end - start + 1)).Aggregate(0L, (result, item) =>
        {
            long num = item + start;
            string numStr = num.ToString();
            for (var len = numStr.Length / 2; len >= 1; len--)
            {
               if (numStr.Length % len == 0)
                {
                    if (Split(numStr, len).Distinct().Count() == 1)
                    {
                        Console.WriteLine($"Found match: {num}");
                        result += num;
                        break;
                    }
                }
            }
            return result;
        });
    }

    static IEnumerable<string> Split(string str, int chunkSize)
    {
        return Enumerable.Range(0, str.Length / chunkSize)
            .Select(i => str.Substring(i * chunkSize, chunkSize));
    }

    private static List<(long, long)> ParseRanges(string[] lines)
    {
        var ranges = new List<(long, long)>();

        foreach(var line in lines)
        {
            var matches = RangesRegex().Matches(line);
            if (matches == null || matches.Count == 0)
            {
                throw new InvalidOperationException($"Line parsing failed for line: {line}");
            }
            
            foreach(Match match in matches)
            {
                ranges.Add((long.Parse(match.Groups[1].Value), long.Parse(match.Groups[2].Value)));
            }
        }

        return ranges;
    }
}