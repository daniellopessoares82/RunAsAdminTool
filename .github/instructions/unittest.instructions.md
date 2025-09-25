# Unit Testing Instructions

## Overview

This document defines the practices, patterns, and conventions for creating unit tests in .NET projects. The goal is to ensure consistency, quality, and maintainability of tests across the entire solution.

### Fundamental Principles
- **FIRST**: Fast, Independent, Repeatable, Self-Validating, Timely
- **AAA Pattern**: Arrange, Act, Assert
- **One Assertion Per Test**: Each test should validate only one behavior
- **Descriptive Names**: Descriptive names that explain the tested scenario
- **Test Isolation**: Independent and isolated tests

### Testing Guidelines and Priorities
- **Test Only Main Classes**: Focus testing efforts on the### ✓ Implementation
- [ ] AAA pattern implemented (Arrange, Act, Assert)
- [ ] FluentAssertions used for assertions
- [ ] Mocks configured correctly
- [ ] Exception scenarios tested
- [ ] Boundary cases considered
- [ ] Only main classes with business logic are tested
- [ ] Entities tested only if they have actions/events/business rules
- [ ] AI result content is not tested (focus on input validation and process flow)critical and complex classes
- **Entity Testing**: Test entities only when they contain business logic, actions, or events (not simple data containers)
- **AI Result Testing**: AI systems do not need to test their own output results - focus on input validation and process flow
- **Business Logic Priority**: Prioritize testing classes that contain business rules, validations, and core functionality

## What to Test and What Not to Test

### Classes That Should Be Tested
- **Services and Business Logic**: Classes containing business rules, validations, and core functionality
- **Use Cases and Commands**: Application layer components that orchestrate business operations
- **Value Objects**: When they contain validation logic or business rules
- **Entities with Behavior**: Only entities that have methods, events, or state transitions
- **Repositories**: Data access patterns and query logic
- **Controllers**: Input validation, routing logic, and response handling

### Classes That Generally Don't Need Testing
- **Simple Data Transfer Objects (DTOs)**: Pure data containers without logic
- **Configuration Classes**: Simple property holders
- **Auto-Generated Code**: Entity Framework migrations, auto-mappers configurations
- **Simple Entities**: Entities that are just data containers without business logic
- **AI Result Outputs**: AI-generated results don't need automated testing of their content

### Entity Testing Criteria
Entities should only be tested when they have:
- **Business Rules**: Validation logic or constraints
- **State Transitions**: Methods that change entity state
- **Domain Events**: Events triggered by entity actions
- **Calculated Properties**: Properties with business logic
- **Behavior Methods**: Methods that perform operations beyond simple getters/setters

### Example of Entities That Need Testing
```csharp
// This entity SHOULD be tested - has business logic
public class Order
{
    public void Confirm() { /* validation and state change */ }
    public void Cancel() { /* business rules and events */ }
    public decimal CalculateTotal() { /* calculation logic */ }
}
```

### Example of Entities That Don't Need Testing
```csharp
// This entity does NOT need testing - just data container
public class OrderDto
{
    public int Id { get; set; }
    public string CustomerName { get; set; }
    public DateTime OrderDate { get; set; }
}
```

## Test Architecture

### Solution Structure
```
ProjectName/
??? {ProjectName}/                          # Main Project
??? {ProjectName}.Domain/                   # Domain Layer
??? {ProjectName}.Application/              # Application Layer
??? {ProjectName}.Infrastructure/           # Infrastructure Layer
??? {ProjectName}.Domain.Tests/             # Domain Tests
??? {ProjectName}.Application.Tests/        # Application Tests
??? {ProjectName}.Infrastructure.Tests/     # Infrastructure Tests
```

### Test Layers

#### 1. Domain Tests
- **Focus**: Entities, Value Objects, Business Rules
- **Scope**: Isolated tests, no external dependencies
- **Examples**: Entity validation, Value Object behavior

#### 2. Application/Core Tests
- **Focus**: Use Cases, Commands, Handlers, Services
- **Scope**: Tests with mocks of repositories and external services
- **Examples**: Application flows, business validations

#### 3. Infrastructure Tests
- **Focus**: Repositories, Mappings, Database Configurations
- **Scope**: Integration tests with in-memory database
- **Examples**: CRUD operations, specific queries

