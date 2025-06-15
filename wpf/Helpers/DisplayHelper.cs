// <copyright file="DisplayHelper.cs" company="RemMeter">
// Copyright (c) 2025 RemMeter. Licensed under the MIT License.
// </copyright>

using System.Windows;
using System.Windows.Media;
using RemMeter.Models;

namespace RemMeter.Helpers
{
    /// <summary>
    /// Helper class for display and DPI-related calculations.
    /// </summary>
    public static class DisplayHelper
    {
        /// <summary>
        /// Converts physical screen coordinates to logical (DPI-aware) coordinates.
        /// </summary>
        /// <param name="display">The display information to convert.</param>
        /// <param name="visual">The visual element to get DPI information from.</param>
        /// <returns>The logical screen bounds.</returns>
        public static LogicalScreenBounds GetLogicalScreenBounds(DisplayInfo display, Visual visual)
        {
            var dpiScale = VisualTreeHelper.GetDpi(visual);

            return new LogicalScreenBounds
            {
                Width = display.Width / dpiScale.DpiScaleX,
                Height = display.Height / dpiScale.DpiScaleY,
                Left = display.Left / dpiScale.DpiScaleX,
                Top = display.Top / dpiScale.DpiScaleY,
            };
        }

        /// <summary>
        /// Gets the DPI scale factors for a visual element.
        /// </summary>
        /// <param name="visual">The visual element.</param>
        /// <returns>The DPI scale information.</returns>
        public static DpiScale GetDpiScale(Visual visual)
        {
            return VisualTreeHelper.GetDpi(visual);
        }

        /// <summary>
        /// Represents logical (DPI-aware) screen dimensions.
        /// </summary>
        public struct LogicalScreenBounds
        {
            /// <summary>
            /// Gets or sets the logical width of the screen.
            /// </summary>
            public double Width { get; set; }

            /// <summary>
            /// Gets or sets the logical height of the screen.
            /// </summary>
            public double Height { get; set; }

            /// <summary>
            /// Gets or sets the logical left position of the screen.
            /// </summary>
            public double Left { get; set; }

            /// <summary>
            /// Gets or sets the logical top position of the screen.
            /// </summary>
            public double Top { get; set; }
        }
    }
}
