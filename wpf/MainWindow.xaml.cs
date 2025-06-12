// <copyright file="MainWindow.xaml.cs" company="RemainingTimeMeter">
// Copyright (c) 2025 RemainingTimeMeter. Licensed under the MIT License.
// </copyright>

using System;
using System.Collections.Generic;
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
                Logger.Info("MainWindow constructor completed successfully");
            }
            catch (Exception ex)
            {
                Logger.Error("MainWindow constructor failed", ex);
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
            catch (Exception ex)
            {
                Logger.Error("LoadDisplays failed", ex);
                throw;
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
                Logger.Debug($"Input values - Minutes: '{this.MinutesTextBox.Text}', Seconds: '{this.SecondsTextBox.Text}'");

                // Validate input values
                if (!int.TryParse(this.MinutesTextBox.Text, out int minutes) || minutes < 0)
                {
                    Logger.Debug("Invalid minutes input");
                    System.Windows.MessageBox.Show(Properties.Resources.InvalidMinutesValue, Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(this.SecondsTextBox.Text, out int seconds) || seconds < 0 || seconds >= 60)
                {
                    Logger.Debug("Invalid seconds input");
                    System.Windows.MessageBox.Show(Properties.Resources.InvalidSecondsValue, Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

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
                string position = ((ComboBoxItem)this.PositionComboBox.SelectedItem).Content.ToString() ?? Properties.Resources.PositionRight;
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
            catch (Exception ex)
            {
                Logger.Error("StartButton_Click failed", ex);
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
            catch (Exception ex)
            {
                Logger.Error("TextBox_GotFocus failed", ex);
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
            catch (Exception ex)
            {
                Logger.Error("TextBox_PreviewMouseLeftButtonDown failed", ex);
            }
        }
    }
}
