using FluentAssertions;
using RemainingTimeMeter.Validation;

namespace RemainingTimeMeter.Tests.Validation;

public class ValidationResultTests
{
    [Fact]
    public void Success_Should_Create_Valid_Result()
    {
        // Act
        var result = ValidationResult.Success();

        // Assert
        result.IsValid.Should().BeTrue();
        result.ErrorMessages.Should().BeEmpty();
        result.FirstErrorMessage.Should().BeEmpty();
    }

    [Fact]
    public void Failure_With_Single_Message_Should_Create_Invalid_Result()
    {
        // Arrange
        const string errorMessage = "Test error message";

        // Act
        var result = ValidationResult.Failure(errorMessage);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ErrorMessages.Should().ContainSingle().Which.Should().Be(errorMessage);
        result.FirstErrorMessage.Should().Be(errorMessage);
    }

    [Fact]
    public void Failure_With_Multiple_Messages_Should_Create_Invalid_Result()
    {
        // Arrange
        var errorMessages = new[] { "Error 1", "Error 2", "Error 3" };

        // Act
        var result = ValidationResult.Failure(errorMessages);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ErrorMessages.Should().BeEquivalentTo(errorMessages);
        result.FirstErrorMessage.Should().Be("Error 1");
    }

    [Fact]
    public void Failure_With_Empty_Collection_Should_Create_Invalid_Result()
    {
        // Arrange
        var emptyMessages = Array.Empty<string>();

        // Act
        var result = ValidationResult.Failure(emptyMessages);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ErrorMessages.Should().BeEmpty();
        result.FirstErrorMessage.Should().BeEmpty();
    }

    [Fact]
    public void Failure_With_Null_Collection_Should_Create_Invalid_Result()
    {
        // Act
        var result = ValidationResult.Failure((IEnumerable<string>)null!);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ErrorMessages.Should().BeEmpty();
        result.FirstErrorMessage.Should().BeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Failure_With_Null_Or_Whitespace_Message_Should_Handle_Gracefully(string? errorMessage)
    {
        // Act
        var result = ValidationResult.Failure(errorMessage!);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ErrorMessages.Should().ContainSingle();
        result.FirstErrorMessage.Should().Be(errorMessage ?? string.Empty);
    }
}