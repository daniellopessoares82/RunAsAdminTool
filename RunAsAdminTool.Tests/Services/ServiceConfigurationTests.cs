using RunAsAdminTool.Services;
using RunAsAdminTool.Services.Interfaces;

namespace RunAsAdminTool.Tests.Services;

/// <summary>
/// Unit tests for ServiceConfiguration.
/// Tests service registration and configuration setup.
/// </summary>
public class ServiceConfigurationTests
{
    #region ConfigureServices Tests

    [Fact(DisplayName = "ConfigureServices should register all required services")]
    [Trait("Category", "Service")]
    [Trait("Priority", "High")]
    [Trait("Type", "Integration")]
    public void ConfigureServices_ShouldRegisterAllRequiredServices()
    {
        // Arrange
        var container = new ServiceContainer();
        var notifyIcon = new NotifyIcon();

        try
        {
            // Act
            ServiceConfiguration.ConfigureServices(container, notifyIcon);

            // Assert
            container.IsRegistered<IConfigurationService>().Should().BeTrue();
            container.IsRegistered<IApplicationLauncherService>().Should().BeTrue();
            container.IsRegistered<IIconService>().Should().BeTrue();
            container.IsRegistered<INotificationService>().Should().BeTrue();
        }
        finally
        {
            notifyIcon.Dispose();
            container.DisposeServices();
        }
    }

    [Fact(DisplayName = "ConfigureServices should create working service instances")]
    [Trait("Category", "Service")]
    [Trait("Priority", "High")]
    [Trait("Type", "Integration")]
    public void ConfigureServices_ShouldCreateWorkingServiceInstances()
    {
        // Arrange
        var container = new ServiceContainer();
        var notifyIcon = new NotifyIcon();

        try
        {
            // Act
            ServiceConfiguration.ConfigureServices(container, notifyIcon);

            // Assert - All services should be retrievable and functional
            var configService = container.GetService<IConfigurationService>();
            configService.Should().NotBeNull();
            configService.Should().BeOfType<ConfigurationService>();

            var launcherService = container.GetService<IApplicationLauncherService>();
            launcherService.Should().NotBeNull();
            launcherService.Should().BeOfType<ApplicationLauncherService>();

            var iconService = container.GetService<IIconService>();
            iconService.Should().NotBeNull();
            iconService.Should().BeOfType<IconService>();

            var notificationService = container.GetService<INotificationService>();
            notificationService.Should().NotBeNull();
            notificationService.Should().BeOfType<NotificationService>();
        }
        finally
        {
            notifyIcon.Dispose();
            container.DisposeServices();
        }
    }

    [Fact(DisplayName = "ConfigureServices should handle null NotifyIcon gracefully")]
    [Trait("Category", "Service")]
    [Trait("Priority", "Medium")]
    [Trait("Type", "Exception")]
    public void ConfigureServices_WithNullNotifyIcon_ShouldThrowArgumentException()
    {
        // Arrange
        var container = new ServiceContainer();

        // Act & Assert
        var action = () => ServiceConfiguration.ConfigureServices(container, null!);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact(DisplayName = "ConfigureServices should handle null container gracefully")]
    [Trait("Category", "Service")]
    [Trait("Priority", "Medium")]
    [Trait("Type", "Exception")]
    public void ConfigureServices_WithNullContainer_ShouldThrowException()
    {
        // Arrange
        var notifyIcon = new NotifyIcon();

        try
        {
            // Act & Assert
            var action = () => ServiceConfiguration.ConfigureServices(null!, notifyIcon);
            action.Should().Throw<NullReferenceException>();
        }
        finally
        {
            notifyIcon.Dispose();
        }
    }

    #endregion

    #region ConfigureServicesWithFactories Tests

    [Fact(DisplayName = "ConfigureServicesWithFactories should register all required services")]
    [Trait("Category", "Service")]
    [Trait("Priority", "High")]
    [Trait("Type", "Integration")]
    public void ConfigureServicesWithFactories_ShouldRegisterAllRequiredServices()
    {
        // Arrange
        var container = new ServiceContainer();
        var notifyIcon = new NotifyIcon();

        try
        {
            // Act
            ServiceConfiguration.ConfigureServicesWithFactories(container, notifyIcon);

            // Assert
            container.IsRegistered<IConfigurationService>().Should().BeTrue();
            container.IsRegistered<IApplicationLauncherService>().Should().BeTrue();
            container.IsRegistered<IIconService>().Should().BeTrue();
            container.IsRegistered<INotificationService>().Should().BeTrue();
        }
        finally
        {
            notifyIcon.Dispose();
            container.DisposeServices();
        }
    }

