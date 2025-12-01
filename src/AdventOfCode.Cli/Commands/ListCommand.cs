using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using AdventOfCode.Cli.Services;

namespace AdventOfCode.Cli.Commands;

/// <summary>
/// Command to list all available puzzle solutions.
/// </summary>
public static class ListCommand
{
    public static Command Create(IServiceProvider serviceProvider)
    {
        var command = new Command("list", "List all available puzzle solutions");

        command.SetHandler(() =>
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var runner = scope.ServiceProvider.GetRequiredService<PuzzleRunner>();

                var availableDays = runner.GetAvailableDays().ToList();

                if (!availableDays.Any())
                {
                    Console.WriteLine("No solutions found.");
                    Console.WriteLine("Create a solution in src/AdventOfCode.Solutions/Days/DayXX.cs");
                    return;
                }

                Console.WriteLine("Available solutions:");
                foreach (var day in availableDays)
                {
                    Console.WriteLine($"  Day {day:D2}");
                }

                Console.WriteLine($"\nTotal: {availableDays.Count} solution{(availableDays.Count != 1 ? "s" : "")}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                Environment.Exit(1);
            }
        });

        return command;
    }
}
