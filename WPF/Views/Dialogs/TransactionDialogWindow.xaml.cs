using System.Windows;

namespace WPF.Views.Dialogs
{
    public partial class TransactionDialogWindow : Window
    {
        public TransactionDialogWindow()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Here you would typically validate and save the transaction.
            DialogResult = true;
            Close();
        }
    }
}
