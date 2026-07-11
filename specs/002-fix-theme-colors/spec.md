# Feature Specification: Theme Color Synchronization Fix

**Feature Branch**: `[002-fix-theme-colors]`

**Created**: 2026-07-10

**Status**: Draft

**Input**: User description: "đổi màu giao diện ý, mình kiểm tra trên desktop nó lỗi màu và không có sự đồng bộ màu" + DarkTheme.xaml, LightTheme.xaml, ThemeManager.cs

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Consistent Theme Application (Priority: P1)

As a user, I want the application's interface to have consistent, readable colors so that the app looks professional and is easy to use.

**Why this priority**: A broken or inconsistent UI color scheme directly degrades the user experience, causing text to be unreadable or elements to be invisible against their backgrounds.

**Independent Test**: Can be fully tested by launching the desktop application and navigating through all main screens (Wallets, Categories, etc.) to verify color consistency.

**Acceptance Scenarios**:

1. **Given** the user launches the application in Dark Mode, **When** they view any screen, **Then** all text, backgrounds, and interactive elements use the correct Dark palette colors without any hardcoded white/black glitches.
2. **Given** the user launches the application in Light Mode, **When** they view any screen, **Then** all text, backgrounds, and interactive elements use the correct Light palette colors.

---

### User Story 2 - Dynamic Theme Switching (Priority: P2)

As a user, I want to be able to switch between Light and Dark themes dynamically without restarting the application, and have all colors update instantly.

**Why this priority**: Provides a modern, responsive user experience allowing users to adapt the UI to their environment.

**Independent Test**: Can be fully tested by triggering a theme switch via the ThemeManager and verifying the UI updates immediately.

**Acceptance Scenarios**:

1. **Given** the app is running in Dark Mode, **When** the theme is switched to Light Mode via `ThemeManager`, **Then** the UI instantly updates to the Light palette.

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: System MUST define a complete, synchronized set of semantic color resources (e.g., `BackgroundBrush`, `SurfaceBrush`, `TextPrimaryBrush`, `PrimaryAccentBrush`) in both `DarkTheme.xaml` and `LightTheme.xaml`.
- **FR-002**: System MUST ensure all UI controls and styles reference these colors using `DynamicResource` rather than `StaticResource` or hardcoded hex values.
- **FR-003**: System MUST NOT have any missing resource keys in either theme dictionary that would cause a fallback to default WPF colors.
- **FR-004**: `ThemeManager.cs` MUST correctly load, unload, and apply the appropriate ResourceDictionary at runtime to the application scope.

### Key Entities

- **DarkTheme.xaml**: The resource dictionary defining all color brushes for the dark appearance.
- **LightTheme.xaml**: The resource dictionary defining all color brushes for the light appearance.
- **ThemeManager.cs**: The utility class responsible for swapping the active theme resource dictionary at runtime.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: 100% of standard UI elements (text, cards, backgrounds, buttons) use semantic theme brushes (no hardcoded colors).
- **SC-002**: Changing the theme updates the entire visible UI instantly (under 100ms) without requiring a restart.
- **SC-003**: Zero visual bugs (e.g., dark text on dark background, or light text on light background) on all primary screens (Dashboard, Wallets, Categories).

## Assumptions

- The application uses WPF's standard `ResourceDictionary` merging approach for theming.
- The user's system supports standard WPF rendering.
- Custom controls (like LiveCharts) can bind to or be updated by WPF DynamicResources for their coloring (e.g. text colors on charts).
