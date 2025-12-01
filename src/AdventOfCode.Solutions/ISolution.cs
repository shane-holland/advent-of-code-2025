namespace AdventOfCode.Solutions;

/// <summary>
/// Interface for Advent of Code puzzle solutions.
/// Implementations should be named following the pattern: Day{NN} where NN is zero-padded day number.
/// </summary>
public interface ISolution
{
    /// <summary>
    /// Solves level 1 of the puzzle for the given input.
    /// </summary>
    /// <param name="input">The puzzle input as a string</param>
    /// <returns>The answer to level 1</returns>
    string SolveLevel1(string input);

    /// <summary>
    /// Solves level 2 of the puzzle for the given input.
    /// </summary>
    /// <param name="input">The puzzle input as a string</param>
    /// <returns>The answer to level 2</returns>
    string SolveLevel2(string input);
}
