using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace WPF.Features.Categories
{
    public class CategoryItemModel : INotifyPropertyChanged
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string IconBackground { get; set; } = string.Empty;
        public string ColorHex { get; set; } = "#3b82f6";
        public string Description { get; set; } = string.Empty;
        
        public double CurrentSpending { get; set; }
        public double BudgetLimit { get; set; }
        public double ProgressPercentage => BudgetLimit > 0 ? (CurrentSpending / BudgetLimit) * 100 : 0;
        public string CurrentSpendingText => $"{CurrentSpending:N0} ₫";
        public string BudgetLimitText => $"{BudgetLimit:N0} ₫";

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public partial class CategoriesView : UserControl, INotifyPropertyChanged
    {
        private string _totalCategories = "12 Danh mục";
        public string TotalCategories { get => _totalCategories; set { _totalCategories = value; OnPropertyChanged(); } }

        private string _highestSpendingCategory = "Ăn uống";
        public string HighestSpendingCategory { get => _highestSpendingCategory; set { _highestSpendingCategory = value; OnPropertyChanged(); } }

        private string _highestSpendingAmount = "3,500,000 ₫";
        public string HighestSpendingAmount { get => _highestSpendingAmount; set { _highestSpendingAmount = value; OnPropertyChanged(); } }

        private double _budgetHealthPercentage = 68;
        public double BudgetHealthPercentage { get => _budgetHealthPercentage; set { _budgetHealthPercentage = value; OnPropertyChanged(); } }

        public ObservableCollection<CategoryItemModel> Categories { get; set; }

        private string _searchText = string.Empty;
        public string SearchText { get => _searchText; set { _searchText = value; OnPropertyChanged(); } }

        private ISeries[] _categoryBreakdownSeries;
        public ISeries[] CategoryBreakdownSeries { get => _categoryBreakdownSeries; set { _categoryBreakdownSeries = value; OnPropertyChanged(); } }

        public CategoriesView()
        {
            InitializeComponent();
            this.DataContext = this;

            Categories = new ObservableCollection<CategoryItemModel>();
            CategoryBreakdownSeries = System.Array.Empty<ISeries>();
        }

        private void SetAll_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Filter logic
        }

        private void SetIncome_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Filter logic
        }

        private void SetExpense_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Filter logic
        }

        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Add category logic
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

