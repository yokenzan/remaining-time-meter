using System.Windows;

namespace SeekbarTimer
{
    public partial class TimeSetDialog : Window
    {
        public int Minutes { get; private set; }

        public TimeSetDialog(int currentMinutes)
        {
            InitializeComponent();
            MinutesTextBox.Text = currentMinutes.ToString();
            MinutesTextBox.SelectAll();
            MinutesTextBox.Focus();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(MinutesTextBox.Text, out int minutes) && minutes > 0)
            {
                Minutes = minutes;
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("正の整数を入力してください。", "エラー", MessageBoxButton.OK, MessageBoxImage.Warning);
                MinutesTextBox.SelectAll();
                MinutesTextBox.Focus();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}