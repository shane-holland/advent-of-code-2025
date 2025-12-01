using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AdventOfCode.Solutions;
using AdventOfCode.Cli.Services;
using AdventOfCode.Cli.Commands;

namespace AdventOfCode.Cli;

class Program
{
    static async Task<int> Main(string[] args)
    {
        // Setup dependency injection
        var services = new ServiceCollection();
        ConfigureServices(services);
        var serviceProvider = services.BuildServiceProvider();

        // Create root command
        var rootCommand = new RootCommand("Advent of Code 2025 CLI - Fast puzzle solving tool");

        // Add commands
        rootCommand.AddCommand(SolveCommand.Create(serviceProvider));
        rootCommand.AddCommand(FetchCommand.Create(serviceProvider));
        rootCommand.AddCommand(SubmitCommand.Create(serviceProvider));
        rootCommand.AddCommand(ListCommand.Create(serviceProvider));

        // Execute
        return await rootCommand.InvokeAsync(args);
    }

    static void ConfigureServices(IServiceCollection services)
    {
        // Logging
        services.AddLogging(builder =>
        {
            builder.AddConsole();
            builder.SetMinimumLevel(LogLevel.Warning); // Only show warnings and errors by default
        });

        // Core services
        services.AddSingleton<SolutionRegistry>();
        services.AddSingleton<CacheService>();
        services.AddScoped<PuzzleRunner>();

        // HTTP client for Advent of Code
        services.AddHttpClient<AdventOfCodeClient>();
    }
}
