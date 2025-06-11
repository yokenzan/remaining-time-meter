// <copyright file="Constants.cs" company="RemainingTimeMeter">
// Copyright (c) 2025 RemainingTimeMeter. Licensed under the MIT License.
// </copyright>

namespace RemainingTimeMeter
{
    /// <summary>
    /// Application constants for UI layout and timing.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Timer bar width for vertical positioning.
        /// </summary>
        public const double TimerBarWidth = 20;

        /// <summary>
        /// Timer bar height for horizontal positioning.
        /// </summary>
        public const double TimerBarHeight = 20;

        /// <summary>
        /// Screen size ratio for timer bar length.
        /// </summary>
        public const double ScreenSizeRatio = 0.8;

        /// <summary>
        /// Margin from screen edge.
        /// </summary>
        public const double ScreenMargin = 10;

        /// <summary>
        /// Bottom margin for bottom positioning.
        /// </summary>
        public const double BottomMargin = 50;

        /// <summary>
        /// Expanded width for control panel.
        /// </summary>
        public const double ExpandedWidth = 200;

        /// <summary>
        /// Expanded height for control panel.
        /// </summary>
        public const double ExpandedHeight = 150;

        /// <summary>
        /// Notification display duration in milliseconds.
        /// </summary>
        public const int NotificationDuration = 5000;

        /// <summary>
        /// Notification cleanup delay in milliseconds.
        /// </summary>
        public const int NotificationCleanupDelay = 6000;

        /// <summary>
        /// Progress threshold for orange color.
        /// </summary>
        public const double OrangeThreshold = 0.6;

        /// <summary>
        /// Progress threshold for red color.
        /// </summary>
        public const double RedThreshold = 0.8;

        /// <summary>
        /// Animation duration for blinking effect in milliseconds.
        /// </summary>
        public const int BlinkAnimationDuration = 500;

        /// <summary>
        /// Minimum opacity for blinking animation.
        /// </summary>
        public const double BlinkMinOpacity = 0.3;

        /// <summary>
        /// Maximum opacity for blinking animation.
        /// </summary>
        public const double BlinkMaxOpacity = 1.0;
    }
}
