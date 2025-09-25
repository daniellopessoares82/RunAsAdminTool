using RunAsAdminTool.Models;

namespace RunAsAdminTool.Tests.Helpers;

/// <summary>
/// Test data builder for creating valid domain objects in unit tests.
/// Provides factory methods to create properly configured entities and value objects.
/// </summary>
public static class TestDataBuilder
{
    #region Models

    /// <summary>
    /// Creates a valid ApplicationInfo instance for testing.
    /// </summary>
    /// <param name="path">Custom path, or null for default.</param>
    /// <param name="displayName">Custom display name, or null for default.</param>
    /// <returns>A valid ApplicationInfo instance.</returns>
    public static ApplicationInfo CreateValidApplicationInfo(
        string? path = null,
        string? displayName = null)
        => new()
        {
            Path = path ?? @"C:\Windows\System32\notepad.exe",
            DisplayName = displayName ?? $"Test Application {DateTime.Now.Ticks}"
        };

    /// <summary>
    /// Creates an ApplicationInfo instance with empty path for testing validation.
    /// </summary>
    /// <param name="displayName">Custom display name, or null for default.</param>
    /// <returns>An ApplicationInfo instance with empty path.</returns>
    public static ApplicationInfo CreateApplicationInfoWithEmptyPath(string? displayName = null)
        => new()
        {
            Path = string.Empty,
            DisplayName = displayName ?? "Test Application"
        };

    /// <summary>
    /// Creates an ApplicationInfo instance with empty display name for testing validation.
    /// </summary>
    /// <param name="path">Custom path, or null for default.</param>
    /// <returns>An ApplicationInfo instance with empty display name.</returns>
    public static ApplicationInfo CreateApplicationInfoWithEmptyDisplayName(string? path = null)
        => new()
        {
            Path = path ?? @"C:\Windows\System32\notepad.exe",
            DisplayName = string.Empty
        };

    /// <summary>
    /// Creates an ApplicationInfo instance with whitespace-only values for testing validation.
    /// </summary>
    /// <returns>An ApplicationInfo instance with whitespace-only values.</returns>
    public static ApplicationInfo CreateApplicationInfoWithWhitespaceValues()
        => new()
        {
            Path = "   ",
            DisplayName = "   "
        };

    /// <summary>
    /// Creates a list of valid ApplicationInfo instances for testing collections.
    /// </summary>
    /// <param name="count">Number of ApplicationInfo instances to create.</param>
    /// <returns>A list of valid ApplicationInfo instances.</returns>
    public static List<ApplicationInfo> CreateValidApplicationInfoList(int count = 3)
    {
        var applications = new List<ApplicationInfo>();
        for (int i = 0; i < count; i++)
        {
            applications.Add(CreateValidApplicationInfo(
                path: $@"C:\Windows\System32\app{i}.exe",
                displayName: $"Test Application {i + 1}"));
        }
        return applications;
    }

    /// <summary>
    /// Creates a mixed list of valid and invalid ApplicationInfo instances for testing filtering.
    /// </summary>
    /// <returns>A list containing both valid and invalid ApplicationInfo instances.</returns>
    public static List<ApplicationInfo> CreateMixedApplicationInfoList()
        => new()
        {
            CreateValidApplicationInfo(),
            CreateApplicationInfoWithEmptyPath(),
            CreateValidApplicationInfo(path: @"C:\Test\app2.exe", displayName: "App 2"),
            CreateApplicationInfoWithEmptyDisplayName(),
            CreateApplicationInfoWithWhitespaceValues(),
            CreateValidApplicationInfo(path: @"C:\Test\app3.exe", displayName: "App 3")
        };

    #endregion

    #region Common Test Values

    /// <summary>
    /// Gets a list of valid executable paths for testing.
    /// </summary>
    public static readonly List<string> ValidExecutablePaths = new()
    {
        @"C:\Windows\System32\cmd.exe",
        @"C:\Windows\System32\notepad.exe",
        @"C:\Windows\System32\calc.exe",
        @"C:\Windows\System32\mspaint.exe",
        @"C:\Program Files\Test\TestApp.exe"
    };

    /// <summary>
    /// Gets a list of valid display names for testing.
    /// </summary>
    public static readonly List<string> ValidDisplayNames = new()
    {
        "Command Prompt",
        "Notepad",
        "Calculator",
        "Paint",
        "Test Application"
    };

    /// <summary>
    /// Gets a list of invalid paths for testing.
    /// </summary>
    public static readonly List<string> InvalidPaths = new()
    {
        string.Empty,
        "   ",
        "\t",
        "\n",
        null!
    };

    /// <summary>
    /// Gets a list of invalid display names for testing.
    /// </summary>
    public static readonly List<string> InvalidDisplayNames = new()
    {
        string.Empty,
        "   ",
        "\t",
        "\n",
        null!
    };

    #endregion
}