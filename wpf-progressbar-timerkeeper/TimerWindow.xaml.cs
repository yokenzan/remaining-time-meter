// <copyright file="TimerWindow.xaml.cs" company="MeterTimeKeeper">
// Copyright (c) MeterTimeKeeper. All rights reserved.
// </copyright>

using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using ProgressBarTimerKeeper.Models;

namespace ProgressBarTimerKeeper
{
    /// <summary>
    /// Interaction logic for TimerWindow.xaml.
    /// </summary>
    public partial class TimerWindow : Window
    {
        private readonly DispatcherTimer timer;
        private readonly TimerPosition position;
        private readonly DisplayInfo targetDisplay;
        private int totalSeconds;
        private int remainingSeconds;
        private bool isPaused = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerWindow"/> class.
        /// </summary>
        /// <param name="totalSeconds">Total seconds for the timer.</param>
        /// <param name="position">Position string for the timer.</param>
        /// <param name="targetDisplay">Target display information.</param>
        public TimerWindow(int totalSeconds, string position, DisplayInfo targetDisplay)
        {
            this.InitializeComponent();
            this.totalSeconds = totalSeconds;
            this.remainingSeconds = totalSeconds;
            this.position = this.ParsePosition(position);
            this.targetDisplay = targetDisplay;

            this.timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1),
            };
            this.timer.Tick += this.Timer_Tick;

            this.SetupWindowPosition();
            this.UpdateTimeDisplay();
            this.UpdateProgressBar();
            this.timer.Start();
        }

        /// <summary>
        /// Event triggered when main window is requested.
        /// </summary>
        public event Action? MainWindowRequested;

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
                    this.Width = 20;
                    this.Height = logicalScreenHeight * 0.8;
                    this.Left = logicalScreenLeft + logicalScreenWidth - this.Width - 10;
                    this.Top = logicalScreenTop + ((logicalScreenHeight - this.Height) / 2);
                    break;
                case TimerPosition.Left:
                    this.Width = 20;
                    this.Height = logicalScreenHeight * 0.8;
                    this.Left = logicalScreenLeft + 10;
                    this.Top = logicalScreenTop + ((logicalScreenHeight - this.Height) / 2);
                    break;
                case TimerPosition.Top:
                    this.Width = logicalScreenWidth * 0.8;
                    this.Height = 20;
                    this.Left = logicalScreenLeft + ((logicalScreenWidth - this.Width) / 2);
                    this.Top = logicalScreenTop + 10;
                    break;
                case TimerPosition.Bottom:
                    this.Width = logicalScreenWidth * 0.8;
                    this.Height = 20;
                    this.Left = logicalScreenLeft + ((logicalScreenWidth - this.Width) / 2);
                    this.Top = logicalScreenTop + logicalScreenHeight - this.Height - 50;
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

            if (progress >= 0.8)
            {
                this.ProgressBar.Fill = new SolidColorBrush(Colors.Red);

                // 点滅効果
                var animation = new DoubleAnimation(0.3, 1.0, TimeSpan.FromMilliseconds(500))
                {
                    AutoReverse = true,
                    RepeatBehavior = RepeatBehavior.Forever,
                };
                this.ProgressBar.BeginAnimation(OpacityProperty, animation);
            }
            else if (progress >= 0.6)
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
                notifyIcon.ShowBalloonTip(5000, title, message, System.Windows.Forms.ToolTipIcon.Info);

                // 通知表示後に自動的にアイコンを非表示にする
                System.Threading.Tasks.Task.Run(async () =>
                {
                    await System.Threading.Tasks.Task.Delay(6000);
                    this.Dispatcher.Invoke(() =>
                    {
                        notifyIcon.Visible = false;
                        notifyIcon.Dispose();
                    });
                });
            }
            catch
            {
                notifyIcon.Dispose();
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

            double expandedWidth = 200;
            double expandedHeight = 150;

            switch (this.position)
            {
                case TimerPosition.Right:
                    this.Width = expandedWidth;
                    this.Height = Math.Max(this.Height, expandedHeight);
                    this.Left = logicalScreenLeft + logicalScreenWidth - expandedWidth - 10;
                    break;
                case TimerPosition.Left:
                    this.Width = expandedWidth;
                    this.Height = Math.Max(this.Height, expandedHeight);
                    this.Left = logicalScreenLeft + 10;
                    break;
                case TimerPosition.Top:
                    this.Width = Math.Max(this.Width, expandedWidth);
                    this.Height = expandedHeight;
                    this.Top = logicalScreenTop + 10;
                    break;
                case TimerPosition.Bottom:
                    this.Width = Math.Max(this.Width, expandedWidth);
                    this.Height = expandedHeight;
                    this.Top = logicalScreenTop + logicalScreenHeight - expandedHeight - 50;
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
            this.isPaused = !this.isPaused;
            this.PauseResumeButton.Content = this.isPaused ? "再開" : "一時停止";
            this.UpdateBarColor(); // 一時停止状態の変更時に色を更新
        }

        /// <summary>
        /// Handles stop button click event.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            this.timer.Stop();
            this.MainWindowRequested?.Invoke();
            this.Close();
        }

        /// <summary>
        /// Handles close button click event.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.timer.Stop();
            this.MainWindowRequested?.Invoke();
            this.Close();
        }
    }
}
