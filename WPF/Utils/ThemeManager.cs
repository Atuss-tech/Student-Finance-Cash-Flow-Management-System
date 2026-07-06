using System;
using System.Linq;
using System.Windows;

namespace WPF.Utils
{
    public static class ThemeManager
    {
        public static bool IsDarkMode { get; private set; } = true;

        public static void ToggleTheme()
        {
            IsDarkMode = !IsDarkMode;
            ApplyTheme(IsDarkMode);
        }

        public static void ApplyTheme(bool isDark)
        {
            IsDarkMode = isDark;
            string themeFileName = isDark ? "DarkTheme.xaml" : "LightTheme.xaml";
            var uri = new Uri($"pack://application:,,,/WPF;component/Resources/Themes/{themeFileName}");
            
            var existingTheme = Application.Current.Resources.MergedDictionaries
                .FirstOrDefault(d => d.Source != null && d.Source.OriginalString.Contains("Theme.xaml"));

            if (existingTheme != null)
            {
                Application.Current.Resources.MergedDictionaries.Remove(existingTheme);
            }

            var newTheme = new ResourceDictionary { Source = uri };
            Application.Current.Resources.MergedDictionaries.Insert(0, newTheme);
        }
    }
}
