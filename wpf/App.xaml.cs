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
            Logger.Info("Application starting up");
            this.Startup += this.OnApplicationStartup;
            this.Exit += this.OnApplicationExit;
        }

        /// <summary>
        /// Handles application startup event.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnApplicationStartup(object sender, StartupEventArgs e)
        {
            Logger.Info($"Application startup completed. Log file: {Logger.LogFile}");
        }

        /// <summary>
        /// Handles application exit event.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnApplicationExit(object sender, ExitEventArgs e)
        {
            Logger.Info($"Application exiting with code: {e.ApplicationExitCode}");
        }
    }
}