## Conventions and Best Practices

### 1. Test Projects
- One test project per main project
- `.Tests` suffix in project name
- Same folder structure as the main project

### 2. Framework and Libraries
- **xUnit**: Main testing framework
- **FluentAssertions**: More readable assertions
- **Moq**: Dependency mocking
- **AutoFixture**: Automatic test data generation

### 3. Project Configuration
```xml
<PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <IsPackable>false</IsPackable>
    <IsTestProject>true</IsTestProject>
</PropertyGroup>

<ItemGroup>
    <PackageReference Include="coverlet.collector" Version="6.0.0" />
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.8.0" />
    <PackageReference Include="xunit" Version="2.5.3" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.5.3" />
    <PackageReference Include="FluentAssertions" Version="6.12.0" />
    <PackageReference Include="Moq" Version="4.20.69" />
    <PackageReference Include="AutoFixture" Version="4.18.0" />
    <PackageReference Include="AutoFixture.Xunit2" Version="4.18.0" />
</ItemGroup>

<ItemGroup>
    <Using Include="Xunit" />
    <Using Include="FluentAssertions" />
</ItemGroup>
```

## Test Project Structure

```
{ProjectName}.Domain.Test/
??? Entities/
?   ??? {EntityName}/
?   ?   ??? {EntityName}Tests.cs
??? ValueObjects/
?   ??? {ValueObject}Tests.cs
??? Services/
?   ??? {Service}Tests.cs
??? Integration/
?   ??? {Integration}Tests.cs
??? Helpers/
?   ??? TestDataBuilder.cs
??? obj/ (generated files)
```

## Tools and Libraries

### xUnit
Main framework for test execution.

```csharp
[Fact]
[Theory]
[InlineData]
```

### FluentAssertions
Library for more readable and expressive assertions.

```csharp
// Instead of
Assert.Equal(expected, actual);

// Use
actual.Should().Be(expected);
result.Should().NotBeNull();
collection.Should().HaveCount(3);
```

### Moq
Framework for creating mocks and verifying interactions.

```csharp
var mockRepository = new Mock<IRepository>();
mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
              .ReturnsAsync(entity);
```

### AutoFixture
Automatic data generation for tests.

```csharp
[Theory, AutoData]
public void Test_WithAutoGeneratedData(string code, int value)
{
    // Automatically generated data
}
```

## Naming Patterns

### Test Classes
```csharp
// Pattern: {TestedClass}Tests
public class UserTests
public class OrderServiceTests
public class ProductRepositoryTests
```

### Test Methods
```csharp
// Pattern: {MethodTested}_{Scenario}_{ExpectedResult}
public void Create_WithValidParameters_ShouldCreateEntity()
public void Constructor_WithNullValue_ShouldThrowArgumentException()
public void Update_WhenEntityNotFound_ShouldThrowNotFoundException()
```

### DisplayName
Use descriptive DisplayNames:

```csharp
[Fact(DisplayName = "Create entity with valid parameters should succeed")]
[Theory(DisplayName = "Constructor with null or empty value should throw exception")]
```

## Test Structure

### AAA Pattern (Arrange, Act, Assert)

```csharp
[Fact(DisplayName = "Create entity with valid parameters should succeed")]
public void Create_WithValidParameters_ShouldCreateEntity()
{
    // Arrange - Prepare data and dependencies
    var validData = TestDataBuilder.CreateValidData();
    var service = new EntityService();

    // Act - Execute the action to be tested
    var result = service.Create(validData);

    // Assert - Verify the result
    result.Should().NotBeNull();
    result.Id.Should().BePositive();
    result.Data.Should().Be(validData);
}
```

### Organization with Regions

```csharp
public class EntityTests
{
    #region Constructor Tests
    
    [Fact]
    public void Constructor_WithValidParameters_ShouldCreateInstance() { }
    
    #endregion

    #region Business Rules Tests
    
    [Fact]
    public void UpdateStatus_WhenClosed_ShouldThrowException() { }
    
    #endregion

    #region Validation Tests
    
    [Theory]
    public void Validate_WithInvalidData_ShouldReturnErrors() { }
    
    #endregion
}
```

