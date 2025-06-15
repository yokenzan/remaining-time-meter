// <copyright file="TimerWindow.xaml.cs" company="RemMeter">
// Copyright (c) 2025 RemMeter. Licensed under the MIT License.
// </copyright>

using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using RemMeter.Helpers;
using RemMeter.Models;

namespace RemMeter
{
    /// <summary>
    /// Interaction logic for TimerWindow.xaml.
    /// </summary>
    public partial class TimerWindow : Window, IDisposable
    {
        private DispatcherTimer timer = null!;
        private TimerPosition position;
        private DisplayInfo targetDisplay = null!;
        private int totalSeconds;
        private int remainingSeconds;
        private bool isPaused = false;
        private bool disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerWindow"/> class.
        /// </summary>
        /// <param name="totalSeconds">Total seconds for the timer.</param>
        /// <param name="position">Position for the timer.</param>
        /// <param name="targetDisplay">Target display information.</param>
        public TimerWindow(int totalSeconds, TimerPosition position, DisplayInfo targetDisplay)
        {
            this.InitializeTimer(totalSeconds, position, targetDisplay);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerWindow"/> class.
        /// </summary>
        /// <param name="totalSeconds">Total seconds for the timer.</param>
        /// <param name="positionString">Position string for the timer (for backward compatibility).</param>
        /// <param name="targetDisplay">Target display information.</param>
        public TimerWindow(int totalSeconds, string positionString, DisplayInfo targetDisplay)
        {
            TimerPosition position = PositionMapper.ParsePosition(positionString);
            this.InitializeTimer(totalSeconds, position, targetDisplay);
        }

        /// <summary>
        /// Event raised when the main window should be shown.
        /// </summary>
        public event Action? MainWindowRequested;

        /// <summary>
        /// Releases all resources used by this instance.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.timer?.Stop();
                }

