// <copyright file="PositionMapper.cs" company="RemMeter">
// Copyright (c) 2025 RemMeter. Licensed under the MIT License.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using RemMeter.Models;

namespace RemMeter.Helpers
{
    /// <summary>
    /// Provides efficient position mapping between strings and enum values.
    /// </summary>
    public static class PositionMapper
    {
        /// <summary>
        /// Default position when parsing fails.
        /// </summary>
        public const TimerPosition DefaultPosition = TimerPosition.Right;

        /// <summary>
        /// Static mapping dictionary for fast position lookups.
        /// </summary>
        private static readonly Dictionary<string, TimerPosition> StringToPosition = new()
        {
            // English enum names (primary)
            { "Right", TimerPosition.Right },
            { "Left", TimerPosition.Left },
            { "Top", TimerPosition.Top },
            { "Bottom", TimerPosition.Bottom },

            // Case-insensitive variants
            { "right", TimerPosition.Right },
            { "left", TimerPosition.Left },
            { "top", TimerPosition.Top },
            { "bottom", TimerPosition.Bottom },

            // Additional aliases
            { "R", TimerPosition.Right },
            { "L", TimerPosition.Left },
            { "T", TimerPosition.Top },
            { "B", TimerPosition.Bottom },
        };

        /// <summary>
        /// Reverse mapping for enum to string conversion.
        /// </summary>
        private static readonly Dictionary<TimerPosition, string> PositionToStringMap = new()
        {
            { TimerPosition.Right, "Right" },
            { TimerPosition.Left, "Left" },
            { TimerPosition.Top, "Top" },
            { TimerPosition.Bottom, "Bottom" },
        };

        /// <summary>
        /// Parses a position string to TimerPosition enum with high performance.
        /// </summary>
        /// <param name="position">The position string to parse.</param>
        /// <returns>The corresponding TimerPosition enum value.</returns>
        public static TimerPosition ParsePosition(string position)
        {
            if (string.IsNullOrWhiteSpace(position))
            {
                return DefaultPosition;
            }

            // First try direct dictionary lookup (fastest)
            if (StringToPosition.TryGetValue(position, out TimerPosition result))
            {
                return result;
            }

            // Try with localized resource strings (for backward compatibility)
            result = TryParseLocalizedPosition(position);
            if (result != DefaultPosition)
            {
                return result;
            }

            // Fallback: case-insensitive search
            foreach (var kvp in StringToPosition)
            {
                if (string.Equals(kvp.Key, position, StringComparison.OrdinalIgnoreCase))
                {
                    return kvp.Value;
                }
            }

            return DefaultPosition;
        }

        /// <summary>
        /// Converts TimerPosition enum to its string representation.
        /// </summary>
        /// <param name="position">The TimerPosition enum value.</param>
        /// <returns>The string representation of the position.</returns>
        public static string PositionToString(TimerPosition position)
        {
            return PositionToStringMap.TryGetValue(position, out string? result) ? result : "Right";
        }

        /// <summary>
        /// Gets all valid position strings.
        /// </summary>
        /// <returns>Array of valid position strings.</returns>
        public static string[] GetValidPositions()
        {
            return new[] { "Right", "Left", "Top", "Bottom" };
        }

        /// <summary>
        /// Gets all TimerPosition enum values.
        /// </summary>
        /// <returns>Array of all TimerPosition values.</returns>
        public static TimerPosition[] GetAllPositions()
        {
            return new[] { TimerPosition.Right, TimerPosition.Left, TimerPosition.Top, TimerPosition.Bottom };
        }

        /// <summary>
        /// Checks if a position string is valid.
        /// </summary>
        /// <param name="position">The position string to validate.</param>
        /// <returns>True if the position is valid, false otherwise.</returns>
        public static bool IsValidPosition(string position)
        {
            return !string.IsNullOrWhiteSpace(position) &&
                   StringToPosition.ContainsKey(position);
        }

        /// <summary>
        /// Gets the localized display name for a position.
        /// </summary>
        /// <param name="position">The TimerPosition enum value.</param>
        /// <returns>The localized display name.</returns>
        public static string GetLocalizedDisplayName(TimerPosition position)
        {
            try
            {
                return position switch
                {
                    TimerPosition.Right => Properties.Resources.PositionRight ?? "Right",
                    TimerPosition.Left => Properties.Resources.PositionLeft ?? "Left",
                    TimerPosition.Top => Properties.Resources.PositionTop ?? "Top",
                    TimerPosition.Bottom => Properties.Resources.PositionBottom ?? "Bottom",
                    _ => "Right",
                };
            }
            catch (Exception)
            {
                // Fallback to English if resource loading fails
                return PositionToString(position);
            }
        }

        /// <summary>
        /// Gets the description attribute value for a position enum.
        /// </summary>
        /// <param name="position">The TimerPosition enum value.</param>
        /// <returns>The description text or the enum name if no description exists.</returns>
        public static string GetDescription(TimerPosition position)
        {
            FieldInfo? field = typeof(TimerPosition).GetField(position.ToString());
            if (field != null)
            {
                DescriptionAttribute? attribute = field.GetCustomAttribute<DescriptionAttribute>();
                if (attribute != null)
                {
                    return attribute.Description;
                }
            }

            return position.ToString();
        }

        /// <summary>
        /// Tries to parse localized position strings from resources.
        /// </summary>
        /// <param name="position">The position string to parse.</param>
        /// <returns>The corresponding TimerPosition or DefaultPosition if not found.</returns>
        private static TimerPosition TryParseLocalizedPosition(string position)
        {
            try
            {
                if (position == Properties.Resources.PositionRight)
                {
                    return TimerPosition.Right;
                }

                if (position == Properties.Resources.PositionLeft)
                {
                    return TimerPosition.Left;
                }

                if (position == Properties.Resources.PositionTop)
                {
                    return TimerPosition.Top;
                }

                if (position == Properties.Resources.PositionBottom)
                {
                    return TimerPosition.Bottom;
                }
            }
            catch (Exception)
            {
                // Resource loading failed, continue with fallback
            }

            return DefaultPosition;
        }
    }
}
