using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace WPF.Common
{
    public enum CustomDialogType
    {
        Info,
        Warning,
        Error,
        Success,
        Question
    }

    public partial class CustomMessageBoxWindow : Window
    {
        public bool IsConfirmed { get; private set; } = false;

        public CustomMessageBoxWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            IsConfirmed = true;
            this.DialogResult = true;
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            IsConfirmed = false;
            this.DialogResult = false;
            this.Close();
        }

        public static bool ShowConfirm(
            Window? owner,
            string title,
            string message,
            string? highlightNote = null,
            string confirmText = "Đồng ý",
            string cancelText = "Hủy bỏ",
            CustomDialogType dialogType = CustomDialogType.Warning,
            string? primaryButtonColorHex = null)
        {
            var dlg = new CustomMessageBoxWindow();
            if (owner != null)
            {
                dlg.Owner = owner;
            }
            else if (Application.Current.MainWindow != null && Application.Current.MainWindow.IsVisible)
            {
                dlg.Owner = Application.Current.MainWindow;
            }

            dlg.TitleTextBlock.Text = title;
            dlg.MessageTextBlock.Text = message;

            if (!string.IsNullOrWhiteSpace(highlightNote))
            {
                dlg.HighlightTextBlock.Text = highlightNote;
                dlg.HighlightBorder.Visibility = Visibility.Visible;
            }
            else
            {
                dlg.HighlightBorder.Visibility = Visibility.Collapsed;
            }

            dlg.ConfirmBtn.Content = confirmText;
            dlg.CancelBtn.Content = cancelText;

            // Configure Icon & Colors based on DialogType
            switch (dialogType)
            {
                case CustomDialogType.Warning:
                case CustomDialogType.Question:
                    dlg.IconText.Text = "🔒";
                    dlg.IconBadge.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FEF2F2")!;
                    dlg.ConfirmBtn.Background = (SolidColorBrush)new BrushConverter().ConvertFrom(primaryButtonColorHex ?? "#DC2626")!; // Red action button
                    break;
                case CustomDialogType.Info:
                    dlg.IconText.Text = "ℹ️";
                    dlg.IconBadge.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#EFF6FF")!;
                    dlg.ConfirmBtn.Background = (SolidColorBrush)new BrushConverter().ConvertFrom(primaryButtonColorHex ?? "#0052FF")!; // Blue action button
                    break;
                case CustomDialogType.Success:
                    dlg.IconText.Text = "✅";
                    dlg.IconBadge.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#ECFDF5")!;
                    dlg.ConfirmBtn.Background = (SolidColorBrush)new BrushConverter().ConvertFrom(primaryButtonColorHex ?? "#10B981")!; // Emerald green action button
                    break;
                case CustomDialogType.Error:
                    dlg.IconText.Text = "❌";
                    dlg.IconBadge.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FEF2F2")!;
                    dlg.ConfirmBtn.Background = (SolidColorBrush)new BrushConverter().ConvertFrom(primaryButtonColorHex ?? "#DC2626")!;
                    break;
            }

            var res = dlg.ShowDialog();
            return res == true;
        }

        public static void ShowInfo(Window? owner, string title, string message, string? highlightNote = null)
        {
            var dlg = new CustomMessageBoxWindow();
            if (owner != null)
            {
                dlg.Owner = owner;
            }
            else if (Application.Current.MainWindow != null && Application.Current.MainWindow.IsVisible)
            {
                dlg.Owner = Application.Current.MainWindow;
            }

            dlg.TitleTextBlock.Text = title;
            dlg.MessageTextBlock.Text = message;

            if (!string.IsNullOrWhiteSpace(highlightNote))
            {
                dlg.HighlightTextBlock.Text = highlightNote;
                dlg.HighlightBorder.Visibility = Visibility.Visible;
            }

            dlg.IconText.Text = "ℹ️";
            dlg.IconBadge.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#EFF6FF")!;
            dlg.CancelBtn.Visibility = Visibility.Collapsed;
            dlg.ConfirmBtn.Content = "Đã hiểu";
            dlg.ConfirmBtn.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#0052FF")!;

            dlg.ShowDialog();
        }
    }
}
