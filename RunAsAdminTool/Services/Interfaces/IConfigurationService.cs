using RunAsAdminTool.Models;

namespace RunAsAdminTool.Services.Interfaces;

/// <summary>
/// Interface for configuration service that manages application configurations.
/// </summary>
public interface IConfigurationService
{
    /// <summary>
    /// Loads applications from the configuration source.
    /// </summary>
    /// <returns>A list of application information objects.</returns>
    List<ApplicationInfo> LoadApplications();
}