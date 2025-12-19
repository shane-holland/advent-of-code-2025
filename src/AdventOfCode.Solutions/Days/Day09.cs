using System.Drawing;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

namespace AdventOfCode.Solutions.Days;

/// <summary>
/// Solution for Day 9: Movie Theater
/// </summary>
public partial class Day09 : ISolution
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
        long largest = 0;

        List<Point> points = ParsePuzzleInput(lines);

        while(points.Count > 1)
        {
            var current = points[0];
            points.Remove(current);

            foreach(var point in points)
            {
                var area = GetRectangleArea(current, point);
                if (area > largest)
                {
                    Console.WriteLine($"{current}, {point}: {area}");
                    largest = area;
                }
                
            }
        }

        return largest;

    }

    private static long ComputeLevel2(string[] lines)
    {
        long largest = 0;

        List<Point> all = ParsePuzzleInput(lines);
        List<Point> points = [..all];

        while(points.Count > 1)
        {
            var current = points[0];
            points.Remove(current);

            foreach(var point in points)
            {
                if (PointIsRedOrGreen(new Point(current.X, point.Y), all) && PointIsRedOrGreen(new Point(point.X, current.Y), all))
                {
                    var area = GetRectangleArea(current, point);
                    if (area > largest)
                    {
                        Console.WriteLine($"{current}, {point}: {area}");
                                largest = area;
                    }
                }
                
            }
        }

        return largest;

    }

    private static bool PointIsRedOrGreen(Point point, List<Point> points)
    {
        // Red
        if (points.Contains(point))
        {
            return true;
        }
        // Green (Vertically Inclusive)
        if(
            (points.Where(p => p.X == point.X && p.Y <= point.Y).ToList().Count > 0) && 
            (points.Where(p => p.X == point.X && p.Y >= point.Y).ToList().Count > 0)
        )
        {
            return true;
        }
        // Green (Horizontally Inclusive)
        if(
            (points.Where(p => p.Y == point.Y && p.X <= point.X).ToList().Count > 0) && 
            (points.Where(p => p.Y == point.Y && p.X >= point.X).ToList().Count > 0)
        )
        {
            return true;
        }

        return false;
    }

    private static List<Point> ParsePuzzleInput(string[] lines)
    {
        return [.. lines.Select(l => new Point(int.Parse(l.Split(",")[0]), int.Parse(l.Split(",")[1])))];
    }

    private static long GetRectangleArea(Point p1, Point p2)
    {
        return ((long)(Math.Abs(p1.X - p2.X)+1)) * ((long)(Math.Abs(p1.Y - p2.Y)+1));
    }
}