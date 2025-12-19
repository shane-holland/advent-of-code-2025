using AdventOfCode.Solutions.Days;
using Xunit;

namespace AdventOfCode.Tests.SolutionTests;

public class Day011Tests
{
    private readonly Day11 _solution = new();
    private static readonly string PUZZLE_INPUT = @"aaa: you hhh
you: bbb ccc
bbb: ddd eee
ccc: ddd eee fff
ddd: ggg
eee: out
fff: out
ggg: out
hhh: ccc fff iii
iii: out";

    private static readonly string LEVEL_1_SOLUTION = "5";
    private static readonly string LEVEL_2_SOLUTION = "2";
    
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