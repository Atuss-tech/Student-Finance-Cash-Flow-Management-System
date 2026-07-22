using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using WPF.UIData;
using Services;
using Repositories;

namespace WPF.Features.Transactions
{
    public partial class AllTransactionsWindow : Window
    {
        public ObservableCollection<TransactionData> AllTransactions { get; set; }
        private readonly ITransactionService _transactionService;

        public AllTransactionsWindow()
        {
            InitializeComponent();
            DataContext = this;
            AllTransactions = new ObservableCollection<TransactionData>();
            _transactionService = new TransactionService(new TransactionRepository());
            
            this.Loaded += AllTransactionsWindow_Loaded;
        }

        private void AllTransactionsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                int userId = 1; // context hiện tại
                
                // Lấy toàn bộ giao dịch của người dùng
                var txList = _transactionService.GetTransactionsByUserId(userId);
                
                var orderedList = txList.OrderByDescending(t => t.TransactionDate).ToList();
                
                AllTransactions.Clear();
                foreach (var t in orderedList)
                {
                    AllTransactions.Add(new TransactionData
                    {
                        Title = t.Description ?? "Giao dịch",
                        Category = t.Category?.CategoryName ?? "Khác",
                        Amount = t.Amount,
                        Date = t.TransactionDate.ToDateTime(TimeOnly.MinValue),
                        IsExpense = t.TransactionType == "Expense",
                        Icon = t.TransactionType == "Expense" ? "🔥" : "💵",
                        IconBackground = t.TransactionType == "Expense" ? "#45f43f5e" : "#4510d9a0"
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu giao dịch: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
