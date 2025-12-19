using AdventOfCode.Solutions.Days;
using Xunit;

namespace AdventOfCode.Tests.SolutionTests;

public class Day06Tests
{
    private readonly Day06 _solution = new();
    private static readonly string PUZZLE_INPUT = @"123 328  51 64 
 45 64  387 23 
  6 98  215 314
*   +   *   +  ";

    private static readonly string LEVEL_1_SOLUTION = "4277556";
    private static readonly string LEVEL_2_SOLUTION = "3263827";
    
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