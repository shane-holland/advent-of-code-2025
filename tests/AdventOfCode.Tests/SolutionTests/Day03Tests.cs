using AdventOfCode.Solutions.Days;
using Xunit;

namespace AdventOfCode.Tests.SolutionTests;

public class Day03Tests
{
    private readonly Day03 _solution = new();
    private static readonly string PUZZLE_INPUT = @"";

    private static readonly string LEVEL_1_SOLUTION = "0";
    private static readonly string LEVEL_2_SOLUTION = "0";
    
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