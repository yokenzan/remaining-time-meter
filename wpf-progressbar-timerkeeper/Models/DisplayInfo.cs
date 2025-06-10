// <copyright file="DisplayInfo.cs" company="MeterTimeKeeper">
// Copyright (c) MeterTimeKeeper. All rights reserved.
// </copyright>

namespace ProgressBarTimerKeeper.Models
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
        /// Gets or sets a value indicating whether this display is the primary display.
        /// </summary>
        public bool IsPrimary { get; set; }
    }
}
