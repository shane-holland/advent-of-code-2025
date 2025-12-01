using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using AdventOfCode.Cli.Services;

namespace AdventOfCode.Cli.Commands;

/// <summary>
/// Command to solve a puzzle with custom input from stdin or file.
/// </summary>
public static class SolveCommand
{
    public static Command Create(IServiceProvider serviceProvider)
    {
        var command = new Command("solve", "Solve a puzzle with custom input (from stdin or file)");

        var dayArgument = new Argument<int>("day", "Puzzle day (1-25)");
        var levelArgument = new Argument<int>("level", "Puzzle level (1 or 2)");
        var inputOption = new Option<FileInfo?>("--input", "Input file (if not provided, reads from stdin)");
        inputOption.AddAlias("-i");

        command.AddArgument(dayArgument);
        command.AddArgument(levelArgument);
        command.AddOption(inputOption);

        command.SetHandler(async (int day, int level, FileInfo? inputFile) =>
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var runner = scope.ServiceProvider.GetRequiredService<PuzzleRunner>();

                // Read input from file or stdin
                string input;
                if (inputFile != null)
                {
                    if (!inputFile.Exists)
                    {
                        Console.Error.WriteLine($"Error: Input file not found: {inputFile.FullName}");
                        Environment.Exit(1);
                        return;
                    }
                    input = await File.ReadAllTextAsync(inputFile.FullName);
                }
                else
                {
                    input = await Console.In.ReadToEndAsync();
                }

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.Error.WriteLine("Error: No input provided");
                    Environment.Exit(1);
                    return;
                }

                // Run solution
                var (answer, executionTime, wasCached) = runner.RunSolution(day, level, input.TrimEnd());

                // Output result
                Console.WriteLine($"Answer: {answer}");
                Console.WriteLine($"Execution time: {executionTime}ms{(wasCached ? " (cached)" : "")}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                Environment.Exit(1);
            }
        }, dayArgument, levelArgument, inputOption);

        return command;
    }
}
