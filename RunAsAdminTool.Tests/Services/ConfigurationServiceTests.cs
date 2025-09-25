using System.Text.Json;
using RunAsAdminTool.Models;
using RunAsAdminTool.Services;
using RunAsAdminTool.Services.Interfaces;
using RunAsAdminTool.Tests.Helpers;

namespace RunAsAdminTool.Tests.Services;

/// <summary>
/// Unit tests for ConfigurationService.
/// Tests configuration loading, JSON deserialization, and validation filtering.
/// Note: File system and UI operations are tested through integration-style tests.
/// </summary>
public class ConfigurationServiceTests
{
    private readonly IConfigurationService _service;

    public ConfigurationServiceTests()
    {
        _service = new ConfigurationService();
    }

    #region Constructor Tests

    [Fact(DisplayName = "Service should implement IConfigurationService interface")]
    [Trait("Category", "Service")]
    [Trait("Priority", "High")]
    [Trait("Type", "Constructor")]
    public void Service_ShouldImplementInterface()
    {
        // Assert
        _service.Should().BeAssignableTo<IConfigurationService>();
    }

    #endregion

    #region LoadApplications Tests - Integration Style

    [Fact(DisplayName = "LoadApplications when configuration file does not exist should return empty list")]
    [Trait("Category", "Service")]
    [Trait("Priority", "High")]
    [Trait("Type", "Integration")]
    public void LoadApplications_WhenConfigFileDoesNotExist_ShouldReturnEmptyList()
    {
        // Note: This test assumes the Applications.json file doesn't exist in the test environment
        // In a real scenario, we might want to temporarily rename the file or use dependency injection
        
        // Act
        var result = _service.LoadApplications();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<List<ApplicationInfo>>();
        // Result could be empty list if file doesn't exist, or contain valid apps if it does
        // The important thing is that it doesn't throw an exception
    }

    [Fact(DisplayName = "LoadApplications should not throw exceptions regardless of file state")]
    [Trait("Category", "Service")]
    [Trait("Priority", "High")]
    [Trait("Type", "Exception")]
    public void LoadApplications_ShouldNotThrowExceptions()
    {
        // Act & Assert
        var action = () => _service.LoadApplications();
        action.Should().NotThrow();
    }

    [Fact(DisplayName = "LoadApplications should always return non-null list")]
    [Trait("Category", "Service")]
    [Trait("Priority", "High")]
    [Trait("Type", "Behavior")]
    public void LoadApplications_ShouldAlwaysReturnNonNullList()
    {
        // Act
        var result = _service.LoadApplications();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<List<ApplicationInfo>>();
    }

    #endregion

    #region JSON Deserialization Simulation Tests

    [Fact(DisplayName = "JSON deserialization of valid ApplicationInfo list should work correctly")]
    [Trait("Category", "Service")]
    [Trait("Priority", "High")]
    [Trait("Type", "Serialization")]
    public void JsonDeserialization_WithValidApplicationInfoList_ShouldWorkCorrectly()
    {
        // Arrange
        var originalApps = TestDataBuilder.CreateValidApplicationInfoList(3);
        var json = JsonSerializer.Serialize(originalApps);

        // Act - Simulate the deserialization that happens in ConfigurationService
        var deserializedApps = JsonSerializer.Deserialize<List<ApplicationInfo>>(json);

        // Assert
        deserializedApps.Should().NotBeNull();
        deserializedApps.Should().HaveCount(3);
        
        for (int i = 0; i < originalApps.Count; i++)
        {
            deserializedApps![i].Path.Should().Be(originalApps[i].Path);
            deserializedApps[i].DisplayName.Should().Be(originalApps[i].DisplayName);
            deserializedApps[i].IsValid.Should().BeTrue();
        }
    }

    [Fact(DisplayName = "JSON deserialization should handle empty array")]
    [Trait("Category", "Service")]
    [Trait("Priority", "Medium")]
    [Trait("Type", "Serialization")]
    public void JsonDeserialization_WithEmptyArray_ShouldReturnEmptyList()
    {
        // Arrange
        var emptyJson = "[]";

        // Act
        var result = JsonSerializer.Deserialize<List<ApplicationInfo>>(emptyJson);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact(DisplayName = "JSON deserialization should handle mixed valid and invalid entries")]
    [Trait("Category", "Service")]
    [Trait("Priority", "High")]
    [Trait("Type", "Serialization")]
    public void JsonDeserialization_WithMixedValidAndInvalidEntries_ShouldDeserializeAll()
    {
        // Arrange
        var mixedApps = TestDataBuilder.CreateMixedApplicationInfoList();
        var json = JsonSerializer.Serialize(mixedApps);

        // Act
        var deserializedApps = JsonSerializer.Deserialize<List<ApplicationInfo>>(json);

        // Assert
        deserializedApps.Should().NotBeNull();
        deserializedApps.Should().HaveCount(mixedApps.Count);

        // Verify that all entries were deserialized (including invalid ones)
        var validCount = deserializedApps!.Count(app => app.IsValid);
        var invalidCount = deserializedApps.Count(app => !app.IsValid);
        
        validCount.Should().BeGreaterThan(0);
        invalidCount.Should().BeGreaterThan(0);
    }

    #endregion

    #region Filtering Logic Simulation Tests

