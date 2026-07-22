using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;

namespace WPF.UIData
{
    public class DashboardSummaryData : INotifyPropertyChanged
    {
        public string Title { get; set; } = string.Empty;
        public string Subtext { get; set; } = string.Empty;
        public string BorderAccentBrush { get; set; } = "#10d9a0";
        public string Icon { get; set; } = string.Empty;
        public double ChangePercentage { get; set; }

        private decimal _targetAmount;
        public decimal TargetAmount { get => _targetAmount; set { _targetAmount = value; OnPropertyChanged(); } }

        private decimal _currentAmount;
        public decimal CurrentAmount
        {
            get => _currentAmount;
            set { _currentAmount = value; OnPropertyChanged(); OnPropertyChanged(nameof(FormattedValue)); }
        }

        public string FormattedValue
        {
            get
            {
                if (CurrentAmount >= 1_000_000) return (CurrentAmount / 1_000_000).ToString("0.#") + "Mđ";
                if (CurrentAmount >= 1_000) return (CurrentAmount / 1_000).ToString("0") + "Kđ";
                return CurrentAmount.ToString("0") + "đ";
            }
        }

        public void AnimateTo(decimal target)
        {
            TargetAmount = target;
            int steps = 30;
            int tick = 0;
            var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(30) };
            timer.Tick += (s, e) =>
            {
                tick++;
                double t = (double)tick / steps;
                double eased = 1 - Math.Pow(1 - t, 3);
                CurrentAmount = (decimal)(eased * (double)target);
                if (tick >= steps) { CurrentAmount = target; timer.Stop(); }
            };
            timer.Start();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class TransactionData : INotifyPropertyChanged
    {
        public int TransactionId { get; set; }
        public int WalletId { get; set; }
        public string WalletName { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public bool IsExpense { get; set; }
        public string Status { get; set; } = "Hoàn thành";
        public string Icon { get; set; } = string.Empty;
        public string IconBackground { get; set; } = "#4510d9a0";
        public string Note { get; set; } = string.Empty;

        public string StatusForeground => Status == "Hoàn thành" ? "#10b981" : "#f59e0b";
        public string StatusBackground => Status == "Hoàn thành" ? "#1A10b981" : "#1Af59e0b";

        public string FormattedAmount
        {
            get
            {
                string prefix = IsExpense ? "-" : "+";
                if (Amount >= 1_000_000) return prefix + (Amount / 1_000_000).ToString("0.#") + "Mđ";
                if (Amount >= 1_000) return prefix + (Amount / 1_000).ToString("0") + "Kđ";
                return prefix + Amount.ToString("0") + "đ";
            }
        }

        public string AmountColorBrush => IsExpense ? "#f43f5e" : "#10d9a0";
        public string FormattedDate => Date.ToString("dd/MM/yyyy HH:mm");

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class BudgetData : INotifyPropertyChanged
    {
        public int BudgetId { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }

        private decimal _spentAmount;
        public decimal SpentAmount
        {
            get => _spentAmount;
            set { _spentAmount = value; OnPropertyChanged(); OnPropertyChanged(nameof(SpentFormatted)); OnPropertyChanged(nameof(ProgressValue)); OnPropertyChanged(nameof(AnimatedProgress)); OnPropertyChanged(nameof(ProgressPercentage)); OnPropertyChanged(nameof(StatusText)); OnPropertyChanged(nameof(ProgressBrush)); }
        }

        public string TotalFormatted
        {
            get
            {
                if (TotalAmount >= 1_000_000) return (TotalAmount / 1_000_000).ToString("0.#") + "Mđ";
                return (TotalAmount / 1_000).ToString("0") + "Kđ";
            }
        }

        public string SpentFormatted
        {
            get
            {
                if (SpentAmount >= 1_000_000) return (SpentAmount / 1_000_000).ToString("0.#") + "Mđ";
                return (SpentAmount / 1_000).ToString("0") + "Kđ";
            }
        }
        public double AnimatedProgress => ProgressValue;
        public double ProgressPercentage => ProgressValue;
        
        public string StatusText 
        { 
            get 
            {
                if (ProgressValue >= 100) return "Vượt ngân sách";
                if (ProgressValue >= 80) return "Sắp hết";
                return "An toàn";
            }
        }
        
        public string ProgressBrush 
        { 
            get 
            {
                if (ProgressValue >= 100) return "#f43f5e";
                if (ProgressValue >= 80) return "#f59e0b";
                return "#10d9a0";
            }
        }

        public double ProgressValue => TotalAmount == 0 ? 0 : (double)(SpentAmount / TotalAmount * 100);

        public void AnimateProgress()
        {
            decimal target = SpentAmount;
            SpentAmount = 0;
            int steps = 30;
            int tick = 0;
            var timer = new System.Windows.Threading.DispatcherTimer { Interval = TimeSpan.FromMilliseconds(30) };
            timer.Tick += (s, e) =>
            {
                tick++;
                double t = (double)tick / steps;
                double eased = 1 - Math.Pow(1 - t, 3);
                SpentAmount = (decimal)(eased * (double)target);
                OnPropertyChanged(nameof(AnimatedProgress));
                if (tick >= steps) { SpentAmount = target; timer.Stop(); }
            };
            timer.Start();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class WalletData : INotifyPropertyChanged
    {
        public int WalletId { get; set; }
        public int UserId { get; set; }
        public string WalletName { get; set; } = string.Empty;
        public string WalletType { get; set; } = "Cash";
        public string Note { get; set; } = string.Empty;
        public string IconText { get; set; } = string.Empty;

        private string _iconBackground = "#ffffff";
        public string IconBackground
        {
            get => _iconBackground;
            set
            {
                _iconBackground = value;
                try { IconBackgroundBrush = new System.Windows.Media.SolidColorBrush(
                    (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(value)); }
                catch { IconBackgroundBrush = System.Windows.Media.Brushes.White; }
            }
        }

        private string _iconForeground = "#cf202f";
        public string IconForeground
        {
            get => _iconForeground;
            set
            {
                _iconForeground = value;
                try { IconForegroundBrush = new System.Windows.Media.SolidColorBrush(
                    (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(value)); }
                catch { IconForegroundBrush = System.Windows.Media.Brushes.Black; }
            }
        }

        public System.Windows.Media.SolidColorBrush IconBackgroundBrush { get; private set; }
            = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White);
        public System.Windows.Media.SolidColorBrush IconForegroundBrush { get; private set; }
            = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(0xcf, 0x20, 0x2f));

        public string Subtext { get; set; } = string.Empty;

        private string _status = "Hoạt động";
        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(StatusBackgroundBrush));
                OnPropertyChanged(nameof(StatusBorderBrush));
                OnPropertyChanged(nameof(StatusForegroundBrush));
            }
        }

        public System.Windows.Media.SolidColorBrush StatusBackgroundBrush =>
            Status == "Hoạt động" || Status == "Active"
                ? new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(0x1A, 0x00, 0x52, 0xFF))
                : new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(0x1A, 0xCF, 0x20, 0x2F));

        public System.Windows.Media.SolidColorBrush StatusBorderBrush =>
            Status == "Hoạt động" || Status == "Active"
                ? new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(0x33, 0x00, 0x52, 0xFF))
                : new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(0x33, 0xCF, 0x20, 0x2F));

        public System.Windows.Media.SolidColorBrush StatusForegroundBrush =>
            Status == "Hoạt động" || Status == "Active"
                ? new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(0x00, 0x52, 0xFF))
                : new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(0xCF, 0x20, 0x2F));

        public decimal Balance { get; set; }
        public string FormattedBalance 
        {
            get 
            {
                return Balance.ToString("#,##0", new System.Globalization.CultureInfo("vi-VN")) + " đ";
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
