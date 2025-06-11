// <copyright file="App.xaml.cs" company="RemainingTimeMeter">
// Copyright (c) 2025 RemainingTimeMeter. Licensed under the MIT License.
// </copyright>

using System.Windows;

namespace RemainingTimeMeter
{
    /// <summary>
    /// Application entry point.
    /// </summary>
    public partial class App : System.Windows.Application
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        public App()
        {
            try
            {
                Logger.Info("Application starting up");
            }
            catch
            {
                // Ignore logging errors during startup
            }

            this.Startup += this.App_Startup;
            this.Exit += this.App_Exit;
        }

        /// <summary>
        /// Handles application startup event.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void App_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                Logger.Info($"Application startup completed. Log file: {Logger.LogFile}");
            }
            catch
            {
                // Ignore logging errors
            }
        }

        /// <summary>
        /// Handles application exit event.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void App_Exit(object sender, ExitEventArgs e)
        {
            try
            {
                Logger.Info($"Application exiting with code: {e.ApplicationExitCode}");
            }
            catch
            {
                // Ignore logging errors
            }
        }
    }
}
