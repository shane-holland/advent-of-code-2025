using AdventOfCode.Solutions.Days;
using Xunit;

namespace AdventOfCode.Tests.SolutionTests;

public class Day09Tests
{
    private readonly Day09 _solution = new();
    private static readonly string PUZZLE_INPUT = @"7,1
11,1
11,7
9,7
9,5
2,5
2,3
7,3";

    private static readonly string LEVEL_1_SOLUTION = "50";
    private static readonly string LEVEL_2_SOLUTION = "24";
    
    [Fact]
    public void TestLevel1_WithSampleInput()
    {
        var result = _solution.SolveLevel1(PUZZLE_INPUT);
        Assert.Equal(LEVEL_1_SOLUTION, result);
    }

    [Fact]
    public void TestLevel2_WithSampleInput()
    {
        var result = _solution.SolveLevel2(PUZZLE_INPUT);
        Assert.Equal(LEVEL_2_SOLUTION, result);
    }
}