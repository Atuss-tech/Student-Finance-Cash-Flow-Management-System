using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using System.Windows.Threading;

namespace WPF.UIData
{
    /// <summary>
    /// Model đại diện cho các thẻ thống kê tổng quan ngân sách (Tổng, Đã chi, Còn lại, Tỷ lệ).
    /// </summary>
    public class BudgetStatCardModel : INotifyPropertyChanged
    {
        public string Label { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string AccentColor { get; set; } = "#10d9a0";
        public Brush AccentBrush => new SolidColorBrush((Color)ColorConverter.ConvertFromString(AccentColor));

        private decimal _targetValue;
        public decimal TargetValue { get => _targetValue; set { _targetValue = value; } }

        private decimal _value;
        public decimal Value
        {
            get => _value;
            set { _value = value; OnPropertyChanged(); OnPropertyChanged(nameof(FormattedValue)); }
        }

        public bool IsPercentage { get; set; }

        public string FormattedValue
        {
            get
            {
                if (IsPercentage) return Value.ToString("0.0") + "%";
                return Value >= 1_000_000
                    ? (Value / 1_000_000).ToString("0.#") + "Mđ"
                    : (Value / 1_000).ToString("0") + "Kđ";
            }
        }

        /// <summary>
        /// Tạo hiệu ứng animation chạy số tiền từ 0 lên giá trị thực tế.
        /// </summary>
        public void Animate()
        {
            int steps = 35;
            int tick = 0;
            decimal target = TargetValue;
            var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(30) };
            timer.Tick += (s, e) =>
            {
                tick++;
                double t = (double)tick / steps;
                double eased = 1 - Math.Pow(1 - t, 3);
                Value = (decimal)(eased * (double)target);
                if (tick >= steps) { Value = target; timer.Stop(); }
            };
            timer.Start();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
