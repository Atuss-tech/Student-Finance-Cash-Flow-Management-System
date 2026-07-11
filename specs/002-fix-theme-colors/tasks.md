---
description: "Task list for Theme Color Synchronization Fix"
---

# Tasks: Theme Color Synchronization Fix

**Input**: Design documents from `/specs/002-fix-theme-colors/`
**Prerequisites**: plan.md, spec.md, research.md, data-model.md, quickstart.md

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- **[Story]**: Which user story this task belongs to (e.g., US1, US2)
- Include exact file paths in descriptions

## Path Conventions

- Paths are relative to the repository root.

---

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Project initialization and basic structure

- No setup tasks required for this feature as the project already exists.

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Core infrastructure that MUST be complete before ANY user story can be implemented

- No foundational tasks required. The structure and framework are already in place.

---

## Phase 3: User Story 1 - Consistent Theme Application (Priority: P1) 🎯 MVP

**Goal**: Ensure all text, backgrounds, and interactive elements use correct semantic colors in both Dark Mode and Light Mode without visual bugs.

**Independent Test**: Can be fully tested by launching the application in both modes and verifying all UI components are visible and have proper contrast.

### Implementation for User Story 1

- [x] T001 [P] [US1] Ensure all defined Stitch AI colors and translucent brushes are complete in WPF/Resources/DarkTheme.xaml
- [x] T002 [P] [US1] Add all missing semantic brushes (StitchMomoRed, TranslucentMomoBrush, SurfaceHigh, etc.) to WPF/Resources/LightTheme.xaml with light-appropriate variations

**Checkpoint**: At this point, both `DarkTheme.xaml` and `LightTheme.xaml` have exactly matching keys, resolving all `DependencyProperty.UnsetValue` runtime errors.

---

## Phase 4: User Story 2 - Dynamic Theme Switching (Priority: P2)

**Goal**: Switch between Light and Dark themes dynamically without restarting the application, ensuring immediate color update.

**Independent Test**: Can be fully tested by toggling themes at runtime and observing instant color changes.

### Implementation for User Story 2

- [x] T003 [US2] Update `ApplyTheme` logic in WPF/Utils/ThemeManager.cs to properly locate and replace the existing theme dictionary without breaking ordering or leaving stale resources.

**Checkpoint**: At this point, User Stories 1 AND 2 should both work independently. Themes can be toggled reliably.

---

## Phase 5: Polish & Cross-Cutting Concerns

**Purpose**: Improvements that affect multiple user stories

- [x] T004 Build and run the WPF application to verify quickstart.md scenarios

---

## Dependencies & Execution Order

### Phase Dependencies

- **User Stories (Phase 3+)**: Can start immediately as there are no Foundational blockers.
- **Polish (Final Phase)**: Depends on all desired user stories being complete.

### User Story Dependencies

- **User Story 1 (P1)**: Can start immediately.
- **User Story 2 (P2)**: Can start concurrently but ideally validated after US1 so the updated brushes exist.

### Parallel Opportunities

- T001 and T002 can be done in parallel as they touch different XAML files.
