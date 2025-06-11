// <copyright file="TimerPosition.cs" company="RemainingTimeMeter">
// Copyright (c) 2025 RemainingTimeMeter. Licensed under the MIT License.
// </copyright>

namespace RemainingTimeMeter.Models
{
    /// <summary>
    /// Represents the position where the timer should be displayed.
    /// </summary>
    public enum TimerPosition
    {
        /// <summary>
        /// Display timer on the right side of the screen.
        /// </summary>
        Right,

        /// <summary>
        /// Display timer on the left side of the screen.
        /// </summary>
        Left,

        /// <summary>
        /// Display timer on the top of the screen.
        /// </summary>
        Top,

        /// <summary>
        /// Display timer on the bottom of the screen.
        /// </summary>
        Bottom,
    }
}
