# Implementation Plan: Fix Theme Colors & Synchronization

**Branch**: `[002-fix-theme-colors]` | **Date**: 2026-07-10
**Input**: Feature specification from `/specs/002-fix-theme-colors/spec.md`

## Summary

The current application has inconsistencies between `DarkTheme.xaml` and `LightTheme.xaml`. Specifically, the new brushes introduced for the Stitch AI UI implementation (`TranslucentSecondaryBrush`, `TranslucentTertiaryBrush`, `SurfaceHighBrush`, etc.) were only added to `DarkTheme.xaml` and are missing from `LightTheme.xaml`. This causes visual glitches or missing colors when switching to Light Mode on desktop. Furthermore, we must ensure `ThemeManager.cs` reliably switches the themes without leaving old resources behind.

## User Review Required
> [!IMPORTANT]
> The light theme needs equivalent colors for the Stitch AI-specific brushes (like `StitchMomoRed`, `StitchSecondary`, `StitchSurfaceHigh`). I will generate a consistent light-mode palette for these missing brushes. Please review the proposed approach below.

## Proposed Changes

---

### WPF/Resources

#### [MODIFY] [LightTheme.xaml](file:///f:/SU26/Prn212/Practice/Student%20Finance%20&%20Cash%20Flow%20Management%20System/WPF/Resources/LightTheme.xaml)
- Add all missing keys that are currently only in `DarkTheme.xaml`. This includes:
  - `StitchMomoRed`, `StitchSecondary`, `StitchTertiary`, `StitchPrimaryContainer`, `StitchSurfaceLow`, `StitchSurfaceHigh` (and their corresponding SolidColorBrushes)
  - `TranslucentMomoBrush`, `TranslucentSecondaryBrush`, `TranslucentTertiaryBrush`
  - `SurfaceHighBrush`, `SurfaceLowBrush`
- Map these new keys to appropriate Light Theme color variants (e.g., instead of dark gray for `SurfaceHigh`, use a very light gray or white variant).

#### [MODIFY] [DarkTheme.xaml](file:///f:/SU26/Prn212/Practice/Student%20Finance%20&%20Cash%20Flow%20Management%20System/WPF/Resources/DarkTheme.xaml)
- Ensure no keys are missing.
- Define `SurfaceHighBrush` and `SurfaceLowBrush` properly, currently they are mapped to `StitchSurfaceHighBrush` etc. but might be missing direct definition if the UI calls for `DynamicResource SurfaceHighBrush`.

---

### WPF/Utils

#### [MODIFY] [ThemeManager.cs](file:///f:/SU26/Prn212/Practice/Student%20Finance%20&%20Cash%20Flow%20Management%20System/WPF/Utils/ThemeManager.cs)
- Update the logic in `ApplyTheme(bool isDark)` to correctly replace the ResourceDictionary.
- Ensure that the new theme is merged at the end of the list rather than inserted at index 0, so its resources correctly override any underlying defaults, or explicitly ensure it replaces the exact index of the old theme.
- Add `Application.Current.MainWindow.UpdateDefaultStyle()` if needed to force a UI refresh across all controls.

## Verification Plan

### Automated Tests
- Run `dotnet build` to ensure all XAML parses correctly without missing static resources.

### Manual Verification
- Launch the WPF application.
- Navigate to the Wallets and Categories screens.
- Switch between Light Mode and Dark Mode (if there's a toggle button on the UI, or by changing the default in `App.xaml.cs`).
- Verify that no elements disappear, no text becomes unreadable (e.g., white text on white background), and all gradients/glassmorphism effects render correctly in both themes.
