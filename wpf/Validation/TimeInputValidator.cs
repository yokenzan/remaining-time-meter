// <copyright file="TimeInputValidator.cs" company="RemMeter">
// Copyright (c) 2025 RemMeter. Licensed under the MIT License.
// </copyright>

using System;
using System.Text.RegularExpressions;

namespace RemMeter.Validation
{
    /// <summary>
    /// Provides validation and parsing functionality for timer input.
    /// </summary>
    public static class TimeInputValidator
    {
        /// <summary>
        /// Maximum allowed minutes value.
        /// </summary>
        public const int MaxMinutes = 99;

        /// <summary>
        /// Maximum allowed seconds value.
        /// </summary>
        public const int MaxSeconds = 99;

        /// <summary>
        /// Parses time input string and returns validated minutes and seconds.
        /// </summary>
        /// <param name="input">The input string to parse.</param>
        /// <returns>A tuple containing validated minutes and seconds.</returns>
        public static (int minutes, int seconds) ParseTimeInput(string input)
        {
            try
            {
                // Handle empty or null input
                if (string.IsNullOrWhiteSpace(input))
                {
                    return (0, 0);
                }

                // Remove any non-numeric characters except colon
                string cleanInput = Regex.Replace(input, @"[^\d:]", string.Empty);

                // Handle empty after cleaning
                if (string.IsNullOrEmpty(cleanInput))
                {
                    return (0, 0);
                }

                // Handle colon-separated format (MM:SS)
                if (cleanInput.Contains(":"))
                {
                    return ParseColonSeparatedInput(cleanInput);
                }

                // Handle numeric-only input
                if (int.TryParse(cleanInput, out int value))
                {
                    return ParseNumericInput(value);
                }

                return (0, 0);
            }
            catch (Exception ex)
            {
                Logger.Error("ParseTimeInput failed", ex);
                return (0, 0);
            }
        }

        /// <summary>
        /// Validates and normalizes time values to ensure they are within acceptable bounds.
        /// </summary>
        /// <param name="minutes">Input minutes.</param>
        /// <param name="seconds">Input seconds.</param>
        /// <returns>Validated and normalized time tuple.</returns>
        public static (int minutes, int seconds) ValidateTime(int minutes, int seconds)
        {
            // Ensure non-negative values
            minutes = Math.Max(0, minutes);
            seconds = Math.Max(0, seconds);

            // Enforce maximum limits
            if (minutes > MaxMinutes)
            {
                minutes = MaxMinutes;
                seconds = MaxSeconds;
            }
            else if (minutes == MaxMinutes && seconds > MaxSeconds)
            {
                seconds = MaxSeconds;
            }
            else if (seconds > MaxSeconds)
            {
                seconds = MaxSeconds;
            }

            return (minutes, seconds);
        }

        /// <summary>
        /// Validates that the total time (in seconds) is greater than zero.
        /// </summary>
        /// <param name="minutes">Minutes component.</param>
        /// <param name="seconds">Seconds component.</param>
        /// <returns>True if total time is valid (greater than 0), false otherwise.</returns>
        public static bool IsValidTotalTime(int minutes, int seconds)
        {
            int totalSeconds = (minutes * 60) + seconds;
            return totalSeconds > 0;
        }

        /// <summary>
        /// Formats time for display in the input field.
        /// </summary>
        /// <param name="minutes">Minutes to format.</param>
        /// <param name="seconds">Seconds to format.</param>
        /// <returns>Formatted string for input field.</returns>
        public static string FormatTimeForInput(int minutes, int seconds)
        {
            var (validMinutes, validSeconds) = ValidateTime(minutes, seconds);

            // Use simple format only for seconds under 100
            if (validMinutes == 0 && validSeconds < 100)
            {
                return validSeconds.ToString();
            }
            else
            {
                return $"{validMinutes:D2}{validSeconds:D2}";
            }
        }

        /// <summary>
        /// Formats time for display (MM:SS format).
        /// </summary>
        /// <param name="minutes">Minutes to format.</param>
        /// <param name="seconds">Seconds to format.</param>
        /// <returns>Formatted time string in MM:SS format.</returns>
        public static string FormatTimeForDisplay(int minutes, int seconds)
        {
            var (validMinutes, validSeconds) = ValidateTime(minutes, seconds);
            return $"{validMinutes:D2}:{validSeconds:D2}";
        }

        /// <summary>
        /// Determines if the input represents a quick time selection (1-2 digits as seconds).
        /// </summary>
        /// <param name="input">The input string to check.</param>
        /// <returns>True if input is a quick time format, false otherwise.</returns>
        public static bool IsQuickTimeFormat(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            string cleanInput = Regex.Replace(input, @"[^\d]", string.Empty);
            return int.TryParse(cleanInput, out int value) && value >= 1 && value <= 99;
        }

        /// <summary>
        /// Parses colon-separated time input (MM:SS format).
        /// </summary>
        /// <param name="input">The cleaned input string containing colon.</param>
        /// <returns>Validated time tuple.</returns>
        private static (int minutes, int seconds) ParseColonSeparatedInput(string input)
        {
            var parts = input.Split(':');
            if (parts.Length >= 2 &&
                int.TryParse(parts[0], out int minutes) &&
                int.TryParse(parts[1], out int seconds))
            {
                return ValidateTime(minutes, seconds);
            }

            return (0, 0);
        }

        /// <summary>
        /// Parses numeric-only input with smart interpretation based on digit count.
        /// </summary>
        /// <param name="value">The numeric value to parse.</param>
        /// <returns>Validated time tuple.</returns>
        private static (int minutes, int seconds) ParseNumericInput(int value)
        {
            if (value <= 9)
            {
                // Single digit - treat as seconds
                return ValidateTime(0, value);
            }
            else if (value <= 99)
            {
                // Double digit - treat as seconds
                return ValidateTime(0, value);
            }
            else if (value <= 9999)
            {
                // 3-4 digits - treat as MMSS
                int minutes = value / 100;
                int seconds = value % 100;
                return ValidateTime(minutes, seconds);
            }
            else
            {
                // 5+ digits - treat as total seconds with upper limit
                int maxTotalSeconds = (MaxMinutes * 60) + MaxSeconds;
                int totalSeconds = Math.Min(value, maxTotalSeconds);
                return ValidateTime(totalSeconds / 60, totalSeconds % 60);
            }
        }
    }
}