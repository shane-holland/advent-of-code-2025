using System.Drawing;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

namespace AdventOfCode.Solutions.Days;

/// <summary>
/// Solution for Day 8: Cafeteria
/// </summary>
public partial class Day08 : ISolution
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
        List<Point3d> points = [..lines.Select(Point3d.Parse)];
        Dictionary<Point3d, List<Point3d>> closest = ConnectClosest(points, lines.Length < 100? 10 : 1000);

        List<List<Point3d>> networks = [];
        List<Point3d> visited = [];


        Stack<Point3d> stack = new(closest.Keys);
        
        Console.WriteLine($"Collecting Networks from closest nodes...");
        while (stack.Count > 0)
        {
            var current = stack.Pop();
            List<Point3d> network = [];
            if (!visited.Contains(current))
            {
                network.Add(current);
                visited.Add(current);
                Queue<Point3d> nodes = new(closest[current]);

                while(nodes.Count>0)
                {
                    var node = nodes.Dequeue();
                    if(!visited.Contains(node))
                    {
                        network.Add(node);
                        visited.Add(node);

                        foreach(var c in closest[node])
                        {
                            if (!visited.Contains(c))
                            {
                                nodes.Enqueue(c);
                            }
                        }
                    }
                }
            }
            
            if (network.Count > 0) {
                Console.WriteLine($"====> Added network -- {network.Count} items");
                networks.Add(network);
            }
        }

        networks = [.. networks.OrderBy(i => i.Count).Reverse()];

        return networks[0].Count * networks[1].Count * networks[2].Count;

    }

    private static long ComputeLevel2(string[] lines)
    {
        return CreateSingleNetwork([..lines.Select(Point3d.Parse)]);

    }
    

    private static Dictionary<Point3d, List<Point3d>> ConnectClosest(List<Point3d> points, int connections)
    {
        Dictionary<Point3d, List<Point3d>> closest = [];
        
        Console.WriteLine($"Connecting the first {connections} closest nodes...");

        for(var i=0; i < connections; i++) {
            var dist = double.MaxValue;
            (Point3d, Point3d) nearest = (new(0,0,0), new(0,0,0));

            foreach(var p1 in points)
            {
                foreach (var p2 in points)
                {
                    if ((!closest.ContainsKey(p1) || !closest[p1].Contains(p2)) && p1 != p2 && p1.Distance(p2) < dist)
                    {
                        dist = p1.Distance(p2);
                        nearest = (p1, p2);
                    }
                }
                
            }

            if (!closest.TryGetValue(nearest.Item1, out List<Point3d>? v1)) {
                closest.Add(nearest.Item1, [nearest.Item2]);
            } else
            {
                v1.Add(nearest.Item2);
            }

            if (!closest.TryGetValue(nearest.Item2, out List<Point3d>? v2)) {
                closest.Add(nearest.Item2, [nearest.Item1]);
            } else
            {
                v2.Add(nearest.Item1);
            }

            Console.WriteLine($"--> Connected {i+1} closest connections...");
        }

        return closest;
    }

    private static long CreateSingleNetwork(List<Point3d> points)
    {

        Dictionary<Point3d, List<Point3d>> closest = [];
        List<Point3d> largestNetwork = [];
        long answer = 0;
        
        Console.WriteLine($"Connecting the closest nodes...");   

        while(answer == 0) {
            var dist = double.MaxValue;
            (Point3d, Point3d) nearest = (new(0,0,0), new(0,0,0));

            // Find the next closest connection
            foreach(var p1 in points.Where(p => !largestNetwork.Contains(p)))
            {
                foreach (var p2 in points)
                {
                    if ((!closest.ContainsKey(p1) || !closest[p1].Contains(p2)) && p1 != p2 && p1.Distance(p2) < dist)
                    {
                        dist = p1.Distance(p2);
                        nearest = (p1, p2);
                    }
                }   
            }

            if (!closest.TryGetValue(nearest.Item1, out List<Point3d>? v1)) {
                closest.Add(nearest.Item1, [nearest.Item2]);
            } else
            {
                v1.Add(nearest.Item2);
                
            }

            if (!closest.TryGetValue(nearest.Item2, out List<Point3d>? v2)) {
                closest.Add(nearest.Item2, [nearest.Item1]);
            } else
            {
                v2.Add(nearest.Item1);
            }

            // Check the network from that connection
            if (!largestNetwork.Contains(nearest.Item1) || !largestNetwork.Contains(nearest.Item2))
            {
                List<Point3d> visited = [];
                Stack<Point3d> stack = new(closest.Keys);

                while (stack.Count > 0)
                {
                    var current = stack.Pop();
                    List<Point3d> network = [];
                    if (!visited.Contains(current))
                    {
                        network.Add(current);
                        visited.Add(current);
                        Queue<Point3d> nodes = new(closest[current]);

                        while(nodes.Count>0)
                        {
                            var node = nodes.Dequeue();
                            if(!visited.Contains(node))
                            {
                                network.Add(node);
                                visited.Add(node);

                                foreach(var c in closest[node])
                                {
                                    if (!visited.Contains(c))
                                    {
                                        nodes.Enqueue(c);
                                    }
                                }
                            }
                        }
                    }

                    if (network.Count > largestNetwork.Count)
                    {
                        largestNetwork = network;
                        Console.WriteLine($"New network created with {network.Count} items:  Target={points.Count}");
                        answer = network.Count == points.Count? (long)nearest.Item1.X * nearest.Item2.X : 0;
                    } 
                    
                }
            }

            
            
        }

        return answer;
    }

    private record Point3d(int X, int Y, int Z)
    {
        public double Distance(Point3d p)
        {
            return Math.Sqrt(
                Math.Pow(X-p.X, 2) + Math.Pow(Y-p.Y, 2) + Math.Pow(Z-p.Z, 2)
            );
        }

        public static Point3d Parse(string item)
        {
            int[] coords = [..item.Split(",").Select(int.Parse)];
            return new Point3d(coords[0], coords[1], coords[2]);
        }
    }
}