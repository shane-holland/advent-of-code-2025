using System.Drawing;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

namespace AdventOfCode.Solutions.Days;

/// <summary>
/// Solution for Day 7: Laboratories
/// </summary>
public partial class Day07 : ISolution
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
        return ParseSplitterGraph(lines).Count;

    }

    private static long ComputeLevel2(string[] lines)
    {
        var graph = ParseSplitterGraph(lines);

        Point start = new();

        for(var y=0; y < lines.Length; y++)
        {
            if (lines[y].Contains('^'))
            {
                start = new Point(lines[y].IndexOf('^'), y);
                break;
            }
        }

        Dictionary<Point, long> cache = [];
        return PathsFromNode(graph, start, ref cache);

    }

    private static long TryAgain(string[] lines)
    {
        var graph = ParseSplitterGraph(lines);

        Point start = new();

        for(var y=0; y < lines.Length; y++)
        {
            if (lines[y].Contains('^'))
            {
                start = new Point(lines[y].IndexOf('^'), y);
                break;
            }
        }

        Stack<Point> nodes = new([start]);
        long count = 0;

        while(nodes.Count > 0)
        {
            var cur = nodes.Pop();
            if (graph[cur].Item1.HasValue)
            {
                nodes.Push(graph[cur].Item1.Value);
            } else
            {
                count++;
            }

            if (graph[cur].Item2.HasValue)
            {
                nodes.Push(graph[cur].Item2.Value);
            } else
            {
                count++;
            }
        }

        return count;

    }

    private static long PathsFromNode(Dictionary<Point, (Point?, Point?)> graph, Point node, ref Dictionary<Point, long> paths)
    {
        var count = 0L;

        count += !graph[node].Item1.HasValue? 1 : paths.TryGetValue(graph[node].Item1.Value, out long value) ? value : PathsFromNode(graph, graph[node].Item1.Value, ref paths);
        count += !graph[node].Item2.HasValue? 1 : paths.TryGetValue(graph[node].Item2.Value, out long v) ? v : PathsFromNode(graph, graph[node].Item2.Value, ref paths);

        paths[node] = count;

        Console.WriteLine($"Calculated paths from {node}: {count}");

        return count;
    }

    private static Dictionary<Point, (Point?, Point?)> ParseSplitterGraph(string[] lines)
    {
        Dictionary<Point, (Point?, Point?)> graph = [];
        Stack<Point> stack = new();

        for(var y=0; y < lines.Length; y++)
        {
            if (lines[y].Contains('^'))
            {
                Point first = new(lines[y].IndexOf('^'), y);
                stack.Push(first);
                break;
            }
        }

        while(stack.Count > 0)
        {
            var current = stack.Pop();
            if (!graph.TryGetValue(current, out (Point?, Point?) value))
            {
                value = (null, null);
                graph.Add(current, value);
            }

            var y = current.Y+1;
            while(y < lines.Length && (value.Item1 == null && value.Item2 == null))
            {
                // Left
                if (graph[current].Item1 == null && lines[y][current.X-1] == '^')
                {
                    stack.Push(new Point(current.X-1, y));
                    graph[current] = (new Point(current.X-1, y), graph[current].Item2);
                }

                if (graph[current].Item2 == null && lines[y][current.X+1] == '^')
                {
                    stack.Push(new Point(current.X+1, y));
                    graph[current] = (graph[current].Item1, new Point(current.X+1, y));
                }
                y++;
            }
        }

        return graph;
    }
}