using System.Windows;
using System.Windows.Threading;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace SeekbarTimer
{
    public partial class TimerWindow : Window
    {
        private DispatcherTimer _timer = null!;
        private int _totalSeconds;
        private int _remainingSeconds;
        private bool _isPaused = false;
        private string _position;

        public TimerWindow(int totalSeconds, string position)
        {
            InitializeComponent();
            _totalSeconds = totalSeconds;
            _remainingSeconds = totalSeconds;
            _position = position;

            SetupTimer();
            SetupWindowPosition();
            UpdateTimeDisplay();
        }

        private void SetupTimer()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void SetupWindowPosition()
        {
            var screenWidth = SystemParameters.PrimaryScreenWidth;
            var screenHeight = SystemParameters.PrimaryScreenHeight;

            switch (_position)
            {
                case "右端":
                    Width = 20;
                    Height = screenHeight * 0.8;
                    Left = screenWidth - Width - 10;
                    Top = (screenHeight - Height) / 2;
                    break;
                case "左端":
                    Width = 20;
                    Height = screenHeight * 0.8;
                    Left = 10;
                    Top = (screenHeight - Height) / 2;
                    break;
                case "上端":
                    Width = screenWidth * 0.8;
                    Height = 20;
                    Left = (screenWidth - Width) / 2;
                    Top = 10;
                    ProgressBar.VerticalAlignment = VerticalAlignment.Stretch;
                    ProgressBar.HorizontalAlignment = HorizontalAlignment.Left;
                    break;
                case "下端":
                    Width = screenWidth * 0.8;
                    Height = 20;
                    Left = (screenWidth - Width) / 2;
                    Top = screenHeight - Height - 50;
                    ProgressBar.VerticalAlignment = VerticalAlignment.Stretch;
                    ProgressBar.HorizontalAlignment = HorizontalAlignment.Left;
                    break;
            }
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (_isPaused) return;

            _remainingSeconds--;
            UpdateTimeDisplay();
            UpdateProgressBar();
            UpdateBarColor();

            if (_remainingSeconds <= 0)
            {
                _timer.Stop();
                ShowTimeUpNotification();
            }
        }

        private void UpdateTimeDisplay()
        {
            int minutes = _remainingSeconds / 60;
            int seconds = _remainingSeconds % 60;
            TimeDisplay.Text = $"{minutes}:{seconds:D2}";
        }

        private void UpdateProgressBar()
        {
            double progress = (double)(_totalSeconds - _remainingSeconds) / _totalSeconds;
            
            if (_position == "上端" || _position == "下端")
            {
                ProgressBar.Width = Width * progress;
            }
            else
            {
                ProgressBar.Height = Height * progress;
            }
        }

        private void UpdateBarColor()
        {
            double progress = (double)(_totalSeconds - _remainingSeconds) / _totalSeconds;
            
            if (progress >= 0.8)
            {
                ProgressBar.Fill = new SolidColorBrush(Colors.Red);
                // 点滅効果
                var animation = new DoubleAnimation(0.3, 1.0, TimeSpan.FromMilliseconds(500))
                {
                    AutoReverse = true,
                    RepeatBehavior = RepeatBehavior.Forever
                };
                ProgressBar.BeginAnimation(OpacityProperty, animation);
            }
            else if (progress >= 0.6)
            {
                ProgressBar.Fill = new SolidColorBrush(Colors.Orange);
                ProgressBar.BeginAnimation(OpacityProperty, null);
                ProgressBar.Opacity = 1.0;
            }
            else
            {
                ProgressBar.Fill = new SolidColorBrush(Colors.Green);
                ProgressBar.BeginAnimation(OpacityProperty, null);
                ProgressBar.Opacity = 1.0;
            }
        }

        private void ShowTimeUpNotification()
        {
            MessageBox.Show("時間切れです！", "タイマー", MessageBoxButton.OK, MessageBoxImage.Information);
            Application.Current.MainWindow?.Show();
            this.Close();
        }

        private void Window_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            // ホバー時にウィンドウを拡大してコントロールパネルを表示
            Width = Math.Max(Width, 200);
            Height = Math.Max(Height, 150);
            ControlPanel.Visibility = Visibility.Visible;
            TimerBorder.Visibility = Visibility.Collapsed;
        }

        private void Window_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            // ホバー終了時に元のサイズに戻す
            SetupWindowPosition();
            ControlPanel.Visibility = Visibility.Collapsed;
            TimerBorder.Visibility = Visibility.Visible;
        }

        private void PauseResumeButton_Click(object sender, RoutedEventArgs e)
        {
            _isPaused = !_isPaused;
            PauseResumeButton.Content = _isPaused ? "再開" : "一時停止";
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
            Application.Current.MainWindow?.Show();
            this.Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
            Application.Current.MainWindow?.Show();
            this.Close();
        }
    }
} 