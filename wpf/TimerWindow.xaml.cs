// <copyright file="TimerWindow.xaml.cs" company="RemainingTimeMeter">
// Copyright (c) 2025 RemainingTimeMeter. Licensed under the MIT License.
// </copyright>

using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using RemainingTimeMeter.Models;

namespace RemainingTimeMeter
{
    /// <summary>
    /// Interaction logic for TimerWindow.xaml.
    /// </summary>
    public partial class TimerWindow : Window, IDisposable
    {
        private readonly DispatcherTimer timer;
        private readonly TimerPosition position;
        private readonly DisplayInfo targetDisplay;
        private int totalSeconds;
        private int remainingSeconds;
        private bool isPaused = false;
        private bool disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerWindow"/> class.
        /// </summary>
        /// <param name="totalSeconds">Total seconds for the timer.</param>
        /// <param name="position">Position string for the timer.</param>
        /// <param name="targetDisplay">Target display information.</param>
        public TimerWindow(int totalSeconds, string position, DisplayInfo targetDisplay)
        {
            Logger.Info($"TimerWindow constructor started - totalSeconds: {totalSeconds}, position: {position}");
            try
            {
                this.InitializeComponent();
                Logger.Debug("TimerWindow InitializeComponent completed");
                this.totalSeconds = totalSeconds;
                this.remainingSeconds = totalSeconds;
                this.position = this.ParsePosition(position);
                this.targetDisplay = targetDisplay;
                Logger.Debug($"TimerWindow properties set - position enum: {this.position}, display: {targetDisplay.Width}x{targetDisplay.Height}");

                this.timer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(1),
                };
                this.timer.Tick += this.Timer_Tick;
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
            catch (Exception ex)
            {
                Logger.Error("TimerWindow constructor failed", ex);
                throw;
            }
        }

        /// <summary>
        /// Event triggered when main window is requested.
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
        /// Parses position string to TimerPosition enum.
        /// </summary>
        /// <param name="position">Position string.</param>
        /// <returns>TimerPosition enum value.</returns>
        private TimerPosition ParsePosition(string position)
        {
            return position switch
            {
                "右端" => TimerPosition.Right,
                "左端" => TimerPosition.Left,
                "上端" => TimerPosition.Top,
                "下端" => TimerPosition.Bottom,
                _ => TimerPosition.Right,
            };
        }

        /// <summary>
        /// Sets up the window position based on the target display and position.
        /// </summary>
        private void SetupWindowPosition()
        {
            // DPIスケーリングを考慮した座標計算
            var dpiScale = System.Windows.Media.VisualTreeHelper.GetDpi(this);

            // 物理ピクセルでのスクリーン情報
            var screenWidth = this.targetDisplay.Width;
            var screenHeight = this.targetDisplay.Height;
            var screenLeft = this.targetDisplay.Left;
            var screenTop = this.targetDisplay.Top;

            // WPFのロジカルピクセルに変換
            var logicalScreenWidth = screenWidth / dpiScale.DpiScaleX;
            var logicalScreenHeight = screenHeight / dpiScale.DpiScaleY;
            var logicalScreenLeft = screenLeft / dpiScale.DpiScaleX;
            var logicalScreenTop = screenTop / dpiScale.DpiScaleY;

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
        private void Timer_Tick(object? sender, EventArgs e)
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
                // 横方向の場合は左から右に向かって進捗を表示
                this.ProgressBar.Width = this.Width * progress;
                this.ProgressBar.Height = this.Height; // 高さは全体に設定
                this.ProgressBar.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                this.ProgressBar.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            }
            else
            {
                // 縦方向の場合は下から上に向かって進捗を表示
                this.ProgressBar.Height = this.Height * progress;
                this.ProgressBar.Width = this.Width; // 幅は全体に設定
                this.ProgressBar.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
                this.ProgressBar.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            }
        }

        /// <summary>
        /// Updates the progress bar color based on progress and pause state.
        /// </summary>
        private void UpdateBarColor()
        {
            // 一時停止中は darkslateblue に設定
            if (this.isPaused)
            {
                this.ProgressBar.Fill = new SolidColorBrush(Colors.DarkSlateBlue);
                this.ProgressBar.BeginAnimation(OpacityProperty, null);
                this.ProgressBar.Opacity = 1.0;
                return;
            }

            double progress = (double)(this.totalSeconds - this.remainingSeconds) / this.totalSeconds;

            if (progress >= Constants.RedThreshold)
            {
                this.ProgressBar.Fill = new SolidColorBrush(Colors.Red);

                // 点滅効果
                var animation = new DoubleAnimation(Constants.BlinkMinOpacity, Constants.BlinkMaxOpacity, TimeSpan.FromMilliseconds(Constants.BlinkAnimationDuration))
                {
                    AutoReverse = true,
                    RepeatBehavior = RepeatBehavior.Forever,
                };
                this.ProgressBar.BeginAnimation(OpacityProperty, animation);
            }
            else if (progress >= Constants.OrangeThreshold)
            {
                this.ProgressBar.Fill = new SolidColorBrush(Colors.Orange);
                this.ProgressBar.BeginAnimation(OpacityProperty, null);
                this.ProgressBar.Opacity = 1.0;
            }
            else
            {
                this.ProgressBar.Fill = new SolidColorBrush(Colors.Green);
                this.ProgressBar.BeginAnimation(OpacityProperty, null);
                this.ProgressBar.Opacity = 1.0;
            }
        }

