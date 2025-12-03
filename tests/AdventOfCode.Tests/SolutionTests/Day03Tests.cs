using AdventOfCode.Solutions.Days;
using Xunit;

namespace AdventOfCode.Tests.SolutionTests;

public class Day03Tests
{
    private readonly Day03 _solution = new();
    private static readonly string PUZZLE_INPUT = @"987654321111111
811111111111119
234234234234278
818181911112111";

    private static readonly string LEVEL_1_SOLUTION = "357";
    private static readonly string LEVEL_2_SOLUTION = "3121910778619";
    
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