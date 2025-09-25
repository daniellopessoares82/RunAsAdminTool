# RunAsAdminTool

RunAsAdminTool is a lightweight Windows application designed to execute other programs with elevated (administrator) privileges. It serves as a secure and centralized launcher for administrative tasks, simplifying the execution of trusted tools and scripts in environments where elevated access is required.

## Features

- **System Tray Integration**: Runs minimized in the system tray for easy access
- **Configurable Applications**: Load applications from a JSON configuration file
- **Administrator Privileges**: Launch applications with elevated permissions using Windows UAC
- **Custom Icon Support**: Support for custom application icons
- **Balloon Notifications**: User-friendly notifications for status updates
- **Minimal Resource Usage**: Lightweight application with low memory footprint
- **Dependency Injection**: Modern service-oriented architecture with proper separation of concerns
- **Comprehensive Testing**: Full unit test coverage with 84+ test cases

## Technical Details

### Architecture

The application follows a service-oriented architecture with clear separation of concerns and dependency injection:

- **Models**: Data structures for application configuration
- **Services**: Business logic and external integrations with interface-based design
- **Service Container**: Custom dependency injection container for service management
- **Presentation**: Windows Forms UI components with minimal coupling
- **Testing**: Comprehensive unit test suite following best practices

### Technologies Used

- **.NET 9**: Latest .NET framework for Windows applications
- **Windows Forms**: Native Windows UI framework
- **System.Text.Json**: Built-in JSON serialization
- **NotifyIcon**: System tray integration
- **ProcessStartInfo**: Application launching with elevated privileges
- **Dependency Injection**: Custom service container implementation
- **Interface-based Design**: Loose coupling through service interfaces

### Project Structure

```
RunAsAdminTool/
??? Models/
?   ??? ApplicationInfo.cs          # Application configuration model
??? Services/
?   ??? Interfaces/                 # Service interface definitions
?   ?   ??? IConfigurationService.cs
?   ?   ??? IApplicationLauncherService.cs
?   ?   ??? IIconService.cs
?   ?   ??? INotificationService.cs
?   ??? ConfigurationService.cs     # JSON configuration management
?   ??? ApplicationLauncherService.cs # Process launching service
?   ??? IconService.cs              # Icon management service
?   ??? NotificationService.cs      # System tray notifications
?   ??? ServiceContainer.cs         # Dependency injection container
?   ??? ServiceConfiguration.cs     # Service registration setup
??? MainForm.cs                     # Main application form (renamed from Presentation.cs)
??? MainForm.Designer.cs            # Windows Forms designer file
??? Program.cs                      # Application entry point
??? Applications.json               # Configuration file
??? business.ico                    # Application icon
```

### Test Project Structure

```
RunAsAdminTool.Tests/
??? Models/
?   ??? ApplicationInfoTests.cs     # Tests for ApplicationInfo model
??? Services/
?   ??? ApplicationLauncherServiceTests.cs  # Tests for ApplicationLauncherService
?   ??? ConfigurationServiceTests.cs        # Tests for ConfigurationService
?   ??? ServiceContainerTests.cs            # Tests for ServiceContainer
?   ??? ServiceConfigurationTests.cs        # Tests for ServiceConfiguration
??? Helpers/
?   ??? TestDataBuilder.cs          # Test data creation utilities
??? GlobalAssemblyInfo.cs           # Global test configurations
??? GlobalUsings.cs                 # Global using statements
??? README.md                       # Test project documentation
```

### Configuration

The application uses a JSON configuration file (`Applications.json`) to define which applications can be launched:

```json
[
  {
    "Path": "C:\\Windows\\System32\\cmd.exe",
    "DisplayName": "Command Prompt"
  },
  {
    "Path": "C:\\Windows\\System32\\notepad.exe",
    "DisplayName": "Notepad"
  },
  {
    "Path": "C:\\Windows\\System32\\taskmgr.exe",
    "DisplayName": "Task Manager"
  }
]
```

#### Configuration Properties

- **Path**: Full path to the executable file
- **DisplayName**: Name shown in the context menu

#### Configuration Validation

The application automatically validates configuration entries and filters out invalid ones:
- Both `Path` and `DisplayName` must be non-empty
- Whitespace-only values are considered invalid
- Invalid entries are automatically excluded from the menu

### Service Architecture

#### Dependency Injection

The application uses a custom `ServiceContainer` for dependency injection:

```csharp
// Service registration
ServiceConfiguration.ConfigureServices(container, notifyIcon);

// Service resolution
var configService = container.GetService<IConfigurationService>();
```

#### Service Interfaces

All services implement interfaces for better testability and loose coupling:

- **IConfigurationService**: Configuration loading and management
- **IApplicationLauncherService**: Application launching with admin privileges
- **IIconService**: Icon loading and resource management
- **INotificationService**: System tray notifications

### Core Services

#### ConfigurationService
- Loads and validates application configurations from JSON
- Handles file system errors gracefully
- Filters invalid configuration entries automatically
- Supports hot-reloading of configuration

#### ApplicationLauncherService
- Launches applications with administrator privileges using UAC
- Comprehensive error handling and user feedback
- Validates input parameters before execution
- Handles various exception scenarios gracefully

#### IconService
- Manages application icon loading with fallback support
- Supports custom icons with automatic fallback to system defaults
- Proper resource cleanup and disposal
- Development and production environment support

#### NotificationService
- Provides system tray balloon notifications
- Supports different notification types (info, warning, error)
- Configurable timeout settings
- Non-blocking notification display

#### ServiceContainer
- Custom dependency injection container
- Singleton service lifetime management
- Factory-based service registration
- Automatic disposal of disposable services
- Type-safe service resolution