        /// <summary>
        /// Shows time up notification and closes the window.
        /// </summary>
        private void ShowTimeUpNotification()
        {
            // 経過時間を計算
            int minutes = this.totalSeconds / 60;
            int seconds = this.totalSeconds % 60;
            string message = $"時間です！{minutes}分{seconds}秒経過しました！";

            try
            {
                // Windows 10/11のシステム通知を使用
                this.ShowWindowsNotification("タイマー", message);
            }
            catch
            {
                // フォールバックとしてメッセージボックスを表示
                System.Windows.MessageBox.Show(message, "タイマー", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            this.MainWindowRequested?.Invoke();
            this.Close();
        }

        /// <summary>
        /// Shows Windows system notification using shell_notifyicon.
        /// </summary>
        /// <param name="title">Notification title.</param>
        /// <param name="message">Notification message.</param>
        private void ShowWindowsNotification(string title, string message)
        {
            // NotifyIconを使用してシステム通知を表示
            var notifyIcon = new System.Windows.Forms.NotifyIcon();
            try
            {
                notifyIcon.Icon = System.Drawing.SystemIcons.Information;
                notifyIcon.Visible = true;
                notifyIcon.ShowBalloonTip(Constants.NotificationDuration, title, message, System.Windows.Forms.ToolTipIcon.Info);

                // 通知表示後に自動的にアイコンを非表示にする
                var timer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromMilliseconds(Constants.NotificationCleanupDelay),
                };
                timer.Tick += (s, e) =>
                {
                    timer.Stop();
                    notifyIcon.Visible = false;
                    notifyIcon.Dispose();
                };
                timer.Start();
            }
            catch
            {
                notifyIcon?.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Handles mouse enter event to show control panel.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void Window_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            // DPIスケーリングを考慮した座標計算
            var dpiScale = System.Windows.Media.VisualTreeHelper.GetDpi(this);

            var screenWidth = this.targetDisplay.Width;
            var screenHeight = this.targetDisplay.Height;
            var screenLeft = this.targetDisplay.Left;
            var screenTop = this.targetDisplay.Top;

            // WPFのロジカルピクセルに変換
            var logicalScreenWidth = screenWidth / dpiScale.DpiScaleX;
            var logicalScreenHeight = screenHeight / dpiScale.DpiScaleY;
            var logicalScreenLeft = screenLeft / dpiScale.DpiScaleX;
            var logicalScreenTop = screenTop / dpiScale.DpiScaleY;

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
        private void Window_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            // ホバー終了時に元のサイズに戻す
            this.SetupWindowPosition();
            this.ControlPanel.Visibility = Visibility.Collapsed;
            this.TimerBorder.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Handles pause/resume button click event.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void PauseResumeButton_Click(object sender, RoutedEventArgs e)
        {
            Logger.Info($"PauseResumeButton_Click - current state: isPaused={this.isPaused}");
            try
            {
                this.isPaused = !this.isPaused;
                this.PauseResumeButton.Content = this.isPaused ? "再開" : "一時停止";
                this.UpdateBarColor(); // 一時停止状態の変更時に色を更新
                Logger.Info($"PauseResumeButton_Click completed - new state: isPaused={this.isPaused}");
            }
            catch (Exception ex)
            {
                Logger.Error("PauseResumeButton_Click failed", ex);
            }
        }

        /// <summary>
        /// Handles stop button click event.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            Logger.Info("StopButton_Click started");
            try
            {
                this.timer.Stop();
                Logger.Debug("Timer stopped");
                this.MainWindowRequested?.Invoke();
                Logger.Debug("MainWindowRequested event invoked");
                this.Close();
                Logger.Info("StopButton_Click completed - window closed");
            }
            catch (Exception ex)
            {
                Logger.Error("StopButton_Click failed", ex);
            }
        }

        /// <summary>
        /// Handles close button click event.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Logger.Info("CloseButton_Click started");
            try
            {
                this.timer.Stop();
                Logger.Debug("Timer stopped");
                this.MainWindowRequested?.Invoke();
                Logger.Debug("MainWindowRequested event invoked");
                this.Close();
                Logger.Info("CloseButton_Click completed - window closed");
            }
            catch (Exception ex)
            {
                Logger.Error("CloseButton_Click failed", ex);
            }
        }
    }
}