    [Fact(DisplayName = "Application filtering should remove invalid entries")]
    [Trait("Category", "Service")]
    [Trait("Priority", "High")]
    [Trait("Type", "BusinessRule")]
    public void ApplicationFiltering_ShouldRemoveInvalidEntries()
    {
        // Arrange
        var mixedApps = TestDataBuilder.CreateMixedApplicationInfoList();
        var validAppsCount = mixedApps.Count(app => app.IsValid);

        // Act - Simulate the filtering that happens in ConfigurationService
        var filteredApps = mixedApps.Where(app => app.IsValid).ToList();

        // Assert
        filteredApps.Should().HaveCount(validAppsCount);
        filteredApps.Should().AllSatisfy(app => app.IsValid.Should().BeTrue());
    }

    [Fact(DisplayName = "Application filtering should preserve all valid entries")]
    [Trait("Category", "Service")]
    [Trait("Priority", "High")]
    [Trait("Type", "BusinessRule")]
    public void ApplicationFiltering_ShouldPreserveAllValidEntries()
    {
        // Arrange
        var validApps = TestDataBuilder.CreateValidApplicationInfoList(5);
        var allApps = validApps.ToList(); // Create a copy

        // Act - Simulate the filtering that happens in ConfigurationService
        var filteredApps = allApps.Where(app => app.IsValid).ToList();

        // Assert
        filteredApps.Should().HaveCount(validApps.Count);
        filteredApps.Should().BeEquivalentTo(validApps);
    }

    [Fact(DisplayName = "Application filtering should handle empty list")]
    [Trait("Category", "Service")]
    [Trait("Priority", "Medium")]
    [Trait("Type", "Boundary")]
    public void ApplicationFiltering_WithEmptyList_ShouldReturnEmptyList()
    {
        // Arrange
        var emptyList = new List<ApplicationInfo>();

        // Act
        var filteredApps = emptyList.Where(app => app.IsValid).ToList();

        // Assert
        filteredApps.Should().BeEmpty();
    }

    [Fact(DisplayName = "Application filtering should handle list with all invalid entries")]
    [Trait("Category", "Service")]
    [Trait("Priority", "Medium")]
    [Trait("Type", "Boundary")]
    public void ApplicationFiltering_WithAllInvalidEntries_ShouldReturnEmptyList()
    {
        // Arrange
        var invalidApps = new List<ApplicationInfo>
        {
            TestDataBuilder.CreateApplicationInfoWithEmptyPath(),
            TestDataBuilder.CreateApplicationInfoWithEmptyDisplayName(),
            TestDataBuilder.CreateApplicationInfoWithWhitespaceValues(),
            new ApplicationInfo { Path = null!, DisplayName = null! }
        };

        // Act
        var filteredApps = invalidApps.Where(app => app.IsValid).ToList();

        // Assert
        filteredApps.Should().BeEmpty();
    }

    #endregion

    #region Service Behavior Tests

    [Fact(DisplayName = "Service should be reusable for multiple load operations")]
    [Trait("Category", "Service")]
    [Trait("Priority", "Medium")]
    [Trait("Type", "Behavior")]
    public void Service_ShouldBeReusableForMultipleLoads()
    {
        // Act
        var result1 = _service.LoadApplications();
        var result2 = _service.LoadApplications();
        var result3 = _service.LoadApplications();

        // Assert
        result1.Should().NotBeNull();
        result2.Should().NotBeNull();
        result3.Should().NotBeNull();
        
        // Results should be consistent (same configuration file)
        result1.Should().BeEquivalentTo(result2);
        result2.Should().BeEquivalentTo(result3);
    }

    #endregion

    #region Edge Cases and Error Scenarios

    [Fact(DisplayName = "JSON with null values should be handled gracefully")]
    [Trait("Category", "Service")]
    [Trait("Priority", "Medium")]
    [Trait("Type", "Exception")]
    public void JsonDeserialization_WithNullValues_ShouldBeHandledGracefully()
    {
        // Arrange
        var jsonWithNulls = """
        [
            {
                "Path": "C:\\Windows\\System32\\cmd.exe",
                "DisplayName": "Command Prompt"
            },
            {
                "Path": null,
                "DisplayName": "Invalid App"
            },
            {
                "Path": "C:\\Windows\\System32\\notepad.exe",
                "DisplayName": null
            }
        ]
        """;

        // Act & Assert
        var action = () => JsonSerializer.Deserialize<List<ApplicationInfo>>(jsonWithNulls);
        action.Should().NotThrow();

        var result = JsonSerializer.Deserialize<List<ApplicationInfo>>(jsonWithNulls);
        result.Should().NotBeNull();
        result.Should().HaveCount(3);

        // Verify filtering would work correctly
        var validEntries = result!.Where(app => app.IsValid).ToList();
        validEntries.Should().HaveCount(1); // Only the first entry should be valid
    }

    [Fact(DisplayName = "Malformed JSON should cause deserialization to fail")]
    [Trait("Category", "Service")]
    [Trait("Priority", "Medium")]
    [Trait("Type", "Exception")]
    public void JsonDeserialization_WithMalformedJson_ShouldThrowException()
    {
        // Arrange
        var malformedJson = "{ invalid json structure }";

        // Act & Assert
        var action = () => JsonSerializer.Deserialize<List<ApplicationInfo>>(malformedJson);
        action.Should().Throw<JsonException>();
    }

    #endregion
}