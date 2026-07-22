# Implementation Plan: 006-dashboard

**Branch**: `006-dashboard` | **Date**: 2026-07-20 | **Spec**: [specs/006-dashboard/spec.md](file:///f:/SU26/Prn212/Practice/Student%20Finance%20&%20Cash%20Flow%20Management%20System/specs/006-dashboard/spec.md)

**Input**: Feature specification from `/specs/006-dashboard/spec.md`

## Summary

Enable navigation from the Dashboard's "Recent Transactions" section to the full Transactions list view when the user clicks the "Xem tất cả" (See all) button by firing an event from the `DashboardHomeView` up to the `MainWindow`.

## Technical Context

**Language/Version**: C# 12 (.NET 8 WPF)

**Primary Dependencies**: None new

**Storage**: SQLite (existing)

**Testing**: N/A for this simple UI tweak

**Target Platform**: Windows Desktop

**Project Type**: WPF Desktop Application

**Performance Goals**: Instantaneous view switching (<50ms)

**Constraints**: Must match existing navigation patterns in `MainWindow.xaml.cs`.

**Scale/Scope**: 1 UI event, 2 files modified

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

No complex architecture violations. Simply using built-in WPF events for UserControl to Window communication.

## Project Structure

### Documentation (this feature)

```text
specs/006-dashboard/
├── plan.md              # This file (/speckit-plan command output)
├── research.md          # Phase 0 output (/speckit-plan command)
├── data-model.md        # Phase 1 output (/speckit-plan command)
├── quickstart.md        # Phase 1 output (/speckit-plan command)
└── tasks.md             # Phase 2 output (/speckit-tasks command - NOT created by /speckit-plan)
```

### Source Code (repository root)

```text
WPF/
├── MainWindow.xaml
├── MainWindow.xaml.cs
└── Features/
    └── Dashboard/
        ├── DashboardHomeView.xaml
        └── DashboardHomeView.xaml.cs
```

**Structure Decision**: Modifying existing WPF Views and ViewModels as per the above directory layout.
