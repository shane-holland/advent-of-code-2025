using System.Drawing;
using System.Numerics;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions.Days;

/// <summary>
/// Solution for Day 4: Printing Department
/// </summary>
public partial class Day04 : ISolution
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
        var graph = parseGraph(lines);
        
        var filtered = graph.Where(pair =>  pair.Value.Count < 4)
        .ToDictionary(pair => pair.Key, pair => pair.Value);

        return filtered.Count;
        
    }

    private static long ComputeLevel2(string[] lines)
    {
        var graph = parseGraph(lines);
        
        // Get removable rolls
        var removable = graph.Where(pair =>  pair.Value.Count < 4)
        .ToDictionary(pair => pair.Key, pair => pair.Value);

        var totalRemoved = removable.Count;

        // Filter Graph to remaining rolls
        var updated = graph.Where(pair =>  pair.Value.Count >= 4)
        .ToDictionary(pair => pair.Key, pair => pair.Value);

        while (removable.Count > 0)
        {
            // Remove edges which are no longer available
            foreach( Point point in updated.Keys)
            {
                updated[point] = [.. updated[point].Where(val => !removable.ContainsKey(val))];
            }

            // Get removable rolls
            removable = updated.Where(pair =>  pair.Value.Count < 4)
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            totalRemoved += removable.Count;

            // Filter Graph to remaining rolls
            updated = updated.Where(pair =>  pair.Value.Count >= 4)
                .ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        return totalRemoved;
    }

    private static Dictionary<Point, List<Point>> parseGraph(string[] lines)
    {

        var graph = new Dictionary<Point, List<Point>>();

        var MAX_X = lines[0].Length - 1;
        var MAX_Y = lines.Length - 1;
        for (var y = 0; y < lines.Length; y++)
        {
            var line = lines[y];
            for (var x = 0; x < line.Length; x++)
            {
                if (line[x] == '@')
                {
                    var point = new Point(x,y);
                    graph.Add(point, []);

                    // Find edges
                    for (var i = Math.Max(0, y-1); i <= Math.Min(MAX_Y, y+1); i++)
                    {
                        for (var j = Math.Max(0, x-1); j <= Math.Min(MAX_X, x+1); j++)
                        {
                            if ((j != x || i != y) && lines[i][j] == '@')
                            {
                                graph[point].Add(new Point(j,i));
                            }
                        }
                    }
                }
            }
        }

        return graph;
    }
}