    [Fact(DisplayName = "ConfigureServicesWithFactories should create working service instances")]
    [Trait("Category", "Service")]
    [Trait("Priority", "High")]
    [Trait("Type", "Integration")]
    public void ConfigureServicesWithFactories_ShouldCreateWorkingServiceInstances()
    {
        // Arrange
        var container = new ServiceContainer();
        var notifyIcon = new NotifyIcon();

        try
        {
            // Act
            ServiceConfiguration.ConfigureServicesWithFactories(container, notifyIcon);

            // Assert - All services should be retrievable and functional
            var configService = container.GetService<IConfigurationService>();
            configService.Should().NotBeNull();
            configService.Should().BeOfType<ConfigurationService>();

            var launcherService = container.GetService<IApplicationLauncherService>();
            launcherService.Should().NotBeNull();
            launcherService.Should().BeOfType<ApplicationLauncherService>();

            var iconService = container.GetService<IIconService>();
            iconService.Should().NotBeNull();
            iconService.Should().BeOfType<IconService>();

            var notificationService = container.GetService<INotificationService>();
            notificationService.Should().NotBeNull();
            notificationService.Should().BeOfType<NotificationService>();
        }
        finally
        {
            notifyIcon.Dispose();
            container.DisposeServices();
        }
    }

    #endregion

    #region Service Functionality Integration Tests

    [Fact(DisplayName = "Both configuration methods should produce equivalent results")]
    [Trait("Category", "Service")]
    [Trait("Priority", "Medium")]
    [Trait("Type", "Integration")]
    public void BothConfigurationMethods_ShouldProduceEquivalentResults()
    {
        // Arrange
        var container1 = new ServiceContainer();
        var container2 = new ServiceContainer();
        var notifyIcon1 = new NotifyIcon();
        var notifyIcon2 = new NotifyIcon();

        try
        {
            // Act
            ServiceConfiguration.ConfigureServices(container1, notifyIcon1);
            ServiceConfiguration.ConfigureServicesWithFactories(container2, notifyIcon2);

            // Assert - Both containers should have the same services registered
            container1.IsRegistered<IConfigurationService>().Should().Be(container2.IsRegistered<IConfigurationService>());
            container1.IsRegistered<IApplicationLauncherService>().Should().Be(container2.IsRegistered<IApplicationLauncherService>());
            container1.IsRegistered<IIconService>().Should().Be(container2.IsRegistered<IIconService>());
            container1.IsRegistered<INotificationService>().Should().Be(container2.IsRegistered<INotificationService>());

            // Both should be able to retrieve all services
            var config1 = container1.GetService<IConfigurationService>();
            var config2 = container2.GetService<IConfigurationService>();
            
            config1.Should().NotBeNull();
            config2.Should().NotBeNull();
            config1.GetType().Should().Be(config2.GetType());
        }
        finally
        {
            notifyIcon1.Dispose();
            notifyIcon2.Dispose();
            container1.DisposeServices();
            container2.DisposeServices();
        }
    }

    #endregion

    #region Service Lifecycle Tests

    [Fact(DisplayName = "Services should be properly disposed when container is disposed")]
    [Trait("Category", "Service")]
    [Trait("Priority", "Medium")]
    [Trait("Type", "Cleanup")]
    public void Services_ShouldBeProperlyDisposedWhenContainerIsDisposed()
    {
        // Arrange
        var container = new ServiceContainer();
        var notifyIcon = new NotifyIcon();

        ServiceConfiguration.ConfigureServices(container, notifyIcon);

        // Act - Get the icon service (which is IDisposable)
        var iconService = container.GetService<IIconService>();
        iconService.Should().NotBeNull();

        // Act - Dispose container
        container.DisposeServices();
        notifyIcon.Dispose();

        // Assert - Container should be cleaned up
        container.IsRegistered<IIconService>().Should().BeFalse();
    }

    [Fact(DisplayName = "Multiple configuration calls should not cause issues")]
    [Trait("Category", "Service")]
    [Trait("Priority", "Low")]
    [Trait("Type", "Behavior")]
    public void MultipleConfigurationCalls_ShouldNotCauseIssues()
    {
        // Arrange
        var container = new ServiceContainer();
        var notifyIcon = new NotifyIcon();

        try
        {
            // Act - Configure multiple times
            ServiceConfiguration.ConfigureServices(container, notifyIcon);
            ServiceConfiguration.ConfigureServices(container, notifyIcon);
            ServiceConfiguration.ConfigureServicesWithFactories(container, notifyIcon);

            // Assert - Should still work correctly
            container.IsRegistered<IConfigurationService>().Should().BeTrue();
            
            var configService = container.GetService<IConfigurationService>();
            configService.Should().NotBeNull();
        }
        finally
        {
            notifyIcon.Dispose();
            container.DisposeServices();
        }
    }

    #endregion
}