namespace RunAsAdminTool.Models;

/// <summary>
/// Represents information about an application that can be launched with administrator privileges.
/// </summary>
public class ApplicationInfo
{
    /// <summary>
    /// Gets or sets the file path to the executable application.
    /// </summary>
    public string Path { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the display name shown in the context menu.
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// Gets a value indicating whether this application info is valid.
    /// </summary>
    public bool IsValid => !string.IsNullOrWhiteSpace(Path) && !string.IsNullOrWhiteSpace(DisplayName);
}