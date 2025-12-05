using AdventOfCode.Solutions.Days;
using Xunit;

namespace AdventOfCode.Tests.SolutionTests;

public class Day05Tests
{
    private readonly Day05 _solution = new();
    private static readonly string PUZZLE_INPUT = @"3-5
10-14
16-20
12-18

1
5
8
11
17
32";

    private static readonly string LEVEL_1_SOLUTION = "3";
    private static readonly string LEVEL_2_SOLUTION = "14";
    
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