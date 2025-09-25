using System.Text.Json;
using RunAsAdminTool.Models;
using RunAsAdminTool.Services.Interfaces;

namespace RunAsAdminTool.Services;

/// <summary>
/// Service responsible for loading and managing application configurations from JSON file.
/// </summary>
public class ConfigurationService : IConfigurationService
{
    private const string ConfigFileName = "Applications.json";

    /// <summary>
    /// Loads applications from the JSON configuration file.
    /// </summary>
    /// <returns>A list of application information objects.</returns>
    public List<ApplicationInfo> LoadApplications()
    {
        try
        {
            var jsonPath = GetConfigurationFilePath();
            
            if (!File.Exists(jsonPath))
            {
                ShowWarning($"Configuration file not found at: {jsonPath}");
                return [];
            }

            var jsonContent = File.ReadAllText(jsonPath);
            var applications = JsonSerializer.Deserialize<List<ApplicationInfo>>(jsonContent) ?? [];

            // Filter out invalid entries
            return [.. applications.Where(app => app.IsValid)];
        }
        catch (Exception ex)
        {
            ShowError($"Error loading applications: {ex.Message}");
            return [];
        }
    }

    /// <summary>
    /// Gets the full path to the configuration file.
    /// </summary>
    /// <returns>The configuration file path.</returns>
    private static string GetConfigurationFilePath()
    {
        return Path.Combine(Application.StartupPath, ConfigFileName);
    }

    /// <summary>
    /// Shows a warning message to the user.
    /// </summary>
    /// <param name="message">The warning message.</param>
    private static void ShowWarning(string message)
    {
        MessageBox.Show(message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }

    /// <summary>
    /// Shows an error message to the user.
    /// </summary>
    /// <param name="message">The error message.</param>
    private static void ShowError(string message)
    {
        MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}