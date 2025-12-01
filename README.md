# Advent of Code 2025 - .NET Aspire API

A REST API and CLI based solution for [Advent of Code 2025](https://adventofcode.com/2025) built with .NET 9 and Aspire. This project provides minimal-boilerplate infrastructure for solving daily coding puzzles through a RESTful interface.

## Features

- **RESTful API**: Three endpoints (GET, PUT, POST) for different workflows
- **Intelligent Caching**: Results are cached based on puzzle input hash and solution code hash
- **Advent of Code Integration**: Fetch puzzle input and submit answers programmatically
- **Built on Aspire**: Includes observability, service discovery, and container orchestration
- **Dev Container Ready**: Fully configured devcontainer for consistent development environment

## Project Structure

```
advent-of-code-2025/
├── src/
│   ├── AdventOfCode.ApiService/        # Main REST API service
│   ├── AdventOfCode.AppHost/           # Aspire orchestrator
│   ├── AdventOfCode.Solutions/         # Your puzzle solutions
│   │   ├── Days/                       # Individual day solutions
│   │   ├── ISolution.cs                # Solution interface
│   │   └── SolutionRegistry.cs         # Auto-discovery system
│   └── AdventOfCode.ServiceDefaults/   # Aspire defaults
├── tests/
│   └── AdventOfCode.Tests/             # Unit tests for solutions
├── .devcontainer/                      # Dev container configuration
└── data/                               # Cached puzzle inputs/outputs
```

## Quick Start

### Prerequisites

**Option 1: Using Dev Container**
- Open project in VS Code
- Click "Reopen in Container" when prompted
- Everything is automatically configured
**Option 2: Local .NET SDK**
- .NET 9 SDK
- Aspire workload: `dotnet workload install aspire`

### Build and Run

```bash
# Restore dependencies
dotnet restore

# Build solution
dotnet build

# Run the AppHost (starts API on port 1100)
cd src/AdventOfCode.AppHost
dotnet run

# Or run API service directly
cd src/AdventOfCode.ApiService
dotnet run --urls http://localhost:1100
```

## Creating a Solution

Solutions require **zero boilerplate**. Simply create a class that implements `ISolution`:

### Step 1: Create Solution File

Create a new file in `src/AdventOfCode.Solutions/Days/` named `Day{NN}.cs` where `{NN}` is the zero-padded day number (01-25):

```csharp
using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions.Days;

/// <summary>
/// Solution for Day 1: Historian Hysteria
/// </summary>
public partial class Day01 : ISolution
{
    public string SolveLevel1(string input)
    {
        // Parse input
        var lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        // Solve Level 1
        var answer = ComputeLevel1(lines);

        return answer.ToString();
    }

    public string SolveLevel2(string input)
    {
        // Parse input
        var lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        // Solve Level 2
        var answer = ComputeLevel2(lines);

        return answer.ToString();
    }

    private int ComputeLevel1(string[] lines)
    {
        // Your Level 1 logic here
        return 0;
    }

    private int ComputeLevel2(string[] lines)
    {
        // Your Level 2 logic here
        return 0;
    }
}
```

**That's it!** The solution is automatically discovered and registered. No manual registration needed.

### Step 2: (Optional) Create Unit Tests

Create `tests/AdventOfCode.Tests/SolutionTests/Day{NN}Tests.cs`:

```csharp
using AdventOfCode.Solutions.Days;
using Xunit;

namespace AdventOfCode.Tests.SolutionTests;

public class Day01Tests
{
    private readonly Day01 _solution = new();

    [Fact]
    public void TestLevel1_WithSampleInput()
    {
        var input = @"Sample input here";
        var result = _solution.SolveLevel1(input);
        Assert.Equal("expected", result);
    }
}
```

## API Usage

The API provides three endpoints for different workflows:

### 1. PUT - Test with Custom Input

Test your solution with custom input:

```bash
curl -X PUT http://localhost:1100/api/puzzle/day/1/level/1 \
  -H "Content-Type: text/plain" \
  --data-binary @input.txt
```

**Response:**
```json
{
  "puzzle": { "day": 1, "level": 1 },
  "executionTime": "15ms",
  "answer": "142"
}
```

### 2. GET - Fetch Input and Solve

Automatically fetch input from Advent of Code and solve:

```bash
curl -X GET http://localhost:1100/api/puzzle/day/1/level/1 \
  -H "Session: your-session-cookie"
```

**Response:**
```json
{
  "puzzle": { "day": 1, "level": 1 },
  "executionTime": "12ms",
  "answer": "54321"
}
```

### 3. POST - Solve and Submit

Fetch input, solve, and submit answer to Advent of Code:

```bash
curl -X POST http://localhost:1100/api/puzzle/day/1/level/1 \
  -H "Session: your-session-cookie"
```

**Response:**
```json
{
  "puzzle": { "day": 1, "level": 1 },
  "executionTime": "12ms",
  "answer": "54321",
  "result": "success"
}
```

## Getting Your Session Cookie

To use GET and POST endpoints, you need your Advent of Code session cookie:

1. Log in to [adventofcode.com](https://adventofcode.com)
2. Open Developer Tools (F12)
3. Go to Application/Storage → Cookies → `https://adventofcode.com`
4. Copy the value of the `session` cookie

**Security Note:** Never commit your session cookie to version control!

## Caching

Results are automatically cached based on:
- **Puzzle Input Hash**: SHA256 of the puzzle input
- **Solution Code Hash**: Assembly modification timestamp

Cache is invalidated when:
- Puzzle input changes
- Solution code is recompiled

Cached results return in ~0ms.

## Running Tests

```bash
# Run all tests
dotnet test

# Run tests with detailed output
dotnet test --verbosity normal

# Run specific test
dotnet test --filter "Day01Tests"
```

## Development Workflow

### Using the AppHost

The Aspire AppHost provides:
- **Aspire Dashboard**: View logs, metrics, and traces at `http://localhost:18888`
- **Service Orchestration**: Automatically manages API lifecycle
- **Built-in Observability**: OpenTelemetry integration

```bash
cd src/AdventOfCode.AppHost
dotnet run
```

### Using API Service Directly

For simpler debugging:

```bash
cd src/AdventOfCode.ApiService
dotnet run --urls http://localhost:1100
```

## Solution Naming Convention

The auto-discovery system requires specific naming:

- ✅ `Day01.cs` → Registered as Day 1
- ✅ `Day15.cs` → Registered as Day 15
- ✅ `Day25.cs` → Registered as Day 25
- ❌ `Day1.cs` → Not discovered (missing leading zero)
- ❌ `Puzzle01.cs` → Not discovered (doesn't start with "Day")

## Tips and Best Practices

1. **Start with Tests**: Write unit tests with sample input before implementing
2. **Keep It Simple**: The `SolveLevel1` and `SolveLevel2` methods should be your main entry points
3. **Use Helper Methods**: Extract complex logic into private methods
4. **Cache-Friendly**: Pure functions work best with the caching system
5. **Regex Performance**: Use `[GeneratedRegex]` attribute for better performance

## Example: Complete Day 1 Solution

```csharp
using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions.Days;

public partial class Day01 : ISolution
{
    public string SolveLevel1(string input)
    {
        var (left, right) = ParseInput(input);

        left.Sort();
        right.Sort();

        // Level 1: Sum of absolute differences
        var distance = left.Zip(right)
            .Sum(pair => Math.Abs(pair.First - pair.Second));

        return distance.ToString();
    }

    public string SolveLevel2(string input)
    {
        var (left, right) = ParseInput(input);

        // Level 2: Similarity score
        var similarity = left.Sum(num =>
            num * right.Count(r => r == num));

        return similarity.ToString();
    }

    private (List<int> Left, List<int> Right) ParseInput(string input)
    {
        var left = new List<int>();
        var right = new List<int>();

        foreach (var line in input.Split('\n', StringSplitOptions.RemoveEmptyEntries))
        {
            var match = NumberPairRegex().Match(line);
            if (match.Success)
            {
                left.Add(int.Parse(match.Groups[1].Value));
                right.Add(int.Parse(match.Groups[2].Value));
            }
        }

        return (left, right);
    }

    [GeneratedRegex(@"(\d+)\s+(\d+)")]
    private static partial Regex NumberPairRegex(); /
}
```

## Troubleshooting

### Port Already in Use

```bash
# Kill existing instances
pkill -f "dotnet run"
```

### Aspire Workload Missing

```bash
dotnet workload install aspire
```

### Solution Not Discovered

- Verify class name matches pattern: `Day{NN}.cs`
- Ensure class implements `ISolution`
- Rebuild the project: `dotnet build`

## Technology Stack

- **.NET 9**: Latest .NET framework
- **Aspire**: Cloud-ready app orchestration
- **Minimal APIs**: Fast, lightweight endpoints
- **OpenTelemetry**: Built-in observability
- **xUnit**: Unit testing framework

## License

This is a learning project for Advent of Code 2025. Feel free to use it as a template for your own solutions!

## Acknowledgments

- [Advent of Code](https://adventofcode.com/) by Eric Wastl

Happy Coding! 🎄

