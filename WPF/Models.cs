using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;

namespace WPF.Models
{
    public class DashboardSummaryModel : INotifyPropertyChanged
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
                if (CurrentAmount >= 1_000_000) return (CurrentAmount / 1_000_000).ToString("0.#") + "M₫";
                if (CurrentAmount >= 1_000) return (CurrentAmount / 1_000).ToString("0") + "K₫";
                return CurrentAmount.ToString("0") + "₫";
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

    public class TransactionModel : INotifyPropertyChanged
    {
        public string Title { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public bool IsExpense { get; set; }
        public string Status { get; set; } = "Hoàn thành";
        public string Icon { get; set; } = string.Empty;
        public string IconBackground { get; set; } = "#4510d9a0";

        public string StatusForeground => Status == "Hoàn thành" ? "#10b981" : "#f59e0b";
        public string StatusBackground => Status == "Hoàn thành" ? "#1A10b981" : "#1Af59e0b";

        public string FormattedAmount
        {
            get
            {
                string prefix = IsExpense ? "-" : "+";
                if (Amount >= 1_000_000) return prefix + (Amount / 1_000_000).ToString("0.#") + "M₫";
                if (Amount >= 1_000) return prefix + (Amount / 1_000).ToString("0") + "K₫";
                return prefix + Amount.ToString("0") + "₫";
            }
        }

        public string AmountColorBrush => IsExpense ? "#f43f5e" : "#10d9a0";
        public string FormattedDate => Date.ToString("dd/MM/yyyy HH:mm");

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class BudgetModel : INotifyPropertyChanged
    {
        public string CategoryName { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }

        private decimal _spentAmount;
        public decimal SpentAmount
        {
            get => _spentAmount;
            set { _spentAmount = value; OnPropertyChanged(); OnPropertyChanged(nameof(SpentFormatted)); OnPropertyChanged(nameof(ProgressValue)); }
        }

        public string TotalFormatted
        {
            get
            {
                if (TotalAmount >= 1_000_000) return (TotalAmount / 1_000_000).ToString("0.#") + "M₫";
                return (TotalAmount / 1_000).ToString("0") + "K₫";
            }
        }

        public string SpentFormatted
        {
            get
            {
                if (SpentAmount >= 1_000_000) return (SpentAmount / 1_000_000).ToString("0.#") + "M₫";
                return (SpentAmount / 1_000).ToString("0") + "K₫";
            }
        }

        public double ProgressValue => TotalAmount == 0 ? 0 : (double)(SpentAmount / TotalAmount * 100);

        public void AnimateProgress()
        {
            decimal target = SpentAmount;
            SpentAmount = 0;
            int steps = 30;
            int tick = 0;
            var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(30) };
            timer.Tick += (s, e) =>
            {
                tick++;
                double t = (double)tick / steps;
                double eased = 1 - Math.Pow(1 - t, 3);
                SpentAmount = (decimal)(eased * (double)target);
                if (tick >= steps) { SpentAmount = target; timer.Stop(); }
            };
            timer.Start();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
