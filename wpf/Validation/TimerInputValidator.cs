// <copyright file="TimerInputValidator.cs" company="RemMeter">
// Copyright (c) 2025 RemMeter. Licensed under the MIT License.
// </copyright>

using System;
using System.Collections.Generic;
using RemMeter.Properties;

namespace RemMeter.Validation
{
    /// <summary>
    /// Provides comprehensive validation for timer input with detailed error reporting.
    /// </summary>
    public static class TimerInputValidator
    {
        /// <summary>
        /// Validates timer input and provides detailed error information.
        /// </summary>
        /// <param name="input">The input string to validate.</param>
        /// <returns>A validation result with success/failure status and error messages.</returns>
        public static ValidationResult ValidateInput(string input)
        {
            var errors = new List<string>();

            // Parse the input
            var (minutes, seconds) = TimeInputValidator.ParseTimeInput(input);

            // Check if total time is valid
            if (!TimeInputValidator.IsValidTotalTime(minutes, seconds))
            {
                errors.Add(Resources.PleaseSetTimeCorrectly ?? "Please set the time correctly.");
            }

            // Check for reasonable upper bounds (beyond technical limits)
            int totalSeconds = (minutes * 60) + seconds;

            // More than 1 hour
            if (totalSeconds > 3600)
            {
                errors.Add("Timer duration should not exceed 1 hour for practical use.");
            }

            // Provide informational feedback for very short durations
            if (totalSeconds > 0 && totalSeconds < 10)
            {
                // This is not an error, just information
                // Could be used for warnings in UI
            }

            return errors.Count == 0 ? ValidationResult.Success() : ValidationResult.Failure(errors);
        }

        /// <summary>
        /// Validates display selection.
        /// </summary>
        /// <param name="displayInfo">The selected display information.</param>
        /// <returns>A validation result for display selection.</returns>
        public static ValidationResult ValidateDisplay(Models.DisplayInfo? displayInfo)
        {
            if (displayInfo == null)
            {
                return ValidationResult.Failure("No display selected.");
            }

            // Check for reasonable display dimensions
            if (displayInfo.Width < 800 || displayInfo.Height < 600)
            {
                return ValidationResult.Failure("Selected display resolution is too small for optimal timer visibility.");
            }

            return ValidationResult.Success();
        }

        /// <summary>
        /// Validates position selection.
        /// </summary>
        /// <param name="position">The selected position string.</param>
        /// <returns>A validation result for position selection.</returns>
        public static ValidationResult ValidatePosition(string? position)
        {
            if (string.IsNullOrWhiteSpace(position))
            {
                return ValidationResult.Failure("No position selected.");
            }

            var validPositions = new[] { "Right", "Left", "Top", "Bottom" };
            if (Array.IndexOf(validPositions, position) == -1)
            {
                return ValidationResult.Failure($"Invalid position: {position}. Valid positions are: {string.Join(", ", validPositions)}");
            }

            return ValidationResult.Success();
        }

        /// <summary>
        /// Validates all timer setup parameters comprehensively.
        /// </summary>
        /// <param name="timeInput">The time input string.</param>
        /// <param name="displayInfo">The selected display information.</param>
        /// <param name="position">The selected position string.</param>
        /// <returns>A comprehensive validation result.</returns>
        public static ValidationResult ValidateTimerSetup(string timeInput, Models.DisplayInfo? displayInfo, string? position)
        {
            var errors = new List<string>();

            // Validate time input
            var timeValidation = ValidateInput(timeInput);
            if (!timeValidation.IsValid)
            {
                errors.AddRange(timeValidation.ErrorMessages);
            }

            // Validate display
            var displayValidation = ValidateDisplay(displayInfo);
            if (!displayValidation.IsValid)
            {
                errors.AddRange(displayValidation.ErrorMessages);
            }

            // Validate position
            var positionValidation = ValidatePosition(position);
            if (!positionValidation.IsValid)
            {
                errors.AddRange(positionValidation.ErrorMessages);
            }

            return errors.Count == 0 ? ValidationResult.Success() : ValidationResult.Failure(errors);
        }

        /// <summary>
        /// Gets user-friendly suggestions for common input patterns.
        /// </summary>
        /// <param name="input">The input string to analyze.</param>
        /// <returns>A list of suggestions for the user.</returns>
        public static List<string> GetInputSuggestions(string input)
        {
            var suggestions = new List<string>();

            if (string.IsNullOrWhiteSpace(input))
            {
                suggestions.Add("Try entering a number like '5' for 5 seconds, or '130' for 1 minute 30 seconds");
                suggestions.Add("You can also use colon format like '5:30' for 5 minutes 30 seconds");
                return suggestions;
            }

            var (minutes, seconds) = TimeInputValidator.ParseTimeInput(input);
            if (TimeInputValidator.IsQuickTimeFormat(input))
            {
                suggestions.Add($"Interpreted as {seconds} seconds");
            }
            else if (input.Contains(":"))
            {
                suggestions.Add($"Interpreted as {minutes} minutes {seconds} seconds");
            }
            else if (input.Length >= 3)
            {
                suggestions.Add($"Interpreted as {minutes} minutes {seconds} seconds (MMSS format)");
            }

            // Suggest alternative formats
            if (minutes == 0 && seconds > 0)
            {
                suggestions.Add($"Alternative: '{minutes}:{seconds:D2}' for the same duration");
            }
            else if (minutes > 0)
            {
                suggestions.Add($"Alternative: '{seconds + (minutes * 60)}' for total seconds");
            }

            return suggestions;
        }
    }
}
