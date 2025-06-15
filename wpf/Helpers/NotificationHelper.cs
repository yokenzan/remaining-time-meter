// <copyright file="NotificationHelper.cs" company="RemMeter">
// Copyright (c) 2025 RemMeter. Licensed under the MIT License.
// </copyright>

using System;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace RemMeter.Helpers
{
    /// <summary>
    /// Provides helper methods for system notifications with proper resource management.
    /// </summary>
    public static class NotificationHelper
    {
        /// <summary>
        /// Shows a system notification with automatic resource cleanup.
        /// </summary>
        /// <param name="title">The notification title.</param>
        /// <param name="message">The notification message.</param>
        /// <returns>A task that completes when the notification is shown.</returns>
        public static async Task ShowNotificationAsync(string title, string message)
        {
            using var notification = new ManagedNotifyIcon(title, message);
            await notification.ShowAsync();
        }

        /// <summary>
        /// Shows a system notification with fallback to message box on failure.
        /// </summary>
        /// <param name="title">The notification title.</param>
        /// <param name="message">The notification message.</param>
        public static void ShowNotificationWithFallback(string title, string message)
        {
            try
            {
                // Use async method but don't await to avoid blocking UI
                _ = ShowNotificationAsync(title, message);
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                Logger.Debug($"Notification failed due to Windows API error: {ex.Message} - using message box fallback");
                ShowMessageBoxFallback(title, message);
            }
            catch (InvalidOperationException ex)
            {
                Logger.Debug($"Notification failed due to invalid operation: {ex.Message} - using message box fallback");
                ShowMessageBoxFallback(title, message);
            }
            catch (UnauthorizedAccessException ex)
            {
                Logger.Debug($"Notification failed due to access denied: {ex.Message} - using message box fallback");
                ShowMessageBoxFallback(title, message);
            }
            catch (Exception ex)
            {
                Logger.Error("Unexpected error showing notification", ex);
                ShowMessageBoxFallback(title, message);
            }
        }

        /// <summary>
        /// Shows a message box as fallback when system notifications fail.
        /// </summary>
        /// <param name="title">The message box title.</param>
        /// <param name="message">The message box content.</param>
        private static void ShowMessageBoxFallback(string title, string message)
        {
            try
            {
                System.Windows.MessageBox.Show(message, title, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                Logger.Error("Failed to show fallback message box", ex);
            }
        }
    }
}