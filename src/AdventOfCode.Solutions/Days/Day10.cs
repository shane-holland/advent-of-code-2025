using System.Drawing;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

namespace AdventOfCode.Solutions.Days;

/// <summary>
/// Solution for Day 10: Factory
/// </summary>
public partial class Day10 : ISolution
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
        List<Shreds> machines = ParsePuzzleInput(lines);

        return machines.Aggregate(0, (result, m) => result + FewestPresses(m));

    }

    private static long ComputeLevel2(string[] lines)
    {
        return 0;

    }

    private static int FewestPresses(Shreds machine)
    {
        if (machine.Buttons.Contains(machine.Indicator))
        {
            return 1;
        }
        
        var presses = 1;

        while(presses++ < 10)
        {
            var queue = ButtonPressQueue(machine.Buttons, presses);
            while (queue.Count > 0)
            {
                var attempt = queue.Dequeue();
                var cur = 0;
                foreach(var press in attempt)
                {
                    cur ^= press;
                }

                if (cur == machine.Indicator)
                {
                    return presses;
                }
            }
        }

        throw new IndexOutOfRangeException("Exceeded 10 button presses");
    }

    private static Queue<List<int>> ButtonPressQueue(List<int> buttons, int depth)
    {
        Queue<List<int>> queue = [];
        RecursivePresses(ref queue, [], buttons, 0, depth);
        return queue;
    }

    private static void RecursivePresses(ref Queue<List<int>> queue, List<int> current, List<int> buttons, int index, int depth)
    {
        for(var i=index; i<buttons.Count; i++)
        {
            if (depth == 1)
            {
                queue.Enqueue([..current.Append(buttons[i])]);
            } else
            {
                RecursivePresses(ref queue, [..current.Append(buttons[i])], buttons, i, depth - 1);
            }
        }
    }

    private static List<Shreds> ParsePuzzleInput(string[] lines)
    {
        List<Shreds> machines = [];
        foreach (var line in lines)
        {
            var components = line.Split(" ");
            string lights = components[0][1..(components[0].Length - 1)];
            int indicator = Convert.ToInt32(lights.Replace(".","0").Replace("#","1"), 2);

            List<int> buttons = components[1..(components.Length - 1)].Select(i => ParseButton(i, lights.Length)).ToList();
            List<int> joltages = ParseJoltages(components.Last());

            machines.Add(new Shreds(indicator, buttons, joltages));
        }
        return machines;
    }

    private static int ParseButton(string but, int length)
    {
        // Seed the button string
        char[] button = new char[length];
        for(var i=0; i< length; i++)
        {
            button[i]= '0';
        }

        foreach(var n in but[1..(but.Length-1)].Split(","))
        {
            button[int.Parse(n)] = '1';
        }

        return Convert.ToInt32(new string(button), 2);
    }

    private static List<int> ParseJoltages(string joltages)
    {
        return joltages[1..(joltages.Length-1)].Split(",").Select(n => int.Parse(n)).ToList();
    }

    public record Shreds(int Indicator, List<int> Buttons, List<int> Joltages)
    {
        
    }
}