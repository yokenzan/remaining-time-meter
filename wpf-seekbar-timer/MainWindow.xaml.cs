using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace SeekbarTimer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            // 入力値の検証
            if (!int.TryParse(MinutesTextBox.Text, out int minutes) || minutes < 0)
            {
                MessageBox.Show("分の値が正しくありません。", "エラー", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(SecondsTextBox.Text, out int seconds) || seconds < 0 || seconds >= 60)
            {
                MessageBox.Show("秒の値が正しくありません（0-59）。", "エラー", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 総時間を秒で計算
            int totalSeconds = minutes * 60 + seconds;
            if (totalSeconds <= 0)
            {
                MessageBox.Show("時間を正しく設定してください。", "エラー", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 配置位置を取得
            string position = ((ComboBoxItem)PositionComboBox.SelectedItem).Content.ToString() ?? "右端";

            // タイマーウィンドウを作成して表示
            var timerWindow = new TimerWindow(totalSeconds, position);
            timerWindow.Show();

            // メインウィンドウを非表示
            this.Hide();
        }
    }
} 