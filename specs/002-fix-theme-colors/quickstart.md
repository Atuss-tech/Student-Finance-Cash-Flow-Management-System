# Quickstart: Validation Guide for Theme Sync

To verify that the theme colors have been fixed:

1. **Build and Run the Application**:
   ```powershell
   dotnet run --project WPF
   ```

2. **Verify Dark Mode (Default)**:
   - Ensure the application opens without crashing.
   - Navigate to the Wallets screen. Confirm the icons have correctly colored translucent backgrounds.
   - Navigate to the Categories screen. Confirm the top summary boxes and list items have correct colors.

3. **Verify Light Mode Toggle**:
   - Locate the theme toggle button in the UI (or manually set `ThemeManager.ApplyTheme(false)` if testing via code).
   - Ensure the application instantly updates to a light background.
   - Confirm that NO elements disappear or turn invisible. Text should switch from light to dark.
   - Verify that the specific Stitch AI colors (Momo Red, Secondary Blue, Translucent Backgrounds) are correctly rendered in their light-mode variations.
