using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using BusinessObjects.Models;
using Services;
using WPF.UIData;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace WPF.Features.Wallets
{
    public partial class WalletsView : UserControl, INotifyPropertyChanged
    {
        private readonly IWalletService _walletService;
        public ObservableCollection<WalletData> Wallets { get; set; } = new ObservableCollection<WalletData>();

        private decimal _totalBalance;
        public decimal TotalBalance
        {
            get => _totalBalance;
            set
            {
                _totalBalance = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FormattedTotalBalance));
            }
        }

        public string FormattedTotalBalance
        {
            get
            {
                return TotalBalance.ToString("#,##0", new CultureInfo("vi-VN")) + " đ";
            }
        }

        private ISeries[] _cashFlowByWalletSeries = Array.Empty<ISeries>();
        public ISeries[] CashFlowByWalletSeries
        {
            get => _cashFlowByWalletSeries;
            set { _cashFlowByWalletSeries = value; OnPropertyChanged(); }
        }

        private Axis[] _cashFlowXAxes = Array.Empty<Axis>();
        public Axis[] CashFlowXAxes
        {
            get => _cashFlowXAxes;
            set { _cashFlowXAxes = value; OnPropertyChanged(); }
        }

        private ISeries[] _walletAllocationSeries = Array.Empty<ISeries>();
        public ISeries[] WalletAllocationSeries
        {
            get => _walletAllocationSeries;
            set { _walletAllocationSeries = value; OnPropertyChanged(); }
        }

        public SolidColorPaint LegendTextPaint { get; } = new SolidColorPaint(SKColor.Parse("#6b7280")) { SKTypeface = SKTypeface.FromFamilyName("Segoe UI") };

        public WalletsView()
        {
            InitializeComponent();
            _walletService = new WalletService();
            this.DataContext = this;
            LoadWallets();
        }

        public void LoadWallets()
        {
            Wallets.Clear();
            var backendWallets = _walletService.GetAllWalletsByUser(1); // Hardcoded UserId = 1 for now

            decimal activeTotal = 0;
            var activeWallets = new List<Wallet>();

            foreach (var w in backendWallets)
            {
                if (w.IsActive)
                {
                    activeTotal += w.Balance;
                    activeWallets.Add(w);
                }

                var wd = new WalletData
                {
                    WalletId = w.WalletId,
                    UserId = w.UserId,
                    WalletName = w.WalletName,
                    WalletType = w.WalletType,
                    Note = w.Note ?? string.Empty,
                    Balance = w.Balance,
                    Status = w.IsActive ? "Active" : "Locked",
                    Subtext = string.IsNullOrEmpty(w.Note) ? "Ví cá nhân" : w.Note
                };

                if (w.WalletType == "Cash")
                {
                    wd.IconText = "đ";
                    wd.IconBackground = "#e5e7eb"; // BorderColor
                    wd.IconForeground = "#111827"; // TextPrimary
                }
                else if (w.WalletType == "EWallet")
                {
                    wd.IconText = w.WalletName.Length > 0 ? w.WalletName.Substring(0,1).ToUpper() : "E";
                    wd.IconBackground = "#a50064"; // Momo color etc
                    wd.IconForeground = "#ffffff";
                }
                else // Bank
                {
                    wd.IconText = w.WalletName.Length > 2 ? w.WalletName.Substring(0, 3).ToUpper() : "BNK";
                    wd.IconBackground = "#ffffff";
                    wd.IconForeground = "#cf202f"; // TCB red
                }

                Wallets.Add(wd);
            }

            TotalBalance = activeTotal;

            // Load data into Column Chart (Số dư theo ví) & Pie Chart (Phân bổ nguồn tiền)
            var walletNames = activeWallets.Select(w => w.WalletName).ToArray();
            var walletBalances = activeWallets.Select(w => (double)w.Balance).ToArray();

            CashFlowXAxes = new Axis[]
            {
                new Axis
                {
                    Labels = walletNames,
                    LabelsPaint = new SolidColorPaint(SKColor.Parse("#4a5568"))
                }
            };

            CashFlowByWalletSeries = new ISeries[]
            {
                new ColumnSeries<double>
                {
                    Values = walletBalances,
                    Name = "Số dư",
                    Fill = new SolidColorPaint(SKColor.Parse("#0052ff")),
                    MaxBarWidth = 35,
                    Rx = 8, Ry = 8
                }
            };

            var palette = new[] { "#0052ff", "#05b169", "#a50064", "#f59e0b", "#7c6df8", "#cf202f" };
            var pieSeriesList = new List<PieSeries<double>>();
            int colorIndex = 0;

            foreach (var w in activeWallets)
            {
                pieSeriesList.Add(new PieSeries<double>
                {
                    Values = new double[] { (double)w.Balance },
                    Name = w.WalletName,
                    InnerRadius = 50,
                    Fill = new SolidColorPaint(SKColor.Parse(palette[colorIndex % palette.Length]))
                });
                colorIndex++;
            }

            WalletAllocationSeries = pieSeriesList.ToArray();
        }

        private void AddWallet_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var window = new AddWalletWindow();
            if (window.ShowDialog() == true)
            {
                LoadWallets();
            }
        }

        private void WalletCard_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is System.Windows.FrameworkElement fe && fe.DataContext is WalletData wallet)
            {
                var window = new AddWalletWindow(wallet);
                window.Owner = System.Windows.Window.GetWindow(this);
                if (window.ShowDialog() == true)
                {
                    LoadWallets();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

