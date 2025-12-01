using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions.Days;

/// <summary>
/// Solution for Day 1: Secret Entrance
/// </summary>
public partial class Day01 : ISolution
{
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

    private int ComputeLevel1(string[] lines)
    {
        var count = 0;
        var curpos = 50;
        foreach (var line in lines)
        {
            var turn = AmountToTurn(line);

            curpos = (((curpos + turn) % 100) + 100) % 100;
            if (curpos == 0) {
                count++;
            }
        }

        return count;
    }

    private int ComputeLevel2(string[] lines)
    {
        // Your Level 2 logic here
        var count = 0;
        var curpos = 50;
        foreach (var line in lines)
        {
            var turn = AmountToTurn(line);

            if (curpos + turn < 0 || curpos + turn >= 100) 
            {
                count += Math.Abs((int)((curpos + turn) / 100.0));
            }

            if (curpos != 0 && curpos + turn <= 0)
            {
                count++;
            }

            curpos = (((curpos + turn) % 100) + 100) % 100;
            
        }
            
        // Your Level 1 logic here
        return count;
    }

    private int AmountToTurn(string line)
    {
        var direction = line[0] == 'L'? -1 : 1;
        var amount = int.Parse(line[1..]);

        return direction * amount;
    }
}