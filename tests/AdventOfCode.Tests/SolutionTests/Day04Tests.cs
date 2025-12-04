using AdventOfCode.Solutions.Days;
using Xunit;

namespace AdventOfCode.Tests.SolutionTests;

public class Day04Tests
{
    private readonly Day04 _solution = new();
    private static readonly string PUZZLE_INPUT = @"..@@.@@@@.
@@@.@.@.@@
@@@@@.@.@@
@.@@@@..@.
@@.@@@@.@@
.@@@@@@@.@
.@.@.@.@@@
@.@@@.@@@@
.@@@@@@@@.
@.@.@@@.@.";

    private static readonly string LEVEL_1_SOLUTION = "13";
    private static readonly string LEVEL_2_SOLUTION = "43";
    
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