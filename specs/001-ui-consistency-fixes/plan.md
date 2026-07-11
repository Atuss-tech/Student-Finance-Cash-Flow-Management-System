# Implementation Plan: UI Consistency Fixes

**Branch**: `[###-ui-consistency-fixes]` | **Date**: 2026-07-08 | **Spec**: [spec.md](./spec.md)

**Input**: Feature specification from `specs/001-ui-consistency-fixes/spec.md`

## Summary

This feature aligns the WPF UI strictly with the HTML design specifications provided by Stitch AI (Project ID: `10832798083950474270`). Previously, only colors and basic glassmorphism were applied. The new requirement dictates that the exact structural layout—including summary cards, specific icons, missing chart placeholders, grid structures, and bento-box layouts—must be recreated in WPF for all 5 primary views (Dashboard, Transactions, Wallets, Budgets, Categories).

## Technical Context

**Language/Version**: C#, .NET 8.0 (WPF)

**Primary Dependencies**: WPF UI Framework, LiveChartsCore

**Storage**: N/A (UI only)

**Testing**: Manual Visual Verification

**Target Platform**: Windows Desktop (WPF)

**Project Type**: Desktop App

**Performance Goals**: N/A (UI styling changes)

**Constraints**: Must use existing `DynamicResource` keys from `DarkTheme.xaml`

**Scale/Scope**: ~7 primary user control views

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

No violations detected.

## Project Structure

### Documentation (this feature)

```text
specs/001-ui-consistency-fixes/
├── plan.md              # This file (/speckit-plan command output)
├── research.md          # Phase 0 output (/speckit-plan command)
├── data-model.md        # Phase 1 output (/speckit-plan command)
├── quickstart.md        # Phase 1 output (/speckit-plan command)
├── contracts/           # Phase 1 output (/speckit-plan command)
└── tasks.md             # Phase 2 output (/speckit-tasks command - NOT created by /speckit-plan)
```

### Source Code (repository root)

```text
WPF/
├── Resources/
│   ├── DarkTheme.xaml
│   └── Styles.xaml
└── Views/
    └── UserControls/
        ├── DashboardHomeControl.xaml
        ├── WalletsViewControl.xaml
        ├── TransactionsViewControl.xaml
        ├── BudgetsViewControl.xaml
        ├── CategoriesViewControl.xaml
        ├── ReportsViewControl.xaml
        └── ProfileViewControl.xaml
```

**Structure Decision**: The changes will be centralized in the `WPF/Resources` directory (for global styles) and applied across individual view files in `WPF/Views/UserControls/`.

## Complexity Tracking

N/A
