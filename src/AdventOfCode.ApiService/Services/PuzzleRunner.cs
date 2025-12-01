using AdventOfCode.Solutions;
using System.Diagnostics;

namespace AdventOfCode.ApiService.Services;

/// <summary>
/// Service for running puzzle solutions and managing execution.
/// </summary>
public class PuzzleRunner
{
    private readonly SolutionRegistry _registry;
    private readonly CacheService _cacheService;
    private readonly ILogger<PuzzleRunner> _logger;

    public PuzzleRunner(SolutionRegistry registry, CacheService cacheService, ILogger<PuzzleRunner> logger)
    {
        _registry = registry;
        _cacheService = cacheService;
        _logger = logger;
    }

    /// <summary>
    /// Runs a puzzle solution for a specific day and level.
    /// </summary>
    /// <param name="day">The puzzle day (1-25)</param>
    /// <param name="level">The puzzle level (1 or 2)</param>
    /// <param name="input">The puzzle input</param>
    /// <returns>A tuple containing (answer, executionTimeMs, wasCached)</returns>
    public (string Answer, long ExecutionTimeMs, bool WasCached) RunSolution(int day, int level, string input)
    {
        if (level != 1 && level != 2)
        {
            throw new ArgumentException("Level must be 1 or 2", nameof(level));
        }

        var solution = _registry.GetSolution(day);
        if (solution == null)
        {
            throw new InvalidOperationException($"No solution found for day {day}");
        }

        // Compute hashes for caching
        var inputHash = CacheService.ComputeInputHash(input);
        var solutionHash = _registry.GetSolutionHash(day);

        // Check cache first
        var cachedResult = _cacheService.GetCachedResult(day, level, inputHash, solutionHash);
        if (cachedResult != null)
        {
            _logger.LogInformation("Using cached result for day {Day} level {Level}", day, level);
            return (cachedResult, 0, true);
        }

        // Run the solution
        _logger.LogInformation("Executing solution for day {Day} level {Level}", day, level);
        var stopwatch = Stopwatch.StartNew();

        var answer = level == 1 ? solution.SolveLevel1(input) : solution.SolveLevel2(input);

        stopwatch.Stop();

        // Cache the result
        _cacheService.SetCachedResult(day, level, inputHash, solutionHash, answer);

        return (answer, stopwatch.ElapsedMilliseconds, false);
    }

    /// <summary>
    /// Checks if a solution exists for a specific day.
    /// </summary>
    /// <param name="day">The puzzle day (1-25)</param>
    /// <returns>True if a solution exists, false otherwise</returns>
    public bool HasSolution(int day)
    {
        return _registry.HasSolution(day);
    }

    /// <summary>
    /// Gets all available solution days.
    /// </summary>
    /// <returns>An enumerable of day numbers that have solutions</returns>
    public IEnumerable<int> GetAvailableDays()
    {
        return _registry.GetAvailableDays();
    }
}