## Builders and Helpers

### TestDataBuilder
Static class for creating valid objects for tests:

```csharp
namespace {ProjectName}.Test.Helpers;

/// <summary>
/// Test data builder for creating valid domain objects in unit tests.
/// Provides factory methods to create properly configured entities and value objects.
/// </summary>
public static class TestDataBuilder
{
    #region Value Objects

    public static Email CreateValidEmail(string? value = null)
        => new(value ?? "test@example.com");

    public static Name CreateValidName(string? value = null)
        => new(value ?? $"TestName_{DateTime.Now.Ticks}");

    #endregion

    #region Entities

    public static User CreateValidUser(
        string? email = null,
        string? name = null,
        bool isActive = true)
        => new(
            CreateValidEmail(email),
            CreateValidName(name),
            isActive);

    public static Product CreateValidProduct(
        string? name = null,
        decimal? price = null,
        User? createdBy = null)
        => new(
            CreateValidName(name),
            price ?? 100m,
            createdBy ?? CreateValidUser());

    #endregion
}
```

### Benefits of TestDataBuilder
- **Reusability**: Shared methods between tests
- **Maintainability**: Centralized object creation
- **Flexibility**: Allows customization when needed
- **Readability**: Makes tests clearer

## Test Types

### 1. Unit Tests
Test isolated components without external dependencies.

```csharp
[Fact(DisplayName = "Email with valid format should create instance")]
[Trait("Category", "ValueObject")]
[Trait("Priority", "High")]
public void Constructor_WithValidEmail_ShouldCreateInstance()
{
    // Arrange
    var validEmail = "test@example.com";

    // Act
    var email = new Email(validEmail);

    // Assert
    email.Value.Should().Be(validEmail);
}
```

### 2. Integration Tests
Test interaction between multiple components.

```csharp
[Fact(DisplayName = "Complete order workflow should work correctly")]
[Trait("Category", "Integration")]
[Trait("Priority", "High")]
public void CompleteOrderWorkflow_ShouldWorkCorrectly()
{
    // Arrange
    var order = TestDataBuilder.CreateValidOrder();

    // Act & Assert - Test complete workflow
    order.Status.Should().Be(OrderStatus.Pending);
    
    order.Confirm();
    order.Status.Should().Be(OrderStatus.Confirmed);
    
    order.Ship();
    order.Status.Should().Be(OrderStatus.Shipped);
}
```

### 3. Theory Tests (Parameterized Tests)
Test multiple scenarios with different data.

```csharp
[Theory(DisplayName = "Email constructor with valid formats should succeed")]
[Trait("Category", "ValueObject")]
[InlineData("test@example.com")]
[InlineData("user.name@domain.co.uk")]
[InlineData("test+tag@example.org")]
public void Constructor_WithValidEmail_ShouldCreateInstance(string validEmail)
{
    // Act
    var email = new Email(validEmail);

    // Assert
    email.Value.Should().Be(validEmail);
}
```

## Categorization with Traits

Use Traits to categorize and filter tests:

### Main Categories
```csharp
[Trait("Category", "ValueObject")]     // Value Object tests
[Trait("Category", "Entity")]          // Entity tests
[Trait("Category", "Service")]         // Service tests
[Trait("Category", "Repository")]      // Repository tests
[Trait("Category", "Integration")]     // Integration tests
[Trait("Category", "UseCase")]         // Use Case tests
```

### Priorities
```csharp
[Trait("Priority", "High")]            // Critical functionality
[Trait("Priority", "Medium")]          // Important functionality
[Trait("Priority", "Low")]             // Auxiliary functionality
```

### Test Types
```csharp
[Trait("Type", "Validation")]          // Validation tests
[Trait("Type", "Exception")]           // Exception tests
[Trait("Type", "Equality")]            // Equality tests
[Trait("Type", "State")]               // State tests
[Trait("Type", "BusinessRule")]        // Business rule tests
[Trait("Type", "Boundary")]            // Boundary case tests
```

