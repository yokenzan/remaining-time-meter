// <copyright file="MainWindow.xaml.cs" company="RemainingTimeMeter">
// Copyright (c) RemainingTimeMeter. All rights reserved.
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
            this.InitializeComponent();
            this.LoadDisplays();
        }

        /// <summary>
        /// Loads display information into the display combo box.
        /// </summary>
        private void LoadDisplays()
        {
            this.DisplayComboBox.Items.Clear();

            var displayInfos = this.GetDisplayInformation();

            for (int i = 0; i < displayInfos.Count; i++)
            {
                var display = displayInfos[i];
                string displayName;

                if (display.IsPrimary)
                {
                    displayName = $"ディスプレー {i + 1} (主画面) - {display.Width}x{display.Height}";
                }
                else
                {
                    displayName = $"ディスプレー {i + 1} - {display.Width}x{display.Height}";
                }

                var item = new ComboBoxItem
                {
                    Content = displayName,
                    Tag = display,
                };

                this.DisplayComboBox.Items.Add(item);

                // 主画面を既定値として選択
                if (display.IsPrimary)
                {
                    this.DisplayComboBox.SelectedItem = item;
                }
            }
        }

        /// <summary>
        /// Gets information about all available displays.
        /// </summary>
        /// <returns>A list of display information.</returns>
        private List<DisplayInfo> GetDisplayInformation()
        {
            var displays = new List<DisplayInfo>();

            foreach (System.Windows.Forms.Screen screen in System.Windows.Forms.Screen.AllScreens)
            {
                // DPI情報を取得
                var source = PresentationSource.FromVisual(this);
                double scaleX = 1.0;
                double scaleY = 1.0;

                if (source?.CompositionTarget != null)
                {
                    var matrix = source.CompositionTarget.TransformToDevice;
                    scaleX = matrix.M11;
                    scaleY = matrix.M22;
                }

                displays.Add(new DisplayInfo
                {
                    Left = screen.Bounds.Left,
                    Top = screen.Bounds.Top,
                    Width = screen.Bounds.Width,
                    Height = screen.Bounds.Height,
                    ScaleX = scaleX,
                    ScaleY = scaleY,
                    IsPrimary = screen.Primary,
                });
            }

            return displays;
        }

        /// <summary>
        /// Handles the start button click event.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            // 入力値の検証
            if (!int.TryParse(this.MinutesTextBox.Text, out int minutes) || minutes < 0)
            {
                System.Windows.MessageBox.Show("分の値が正しくありません。", "エラー", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(this.SecondsTextBox.Text, out int seconds) || seconds < 0 || seconds >= 60)
            {
                System.Windows.MessageBox.Show("秒の値が正しくありません（0-59）。", "エラー", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 総時間を秒で計算
            int totalSeconds = (minutes * 60) + seconds;
            if (totalSeconds <= 0)
            {
                System.Windows.MessageBox.Show("時間を正しく設定してください。", "エラー", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 配置位置を取得
            string position = ((ComboBoxItem)this.PositionComboBox.SelectedItem).Content.ToString() ?? "右端";

            // 選択されたディスプレーを取得
            var selectedDisplayItem = (ComboBoxItem)this.DisplayComboBox.SelectedItem;
            var selectedDisplay = (DisplayInfo)selectedDisplayItem.Tag;

            // タイマーウィンドウを作成して表示
            var timerWindow = new TimerWindow(totalSeconds, position, selectedDisplay);
            timerWindow.MainWindowRequested += () => this.Show();
            timerWindow.Show();

            // メインウィンドウを非表示
            this.Hide();
        }
    }
}
