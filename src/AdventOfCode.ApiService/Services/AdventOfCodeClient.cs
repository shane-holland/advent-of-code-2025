using AdventOfCode.ApiService.Models;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode.ApiService.Services;

/// <summary>
/// HTTP client for interacting with Advent of Code website.
/// </summary>
public partial class AdventOfCodeClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AdventOfCodeClient> _logger;
    private const string BaseUrl = "https://adventofcode.com";

    public AdventOfCodeClient(HttpClient httpClient, ILogger<AdventOfCodeClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _httpClient.BaseAddress = new Uri(BaseUrl);
    }

    /// <summary>
    /// Fetches puzzle input for a specific year and day.
    /// </summary>
    /// <param name="year">The puzzle year</param>
    /// <param name="day">The puzzle day (1-25)</param>
    /// <param name="sessionCookie">The user's session cookie</param>
    /// <returns>The puzzle input as a string</returns>
    public async Task<string> GetPuzzleInputAsync(int year, int day, string sessionCookie)
    {
        ValidateParameters(year, day);

        var request = new HttpRequestMessage(HttpMethod.Get, $"/{year}/day/{day}/input");
        request.Headers.Add("Cookie", $"session={sessionCookie}");
        request.Headers.Add("User-Agent", "advent-of-code-2025-api/1.0.0 (.NET 9)");

        _logger.LogInformation("Fetching puzzle input for year {Year}, day {Day}", year, day);

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Failed to fetch puzzle input: {StatusCode}", response.StatusCode);
            throw new HttpRequestException($"Failed to fetch puzzle input: {response.StatusCode}");
        }

        var content = await response.Content.ReadAsStringAsync();
        return content.TrimEnd();
    }

    /// <summary>
    /// Submits a puzzle answer to Advent of Code.
    /// </summary>
    /// <param name="year">The puzzle year</param>
    /// <param name="day">The puzzle day (1-25)</param>
    /// <param name="level">The puzzle part (1 or 2)</param>
    /// <param name="answer">The answer to submit</param>
    /// <param name="sessionCookie">The user's session cookie</param>
    /// <returns>The submission result</returns>
    public async Task<SubmissionResult> SubmitAnswerAsync(int year, int day, int level, string answer, string sessionCookie)
    {
        ValidateParameters(year, day);

        if (level != 1 && level != 2)
        {
            throw new ArgumentException("Level must be 1 or 2", nameof(level));
        }

        var request = new HttpRequestMessage(HttpMethod.Post, $"/{year}/day/{day}/answer");
        request.Headers.Add("Cookie", $"session={sessionCookie}");
        request.Headers.Add("User-Agent", "advent-of-code-2025-api/1.0.0 (.NET 9)");

        var formData = new Dictionary<string, string>
        {
            ["level"] = level.ToString(),
            ["answer"] = answer
        };

        request.Content = new FormUrlEncodedContent(formData);

        _logger.LogInformation("Submitting answer for year {Year}, day {Day}, level {Level}: {Answer}", year, day, level, answer);

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Failed to submit answer: {StatusCode}", response.StatusCode);
            throw new HttpRequestException($"Failed to submit answer: {response.StatusCode}");
        }

        var html = await response.Content.ReadAsStringAsync();
        return ParseSubmissionResponse(html);
    }

    /// <summary>
    /// Parses the HTML response from a submission to determine success or failure.
    /// </summary>
    private SubmissionResult ParseSubmissionResponse(string html)
    {
        if (html.Contains("That's the right answer", StringComparison.OrdinalIgnoreCase))
        {
            return new SubmissionResult
            {
                Success = true,
                Message = "Correct answer!"
            };
        }

        if (html.Contains("That's not the right answer", StringComparison.OrdinalIgnoreCase))
        {
            var message = "Incorrect answer";

            if (html.Contains("too high", StringComparison.OrdinalIgnoreCase))
            {
                message += " (too high)";
            }
            else if (html.Contains("too low", StringComparison.OrdinalIgnoreCase))
            {
                message += " (too low)";
            }

            return new SubmissionResult
            {
                Success = false,
                Message = message
            };
        }

        if (html.Contains("You gave an answer too recently", StringComparison.OrdinalIgnoreCase))
        {
            var waitTime = ExtractWaitTime(html);

            return new SubmissionResult
            {
                Success = false,
                Message = "Rate limited - please wait before submitting again",
                WaitTime = waitTime
            };
        }

        if (html.Contains("You don't seem to be solving the right level", StringComparison.OrdinalIgnoreCase))
        {
            return new SubmissionResult
            {
                Success = false,
                Message = "Wrong level - you may have already solved this part"
            };
        }

        return new SubmissionResult
        {
            Success = false,
            Message = "Unknown response from server"
        };
    }

    /// <summary>
    /// Extracts wait time from rate limit message.
    /// </summary>
    private int? ExtractWaitTime(string html)
    {
        // Pattern: "You have 5m 30s left to wait" or "You have 30s left to wait"
        var match = WaitTimeRegex().Match(html);

        if (match.Success)
        {
            var minutes = match.Groups[1].Success ? int.Parse(match.Groups[1].Value) : 0;
            var seconds = int.Parse(match.Groups[2].Value);
            return minutes * 60 + seconds;
        }

        return null;
    }

    [GeneratedRegex(@"You have (?:(\d+)m )?(\d+)s left to wait")]
    private static partial Regex WaitTimeRegex();

    private static void ValidateParameters(int year, int day)
    {
        var currentYear = DateTime.UtcNow.Year;

        if (year < 2015 || year > currentYear)
        {
            throw new ArgumentException($"Year must be between 2015 and {currentYear}", nameof(year));
        }

        if (day < 1 || day > 25)
        {
            throw new ArgumentException("Day must be between 1 and 25", nameof(day));
        }
    }
}
