using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using BusinessObjects.Models;
using Services;
using WPF.UIData;

namespace WPF.Features.Wallets
{
    public partial class WalletsView : UserControl
    {
        private readonly IWalletService _walletService;
        public ObservableCollection<WalletData> Wallets { get; set; } = new ObservableCollection<WalletData>();

        public WalletsView()
        {
            InitializeComponent();
            _walletService = new WalletService();
            this.DataContext = this;
            LoadWallets();
        }

        private void LoadWallets()
        {
            Wallets.Clear();
            var backendWallets = _walletService.GetAllWalletsByUser(1); // Hardcoded UserId = 1 for now

            foreach (var w in backendWallets)
            {
                var wd = new WalletData
                {
                    WalletName = w.WalletName,
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
        }

        private void AddWallet_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var window = new AddWalletWindow();
            if (window.ShowDialog() == true)
            {
                LoadWallets();
            }
        }
    }
}

