using RunAsAdminTool.Services.Interfaces;

namespace RunAsAdminTool.Services;

/// <summary>
/// Configures and sets up all application services.
/// </summary>
public static class ServiceConfiguration
{
    /// <summary>
    /// Configures the service container with all necessary services.
    /// </summary>
    /// <param name="container">The service container to configure.</param>
    /// <param name="notifyIcon">The NotifyIcon instance for the notification service.</param>
    public static void ConfigureServices(ServiceContainer container, NotifyIcon notifyIcon)
    {
        // Register configuration service
        container.RegisterSingleton<IConfigurationService>(new ConfigurationService());

        // Register application launcher service
        container.RegisterSingleton<IApplicationLauncherService>(new ApplicationLauncherService());

        // Register icon service
        container.RegisterSingleton<IIconService>(new IconService());

        // Register notification service with NotifyIcon dependency
        container.RegisterSingleton<INotificationService>(new NotificationService(notifyIcon));
    }

    /// <summary>
    /// Alternative method to configure services using factory functions for more complex scenarios.
    /// </summary>
    /// <param name="container">The service container to configure.</param>
    /// <param name="notifyIcon">The NotifyIcon instance for the notification service.</param>
    public static void ConfigureServicesWithFactories(ServiceContainer container, NotifyIcon notifyIcon)
    {
        // Register services using factory functions
        container.RegisterSingleton<IConfigurationService>(_ => new ConfigurationService());
        
        container.RegisterSingleton<IApplicationLauncherService>(_ => new ApplicationLauncherService());
        
        container.RegisterSingleton<IIconService>(_ => new IconService());
        
        container.RegisterSingleton<INotificationService>(_ => new NotificationService(notifyIcon));
    }
}