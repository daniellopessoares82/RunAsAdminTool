using RunAsAdminTool.Services.Interfaces;

namespace RunAsAdminTool.Services;

/// <summary>
/// Service responsible for managing application icons.
/// </summary>
public class IconService : IIconService
{
    private const string IconFileName = "business.ico";
    private Icon? _customIcon;
    private bool _disposed;

    /// <summary>
    /// Loads and returns the application icon.
    /// </summary>
    /// <returns>The loaded icon or system default if loading fails.</returns>
    public Icon LoadApplicationIcon()
    {
        if (_customIcon != null)
            return _customIcon;

        try
        {
            // Try to load from application directory first
            var iconPath = Path.Combine(Application.StartupPath, IconFileName);
            if (File.Exists(iconPath))
            {
                _customIcon = new Icon(iconPath);
                return _customIcon;
            }

            // Alternative: try to load from current directory (during development)
            var projectIconPath = Path.Combine(Directory.GetCurrentDirectory(), IconFileName);
            if (File.Exists(projectIconPath))
            {
                _customIcon = new Icon(projectIconPath);
                return _customIcon;
            }

            ShowInfo("Custom icon file not found. Using default icon.");
            return SystemIcons.Application;
        }
        catch (Exception ex)
        {
            ShowWarning($"Error loading custom icon: {ex.Message}. Using default icon.");
            return SystemIcons.Application;
        }
    }

    /// <summary>
    /// Disposes of managed resources.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes of managed and unmanaged resources.
    /// </summary>
    /// <param name="disposing">True to dispose managed resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _customIcon?.Dispose();
            _customIcon = null;
            _disposed = true;
        }
    }

    /// <summary>
    /// Shows an information message to the user via balloon tip.
    /// </summary>
    /// <param name="message">The information message.</param>
    private static void ShowInfo(string message)
    {
        // This could be improved to use a notification service
        Console.WriteLine($"INFO: {message}");
    }

    /// <summary>
    /// Shows a warning message to the user via balloon tip.
    /// </summary>
    /// <param name="message">The warning message.</param>
    private static void ShowWarning(string message)
    {
        // This could be improved to use a notification service
        Console.WriteLine($"WARNING: {message}");
    }
}