using FluentAssertions;
using RemainingTimeMeter.Validation;

namespace RemainingTimeMeter.Tests.Validation;

public class TimeInputValidatorTests
{
    [Theory]
    [InlineData("5", 5, 0)] // Simple minutes
    [InlineData("30", 30, 0)] // Higher minutes  
    [InlineData("45", 0, 45)] // Seconds under 60
    [InlineData("90", 1, 30)] // Total seconds converted to minutes:seconds
    [InlineData("123", 2, 3)] // 123 seconds = 2:03
    [InlineData("5:30", 5, 30)] // MM:SS format
    public void ParseTimeInput_Valid_Inputs_Should_Return_Correct_Time(string input, int expectedMinutes, int expectedSeconds)
    {
        // Act
        var (minutes, seconds) = TimeInputValidator.ParseTimeInput(input);

        // Assert
        minutes.Should().Be(expectedMinutes);
        seconds.Should().Be(expectedSeconds);
    }

    [Theory]
    [InlineData("0")] // Zero time
    [InlineData("")] // Empty string
    [InlineData("   ")] // Whitespace
    [InlineData("abc")] // Non-numeric
    public void ParseTimeInput_Invalid_Inputs_Should_Return_Zero_Time(string input)
    {
        // Act
        var (minutes, seconds) = TimeInputValidator.ParseTimeInput(input);

        // Assert - Invalid inputs return (0, 0) according to the implementation
        minutes.Should().Be(0);
        seconds.Should().Be(0);
    }

    [Theory]
    [InlineData(1, 0, true)] // 1 minute
    [InlineData(0, 30, true)] // 30 seconds
    [InlineData(59, 59, true)] // Normal case
    [InlineData(0, 0, false)] // Zero time
    [InlineData(-1, 0, false)] // Negative minutes
    [InlineData(0, -1, false)] // Negative seconds
    public void IsValidTotalTime_Should_Return_Correct_Result(int minutes, int seconds, bool expectedValid)
    {
        // Act
        var result = TimeInputValidator.IsValidTotalTime(minutes, seconds);

        // Assert
        result.Should().Be(expectedValid);
    }

    [Theory]
    [InlineData(5, 0, "5")]
    [InlineData(0, 30, "30")]
    [InlineData(2, 30, "0230")] // 2:30 formatted as MMSS
    [InlineData(10, 5, "1005")] // 10:05 formatted as MMSS
    [InlineData(0, 0, "0")]
    public void FormatTimeForInput_Should_Return_Correct_Format(int minutes, int seconds, string expected)
    {
        // Act
        var result = TimeInputValidator.FormatTimeForInput(minutes, seconds);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(5, 0, "05:00")]
    [InlineData(0, 30, "00:30")]
    [InlineData(2, 30, "02:30")]
    [InlineData(10, 5, "10:05")]
    public void FormatTimeForDisplay_Should_Return_MM_SS_Format(int minutes, int seconds, string expected)
    {
        // Act
        var result = TimeInputValidator.FormatTimeForDisplay(minutes, seconds);

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void ParseTimeInput_Null_Input_Should_Return_Zero_Time()
    {
        // Act
        var (minutes, seconds) = TimeInputValidator.ParseTimeInput(null!);

        // Assert
        minutes.Should().Be(0);
        seconds.Should().Be(0);
    }

    [Theory]
    [InlineData("5", true)] // Valid quick time
    [InlineData("30", true)] // Valid quick time
    [InlineData("99", true)] // Maximum quick time
    [InlineData("0", false)] // Zero not valid
    [InlineData("100", false)] // Too large
    [InlineData("abc", false)] // Non-numeric
    [InlineData("", false)] // Empty
    public void IsQuickTimeFormat_Should_Return_Correct_Result(string input, bool expected)
    {
        // Act
        var result = TimeInputValidator.IsQuickTimeFormat(input);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("59", 0, 59)] // Boundary case: exactly 59 seconds
    [InlineData("60", 1, 0)] // Boundary case: exactly 1 minute
    [InlineData("119", 1, 59)] // Boundary case: 1:59
    [InlineData("5:30", 5, 30)] // Colon format
    [InlineData("05:05", 5, 5)] // Leading zeros
    public void ParseTimeInput_Boundary_Cases_Should_Work_Correctly(string input, int expectedMinutes, int expectedSeconds)
    {
        // Act
        var (minutes, seconds) = TimeInputValidator.ParseTimeInput(input);

        // Assert
        minutes.Should().Be(expectedMinutes);
        seconds.Should().Be(expectedSeconds);
    }

    [Theory]
    [InlineData("  5  ", 5, 0)] // Whitespace trimming
    [InlineData("5m", 5, 0)] // Non-numeric characters removed
    [InlineData("5 minutes", 5, 0)] // Text removed
    public void ParseTimeInput_Should_Clean_Input_Correctly(string input, int expectedMinutes, int expectedSeconds)
    {
        // Act
        var (minutes, seconds) = TimeInputValidator.ParseTimeInput(input);

        // Assert
        minutes.Should().Be(expectedMinutes);
        seconds.Should().Be(expectedSeconds);
    }
}