                this.disposed = true;
            }
        }

        /// <summary>
        /// Common initialization logic for both constructors.
        /// </summary>
        /// <param name="totalSeconds">Total seconds for the timer.</param>
        /// <param name="position">Position for the timer.</param>
        /// <param name="targetDisplay">Target display information.</param>
        private void InitializeTimer(int totalSeconds, TimerPosition position, DisplayInfo targetDisplay)
        {
            Logger.Info($"TimerWindow constructor started - totalSeconds: {totalSeconds}, position: {position}");
            try
            {
                this.InitializeComponent();
                Logger.Debug("TimerWindow InitializeComponent completed");
                this.totalSeconds = totalSeconds;
                this.remainingSeconds = totalSeconds;
                this.position = position;
                this.targetDisplay = targetDisplay;
                Logger.Debug($"TimerWindow properties set - position enum: {this.position}, display: {targetDisplay.Width}x{targetDisplay.Height}");

                this.timer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(1),
                };
                this.timer.Tick += this.OnTimerTick;
                Logger.Debug("Timer created and configured");

                this.SetupWindowPosition();
                Logger.Debug("Window position setup completed");
                this.UpdateTimeDisplay();
                Logger.Debug("Time display updated");
                this.UpdateProgressBar();
                Logger.Debug("Progress bar updated");
                this.timer.Start();
                Logger.Info("TimerWindow constructor completed successfully - timer started");
            }
            catch (InvalidOperationException ex)
            {
                Logger.Error("TimerWindow initialization failed - invalid operation", ex);
                throw;
            }
            catch (ArgumentException ex)
            {
                Logger.Error("TimerWindow initialization failed - invalid argument", ex);
                throw;
            }
            catch (System.Windows.Markup.XamlParseException ex)
            {
                Logger.Error("TimerWindow initialization failed - XAML parse error", ex);
                throw;
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                Logger.Error("TimerWindow initialization failed - Windows API error", ex);
                throw;
            }
            catch (OutOfMemoryException)
            {
                // Critical system exception - don't log to avoid potential recursion
                throw;
            }
        }

        /// <summary>
        /// Parses position string to TimerPosition enum using the optimized PositionMapper.
        /// </summary>
        /// <param name="position">Position string.</param>
        /// <returns>TimerPosition enum value.</returns>
        private TimerPosition ParsePosition(string position)
        {
            return PositionMapper.ParsePosition(position);
        }

        /// <summary>
        /// Sets up the window position based on the target display and position.
        /// </summary>
        private void SetupWindowPosition()
        {
            // DPIスケーリングを考慮した座標計算
            var logicalBounds = DisplayHelper.GetLogicalScreenBounds(this.targetDisplay, this);

            var logicalScreenWidth = logicalBounds.Width;
            var logicalScreenHeight = logicalBounds.Height;
            var logicalScreenLeft = logicalBounds.Left;
            var logicalScreenTop = logicalBounds.Top;

            switch (this.position)
            {
                case TimerPosition.Right:
                    this.Width = Constants.TimerBarWidth;
                    this.Height = logicalScreenHeight * Constants.ScreenSizeRatio;
                    this.Left = logicalScreenLeft + logicalScreenWidth - this.Width - Constants.ScreenMargin;
                    this.Top = logicalScreenTop + ((logicalScreenHeight - this.Height) / 2);
                    break;
                case TimerPosition.Left:
                    this.Width = Constants.TimerBarWidth;
                    this.Height = logicalScreenHeight * Constants.ScreenSizeRatio;
                    this.Left = logicalScreenLeft + Constants.ScreenMargin;
                    this.Top = logicalScreenTop + ((logicalScreenHeight - this.Height) / 2);
                    break;
                case TimerPosition.Top:
                    this.Width = logicalScreenWidth * Constants.ScreenSizeRatio;
                    this.Height = Constants.TimerBarHeight;
                    this.Left = logicalScreenLeft + ((logicalScreenWidth - this.Width) / 2);
                    this.Top = logicalScreenTop + Constants.ScreenMargin;
                    break;
                case TimerPosition.Bottom:
                    this.Width = logicalScreenWidth * Constants.ScreenSizeRatio;
                    this.Height = Constants.TimerBarHeight;
                    this.Left = logicalScreenLeft + ((logicalScreenWidth - this.Width) / 2);
                    this.Top = logicalScreenTop + logicalScreenHeight - this.Height - Constants.BottomMargin;
                    break;
            }
        }

        /// <summary>
        /// Handles the timer tick event.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnTimerTick(object? sender, EventArgs e)
        {
            if (this.isPaused)
            {
                return;
            }

            this.remainingSeconds--;
            this.UpdateTimeDisplay();
            this.UpdateProgressBar();
            this.UpdateBarColor();

            if (this.remainingSeconds <= 0)
            {
                this.timer.Stop();
                this.ShowTimeUpNotification();
            }
        }

        /// <summary>
        /// Updates the time display.
        /// </summary>
        private void UpdateTimeDisplay()
        {
            int minutes = this.remainingSeconds / 60;
            int seconds = this.remainingSeconds % 60;
            this.TimeDisplay.Text = $"{minutes}:{seconds:D2}";
        }

        /// <summary>
        /// Updates the progress bar based on remaining time.
        /// </summary>
        private void UpdateProgressBar()
        {
            double progress = (double)(this.totalSeconds - this.remainingSeconds) / this.totalSeconds;

            if (this.position == TimerPosition.Top || this.position == TimerPosition.Bottom)
            {
                // For horizontal orientation, show progress from left to right
                this.ProgressBar.Width = this.Width * progress;
                this.ProgressBar.Height = this.Height; // Set height to full size
                this.ProgressBar.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                this.ProgressBar.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            }
            else
            {
                // For vertical orientation, show progress from bottom to top
                this.ProgressBar.Height = this.Height * progress;
                this.ProgressBar.Width = this.Width; // Set width to full size
                this.ProgressBar.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
                this.ProgressBar.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            }
        }

        /// <summary>
        /// Updates the progress bar color based on progress and pause state.
        /// </summary>
        private void UpdateBarColor()
        {
            // Set to paused color when paused
            if (this.isPaused)
            {
                this.ProgressBar.Fill = (SolidColorBrush)this.FindResource("PausedBrush");
                this.ProgressBar.BeginAnimation(OpacityProperty, null);
                this.ProgressBar.Opacity = 1.0;
                return;
            }

            double progress = (double)(this.totalSeconds - this.remainingSeconds) / this.totalSeconds;

            if (progress >= Constants.RedThreshold)
            {
                this.ProgressBar.Fill = (SolidColorBrush)this.FindResource("DangerBrush");

                // Blinking effect
                var animation = new DoubleAnimation(Constants.BlinkMinOpacity, Constants.BlinkMaxOpacity, TimeSpan.FromMilliseconds(Constants.BlinkAnimationDuration))
                {
                    AutoReverse = true,
                    RepeatBehavior = RepeatBehavior.Forever,
                };
                this.ProgressBar.BeginAnimation(OpacityProperty, animation);
            }
            else if (progress >= Constants.OrangeThreshold)
            {
                this.ProgressBar.Fill = (SolidColorBrush)this.FindResource("WarningBrush");
                this.ProgressBar.BeginAnimation(OpacityProperty, null);
                this.ProgressBar.Opacity = 1.0;
            }
            else
            {
                this.ProgressBar.Fill = (SolidColorBrush)this.FindResource("PrimaryBrush");
                this.ProgressBar.BeginAnimation(OpacityProperty, null);
                this.ProgressBar.Opacity = 1.0;
            }
        }

        /// <summary>
        /// Shows time up notification and closes the window.
        /// </summary>
        private void ShowTimeUpNotification()
        {
            // Calculate elapsed time
            int minutes = this.totalSeconds / 60;
            int seconds = this.totalSeconds % 60;
            string message = string.Format(Properties.Resources.TimeUpMessage, minutes, seconds);

            // Use the new NotificationHelper with automatic resource management
            NotificationHelper.ShowNotificationWithFallback(Properties.Resources.Timer, message);

            this.MainWindowRequested?.Invoke();
            this.Close();
        }

        /// <summary>
        /// Handles mouse enter event to show control panel.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnControlPanelRequested(object sender, System.Windows.Input.MouseEventArgs e)
        {
            // DPIスケーリングを考慮した座標計算
            var logicalBounds = DisplayHelper.GetLogicalScreenBounds(this.targetDisplay, this);

            var logicalScreenWidth = logicalBounds.Width;
            var logicalScreenHeight = logicalBounds.Height;
            var logicalScreenLeft = logicalBounds.Left;
            var logicalScreenTop = logicalBounds.Top;

            double expandedWidth = Constants.ExpandedWidth;
            double expandedHeight = Constants.ExpandedHeight;

            switch (this.position)
            {
                case TimerPosition.Right:
                    this.Width = expandedWidth;
                    this.Height = Math.Max(this.Height, expandedHeight);
                    this.Left = logicalScreenLeft + logicalScreenWidth - expandedWidth - Constants.ScreenMargin;
                    break;
                case TimerPosition.Left:
                    this.Width = expandedWidth;
                    this.Height = Math.Max(this.Height, expandedHeight);
                    this.Left = logicalScreenLeft + Constants.ScreenMargin;
                    break;
                case TimerPosition.Top:
                    this.Width = Math.Max(this.Width, expandedWidth);
                    this.Height = expandedHeight;
                    this.Top = logicalScreenTop + Constants.ScreenMargin;
                    break;
                case TimerPosition.Bottom:
                    this.Width = Math.Max(this.Width, expandedWidth);
                    this.Height = expandedHeight;
                    this.Top = logicalScreenTop + logicalScreenHeight - expandedHeight - Constants.BottomMargin;
                    break;
            }

            this.ControlPanel.Visibility = Visibility.Visible;
            this.TimerBorder.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Handles mouse leave event to hide control panel.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnControlPanelHidden(object sender, System.Windows.Input.MouseEventArgs e)
        {
            // Restore original size when hover ends
            this.SetupWindowPosition();
            this.ControlPanel.Visibility = Visibility.Collapsed;
            this.TimerBorder.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Handles pause/resume button click event.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnPauseResumeToggled(object sender, RoutedEventArgs e)
        {
            Logger.Info($"OnPauseResumeToggled - current state: isPaused={this.isPaused}");
            try
            {
                this.isPaused = !this.isPaused;
                this.PauseResumeButton.Content = this.isPaused ? Properties.Resources.Resume : Properties.Resources.Pause;
                this.UpdateBarColor(); // Update color when pause state changes
                Logger.Info($"OnPauseResumeToggled completed - new state: isPaused={this.isPaused}");
            }
            catch (InvalidOperationException ex)
            {
                Logger.Error("OnPauseResumeToggled failed - invalid operation", ex);
            }
            catch (ArgumentException ex)
            {
                Logger.Error("OnPauseResumeToggled failed - invalid argument", ex);
            }
        }

        /// <summary>
        /// Handles stop button click event.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnTimerStopped(object sender, RoutedEventArgs e)
        {
            Logger.Info("OnTimerStopped started");
            try
            {
                this.timer.Stop();
                Logger.Debug("Timer stopped");
                this.MainWindowRequested?.Invoke();
                Logger.Debug("MainWindowRequested event invoked");
                this.Close();
                Logger.Info("OnTimerStopped completed - window closed");
            }
            catch (InvalidOperationException ex)
            {
                Logger.Error("OnTimerStopped failed - invalid operation", ex);
            }
            catch (ArgumentException ex)
            {
                Logger.Error("OnTimerStopped failed - invalid argument", ex);
            }
        }
    }
}
