using RunAsAdminTool.Models;
using RunAsAdminTool.Tests.Helpers;

namespace RunAsAdminTool.Tests.Models;

/// <summary>
/// Unit tests for ApplicationInfo model.
/// Tests validation rules, properties, and behavior.
/// </summary>
public class ApplicationInfoTests
{
    #region Constructor and Property Tests

    [Fact(DisplayName = "Create ApplicationInfo with default constructor should initialize with empty values")]
    [Trait("Category", "Model")]
    [Trait("Priority", "High")]
    [Trait("Type", "Constructor")]
    public void Constructor_Default_ShouldInitializeWithEmptyValues()
    {
        // Act
        var applicationInfo = new ApplicationInfo();

        // Assert
        applicationInfo.Path.Should().Be(string.Empty);
        applicationInfo.DisplayName.Should().Be(string.Empty);
        applicationInfo.IsValid.Should().BeFalse();
    }

    [Fact(DisplayName = "Set properties with valid values should store values correctly")]
    [Trait("Category", "Model")]
    [Trait("Priority", "High")]
    [Trait("Type", "Property")]
    public void Properties_WithValidValues_ShouldStoreCorrectly()
    {
        // Arrange
        var expectedPath = @"C:\Windows\System32\notepad.exe";
        var expectedDisplayName = "Notepad";
        var applicationInfo = new ApplicationInfo();

        // Act
        applicationInfo.Path = expectedPath;
        applicationInfo.DisplayName = expectedDisplayName;

        // Assert
        applicationInfo.Path.Should().Be(expectedPath);
        applicationInfo.DisplayName.Should().Be(expectedDisplayName);
        applicationInfo.IsValid.Should().BeTrue();
    }

    #endregion

    #region IsValid Property Tests

    [Theory(DisplayName = "IsValid with valid path and display name should return true")]
    [Trait("Category", "Model")]
    [Trait("Priority", "High")]
    [Trait("Type", "Validation")]
    [InlineData(@"C:\Windows\System32\cmd.exe", "Command Prompt")]
    [InlineData(@"C:\Program Files\Test\app.exe", "Test App")]
    [InlineData(@"D:\Tools\MyTool.exe", "My Tool")]
    public void IsValid_WithValidPathAndDisplayName_ShouldReturnTrue(string path, string displayName)
    {
        // Arrange
        var applicationInfo = new ApplicationInfo
        {
            Path = path,
            DisplayName = displayName
        };

        // Act & Assert
        applicationInfo.IsValid.Should().BeTrue();
    }

    [Theory(DisplayName = "IsValid with invalid path should return false")]
    [Trait("Category", "Model")]
    [Trait("Priority", "High")]
    [Trait("Type", "Validation")]
    [InlineData("", "Valid Display Name")]
    [InlineData("   ", "Valid Display Name")]
    [InlineData("\t", "Valid Display Name")]
    [InlineData("\n", "Valid Display Name")]
    public void IsValid_WithInvalidPath_ShouldReturnFalse(string invalidPath, string validDisplayName)
    {
        // Arrange
        var applicationInfo = new ApplicationInfo
        {
            Path = invalidPath,
            DisplayName = validDisplayName
        };

        // Act & Assert
        applicationInfo.IsValid.Should().BeFalse();
    }

    [Theory(DisplayName = "IsValid with invalid display name should return false")]
    [Trait("Category", "Model")]
    [Trait("Priority", "High")]
    [Trait("Type", "Validation")]
    [InlineData(@"C:\Windows\System32\cmd.exe", "")]
    [InlineData(@"C:\Windows\System32\cmd.exe", "   ")]
    [InlineData(@"C:\Windows\System32\cmd.exe", "\t")]
    [InlineData(@"C:\Windows\System32\cmd.exe", "\n")]
    public void IsValid_WithInvalidDisplayName_ShouldReturnFalse(string validPath, string invalidDisplayName)
    {
        // Arrange
        var applicationInfo = new ApplicationInfo
        {
            Path = validPath,
            DisplayName = invalidDisplayName
        };

        // Act & Assert
        applicationInfo.IsValid.Should().BeFalse();
    }

    [Fact(DisplayName = "IsValid with both null values should return false")]
    [Trait("Category", "Model")]
    [Trait("Priority", "High")]
    [Trait("Type", "Validation")]
    public void IsValid_WithNullValues_ShouldReturnFalse()
    {
        // Arrange
        var applicationInfo = new ApplicationInfo
        {
            Path = null!,
            DisplayName = null!
        };

        // Act & Assert
        applicationInfo.IsValid.Should().BeFalse();
    }

