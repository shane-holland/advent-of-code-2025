using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using AdventOfCode.Cli.Services;

namespace AdventOfCode.Cli.Commands;

/// <summary>
/// Command to fetch puzzle input from Advent of Code and solve it.
/// </summary>
public static class FetchCommand
{
    public static Command Create(IServiceProvider serviceProvider)
    {
        var command = new Command("fetch", "Fetch puzzle input from Advent of Code and solve it");

        var dayArgument = new Argument<int>("day", "Puzzle day (1-25)");
        var levelArgument = new Argument<int>("level", "Puzzle level (1 or 2)");
        var yearOption = new Option<int>("--year", () => 2025, "Puzzle year");
        yearOption.AddAlias("-y");

        command.AddArgument(dayArgument);
        command.AddArgument(levelArgument);
        command.AddOption(yearOption);

        command.SetHandler(async (int day, int level, int year) =>
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var runner = scope.ServiceProvider.GetRequiredService<PuzzleRunner>();
                var aocClient = scope.ServiceProvider.GetRequiredService<AdventOfCodeClient>();

                // Get session cookie from environment variable
                var sessionCookie = Environment.GetEnvironmentVariable("AOC_SESSION");
                if (string.IsNullOrWhiteSpace(sessionCookie))
                {
                    Console.Error.WriteLine("Error: AOC_SESSION environment variable not set");
                    Console.Error.WriteLine("Get your session cookie from https://adventofcode.com and set it:");
                    Console.Error.WriteLine("  export AOC_SESSION=your_session_cookie_here");
                    Environment.Exit(1);
                    return;
                }

                // Fetch input
                Console.WriteLine($"Fetching puzzle input for day {day}...");
                var input = await aocClient.GetPuzzleInputAsync(year, day, sessionCookie);

                // Run solution
                var (answer, executionTime, wasCached) = runner.RunSolution(day, level, input);

                // Output result
                Console.WriteLine($"Answer: {answer}");
                Console.WriteLine($"Execution time: {executionTime}ms{(wasCached ? " (cached)" : "")}");
            }
            catch (HttpRequestException ex)
            {
                Console.Error.WriteLine($"Error fetching from Advent of Code: {ex.Message}");
                Environment.Exit(1);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                Environment.Exit(1);
            }
        }, dayArgument, levelArgument, yearOption);

        return command;
    }
}