### AI and Machine Learning Testing Considerations
When working with AI components:
- **Test Input Validation**: Ensure proper input sanitization and validation
- **Test Process Flow**: Verify that AI components are called correctly in the workflow
- **Test Error Handling**: Ensure proper handling of AI service failures or timeouts
- **Test Configuration**: Verify AI service configuration and parameter passing
- **Don't Test AI Output Content**: AI-generated content quality should be evaluated through other means (manual review, A/B testing, user feedback)

## Practical Examples

### Testing Value Objects

```csharp
namespace {ProjectName}.Domain.Test.ValueObjects;

/// <summary>
/// Unit tests for Email value object.
/// Tests validation rules, equality, and behavior.
/// </summary>
public class EmailTests
{
    [Theory(DisplayName = "Create Email with valid format should succeed")]
    [Trait("Category", "ValueObject")]
    [Trait("Priority", "High")]
    [Trait("Type", "Validation")]
    [InlineData("test@example.com")]
    [InlineData("user.name@domain.co.uk")]
    [InlineData("test+tag@example.org")]
    public void Constructor_WithValidEmail_ShouldCreateInstance(string validEmail)
    {
        // Act
        var email = new Email(validEmail);

        // Assert
        email.Value.Should().Be(validEmail);
    }

    [Theory(DisplayName = "Create Email with invalid format should throw exception")]
    [Trait("Category", "ValueObject")]
    [Trait("Priority", "High")]
    [Trait("Type", "Exception")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("invalid-email")]
    [InlineData("@domain.com")]
    [InlineData("user@")]
    public void Constructor_WithInvalidEmail_ShouldThrowArgumentException(string? invalidEmail)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new Email(invalidEmail!));
        exception.Message.Should().Contain("Invalid email format");
    }

    [Fact(DisplayName = "Email equality with same value should be equal")]
    [Trait("Category", "ValueObject")]
    [Trait("Priority", "High")]
    [Trait("Type", "Equality")]
    public void Equality_WithSameValue_ShouldBeEqual()
    {
        // Arrange
        var email1 = new Email("test@example.com");
        var email2 = new Email("test@example.com");

        // Act & Assert
        email1.Should().Be(email2);
        (email1 == email2).Should().BeTrue();
        email1.GetHashCode().Should().Be(email2.GetHashCode());
    }
}
```

### Testing Entities

```csharp
namespace {ProjectName}.Domain.Test.Entities;

/// <summary>
/// Unit tests for User entity.
/// Tests business rules, state transitions, and behavior.
/// </summary>
public class UserTests
{
    #region Constructor Tests

    [Fact(DisplayName = "Create user with valid parameters should succeed")]
    [Trait("Category", "Entity")]
    [Trait("Priority", "High")]
    public void Constructor_WithValidParameters_ShouldCreateUser()
    {
        // Arrange
        var email = TestDataBuilder.CreateValidEmail();
        var name = TestDataBuilder.CreateValidName();

        // Act
        var user = new User(email, name);

        // Assert
        user.Should().NotBeNull();
        user.Email.Should().Be(email);
        user.Name.Should().Be(name);
        user.IsActive.Should().BeTrue();
    }

    #endregion

    #region Business Rules Tests

    [Fact(DisplayName = "Deactivate user should change status to inactive")]
    [Trait("Category", "Entity")]
    [Trait("Priority", "High")]
    [Trait("Type", "BusinessRule")]
    public void Deactivate_ShouldChangeStatusToInactive()
    {
        // Arrange
        var user = TestDataBuilder.CreateValidUser();

        // Act
        user.Deactivate();

        // Assert
        user.IsActive.Should().BeFalse();
    }

    [Fact(DisplayName = "Update email when user is inactive should throw exception")]
    [Trait("Category", "Entity")]
    [Trait("Priority", "High")]
    [Trait("Type", "Exception")]
    public void UpdateEmail_WhenUserIsInactive_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var user = TestDataBuilder.CreateValidUser();
        user.Deactivate();
        var newEmail = TestDataBuilder.CreateValidEmail("new@example.com");

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => user.UpdateEmail(newEmail));
    }

    #endregion
}
```

### Testing Services with Mocks

