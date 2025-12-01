namespace AdventOfCode.ApiService.Models;

public record SubmissionResult
{
    public required bool Success { get; init; }
    public required string Message { get; init; }
    public int? WaitTime { get; init; }
}
