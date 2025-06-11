# Refactoring TODO List

## High Priority

### 0. Add by yokenzan

- ✅ **about copyright**: this software is OSS, MIT License. **COMPLETED**
  - LICENSE file exists with proper MIT License
  - All .cs files have correct copyright headers with MIT License
  - Consistent licensing across all source files
- ✅ **about line endings**: **COMPLETED**
  - .editorconfig is properly configured for LF line endings
  - `make format` command includes whitespace removal (See PR #19)
- ✅ **lines whose content are only space character**: **COMPLETED**
  - No lines with only spaces found
  - `make format` enhanced to remove all trailing whitespaces (See PR #19)
- ✅ **GitHubAction**: **COMPLETED**
  - Comprehensive CI workflow exists (.github/workflows/ci.yml) with PR checks
  - Build and release workflow exists (.github/workflows/build-wpf.yml)
  - Automated formatting validation and release management
- ✅ **When click textboxes**: **COMPLETED**
  - GotFocus event handlers implemented in MainWindow.xaml.cs
  - Auto-select functionality working for all textboxes
- ✅ **Versioning**: **COMPLETED** (See PR #20)
  - Semantic versioning applied starting from version 0.1.0
  - Version information embedded in executable binaries

### 1. Internationalization and Localization (i18n/l10n)
- **Issue**: Mixed language approach with hardcoded Japanese strings in UI
- **Files**: `MainWindow.xaml.cs:45,49,60,115,121,129`, `MainWindow.xaml:13,15,17,21,26`, `TimerWindow.xaml.cs:93-97,259,384`, `TimerWindow.xaml:43,51,58,66`
- **Action**: 
  - Create `.resx` resource files for Japanese and English
  - Replace all hardcoded strings with resource references
  - Add `x:Uid` attributes to XAML elements for localization

### 2. Extract DPI and Display Calculation Logic
- **Issue**: Duplicate DPI calculation code in `TimerWindow.xaml.cs:106-119,318-330`
- **Files**: `TimerWindow.xaml.cs:SetupWindowPosition()`, `Window_MouseEnter()`
- **Action**:
  - Create `DisplayHelper` or `DpiHelper` utility class
  - Extract common DPI conversion methods
  - Reduce code duplication

### 3. Improve Exception Handling
- **Issue**: Generic `catch` blocks without specific exception handling
- **Files**: `TimerWindow.xaml.cs:266,305`
- **Action**:
  - Replace generic `catch` with specific exception types (e.g., `InvalidOperationException`, `Win32Exception`)
  - Add proper logging for exceptions
  - Consider using `ILogger` interface

## Medium Priority

### 4. Constants Organization
- **Issue**: All constants in single flat class
- **Files**: `Constants.cs`
- **Action**:
  - Group related constants into nested classes:
    ```csharp
    public static class Constants
    {
        public static class Layout { /* TimerBar, Screen dimensions */ }
        public static class Animation { /* Blink, opacity values */ }
        public static class Notification { /* Duration, cleanup delay */ }
        public static class Colors { /* Threshold values */ }
    }
    ```

### 5. XAML Styling and Theming
- **Issue**: Inline styles and hardcoded colors in XAML
- **Files**: `MainWindow.xaml`, `TimerWindow.xaml`
- **Action**:
  - Create WPF Styles and Templates in `App.xaml` or separate ResourceDictionary
  - Extract hardcoded colors to theme resources
  - Implement consistent styling across windows

### 6. Input Validation Refactoring
- **Issue**: Validation logic mixed with UI logic
- **Files**: `MainWindow.xaml.cs:113-131`
- **Action**:
  - Create `TimerInputValidator` class
  - Extract validation methods for better testability
  - Consider using IDataErrorInfo or ValidationRules

## Low Priority

### 7. Position String Mapping
- **Issue**: String-based position mapping in `ParsePosition()`
- **Files**: `TimerWindow.xaml.cs:89-99`
- **Action**:
  - Create static dictionary mapping for better performance
  - Consider using attributes on enum values for localization

### 8. Event Handler Naming Consistency
- **Issue**: Mix of descriptive and generic event handler names
- **Files**: All XAML.cs files
- **Action**:
  - Standardize event handler naming convention
  - Use descriptive names that indicate the action (e.g., `OnStartTimerClicked`)

### 9. Resource Management Enhancement
- **Issue**: Manual resource disposal in notification logic
- **Files**: `TimerWindow.xaml.cs:284-308`
- **Action**:
  - Consider using `using` statements or implement proper async disposal
  - Wrap NotifyIcon usage in disposable wrapper class

### 10. Configuration Management
- **Issue**: No user settings persistence
- **Action**:
  - Add `Settings.settings` file for user preferences
  - Store last used timer duration, position, and display selection
  - Implement `Properties.Settings` pattern

## Technical Debt Notes

### Mixed Framework Usage
- **Current**: Using Windows Forms (`System.Windows.Forms.Screen`, `NotifyIcon`) alongside WPF
- **Justification**: Necessary for multi-monitor support and system notifications
- **Note**: This is acceptable given the functionality requirements

### File Structure
- **Current**: Flat structure with Models subfolder
- **Suggestion**: Consider adding `Services`, `Helpers`, `Resources` folders as the project grows

## Coding Standards Compliance

✅ **Already Following Best Practices:**
- StyleCop compliance with comprehensive rules
- XML documentation throughout
- Proper namespace organization
- Modern C# features (nullable reference types, switch expressions)
- Proper disposal pattern implementation
- Consistent copyright headers