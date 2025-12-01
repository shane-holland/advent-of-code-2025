using AdventOfCode.ApiService.Models;
using AdventOfCode.ApiService.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdventOfCode.ApiService.Controllers;

[ApiController]
[Route("api/puzzle")]
public class PuzzleController : ControllerBase
{
    private readonly PuzzleRunner _puzzleRunner;
    private readonly AdventOfCodeClient _aocClient;
    private readonly ILogger<PuzzleController> _logger;
    private const int DefaultYear = 2025;

    public PuzzleController(
        PuzzleRunner puzzleRunner,
        AdventOfCodeClient aocClient,
        ILogger<PuzzleController> logger)
    {
        _puzzleRunner = puzzleRunner;
        _aocClient = aocClient;
        _logger = logger;
    }

    /// <summary>
    /// Runs a puzzle solution and returns the answer (fetches input from Advent of Code).
    /// </summary>
    /// <param name="day">The puzzle day (1-25)</param>
    /// <param name="level">The puzzle level (1 or 2)</param>
    /// <returns>The puzzle answer with execution time</returns>
    [HttpGet("day/{day}/level/{level}")]
    [ProducesResponseType(typeof(PuzzleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<PuzzleResponse>> GetPuzzleAnswer(int day, int level)
    {
        try
        {
            // Validate parameters
            if (!ValidateDayAndLevel(day, level, out var validationError))
            {
                return BadRequest(new { error = validationError });
            }

            // Check if solution exists
            if (!_puzzleRunner.HasSolution(day))
            {
                return NotFound(new { error = $"No solution implemented for day {day}" });
            }

            // Get session cookie from header
            var sessionCookie = GetSessionCookie();
            if (string.IsNullOrEmpty(sessionCookie))
            {
                return Unauthorized(new { error = "Session cookie is required. Provide it via 'Session' header." });
            }

            // Fetch input from Advent of Code
            _logger.LogInformation("Fetching puzzle input for day {Day}", day);
            var input = await _aocClient.GetPuzzleInputAsync(DefaultYear, day, sessionCookie);

            // Run the solution
            var (answer, executionTime, wasCached) = _puzzleRunner.RunSolution(day, level, input);

            _logger.LogInformation("Solution completed for day {Day} level {Level} in {ExecutionTime}ms (cached: {WasCached})",
                day, level, executionTime, wasCached);

            return Ok(new PuzzleResponse
            {
                Puzzle = new PuzzleIdentifier { Day = day, Level = level },
                ExecutionTime = wasCached ? "0ms (cached)" : $"{executionTime}ms",
                Answer = answer
            });
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error while fetching puzzle input");
            return StatusCode(StatusCodes.Status502BadGateway, new { error = "Failed to fetch puzzle input from Advent of Code" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error running puzzle solution");
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
        }
    }

    /// <summary>
    /// Runs a puzzle solution with provided input.
    /// </summary>
    /// <param name="day">The puzzle day (1-25)</param>
    /// <param name="level">The puzzle level (1 or 2)</param>
    /// <returns>The puzzle answer with execution time</returns>
    [HttpPut("day/{day}/level/{level}")]
    [Consumes("text/plain")]
    [ProducesResponseType(typeof(PuzzleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PuzzleResponse>> RunPuzzleWithInput(int day, int level)
    {
        try
        {
            // Validate parameters
            if (!ValidateDayAndLevel(day, level, out var validationError))
            {
                return BadRequest(new { error = validationError });
            }

            // Check if solution exists
            if (!_puzzleRunner.HasSolution(day))
            {
                return NotFound(new { error = $"No solution implemented for day {day}" });
            }

            // Read input from request body
            using var reader = new StreamReader(Request.Body);
            var input = await reader.ReadToEndAsync();

            if (string.IsNullOrWhiteSpace(input))
            {
                return BadRequest(new { error = "Request body must contain puzzle input" });
            }

            // Run the solution
            var (answer, executionTime, wasCached) = _puzzleRunner.RunSolution(day, level, input.TrimEnd());

            _logger.LogInformation("Solution completed for day {Day} level {Level} in {ExecutionTime}ms (cached: {WasCached})",
                day, level, executionTime, wasCached);

            return Ok(new PuzzleResponse
            {
                Puzzle = new PuzzleIdentifier { Day = day, Level = level },
                ExecutionTime = wasCached ? "0ms (cached)" : $"{executionTime}ms",
                Answer = answer
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error running puzzle solution");
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
        }
    }

    /// <summary>
    /// Runs a puzzle solution and submits the answer to Advent of Code.
    /// </summary>
    /// <param name="day">The puzzle day (1-25)</param>
    /// <param name="level">The puzzle level (1 or 2)</param>
    /// <returns>The puzzle answer, execution time, and submission result</returns>
    [HttpPost("day/{day}/level/{level}")]
    [ProducesResponseType(typeof(PuzzleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<PuzzleResponse>> SubmitPuzzleAnswer(int day, int level)
    {
        try
        {
            // Validate parameters
            if (!ValidateDayAndLevel(day, level, out var validationError))
            {
                return BadRequest(new { error = validationError });
            }

            // Check if solution exists
            if (!_puzzleRunner.HasSolution(day))
            {
                return NotFound(new { error = $"No solution implemented for day {day}" });
            }

            // Get session cookie from header
            var sessionCookie = GetSessionCookie();
            if (string.IsNullOrEmpty(sessionCookie))
            {
                return Unauthorized(new { error = "Session cookie is required. Provide it via 'Session' header." });
            }

            // Fetch input from Advent of Code
            _logger.LogInformation("Fetching puzzle input for day {Day}", day);
            var input = await _aocClient.GetPuzzleInputAsync(DefaultYear, day, sessionCookie);

            // Run the solution
            var (answer, executionTime, wasCached) = _puzzleRunner.RunSolution(day, level, input);

            _logger.LogInformation("Solution completed for day {Day} level {Level} in {ExecutionTime}ms (cached: {WasCached})",
                day, level, executionTime, wasCached);

            // Submit the answer
            _logger.LogInformation("Submitting answer for day {Day} level {Level}: {Answer}", day, level, answer);
            var submissionResult = await _aocClient.SubmitAnswerAsync(DefaultYear, day, level, answer, sessionCookie);

            return Ok(new PuzzleResponse
            {
                Puzzle = new PuzzleIdentifier { Day = day, Level = level },
                ExecutionTime = wasCached ? "0ms (cached)" : $"{executionTime}ms",
                Answer = answer,
                Result = submissionResult.Success ? "success" : $"failure: {submissionResult.Message}"
            });
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error while communicating with Advent of Code");
            return StatusCode(StatusCodes.Status502BadGateway, new { error = "Failed to communicate with Advent of Code" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error running puzzle solution");
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
        }
    }

    private static bool ValidateDayAndLevel(int day, int level, out string? error)
    {
        if (day < 1 || day > 25)
        {
            error = "Day must be between 1 and 25";
            return false;
        }

        if (level != 1 && level != 2)
        {
            error = "Level must be 1 or 2";
            return false;
        }

        error = null;
        return true;
    }

    private string? GetSessionCookie()
    {
        // Try to get from header first
        if (Request.Headers.TryGetValue("Session", out var sessionHeader))
        {
            return sessionHeader.FirstOrDefault();
        }

        // Fallback to cookie
        if (Request.Cookies.TryGetValue("session", out var sessionCookie))
        {
            return sessionCookie;
        }

        return null;
    }
}
