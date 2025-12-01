namespace AdventOfCode.ApiService.Models;

public record PuzzleResponse
{
    public required PuzzleIdentifier Puzzle { get; init; }
    public required string ExecutionTime { get; init; }
    public required string Answer { get; init; }
    public string? Result { get; init; }
}

public record PuzzleIdentifier
{
    public required int Day { get; init; }
    public required int Level { get; init; }
}
