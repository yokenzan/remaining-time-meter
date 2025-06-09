using System;
using System.Media;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace SeekbarTimer
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer _timer;
        private int _totalSeconds = 300; // 5分
        private int _currentSeconds = 0;
        private bool _isRunning = false;

        public MainWindow()
        {
            InitializeComponent();
            InitializeTimer();
            PositionWindow();
            SetupEventHandlers();
        }

        private void InitializeTimer()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
        }

        private void PositionWindow()
        {
            // 画面下端に配置
            var screenWidth = SystemParameters.PrimaryScreenWidth;
            var screenHeight = SystemParameters.PrimaryScreenHeight;
            
            Width = screenWidth;
            Top = screenHeight - Height;
            Left = 0;
        }

        private void SetupEventHandlers()
        {
            // 左クリックで開始/停止
            MouseLeftButtonUp += (s, e) => ToggleTimer();
            
            // キーボードショートカット
            KeyDown += (s, e) =>
            {
                if (e.Key == Key.Space)
                {
                    ToggleTimer();
                    e.Handled = true;
                }
                else if (e.Key == Key.Escape)
                {
                    Close();
                }
            };

            // フォーカス可能にする
            Focusable = true;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _currentSeconds++;
            UpdateProgress();

            if (_currentSeconds >= _totalSeconds)
            {
                _timer.Stop();
                _isRunning = false;
                ShowTimeUpNotification();
            }
        }

        private void UpdateProgress()
        {
            var progress = (double)_currentSeconds / _totalSeconds;
            var progressWidth = Width * progress;
            
            ProgressRect.Width = progressWidth;
            
            // 色を時間経過に応じて変更
            if (progress < 0.5)
            {
                ProgressRect.Fill = new SolidColorBrush(Color.FromArgb(128, 0, 128, 0)); // 緑
            }
            else if (progress < 0.8)
            {
                ProgressRect.Fill = new SolidColorBrush(Color.FromArgb(128, 255, 255, 0)); // 黄
            }
            else
            {
                ProgressRect.Fill = new SolidColorBrush(Color.FromArgb(128, 255, 0, 0)); // 赤
            }

            // ウィンドウタイトルに残り時間を表示
            var remaining = _totalSeconds - _currentSeconds;
            var minutes = remaining / 60;
            var seconds = remaining % 60;
            Title = $"シークバー的タイムキーパー - {minutes:D2}:{seconds:D2}";
        }

        private void ToggleTimer()
        {
            if (_isRunning)
            {
                _timer.Stop();
                _isRunning = false;
            }
            else
            {
                _timer.Start();
                _isRunning = true;
            }
        }

        private void ResetTimer()
        {
            _timer.Stop();
            _isRunning = false;
            _currentSeconds = 0;
            UpdateProgress();
        }

        private void ShowTimeUpNotification()
        {
            // 画面を一瞬赤くする
            var originalFill = ProgressRect.Fill;
            ProgressRect.Fill = new SolidColorBrush(Color.FromArgb(200, 255, 0, 0));
            
            var flashTimer = new DispatcherTimer();
            flashTimer.Interval = TimeSpan.FromMilliseconds(500);
            flashTimer.Tick += (s, e) =>
            {
                ProgressRect.Fill = originalFill;
                flashTimer.Stop();
            };
            flashTimer.Start();

            // 音とメッセージ
            SystemSounds.Exclamation.Play();
            MessageBox.Show("時間終了！", "タイマー", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ShowTimeDialog()
        {
            var dialog = new TimeSetDialog(_totalSeconds / 60);
            if (dialog.ShowDialog() == true)
            {
                _totalSeconds = dialog.Minutes * 60;
                ResetTimer();
            }
        }

        // メニューイベントハンドラ
        private void ToggleTimer_Click(object sender, RoutedEventArgs e)
        {
            ToggleTimer();
        }

        private void ResetTimer_Click(object sender, RoutedEventArgs e)
        {
            ResetTimer();
        }

        private void SetTime_Click(object sender, RoutedEventArgs e)
        {
            ShowTimeDialog();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            _timer?.Stop();
            base.OnClosed(e);
        }
    }
}