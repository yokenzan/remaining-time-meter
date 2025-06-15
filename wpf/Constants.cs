// <copyright file="Constants.cs" company="RemMeter">
// Copyright (c) 2025 RemMeter. Licensed under the MIT License.
// </copyright>

namespace RemMeter
{
    /// <summary>
    /// Application constants organized by functionality.
    /// </summary>
    public static class Constants
    {
        // Legacy property accessors for backward compatibility

        /// <summary>
        /// Gets the timer bar width for vertical positioning.
        /// </summary>
        public static double TimerBarWidth => Layout.TimerBarWidth;

        /// <summary>
        /// Gets the timer bar height for horizontal positioning.
        /// </summary>
        public static double TimerBarHeight => Layout.TimerBarHeight;

        /// <summary>
        /// Gets the screen size ratio for timer bar length.
        /// </summary>
        public static double ScreenSizeRatio => Layout.ScreenSizeRatio;

        /// <summary>
        /// Gets the margin from screen edge.
        /// </summary>
        public static double ScreenMargin => Layout.ScreenMargin;

        /// <summary>
        /// Gets the bottom margin for bottom positioning.
        /// </summary>
        public static double BottomMargin => Layout.BottomMargin;

        /// <summary>
        /// Gets the expanded width for control panel.
        /// </summary>
        public static double ExpandedWidth => Layout.ExpandedWidth;

        /// <summary>
        /// Gets the expanded height for control panel.
        /// </summary>
        public static double ExpandedHeight => Layout.ExpandedHeight;

        /// <summary>
        /// Gets the notification display duration in milliseconds.
        /// </summary>
        public static int NotificationDuration => Notification.Duration;

        /// <summary>
        /// Gets the notification cleanup delay in milliseconds.
        /// </summary>
        public static int NotificationCleanupDelay => Notification.CleanupDelay;

        /// <summary>
        /// Gets the progress threshold for orange color.
        /// </summary>
        public static double OrangeThreshold => Colors.OrangeThreshold;

        /// <summary>
        /// Gets the progress threshold for red color.
        /// </summary>
        public static double RedThreshold => Colors.RedThreshold;

        /// <summary>
        /// Gets the animation duration for blinking effect in milliseconds.
        /// </summary>
        public static int BlinkAnimationDuration => Animation.BlinkAnimationDuration;

        /// <summary>
        /// Gets the minimum opacity for blinking animation.
        /// </summary>
        public static double BlinkMinOpacity => Animation.BlinkMinOpacity;

        /// <summary>
        /// Gets the maximum opacity for blinking animation.
        /// </summary>
        public static double BlinkMaxOpacity => Animation.BlinkMaxOpacity;

        /// <summary>
        /// Layout and positioning constants.
        /// </summary>
        public static class Layout
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
            public const double ExpandedWidth = 120;

            /// <summary>
            /// Expanded height for control panel.
            /// </summary>
            public const double ExpandedHeight = 140;
        }

        /// <summary>
        /// Animation and visual effect constants.
        /// </summary>
        public static class Animation
        {
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

        /// <summary>
        /// Notification system constants.
        /// </summary>
        public static class Notification
        {
            /// <summary>
            /// Notification display duration in milliseconds.
            /// </summary>
            public const int Duration = 5000;

            /// <summary>
            /// Notification cleanup delay in milliseconds.
            /// </summary>
            public const int CleanupDelay = 6000;
        }

        /// <summary>
        /// Color threshold constants.
        /// </summary>
        public static class Colors
        {
            /// <summary>
            /// Progress threshold for orange color.
            /// </summary>
            public const double OrangeThreshold = 0.6;

            /// <summary>
            /// Progress threshold for red color.
            /// </summary>
            public const double RedThreshold = 0.8;
        }
    }
}
