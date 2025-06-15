// <copyright file="DisplayInfo.cs" company="RemMeter">
// Copyright (c) 2025 RemMeter. Licensed under the MIT License.
// </copyright>

namespace RemMeter.Models
{
    /// <summary>
    /// Represents display information including position and dimensions.
    /// </summary>
    public class DisplayInfo
    {
        /// <summary>
        /// Gets or sets the left coordinate of the display.
        /// </summary>
        public int Left { get; set; }

        /// <summary>
        /// Gets or sets the top coordinate of the display.
        /// </summary>
        public int Top { get; set; }

        /// <summary>
        /// Gets or sets the width of the display.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the display.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the DPI scale factor for the X-axis.
        /// </summary>
        public double ScaleX { get; set; } = 1.0;

        /// <summary>
        /// Gets or sets the DPI scale factor for the Y-axis.
        /// </summary>
        public double ScaleY { get; set; } = 1.0;

        /// <summary>
        /// Gets or sets a value indicating whether this display is the primary display.
        /// </summary>
        public bool IsPrimary { get; set; }
    }
}
