# RunAsAdminTool.Tests

This project contains comprehensive unit tests for the RunAsAdminTool application, following the guidelines specified in `unittest.instructions.md`.

## Test Structure

### Test Projects Organization
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
```

## Test Categories and Traits

### Categories
- **Model**: Tests for data models and value objects
- **Service**: Tests for business logic services
- **Integration**: Tests for service integration and workflow

### Priorities
- **High**: Critical functionality that must work correctly
- **Medium**: Important functionality that should work correctly
- **Low**: Auxiliary functionality and edge cases

### Types
- **Constructor**: Constructor and initialization tests
- **Validation**: Input validation and business rule tests
- **Exception**: Error handling and exception scenarios
- **BusinessRule**: Core business logic tests
- **Behavior**: Service behavior and state management
- **Integration**: Component integration tests
- **Cleanup**: Resource cleanup and disposal tests

## Running Tests

### Command Line
```bash
# Run all tests
dotnet test

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run only high priority tests
dotnet test --filter "Priority=High"

# Run only service tests
dotnet test --filter "Category=Service"

# Run tests excluding integration tests
dotnet test --filter "Category!=Integration"

# Run with detailed output
dotnet test --verbosity detailed
```

### Visual Studio
- Use Test Explorer to run and manage tests
- Right-click on test methods/classes for context options
- Use the filter options to run specific categories

## Test Coverage

### What is Tested
- **Models**: ApplicationInfo validation and behavior
- **Services**: All business logic services with interfaces
- **Service Container**: Dependency injection functionality
- **Service Configuration**: Service registration and setup

### Focus Areas
- **Input Validation**: All public methods validate their inputs
- **Error Handling**: Exception scenarios are handled gracefully
- **Business Logic**: Core functionality works as expected
- **Integration**: Services work together correctly
- **Resource Management**: Proper disposal and cleanup

### What is NOT Tested
- **UI Components**: Windows Forms components (MainForm, Designer)
- **File System Operations**: Actual file I/O (mocked or simulated)
- **Process Launching**: Actual application launching (validated through mocking)
- **MessageBox Calls**: UI dialogs (tested through behavior verification)

## Test Data Management

### TestDataBuilder
The `TestDataBuilder` class provides factory methods for creating test data:

- `CreateValidApplicationInfo()`: Creates valid ApplicationInfo instances
- `CreateApplicationInfoWithEmptyPath()`: Creates invalid ApplicationInfo with empty path
- `CreateApplicationInfoWithEmptyDisplayName()`: Creates invalid ApplicationInfo with empty display name
- `CreateValidApplicationInfoList()`: Creates collections of valid ApplicationInfo
- `CreateMixedApplicationInfoList()`: Creates collections with valid and invalid entries

### Benefits
- **Consistency**: All tests use the same data creation patterns
- **Maintainability**: Changes to test data are centralized
- **Readability**: Tests focus on behavior, not data setup
- **Flexibility**: Easy to customize test data when needed

## Best Practices Followed

### AAA Pattern
All tests follow Arrange-Act-Assert pattern:
```csharp
[Fact]
public void Method_Scenario_ExpectedResult()
{
    // Arrange - Setup test data and dependencies
    var input = TestDataBuilder.CreateValidInput();
    
    // Act - Execute the method under test
    var result = service.Method(input);
    
    // Assert - Verify the outcome
    result.Should().Be(expectedValue);
}
```

### Descriptive Names
- Test methods use descriptive names explaining the scenario
- DisplayName attributes provide clear descriptions
- Traits categorize tests for easy filtering

### FluentAssertions
All assertions use FluentAssertions for better readability:
```csharp
result.Should().NotBeNull();
result.Should().BeOfType<ApplicationInfo>();
applications.Should().HaveCount(3);
action.Should().Throw<ArgumentException>();
```

### Independence
- Tests are independent and can run in any order
- Each test sets up its own data and dependencies
- No shared state between tests

### Single Responsibility
- Each test validates only one behavior
- Focused assertions on specific outcomes
- Clear separation of test concerns

## Continuous Integration

### Build Integration
The test project is configured to:
- Run automatically on build
- Generate code coverage reports
- Fail builds on test failures
- Support multiple target frameworks

### Quality Gates
- Minimum 80% code coverage for services
- All tests must pass before merge
- Performance tests complete within acceptable time limits

## Maintenance

### Adding New Tests
1. Follow naming conventions: `{Method}_{Scenario}_{ExpectedResult}`
2. Add appropriate traits for categorization
3. Use TestDataBuilder for test data creation
4. Follow AAA pattern consistently
5. Add descriptive DisplayName attributes

### Updating Existing Tests
1. Maintain backward compatibility when possible
2. Update TestDataBuilder if data models change
3. Ensure trait consistency across similar tests
4. Update documentation as needed

### Performance Considerations
- Keep unit tests fast (< 100ms per test)
- Use mocking for external dependencies
- Avoid actual file system or network operations
- Consider async/await patterns for async methods

## Dependencies

### Testing Frameworks
- **xUnit**: Primary testing framework
- **FluentAssertions**: Readable assertions
- **Moq**: Mocking framework
- **AutoFixture**: Test data generation

### Coverage Tools
- **Coverlet**: Code coverage collection
- **ReportGenerator**: Coverage report generation

### Integration
- **Microsoft.NET.Test.Sdk**: Test SDK
- **xunit.runner.visualstudio**: Visual Studio integration