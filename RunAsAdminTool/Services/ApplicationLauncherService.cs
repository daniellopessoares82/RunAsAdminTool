using System.Diagnostics;
using RunAsAdminTool.Services.Interfaces;

namespace RunAsAdminTool.Services;

/// <summary>
/// Service responsible for launching applications with administrator privileges.
/// </summary>
public class ApplicationLauncherService : IApplicationLauncherService
{
    /// <summary>
    /// Launches the specified application with administrator privileges.
    /// </summary>
    /// <param name="applicationPath">The path to the application to launch.</param>
    /// <param name="displayName">The display name of the application for error messages.</param>
    /// <returns>True if the application was launched successfully; otherwise, false.</returns>
    public bool LaunchAsAdmin(string applicationPath, string displayName)
    {
        if (string.IsNullOrWhiteSpace(applicationPath))
        {
            ShowError("Application path cannot be empty.");
            return false;
        }

        try
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = applicationPath,
                UseShellExecute = true,
                Verb = "runas" // Run as administrator
            };

            Process.Start(startInfo);
            return true;
        }
        catch (Exception ex)
        {
            ShowError($"Failed to launch {displayName ?? "application"}: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Shows an error message to the user.
    /// </summary>
    /// <param name="message">The error message.</param>
    private static void ShowError(string message)
    {
        MessageBox.Show(message, "Launch Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}