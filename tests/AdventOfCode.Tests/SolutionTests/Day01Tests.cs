using AdventOfCode.Solutions.Days;
using Xunit;

namespace AdventOfCode.Tests.SolutionTests;

public class Day01Tests
{
    private readonly Day01 _solution = new();

    [Fact]
    public void TestLevel1_WithSampleInput()
    {
        var input = @"L68
L30
R48
L5
R60
L55
L1
L99
R14
L82
";
        var result = _solution.SolveLevel1(input);
        Assert.Equal("3", result);
    }

    [Fact]
    public void TestLevel2_WithSampleInput()
    {
        var input = @"L68
L30
R48
L5
R60
L55
L1
L99
R14
L82
";
        var result = _solution.SolveLevel2(input);
        Assert.Equal("6", result);
    }
}