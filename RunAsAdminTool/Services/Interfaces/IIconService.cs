namespace RunAsAdminTool.Services.Interfaces;

/// <summary>
/// Interface for icon service that manages application icons.
/// </summary>
public interface IIconService : IDisposable
{
    /// <summary>
    /// Loads and returns the application icon.
    /// </summary>
    /// <returns>The loaded icon or system default if loading fails.</returns>
    Icon LoadApplicationIcon();
}