```csharp
namespace {ProjectName}.Application.Test.Services;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _mockRepository;
    private readonly Mock<ILogger<UserService>> _mockLogger;
    private readonly UserService _service;

    public UserServiceTests()
    {
        _mockRepository = new Mock<IUserRepository>();
        _mockLogger = new Mock<ILogger<UserService>>();
        _service = new UserService(_mockRepository.Object, _mockLogger.Object);
    }

    [Fact(DisplayName = "Get user by id should return user when exists")]
    [Trait("Category", "Service")]
    [Trait("Priority", "High")]
    public async Task GetByIdAsync_WhenUserExists_ShouldReturnUser()
    {
        // Arrange
        var userId = 123;
        var expectedUser = TestDataBuilder.CreateValidUser();
        
        _mockRepository.Setup(x => x.GetByIdAsync(userId))
                      .ReturnsAsync(expectedUser);

        // Act
        var result = await _service.GetByIdAsync(userId);

        // Assert
        result.Should().Be(expectedUser);
        _mockRepository.Verify(x => x.GetByIdAsync(userId), Times.Once);
    }

    [Fact(DisplayName = "Get user by id should throw not found exception when not exists")]
    [Trait("Category", "Service")]
    [Trait("Priority", "High")]
    [Trait("Type", "Exception")]
    public async Task GetByIdAsync_WhenUserNotExists_ShouldThrowNotFoundException()
    {
        // Arrange
        var userId = 999;
        
        _mockRepository.Setup(x => x.GetByIdAsync(userId))
                      .ReturnsAsync((User?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _service.GetByIdAsync(userId));
        _mockRepository.Verify(x => x.GetByIdAsync(userId), Times.Once);
    }
}
```

## Execution and Reports

### .NET CLI Commands

```bash
# Run all tests
dotnet test

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run tests of a specific category
dotnet test --filter "Category=ValueObject"

# Run high priority tests
dotnet test --filter "Priority=High"

# Run tests excluding integration
dotnet test --filter "Category!=Integration"

# Run with detailed verbosity
dotnet test --verbosity detailed
```

### Coverage Reports

Use coverlet to generate coverage reports:

```bash
# Install global tools
dotnet tool install -g dotnet-reportgenerator-globaltool

# Generate coverage report
dotnet test --collect:"XPlat Code Coverage" --results-directory ./TestResults

# Convert to HTML
reportgenerator -reports:"./TestResults/*/coverage.cobertura.xml" -targetdir:"./TestResults/CoverageReport" -reporttypes:Html
```

### Quality Metrics

#### Code Coverage
- **Minimum**: 80% coverage
- **Ideal**: 90%+ for critical code
- **Focus**: Line and branch coverage

#### Performance
- **Unit Tests**: < 100ms per test
- **Integration Tests**: < 5s per test
- **Complete Execution**: < 30s

#### Maintainability
- **Descriptive Names**: Easy to identify what is being tested
- **Independence**: Tests should not depend on each other
- **Simplicity**: One test, one scenario

## Checklist for New Tests

### ? Structure
- [ ] Test project created with `.Test` suffix
- [ ] Correct references to tested project
- [ ] NuGet packages installed (xUnit, FluentAssertions, Moq, AutoFixture)
- [ ] Global usings configured

### ? Organization
- [ ] Folder structure mirrors main project
- [ ] Test classes with `Tests` suffix
- [ ] Methods grouped in regions by functionality
- [ ] TestDataBuilder implemented when necessary

### ? Naming
- [ ] Method names follow `{Method}_{Scenario}_{Result}` pattern
- [ ] Descriptive DisplayName
- [ ] Appropriate Traits (Category, Priority, Type)

### ? Implementation
- [ ] AAA pattern implemented (Arrange, Act, Assert)
- [ ] FluentAssertions used for assertions
- [ ] Mocks configured correctly
- [ ] Exception scenarios tested
- [ ] Boundary cases considered

### ? Quality
- [ ] Independent and isolated tests
- [ ] Valid and representative test data
- [ ] Adequate scenario coverage
- [ ] Acceptable execution time

---

## Conclusion

These instructions establish the foundation for consistent and quality tests across projects. Following these practices ensures:

- **Reliability**: Tests that truly validate behavior
- **Maintainability**: Easy to modify and extend
- **Readability**: Clear for all developers
- **Efficiency**: Fast execution and immediate feedback

Remember: **Good tests are an investment in team quality and productivity**.