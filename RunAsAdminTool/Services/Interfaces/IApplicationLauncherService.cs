namespace RunAsAdminTool.Services.Interfaces;

/// <summary>
/// Interface for application launcher service that handles launching applications with administrator privileges.
/// </summary>
public interface IApplicationLauncherService
{
    /// <summary>
    /// Launches the specified application with administrator privileges.
    /// </summary>
    /// <param name="applicationPath">The path to the application to launch.</param>
    /// <param name="displayName">The display name of the application for error messages.</param>
    /// <returns>True if the application was launched successfully; otherwise, false.</returns>
    bool LaunchAsAdmin(string applicationPath, string displayName);
}