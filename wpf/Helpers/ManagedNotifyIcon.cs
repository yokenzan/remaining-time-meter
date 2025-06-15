// <copyright file="ManagedNotifyIcon.cs" company="RemMeter">
// Copyright (c) 2025 RemMeter. Licensed under the MIT License.
// </copyright>

using System;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace RemMeter.Helpers
{
    /// <summary>
    /// Manages NotifyIcon resources with proper disposal pattern.
    /// </summary>
    internal sealed class ManagedNotifyIcon : IDisposable
    {
        private readonly System.Windows.Forms.NotifyIcon notifyIcon;
        private readonly DispatcherTimer cleanupTimer;
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManagedNotifyIcon"/> class.
        /// </summary>
        /// <param name="title">The notification title.</param>
        /// <param name="message">The notification message.</param>
        public ManagedNotifyIcon(string title, string message)
        {
            this.notifyIcon = new System.Windows.Forms.NotifyIcon
            {
                Icon = System.Drawing.SystemIcons.Information,
                Visible = true,
            };

            this.cleanupTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(Constants.NotificationCleanupDelay),
            };

            this.cleanupTimer.Tick += this.OnCleanupTimerTick;
            this.Title = title;
            this.Message = message;
        }

        /// <summary>
        /// Gets the notification title.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Gets the notification message.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Shows the notification asynchronously.
        /// </summary>
        /// <returns>A task that completes when the notification display process is initiated.</returns>
        public async Task ShowAsync()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(nameof(ManagedNotifyIcon));
            }

            try
            {
                // Show the notification
                this.notifyIcon.ShowBalloonTip(
                    Constants.NotificationDuration,
                    this.Title,
                    this.Message,
                    System.Windows.Forms.ToolTipIcon.Info);

                // Start cleanup timer
                this.cleanupTimer.Start();

                // Return completed task since ShowBalloonTip is not truly async
                await Task.CompletedTask;
            }
            catch
            {
                // Ensure cleanup on any exception
                this.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Releases all resources used by this instance.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Handles the cleanup timer tick event.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnCleanupTimerTick(object? sender, EventArgs e)
        {
            this.Dispose();
        }

        /// <summary>
        /// Releases the unmanaged resources and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.cleanupTimer?.Stop();

                    if (this.notifyIcon != null)
                    {
                        this.notifyIcon.Visible = false;
                        this.notifyIcon.Dispose();
                    }
                }

                this.disposed = true;
            }
        }
    }
}
