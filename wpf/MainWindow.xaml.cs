// <copyright file="MainWindow.xaml.cs" company="RemainingTimeMeter">
// Copyright (c) 2025 RemainingTimeMeter. Licensed under the MIT License.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using RemainingTimeMeter.Models;

namespace RemainingTimeMeter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// The currently selected position for the timer display.
        /// </summary>
        private string selectedPosition = "Right";

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            Logger.Info("MainWindow constructor started");
            try
            {
                this.InitializeComponent();
                Logger.Debug("InitializeComponent completed");
                this.LoadDisplays();
                Logger.Debug("LoadDisplays completed");

                // Initialize time display after UI is fully loaded
                this.Loaded += (s, e) =>
                {
                    this.UpdateTimeDisplay();
                    this.InitializeQuickTimeButtons();
                };

                Logger.Info("MainWindow constructor completed successfully");
            }
            catch (InvalidOperationException ex)
            {
                Logger.Error("MainWindow initialization failed - invalid operation", ex);
                throw;
            }
            catch (ArgumentException ex)
            {
                Logger.Error("MainWindow initialization failed - invalid argument", ex);
                throw;
            }
            catch (System.Windows.Markup.XamlParseException ex)
            {
                Logger.Error("MainWindow initialization failed - XAML parse error", ex);
                throw;
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                Logger.Error("MainWindow initialization failed - Windows API error", ex);
                throw;
            }
            catch (OutOfMemoryException)
            {
                // Critical system exception - don't log to avoid potential recursion
                throw;
            }
        }

        /// <summary>
        /// Loads display information into the display combo box.
        /// </summary>
        private void LoadDisplays()
        {
            Logger.Debug("LoadDisplays started");
            try
            {
                this.DisplayComboBox.Items.Clear();
                Logger.Debug("DisplayComboBox cleared");

                var displayInfos = this.GetDisplayInformation();
                Logger.Debug($"Found {displayInfos.Count} displays");

                for (int i = 0; i < displayInfos.Count; i++)
                {
                    var display = displayInfos[i];
                    string displayName;

                    if (display.IsPrimary)
                    {
                        displayName = string.Format(Properties.Resources.DisplayFormatPrimary, i + 1, display.Width, display.Height);
                        Logger.Debug($"Primary display found: {displayName}");
                    }
                    else
                    {
                        displayName = string.Format(Properties.Resources.DisplayFormat, i + 1, display.Width, display.Height);
                        Logger.Debug($"Secondary display found: {displayName}");
                    }

                    var item = new ComboBoxItem
                    {
                        Content = displayName,
                        Tag = display,
                    };

                    this.DisplayComboBox.Items.Add(item);

                    // Select primary display as default
                    if (display.IsPrimary)
                    {
                        this.DisplayComboBox.SelectedItem = item;
                        Logger.Debug("Primary display set as selected");
                    }
                }

                Logger.Debug("LoadDisplays completed successfully");
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                Logger.Error("Failed to enumerate displays - Windows API error", ex);

                // Create a default display entry to allow application to continue
                this.CreateDefaultDisplayEntry();
            }
            catch (InvalidOperationException ex)
            {
                Logger.Error("Failed to populate display list - invalid operation", ex);
                this.CreateDefaultDisplayEntry();
            }
            catch (ArgumentException ex)
            {
                Logger.Error("Failed to create display entries - invalid argument", ex);
                this.CreateDefaultDisplayEntry();
            }
        }

        /// <summary>
        /// Creates a default display entry when display enumeration fails.
        /// </summary>
        private void CreateDefaultDisplayEntry()
        {
            try
            {
                // Create a default display based on primary screen
                var defaultDisplay = new DisplayInfo
                {
                    Width = 1920,
                    Height = 1080,
                    Left = 0,
                    Top = 0,
                    IsPrimary = true,
                    ScaleX = 1.0,
                    ScaleY = 1.0,
                };

                var item = new ComboBoxItem
                {
                    Content = "Default Display - 1920x1080",
                    Tag = defaultDisplay,
                };

                this.DisplayComboBox.Items.Add(item);
                this.DisplayComboBox.SelectedIndex = 0;
                Logger.Info("Created default display entry as fallback");
            }
            catch (Exception ex)
            {
                Logger.Error("Failed to create default display entry", ex);

                // If even this fails, the application will have to handle it gracefully
            }
        }

        /// <summary>
        /// Gets information about all available displays.
        /// </summary>
        /// <returns>A list of display information.</returns>
        private List<DisplayInfo> GetDisplayInformation()
        {
            Logger.Debug("GetDisplayInformation started");
            var displays = new List<DisplayInfo>();

            foreach (System.Windows.Forms.Screen screen in System.Windows.Forms.Screen.AllScreens)
            {
                Logger.Debug($"Processing screen: {screen.DeviceName}, Primary: {screen.Primary}, Bounds: {screen.Bounds}");

                // Get DPI information - use default values if window is not fully initialized
                double scaleX = 1.0;
                double scaleY = 1.0;

                try
                {
                    var source = PresentationSource.FromVisual(this);
                    if (source?.CompositionTarget != null)
                    {
                        var matrix = source.CompositionTarget.TransformToDevice;
                        scaleX = matrix.M11;
                        scaleY = matrix.M22;
                        Logger.Debug($"DPI scale obtained: X={scaleX}, Y={scaleY}");
                    }
                    else
                    {
                        Logger.Debug("PresentationSource is null or CompositionTarget is null - using default DPI scale");
                    }
                }
                catch (Exception ex)
                {
                    Logger.Debug($"Exception getting DPI scale: {ex.Message} - using default DPI scale");
                }

                var displayInfo = new DisplayInfo
                {
                    Left = screen.Bounds.Left,
                    Top = screen.Bounds.Top,
                    Width = screen.Bounds.Width,
                    Height = screen.Bounds.Height,
                    ScaleX = scaleX,
                    ScaleY = scaleY,
                    IsPrimary = screen.Primary,
                };

                displays.Add(displayInfo);
                Logger.Debug($"Added display: {displayInfo.Width}x{displayInfo.Height} at ({displayInfo.Left}, {displayInfo.Top})");
            }

            Logger.Debug($"GetDisplayInformation completed with {displays.Count} displays");
            return displays;
        }

        /// <summary>
        /// Handles the start button click event.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            Logger.Info("StartButton_Click started");
            try
            {
                // Get time from enhanced input
                var (minutes, seconds) = this.GetCurrentTime();
                Logger.Debug($"Parsed time values - Minutes: {minutes}, Seconds: {seconds}");

                // Calculate total time in seconds
                int totalSeconds = (minutes * 60) + seconds;
                Logger.Debug($"Calculated total seconds: {totalSeconds}");
                if (totalSeconds <= 0)
                {
                    Logger.Debug("Total seconds is zero or negative");
                    System.Windows.MessageBox.Show(Properties.Resources.PleaseSetTimeCorrectly, Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Get position setting
                string position = this.selectedPosition;
                Logger.Debug($"Selected position: {position}");

                // Get selected display
                var selectedDisplayItem = (ComboBoxItem)this.DisplayComboBox.SelectedItem;
                var selectedDisplay = (DisplayInfo)selectedDisplayItem.Tag;
                Logger.Debug($"Selected display: {selectedDisplay.Width}x{selectedDisplay.Height} at ({selectedDisplay.Left}, {selectedDisplay.Top}), Primary: {selectedDisplay.IsPrimary}");

                // Create and show timer window
                Logger.Debug("Creating TimerWindow");
                var timerWindow = new TimerWindow(totalSeconds, position, selectedDisplay);
                timerWindow.MainWindowRequested += () =>
                {
                    Logger.Debug("MainWindow show requested from TimerWindow");
                    this.Show();
                };
                Logger.Debug("Showing TimerWindow");
                timerWindow.Show();

                // Hide main window
                Logger.Debug("Hiding MainWindow");
                this.Hide();

                Logger.Info("StartButton_Click completed successfully");
            }
            catch (ArgumentException ex)
            {
                Logger.Error("StartButton_Click failed - invalid argument", ex);
                System.Windows.MessageBox.Show(string.Format(Properties.Resources.ErrorOccurred, ex.Message), Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException ex)
            {
                Logger.Error("StartButton_Click failed - invalid operation", ex);
                System.Windows.MessageBox.Show(string.Format(Properties.Resources.ErrorOccurred, ex.Message), Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                Logger.Error("StartButton_Click failed - Windows API error", ex);
                System.Windows.MessageBox.Show(string.Format(Properties.Resources.ErrorOccurred, ex.Message), Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Handles the GotFocus event for textboxes to select all text.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            Logger.Debug("TextBox_GotFocus started");
            try
            {
                if (sender is System.Windows.Controls.TextBox textBox)
                {
                    textBox.SelectAll();
                    Logger.Debug($"Selected all text in textbox: {textBox.Name}");
                }
            }
            catch (InvalidOperationException ex)
            {
                Logger.Error("TextBox_GotFocus failed - invalid operation", ex);
            }
            catch (ArgumentException ex)
            {
                Logger.Error("TextBox_GotFocus failed - invalid argument", ex);
            }
        }

        /// <summary>
        /// Handles the PreviewMouseLeftButtonDown event for textboxes to ensure proper focus and selection behavior.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void TextBox_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Logger.Debug("TextBox_PreviewMouseLeftButtonDown started");
            try
            {
                if (sender is System.Windows.Controls.TextBox textBox)
                {
                    if (!textBox.IsKeyboardFocusWithin)
                    {
                        textBox.Focus();
                        e.Handled = true;
                        Logger.Debug($"Focused textbox: {textBox.Name}");
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                Logger.Error("TextBox_PreviewMouseLeftButtonDown failed - invalid operation", ex);
            }
            catch (ArgumentException ex)
            {
                Logger.Error("TextBox_PreviewMouseLeftButtonDown failed - invalid argument", ex);
            }
        }

        /// <summary>
        /// Handles the TextChanged event for the time input textbox.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void TimeInputTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            // Only update if the window is fully loaded to avoid initialization issues
            if (this.IsLoaded)
            {
                this.UpdateTimeDisplay();
            }
        }

        /// <summary>
        /// Handles quick time button clicks.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void QuickTimeButton_Click(object sender, RoutedEventArgs e)
        {
            Logger.Debug("QuickTimeButton_Click started");
            try
            {
                if (sender is System.Windows.Controls.Button button && button.Tag is string tagValue && int.TryParse(tagValue, out int minutes))
                {
                    this.SetTime(minutes, 0);
                    Logger.Debug($"Quick time set to {minutes} minutes");
                }
            }
            catch (Exception ex)
            {
                Logger.Error("QuickTimeButton_Click failed", ex);
            }
        }

        /// <summary>
        /// Parses time input and returns minutes and seconds.
        /// </summary>
        /// <param name="input">The input string to parse.</param>
        /// <returns>A tuple containing minutes and seconds.</returns>
        private (int minutes, int seconds) ParseTimeInput(string input)
        {
            try
            {
                // Handle empty or null input
                if (string.IsNullOrWhiteSpace(input))
                {
                    return (0, 0);
                }

                // Remove any non-numeric characters except colon
                string cleanInput = Regex.Replace(input, @"[^\d:]", string.Empty);

                // Handle empty after cleaning
                if (string.IsNullOrEmpty(cleanInput))
                {
                    return (0, 0);
                }

                // Handle colon-separated format (MM:SS)
                if (cleanInput.Contains(":"))
                {
                    var parts = cleanInput.Split(':');
                    if (parts.Length >= 2 &&
                        int.TryParse(parts[0], out int minutes) &&
                        int.TryParse(parts[1], out int seconds))
                    {
                        return this.ValidateTime(minutes, seconds);
                    }
                }

                // Handle numeric-only input
                if (int.TryParse(cleanInput, out int value))
                {
                    if (value <= 9)
                    {
                        // Single digit - treat as seconds
                        return this.ValidateTime(0, value);
                    }
                    else if (value <= 99)
                    {
                        // Double digit - treat as seconds
                        return this.ValidateTime(0, value);
                    }
                    else if (value <= 9999)
                    {
                        // 3-4 digits - treat as MMSS
                        int minutes = value / 100;
                        int seconds = value % 100;
                        return this.ValidateTime(minutes, seconds);
                    }
                    else
                    {
                        // 5+ digits - treat as total seconds
                        int totalSeconds = Math.Min(value, (99 * 60) + 99);
                        return this.ValidateTime(totalSeconds / 60, totalSeconds % 60);
                    }
                }

                return (0, 0);
            }
            catch (Exception ex)
            {
                Logger.Error("ParseTimeInput failed", ex);
                return (0, 0);
            }
        }

        /// <summary>
        /// Validates and normalizes time values.
        /// </summary>
        /// <param name="minutes">Input minutes.</param>
        /// <param name="seconds">Input seconds.</param>
        /// <returns>Validated time tuple.</returns>
        private (int minutes, int seconds) ValidateTime(int minutes, int seconds)
        {
            // Ensure non-negative values
            minutes = Math.Max(0, minutes);
            seconds = Math.Max(0, seconds);

            // Enforce maximum limits (99:99)
            if (minutes > 99)
            {
                minutes = 99;
                seconds = 99;
            }
            else if (minutes == 99 && seconds > 99)
            {
                seconds = 99;
            }
            else if (seconds > 99)
            {
                seconds = 99;
            }

            return (minutes, seconds);
        }

        /// <summary>
        /// Gets the current time from the UI.
        /// </summary>
        /// <returns>Current time as minutes and seconds.</returns>
        private (int minutes, int seconds) GetCurrentTime()
        {
            return this.ParseTimeInput(this.TimeInputTextBox.Text);
        }

        /// <summary>
        /// Sets the time in the UI.
        /// </summary>
        /// <param name="minutes">Minutes to set.</param>
        /// <param name="seconds">Seconds to set.</param>
        private void SetTime(int minutes, int seconds)
        {
            var (validMinutes, validSeconds) = this.ValidateTime(minutes, seconds);

            // Update input field - use simple format only for seconds under 100
            if (validMinutes == 0 && validSeconds < 100)
            {
                this.TimeInputTextBox.Text = validSeconds.ToString();
            }
            else
            {
                this.TimeInputTextBox.Text = $"{validMinutes:D2}{validSeconds:D2}";
            }

            // This will trigger TextChanged and update the display
        }

        /// <summary>
        /// Updates the time display based on current input.
        /// </summary>
        private void UpdateTimeDisplay()
        {
            try
            {
                // Check if UI elements are initialized
                if (this.TimeDisplayTextBlock == null || this.TimeInputTextBox == null)
                {
                    Logger.Debug("UpdateTimeDisplay called before UI initialization - skipping");
                    return;
                }

                var (minutes, seconds) = this.GetCurrentTime();
                this.TimeDisplayTextBlock.Text = $"{minutes:D2}:{seconds:D2}";
            }
            catch (Exception ex)
            {
                Logger.Error("UpdateTimeDisplay failed", ex);
                if (this.TimeDisplayTextBlock != null)
                {
                    this.TimeDisplayTextBlock.Text = "00:00"; // Fallback
                }
            }
        }

        /// <summary>
        /// Initializes quick time button labels with internationalized text.
        /// </summary>
        private void InitializeQuickTimeButtons()
        {
            try
            {
                // Get the minute unit text from resources
                string minuteUnit = Properties.Resources.Minutes;

                // For Japanese, use short form "分"
                if (System.Threading.Thread.CurrentThread.CurrentUICulture.Name.StartsWith("ja"))
                {
                    this.QuickTime1Button.Content = "1分";
                    this.QuickTime5Button.Content = "5分";
                    this.QuickTime10Button.Content = "10分";
                    this.QuickTime15Button.Content = "15分";
                    this.QuickTime30Button.Content = "30分";
                    this.QuickTime60Button.Content = "60分";
                }
                else if (System.Threading.Thread.CurrentThread.CurrentUICulture.Name.StartsWith("zh"))
                {
                    // Chinese uses "分钟" for minutes
                    this.QuickTime1Button.Content = "1分钟";
                    this.QuickTime5Button.Content = "5分钟";
                    this.QuickTime10Button.Content = "10分钟";
                    this.QuickTime15Button.Content = "15分钟";
                    this.QuickTime30Button.Content = "30分钟";
                    this.QuickTime60Button.Content = "60分钟";
                }
                else
                {
                    // English and others use "min"
                    this.QuickTime1Button.Content = "1min";
                    this.QuickTime5Button.Content = "5min";
                    this.QuickTime10Button.Content = "10min";
                    this.QuickTime15Button.Content = "15min";
                    this.QuickTime30Button.Content = "30min";
                    this.QuickTime60Button.Content = "60min";
                }

                Logger.Debug("Quick time buttons initialized with localized labels");
            }
            catch (Exception ex)
            {
                Logger.Error("InitializeQuickTimeButtons failed", ex);

                // Fallback to English
                this.QuickTime1Button.Content = "1min";
                this.QuickTime5Button.Content = "5min";
                this.QuickTime10Button.Content = "10min";
                this.QuickTime15Button.Content = "15min";
                this.QuickTime30Button.Content = "30min";
                this.QuickTime60Button.Content = "60min";
            }
        }

        /// <summary>
        /// Handles position label clicks.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void PositionLabel_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Logger.Debug("PositionLabel_Click started");
            try
            {
                if (sender is System.Windows.Controls.TextBlock label && label.Tag is string position)
                {
                    this.UpdateSelectedPosition(position);
                    Logger.Debug($"Position changed to: {position}");
                }
            }
            catch (Exception ex)
            {
                Logger.Error("PositionLabel_Click failed", ex);
            }
        }

        /// <summary>
        /// Updates the selected position and visual feedback.
        /// </summary>
        /// <param name="position">The position to select.</param>
        private void UpdateSelectedPosition(string position)
        {
            try
            {
                // Reset all labels to normal style
                this.ResetPositionLabels();

                // Highlight selected label
                var selectedLabel = this.GetPositionLabel(position);
                if (selectedLabel != null)
                {
                    if (position == "Bottom")
                    {
                        // PositionBottomLabel needs special handling for margin
                        var selectedStyle = (Style)this.FindResource("SelectedPositionLabelStyle");
                        var bottomSelectedStyle = new Style(typeof(TextBlock), selectedStyle);
                        bottomSelectedStyle.Setters.Add(new Setter(TextBlock.MarginProperty, new Thickness(0)));
                        selectedLabel.Style = bottomSelectedStyle;
                    }
                    else
                    {
                        selectedLabel.Style = (Style)this.FindResource("SelectedPositionLabelStyle");
                    }
                }

                this.selectedPosition = position;
                Logger.Debug($"Updated selected position to: {position}");
            }
            catch (Exception ex)
            {
                Logger.Error("UpdateSelectedPosition failed", ex);
            }
        }

        /// <summary>
        /// Resets all position labels to normal style.
        /// </summary>
        private void ResetPositionLabels()
        {
            try
            {
                var normalStyle = (Style)this.FindResource("PositionLabelStyle");
                this.PositionRightLabel.Style = normalStyle;
                this.PositionLeftLabel.Style = normalStyle;
                this.PositionTopLabel.Style = normalStyle;

                // PositionBottomLabel needs special handling for margin
                var bottomStyle = new Style(typeof(TextBlock), normalStyle);
                bottomStyle.Setters.Add(new Setter(TextBlock.MarginProperty, new Thickness(0)));
                this.PositionBottomLabel.Style = bottomStyle;
            }
            catch (Exception ex)
            {
                Logger.Error("ResetPositionLabels failed", ex);
            }
        }

        /// <summary>
        /// Gets the TextBlock for the specified position.
        /// </summary>
        /// <param name="position">The position name.</param>
        /// <returns>The corresponding TextBlock or null if not found.</returns>
        private System.Windows.Controls.TextBlock? GetPositionLabel(string position)
        {
            try
            {
                return position switch
                {
                    "Right" => this.PositionRightLabel,
                    "Left" => this.PositionLeftLabel,
                    "Top" => this.PositionTopLabel,
                    "Bottom" => this.PositionBottomLabel,
                    _ => null,
                };
            }
            catch (Exception ex)
            {
                Logger.Error("GetPositionLabel failed", ex);
                return null;
            }
        }
    }
}
