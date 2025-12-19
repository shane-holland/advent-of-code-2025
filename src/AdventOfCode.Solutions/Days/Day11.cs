using System.Drawing;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

namespace AdventOfCode.Solutions.Days;

/// <summary>
/// Solution for Day 11: Reactor
/// </summary>
public partial class Day11 : ISolution
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
        return PathsOut(ParsePuzzleGraph(lines));

    }

    private static long ComputeLevel2(string[] lines)
    {
        return 0;

    }

    private static int PathsOut(Dictionary<string, List<string>> graph)
    {
        var paths = 0;
        Queue<string> nodes = new(["you"]);

        while(nodes.Count > 0)
        {
            var cur = nodes.Dequeue();
            foreach(var node in graph[cur])
            {
                if (node == "out")
                {
                    paths++;
                } else
                {
                    nodes.Enqueue(node);
                }
            }
        }

        return paths;
    }

    private static Dictionary<string, List<string>> ParsePuzzleGraph(string[] lines)
    {
        Dictionary<string, List<string>> graph = [];

        foreach (var line in lines)
        {
            var key = line.Split(": ")[0];
            var val = line.Split(": ")[1].Split(" ");
            graph.Add(key, [..val]);
        }

        return graph;
    }
}