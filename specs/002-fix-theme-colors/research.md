# Research: Theme Color Synchronization Fix

## Gap Analysis: LightTheme vs DarkTheme
- **DarkTheme.xaml** contains new colors created for the Stitch AI UI (e.g., `StitchMomoRed`, `StitchSecondary`, `StitchTertiary`, `TranslucentMomoBrush`, etc.).
- **LightTheme.xaml** does not have any of these brushes. When the WPF application switches to LightTheme and the UI tries to resolve a `DynamicResource` like `TranslucentMomoBrush`, it returns null (or `DependencyProperty.UnsetValue`), leading to invisible elements or crash-like behavior for color bindings.

**Decision**: Replicate the structure of new variables from `DarkTheme.xaml` into `LightTheme.xaml`. Use slightly adjusted pastel/light variants for Light mode if applicable, or keep the brand colors (like Momo Red) identical and just adjust the translucent opacities for better contrast on a white background.

## ThemeManager Logic
- **ThemeManager.cs** currently uses:
  ```csharp
  var newTheme = new ResourceDictionary { Source = uri };
  Application.Current.Resources.MergedDictionaries.Insert(0, newTheme);
  ```
- **Issue**: Inserting at index `0` might place it *before* other dictionaries like `Styles.xaml` or default App resources. Normally, if a theme is supposed to override previous values, it should be appended (added to the end) or it should exactly replace the previous theme at its specific index. Since it's removing the old theme and inserting the new one at `0`, if the original theme was at index `0`, this is fine. But appending `Add(newTheme)` is often safer for overriding. 

**Decision**: Modify `ThemeManager.cs` to locate the exact index of the old theme, remove it, and insert the new theme at that exact index, OR simply `Add` it if it's the main color theme. We will replace the existing theme at its current index to preserve order.
