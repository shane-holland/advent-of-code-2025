using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace AdventOfCode.Cli.Services;

/// <summary>
/// Service for caching puzzle results based on input data and solution code hash.
/// </summary>
public class CacheService
{
    private readonly string _cacheDirectory;
    private readonly ILogger<CacheService>? _logger;

    public CacheService(ILogger<CacheService>? logger = null)
    {
        _logger = logger;
        // Use a relative path from the current directory instead of navigating up
        var baseDir = Path.Combine(Directory.GetCurrentDirectory(), "data", "cache");
        _cacheDirectory = Path.GetFullPath(baseDir);

        if (!Directory.Exists(_cacheDirectory))
        {
            Directory.CreateDirectory(_cacheDirectory);
        }

        _logger?.LogInformation("Cache directory: {CacheDirectory}", _cacheDirectory);
    }

    /// <summary>
    /// Gets a cached result if available and still valid.
    /// </summary>
    /// <param name="day">The puzzle day</param>
    /// <param name="level">The puzzle level (1 or 2)</param>
    /// <param name="inputHash">Hash of the puzzle input</param>
    /// <param name="solutionHash">Hash of the solution code</param>
    /// <returns>The cached answer, or null if not found or invalid</returns>
    public string? GetCachedResult(int day, int level, string inputHash, string solutionHash)
    {
        try
        {
            var cacheKey = GenerateCacheKey(day, level, inputHash, solutionHash);
            var cacheFile = Path.Combine(_cacheDirectory, $"{cacheKey}.json");

            if (!File.Exists(cacheFile))
            {
                return null;
            }

            var json = File.ReadAllText(cacheFile);
            var cacheEntry = JsonSerializer.Deserialize<CacheEntry>(json);

            if (cacheEntry == null)
            {
                return null;
            }

            // Verify the hashes match
            if (cacheEntry.InputHash != inputHash || cacheEntry.SolutionHash != solutionHash)
            {
                _logger?.LogInformation("Cache miss: Hash mismatch for day {Day} level {Level}", day, level);
                return null;
            }

            _logger?.LogInformation("Cache hit for day {Day} level {Level}", day, level);
            return cacheEntry.Answer;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error reading from cache for day {Day} level {Level}", day, level);
            return null;
        }
    }

    /// <summary>
    /// Stores a result in the cache.
    /// </summary>
    /// <param name="day">The puzzle day</param>
    /// <param name="level">The puzzle level (1 or 2)</param>
    /// <param name="inputHash">Hash of the puzzle input</param>
    /// <param name="solutionHash">Hash of the solution code</param>
    /// <param name="answer">The answer to cache</param>
    public void SetCachedResult(int day, int level, string inputHash, string solutionHash, string answer)
    {
        try
        {
            var cacheKey = GenerateCacheKey(day, level, inputHash, solutionHash);
            var cacheFile = Path.Combine(_cacheDirectory, $"{cacheKey}.json");

            var cacheEntry = new CacheEntry
            {
                Day = day,
                Level = level,
                InputHash = inputHash,
                SolutionHash = solutionHash,
                Answer = answer,
                Timestamp = DateTime.UtcNow
            };

            var json = JsonSerializer.Serialize(cacheEntry, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(cacheFile, json);

            _logger?.LogInformation("Cached result for day {Day} level {Level}", day, level);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error writing to cache for day {Day} level {Level}", day, level);
        }
    }

    /// <summary>
    /// Computes a hash of the puzzle input.
    /// </summary>
    /// <param name="input">The puzzle input</param>
    /// <returns>A hexadecimal hash string</returns>
    public static string ComputeInputHash(string input)
    {
        var bytes = Encoding.UTF8.GetBytes(input);
        var hash = SHA256.HashData(bytes);
        return Convert.ToHexString(hash);
    }

    private static string GenerateCacheKey(int day, int level, string inputHash, string solutionHash)
    {
        var combined = $"{day}:{level}:{inputHash}:{solutionHash}";
        var bytes = Encoding.UTF8.GetBytes(combined);
        var hash = SHA256.HashData(bytes);
        return Convert.ToHexString(hash);
    }

    private class CacheEntry
    {
        public int Day { get; set; }
        public int Level { get; set; }
        public string InputHash { get; set; } = string.Empty;
        public string SolutionHash { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }
}