### User Interface

The application provides a minimal UI focused on system tray interaction:

- **System Tray Icon**: Always visible when application is running
- **Context Menu**: Right-click menu with configured applications
- **Double-Click Reload**: Double-click tray icon to reload configuration
- **Balloon Tips**: Non-intrusive notifications for status updates
- **Exit Option**: Clean application shutdown with resource cleanup

### Testing

#### Test Coverage
- **84+ Unit Tests**: Comprehensive test coverage across all components
- **Test Categories**: Model, Service, Integration tests with proper categorization
- **Test Traits**: Priority-based and type-based test organization
- **AAA Pattern**: All tests follow Arrange-Act-Assert pattern

#### Testing Frameworks
- **xUnit**: Primary testing framework
- **FluentAssertions**: Readable and expressive assertions
- **Moq**: Mocking framework for dependencies
- **AutoFixture**: Automatic test data generation

#### Test Categories
- **High Priority**: Critical functionality (58 tests)
- **Medium Priority**: Important functionality (19 tests)  
- **Low Priority**: Edge cases and auxiliary functionality (7 tests)

### Security Considerations

- **UAC Integration**: Uses Windows User Account Control for privilege elevation
- **Path Validation**: Validates application paths before execution
- **Error Handling**: Graceful handling of permission and file system errors
- **Resource Management**: Proper disposal of system resources
- **Input Sanitization**: Validates all user input and configuration data

### Build Requirements

- **.NET 9 SDK**: Required for building the application
- **Windows 10/11**: Target operating system
- **Visual Studio 2022** or **Visual Studio Code**: Recommended development environment
- **Windows Forms workload**: Required for UI development

### Runtime Requirements

- **.NET 9 Runtime**: Required for running the application
- **Windows 10/11**: Operating system with UAC support
- **Administrator Privileges**: User must have administrator privileges to use elevation features

## Usage

1. **Installation**: Copy the application files to a directory
2. **Configuration**: Edit `Applications.json` to add your applications
3. **Launch**: Run the executable - it will minimize to system tray
4. **Access**: Right-click the tray icon to access configured applications
5. **Reload**: Double-click the tray icon to reload configuration changes
6. **Exit**: Right-click and select "Exit" to close the application

## Development

### Building the Application

```bash
# Build the main application
dotnet build

# Build with tests
dotnet build --verbosity normal

# Clean and rebuild
dotnet clean && dotnet build
```

### Running in Development Mode

```bash
# Run the application
dotnet run --project RunAsAdminTool

# Run with specific configuration
dotnet run --project RunAsAdminTool --configuration Debug
```

### Running Tests

```bash
# Run all tests
dotnet test

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run only high priority tests
dotnet test --filter "Priority=High"

# Run only service tests
dotnet test --filter "Category=Service"
```

### Publishing for Distribution

```bash
# Self-contained deployment
dotnet publish -c Release --self-contained true -r win-x64

# Framework-dependent deployment
dotnet publish -c Release --self-contained false -r win-x64

# Single file deployment
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

## Error Handling

The application includes comprehensive error handling:

- **Configuration Errors**: Invalid JSON or missing files
- **Launch Errors**: Application not found or permission issues
- **Resource Errors**: Icon loading or system resource failures
- **User Cancellation**: UAC dialog cancellation handling
- **Service Errors**: Dependency injection and service resolution failures

## Customization

### Adding New Applications

Edit the `Applications.json` file and add new entries with the required `Path` and `DisplayName` properties:

```json
{
  "Path": "C:\\Path\\To\\Your\\Application.exe",
  "DisplayName": "Your Application Name"
}
```

### Custom Icons

Replace the `business.ico` file with your preferred icon:
- Recommended sizes: 16x16, 32x32, 48x48 pixels
- Format: .ICO file with multiple resolutions
- Location: Same directory as the executable

### Notification Settings

Modify the notification timeout values in the `NotificationService` class:

```csharp
public void ShowInfo(string title, string message, int timeout = 3000)
```

### Service Customization

Implement custom services by creating new classes that implement the service interfaces:

```csharp
public class CustomConfigurationService : IConfigurationService
{
    public List<ApplicationInfo> LoadApplications()
    {
        // Custom implementation
    }
}
```

## Architecture Benefits

### Maintainability
- **Loose Coupling**: Services depend on interfaces, not implementations
- **Single Responsibility**: Each service has a focused purpose
- **Testability**: Easy to unit test with mocked dependencies
- **Extensibility**: Easy to add new services or modify existing ones

### Performance
- **Lazy Loading**: Services are created only when needed
- **Singleton Pattern**: Services are reused across the application
- **Resource Management**: Proper disposal prevents memory leaks
- **Minimal UI**: System tray approach reduces resource usage

## Troubleshooting

### Common Issues

1. **Application won't start**: Ensure .NET 9 runtime is installed
2. **No applications in menu**: Check `Applications.json` file format and location
3. **Launch failures**: Verify application paths and user permissions
4. **Icon not loading**: Ensure `business.ico` file exists in application directory

### Debug Mode

Enable verbose logging by modifying the `NotificationService` to show detailed error information.

## Contributing

1. **Fork the repository**
2. **Create a feature branch**
3. **Write unit tests** for new functionality
4. **Ensure all tests pass**: `dotnet test`
5. **Follow coding conventions** and interface-based design
6. **Submit a pull request**

## License

This project is open source. Please refer to the license file for usage terms and conditions.

## Version History

- **v1.0.0**: Initial release with service-oriented architecture, dependency injection, and comprehensive unit tests
