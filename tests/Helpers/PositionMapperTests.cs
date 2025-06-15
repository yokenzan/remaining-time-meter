using FluentAssertions;
using RemainingTimeMeter.Helpers;
using RemainingTimeMeter.Models;

namespace RemainingTimeMeter.Tests.Helpers;

public class PositionMapperTests
{
    [Theory]
    [InlineData("Right", TimerPosition.Right)]
    [InlineData("Left", TimerPosition.Left)]
    [InlineData("Top", TimerPosition.Top)]
    [InlineData("Bottom", TimerPosition.Bottom)]
    public void ParsePosition_Valid_English_Names_Should_Return_Correct_Position(string input, TimerPosition expected)
    {
        // Act
        var result = PositionMapper.ParsePosition(input);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("right", TimerPosition.Right)]
    [InlineData("LEFT", TimerPosition.Left)]
    [InlineData("tOp", TimerPosition.Top)]
    [InlineData("BOTTOM", TimerPosition.Bottom)]
    public void ParsePosition_Case_Insensitive_Should_Work(string input, TimerPosition expected)
    {
        // Act
        var result = PositionMapper.ParsePosition(input);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("R", TimerPosition.Right)]
    [InlineData("L", TimerPosition.Left)]
    [InlineData("T", TimerPosition.Top)]
    [InlineData("B", TimerPosition.Bottom)]
    public void ParsePosition_Single_Letter_Abbreviations_Should_Work(string input, TimerPosition expected)
    {
        // Act
        var result = PositionMapper.ParsePosition(input);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("invalid")]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("Center")]
    [InlineData("Middle")]
    [InlineData("123")]
    public void ParsePosition_Invalid_Input_Should_Return_Default_Position(string input)
    {
        // Act
        var result = PositionMapper.ParsePosition(input);

        // Assert
        result.Should().Be(PositionMapper.DefaultPosition);
    }

    [Fact]
    public void ParsePosition_Null_Input_Should_Return_Default_Position()
    {
        // Act
        var result = PositionMapper.ParsePosition(null!);

        // Assert
        result.Should().Be(PositionMapper.DefaultPosition);
    }

    [Theory]
    [InlineData(TimerPosition.Right, "Right")]
    [InlineData(TimerPosition.Left, "Left")]
    [InlineData(TimerPosition.Top, "Top")]
    [InlineData(TimerPosition.Bottom, "Bottom")]
    public void PositionToString_Should_Return_Correct_String(TimerPosition position, string expected)
    {
        // Act
        var result = PositionMapper.PositionToString(position);

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void PositionToString_Invalid_Enum_Should_Return_Default_String()
    {
        // Arrange
        var invalidPosition = (TimerPosition)999;

        // Act
        var result = PositionMapper.PositionToString(invalidPosition);

        // Assert
        result.Should().Be("Right"); // Default position string
    }

    [Theory]
    [InlineData("Right", true)]
    [InlineData("Left", true)]
    [InlineData("Top", true)]
    [InlineData("Bottom", true)]
    [InlineData("invalid", false)]
    [InlineData("", false)]
    public void IsValidPosition_Should_Return_Correct_Result(string position, bool expected)
    {
        // Act
        var result = PositionMapper.IsValidPosition(position);

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void IsValidPosition_Null_Should_Return_False()
    {
        // Act
        var result = PositionMapper.IsValidPosition(null!);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void DefaultPosition_Should_Be_Right()
    {
        // Assert
        PositionMapper.DefaultPosition.Should().Be(TimerPosition.Right);
    }

    [Theory]
    [InlineData("  Right  ", TimerPosition.Right)] // Whitespace trimming
    [InlineData("\tLeft\t", TimerPosition.Left)] // Tab trimming
    [InlineData("\nTop\n", TimerPosition.Top)] // Newline trimming
    public void ParsePosition_Should_Trim_Whitespace(string input, TimerPosition expected)
    {
        // Act
        var result = PositionMapper.ParsePosition(input);

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void ParsePosition_And_PositionToString_Should_Be_Reversible()
    {
        // Arrange
        var allPositions = Enum.GetValues<TimerPosition>();

        foreach (var position in allPositions)
        {
            // Act
            var stringValue = PositionMapper.PositionToString(position);
            var parsedBack = PositionMapper.ParsePosition(stringValue);

            // Assert
            parsedBack.Should().Be(position, $"Position {position} should round-trip correctly");
        }
    }
}