    [Fact(DisplayName = "IsValid with both empty values should return false")]
    [Trait("Category", "Model")]
    [Trait("Priority", "High")]
    [Trait("Type", "Validation")]
    public void IsValid_WithBothEmptyValues_ShouldReturnFalse()
    {
        // Arrange
        var applicationInfo = TestDataBuilder.CreateApplicationInfoWithEmptyPath();
        applicationInfo.DisplayName = string.Empty;

        // Act & Assert
        applicationInfo.IsValid.Should().BeFalse();
    }

    #endregion

    #region Boundary Cases

    [Fact(DisplayName = "IsValid with very long path and display name should return true")]
    [Trait("Category", "Model")]
    [Trait("Priority", "Medium")]
    [Trait("Type", "Boundary")]
    public void IsValid_WithVeryLongValues_ShouldReturnTrue()
    {
        // Arrange
        var longPath = string.Concat(Enumerable.Repeat(@"C:\VeryLongDirectoryName", 10)) + @"\app.exe";
        var longDisplayName = string.Concat(Enumerable.Repeat("Very Long Display Name ", 10));
        
        var applicationInfo = new ApplicationInfo
        {
            Path = longPath,
            DisplayName = longDisplayName
        };

        // Act & Assert
        applicationInfo.IsValid.Should().BeTrue();
    }

    [Theory(DisplayName = "IsValid with single character values should return true")]
    [Trait("Category", "Model")]
    [Trait("Priority", "Medium")]
    [Trait("Type", "Boundary")]
    [InlineData("a", "b")]
    [InlineData("1", "2")]
    [InlineData("!", "@")]
    public void IsValid_WithSingleCharacterValues_ShouldReturnTrue(string path, string displayName)
    {
        // Arrange
        var applicationInfo = new ApplicationInfo
        {
            Path = path,
            DisplayName = displayName
        };

        // Act & Assert
        applicationInfo.IsValid.Should().BeTrue();
    }

    #endregion

    #region TestDataBuilder Integration Tests

    [Fact(DisplayName = "TestDataBuilder should create valid ApplicationInfo")]
    [Trait("Category", "Model")]
    [Trait("Priority", "Medium")]
    [Trait("Type", "TestHelper")]
    public void TestDataBuilder_CreateValidApplicationInfo_ShouldCreateValidInstance()
    {
        // Act
        var applicationInfo = TestDataBuilder.CreateValidApplicationInfo();

        // Assert
        applicationInfo.Should().NotBeNull();
        applicationInfo.IsValid.Should().BeTrue();
        applicationInfo.Path.Should().NotBeNullOrWhiteSpace();
        applicationInfo.DisplayName.Should().NotBeNullOrWhiteSpace();
    }

    [Fact(DisplayName = "TestDataBuilder should create invalid ApplicationInfo with empty path")]
    [Trait("Category", "Model")]
    [Trait("Priority", "Medium")]
    [Trait("Type", "TestHelper")]
    public void TestDataBuilder_CreateApplicationInfoWithEmptyPath_ShouldCreateInvalidInstance()
    {
        // Act
        var applicationInfo = TestDataBuilder.CreateApplicationInfoWithEmptyPath();

        // Assert
        applicationInfo.Should().NotBeNull();
        applicationInfo.IsValid.Should().BeFalse();
        applicationInfo.Path.Should().BeEmpty();
        applicationInfo.DisplayName.Should().NotBeNullOrWhiteSpace();
    }

    [Fact(DisplayName = "TestDataBuilder should create invalid ApplicationInfo with empty display name")]
    [Trait("Category", "Model")]
    [Trait("Priority", "Medium")]
    [Trait("Type", "TestHelper")]
    public void TestDataBuilder_CreateApplicationInfoWithEmptyDisplayName_ShouldCreateInvalidInstance()
    {
        // Act
        var applicationInfo = TestDataBuilder.CreateApplicationInfoWithEmptyDisplayName();

        // Assert
        applicationInfo.Should().NotBeNull();
        applicationInfo.IsValid.Should().BeFalse();
        applicationInfo.Path.Should().NotBeNullOrWhiteSpace();
        applicationInfo.DisplayName.Should().BeEmpty();
    }

    #endregion
}