using System.Reflection;

namespace AdventOfCode.Solutions;

/// <summary>
/// Registry for auto-discovering and managing puzzle solutions.
/// </summary>
public class SolutionRegistry
{
    private readonly Dictionary<int, ISolution> _solutions = new();

    public SolutionRegistry()
    {
        DiscoverSolutions();
    }

    /// <summary>
    /// Automatically discovers all solution implementations in the assembly.
    /// Solutions must be named "Day{NN}" where NN is a zero-padded day number (01-25).
    /// </summary>
    private void DiscoverSolutions()
    {
        var solutionType = typeof(ISolution);
        var types = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && solutionType.IsAssignableFrom(t));

        foreach (var type in types)
        {
            // Extract day number from class name (e.g., "Day01" -> 1)
            if (type.Name.StartsWith("Day") && type.Name.Length >= 5)
            {
                var dayString = type.Name.Substring(3);
                if (int.TryParse(dayString, out int day) && day >= 1 && day <= 25)
                {
                    var instance = Activator.CreateInstance(type) as ISolution;
                    if (instance != null)
                    {
                        _solutions[day] = instance;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Gets the solution for a specific day.
    /// </summary>
    /// <param name="day">The day number (1-25)</param>
    /// <returns>The solution instance, or null if not found</returns>
    public ISolution? GetSolution(int day)
    {
        return _solutions.TryGetValue(day, out var solution) ? solution : null;
    }

    /// <summary>
    /// Checks if a solution exists for a specific day.
    /// </summary>
    /// <param name="day">The day number (1-25)</param>
    /// <returns>True if a solution exists, false otherwise</returns>
    public bool HasSolution(int day)
    {
        return _solutions.ContainsKey(day);
    }

    /// <summary>
    /// Gets all available solution days.
    /// </summary>
    /// <returns>An enumerable of day numbers that have solutions</returns>
    public IEnumerable<int> GetAvailableDays()
    {
        return _solutions.Keys.OrderBy(k => k);
    }

    /// <summary>
    /// Gets the hash of the solution code for a specific day.
    /// This is used for cache invalidation when solution code changes.
    /// </summary>
    /// <param name="day">The day number (1-25)</param>
    /// <returns>A hash string representing the solution code</returns>
    public string GetSolutionHash(int day)
    {
        if (!_solutions.TryGetValue(day, out var solution))
        {
            return string.Empty;
        }

        // Use the assembly's last write time as a simple hash
        var assembly = solution.GetType().Assembly;
        var location = assembly.Location;

        if (File.Exists(location))
        {
            var lastModified = File.GetLastWriteTimeUtc(location);
            return lastModified.Ticks.ToString();
        }

        return string.Empty;
    }
}
