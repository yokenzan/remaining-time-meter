// <copyright file="App.xaml.cs" company="RemMeter">
// Copyright (c) 2025 RemMeter. Licensed under the MIT License.
// </copyright>

using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;

namespace RemMeter
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

            // Check for language override options
            this.SetLanguageFromParameters(e.Args);
        }

        /// <summary>
        /// Sets the application language based on command-line arguments or environment variables.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        private void SetLanguageFromParameters(string[] args)
        {
            try
            {
                string? languageCode = null;

                // Check command-line arguments for --lang parameter
                for (int i = 0; i < args.Length - 1; i++)
                {
                    if (args[i].Equals("--lang", StringComparison.OrdinalIgnoreCase) ||
                        args[i].Equals("-l", StringComparison.OrdinalIgnoreCase))
                    {
                        languageCode = args[i + 1];
                        break;
                    }
                }

                // Check environment variable if no command-line argument
                if (string.IsNullOrEmpty(languageCode))
                {
                    languageCode = Environment.GetEnvironmentVariable("TIMER_LANG");
                }

                // Apply language if specified and valid
                if (!string.IsNullOrEmpty(languageCode))
                {
                    this.SetApplicationLanguage(languageCode);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Failed to set language from parameters", ex);
            }
        }

        /// <summary>
        /// Sets the application language to the specified culture code.
        /// </summary>
        /// <param name="cultureCode">The culture code (e.g., "en-US", "ja-JP", "zh-CN", "zh-TW").</param>
        private void SetApplicationLanguage(string cultureCode)
        {
            try
            {
                // Validate supported languages
                var supportedLanguages = new[] { "en-US", "ja-JP", "zh-CN", "zh-TW" };
                if (!supportedLanguages.Contains(cultureCode, StringComparer.OrdinalIgnoreCase))
                {
                    Logger.Error($"Unsupported language code: {cultureCode}. Supported: {string.Join(", ", supportedLanguages)}");
                    return;
                }

                // Set the culture for the application
                var culture = new CultureInfo(cultureCode);
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;
                CultureInfo.DefaultThreadCurrentCulture = culture;
                CultureInfo.DefaultThreadCurrentUICulture = culture;

                // Update resources
                RemMeter.Properties.Resources.Culture = culture;

                Logger.Info($"Application language set to: {cultureCode}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to set application language to {cultureCode}", ex);
            }
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
