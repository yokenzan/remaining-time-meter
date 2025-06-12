# API Reference

## Overview

This document provides a comprehensive reference for the Remaining Time Meter application's internal APIs and extensible components.

## Core Classes

### App Class
**Namespace**: `RemainingTimeMeter`  
**File**: `App.xaml.cs`

Application entry point and lifecycle management.

#### Methods
```csharp
protected override void OnStartup(StartupEventArgs e)
```
Handles application startup and initialization.

#### Events
- **Startup**: Application startup event
- **Exit**: Application shutdown event

---

### MainWindow Class
**Namespace**: `RemainingTimeMeter`  
**File**: `MainWindow.xaml.cs`

Main configuration window for timer setup.

#### Constructor
```csharp
public MainWindow()
```
Initializes the main window and loads display information.

#### Methods

##### LoadDisplays()
```csharp
private void LoadDisplays()
```
Populates the display dropdown with available monitors.

**Exceptions**:
- `Win32Exception`: Display enumeration failure
- `InvalidOperationException`: UI operation failure

##### GetDisplayInformation()
```csharp
private List<DisplayInfo> GetDisplayInformation()
```
**Returns**: `List<DisplayInfo>` - Available display configurations

##### CreateDefaultDisplayEntry()
```csharp
private void CreateDefaultDisplayEntry()
```
Creates fallback display entry when enumeration fails.

#### Event Handlers

##### StartButton_Click()
```csharp
private void StartButton_Click(object sender, RoutedEventArgs e)
```
Handles timer start button click and validates input.

**Exceptions**:
- `ArgumentException`: Invalid input parameters
- `InvalidOperationException`: UI state error
- `Win32Exception`: Windows API error

##### TextBox_GotFocus()
```csharp
private void TextBox_GotFocus(object sender, RoutedEventArgs e)
```
Auto-selects text when textbox receives focus.

##### TextBox_PreviewMouseLeftButtonDown()
```csharp
private void TextBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
```
Handles mouse click behavior for textbox focus and selection.

---

### TimerWindow Class
**Namespace**: `RemainingTimeMeter`  
**File**: `TimerWindow.xaml.cs`

Timer display window with progress visualization and controls.

#### Constructor
```csharp
public TimerWindow(int totalSeconds, string position, DisplayInfo targetDisplay)
```
**Parameters**:
- `totalSeconds`: Timer duration in seconds
- `position`: Timer position ("右端", "左端", "上端", "下端")
- `targetDisplay`: Target display configuration

#### Properties

##### MainWindowRequested Event
```csharp
public event Action? MainWindowRequested
```
Triggered when returning to main window is requested.

#### Methods

##### ParsePosition()
```csharp
private TimerPosition ParsePosition(string position)
```
**Parameters**: `position` - Position string in user's language
**Returns**: `TimerPosition` - Corresponding enum value

##### SetupWindowPosition()
```csharp
private void SetupWindowPosition()
```
Calculates and sets window position based on display and position settings.

##### UpdateTimeDisplay()
```csharp
private void UpdateTimeDisplay()
```
Updates the time display text with current remaining time.

##### UpdateProgressBar()
```csharp
private void UpdateProgressBar()
```
Updates progress bar size and position based on elapsed time.

##### UpdateBarColor()
```csharp
private void UpdateBarColor()
```
Updates progress bar color based on elapsed time and pause state.

##### ShowTimeUpNotification()
```csharp
private void ShowTimeUpNotification()
```
Displays completion notification and returns to main window.

##### ShowWindowsNotification()
```csharp
private void ShowWindowsNotification(string title, string message)
```
**Parameters**:
- `title`: Notification title
- `message`: Notification message

Displays Windows system notification.

**Exceptions**:
- `Win32Exception`: Windows API error
- `ArgumentException`: Invalid parameter
- `InvalidOperationException`: Notification system error

#### Event Handlers

##### Timer_Tick()
```csharp
private void Timer_Tick(object? sender, EventArgs e)
```
Handles timer tick events and updates display.

##### Window_MouseEnter()
```csharp
private void Window_MouseEnter(object sender, MouseEventArgs e)
```
Expands window to show control panel on mouse hover.

##### Window_MouseLeave()
```csharp
private void Window_MouseLeave(object sender, MouseEventArgs e)
```
Collapses window and hides control panel when mouse leaves.

##### PauseResumeButton_Click()
```csharp
private void PauseResumeButton_Click(object sender, RoutedEventArgs e)
```
Toggles timer pause/resume state.

##### StopButton_Click()
```csharp
private void StopButton_Click(object sender, RoutedEventArgs e)
```
Stops timer and returns to main window.

##### CloseButton_Click()
```csharp
private void CloseButton_Click(object sender, RoutedEventArgs e)
```
Closes timer window and returns to main window.

---

## Data Models

### DisplayInfo Class
**Namespace**: `RemainingTimeMeter.Models`  
**File**: `Models/DisplayInfo.cs`

Represents display configuration information.

#### Properties
```csharp
public int Width { get; set; }        // Display width in pixels
public int Height { get; set; }       // Display height in pixels  
public int Left { get; set; }         // Left coordinate
public int Top { get; set; }          // Top coordinate
public bool IsPrimary { get; set; }   // Primary display flag
public double ScaleX { get; set; }    // X-axis DPI scale factor
public double ScaleY { get; set; }    // Y-axis DPI scale factor
```

### TimerPosition Enum
**Namespace**: `RemainingTimeMeter.Models`  
**File**: `Models/TimerPosition.cs`

Timer positioning options.

#### Values
```csharp
public enum TimerPosition
{
    Right,   // Right edge of screen
    Left,    // Left edge of screen
    Top,     // Top edge of screen
    Bottom   // Bottom edge of screen
}
```

---

## Helper Classes

### DisplayHelper Class
**Namespace**: `RemainingTimeMeter.Helpers`  
**File**: `Helpers/DisplayHelper.cs`

Utility class for DPI-aware display calculations.

#### Methods

##### GetLogicalScreenBounds()
```csharp
public static Rect GetLogicalScreenBounds(DisplayInfo display, Visual visual)
```
**Parameters**:
- `display`: Physical display information
- `visual`: Visual element for DPI context

**Returns**: `Rect` - Logical screen bounds in WPF units

Converts physical display coordinates to logical WPF coordinates considering DPI scaling.

---

### Logger Class
**Namespace**: `RemainingTimeMeter`  
**File**: `Logger.cs`

Application logging and diagnostics.

#### Methods

##### Debug()
```csharp
public static void Debug(string message)
```
Logs debug-level message.

##### Info()
```csharp
public static void Info(string message)
```
Logs informational message.

##### Warning()
```csharp
public static void Warning(string message)
```
Logs warning message.

##### Error()
```csharp
public static void Error(string message)
public static void Error(string message, Exception exception)
```
Logs error message with optional exception details.

#### Configuration
- **Log File**: `%APPDATA%/RemainingTimeMeter/logs/`
- **Log Rotation**: Daily rotation with 30-day retention
- **Fallback**: Windows Event Log for critical errors

---

## Constants

### Constants Class
**Namespace**: `RemainingTimeMeter`  
**File**: `Constants.cs`

Application-wide configuration constants.

#### UI Constants
```csharp
public const double TimerBarWidth = 20.0;         // Vertical bar width
public const double TimerBarHeight = 20.0;        // Horizontal bar height
public const double ExpandedWidth = 200.0;        // Expanded control panel width
public const double ExpandedHeight = 150.0;       // Expanded control panel height
public const double ScreenMargin = 10.0;          // Screen edge margin
public const double BottomMargin = 50.0;          // Bottom edge additional margin
public const double ScreenSizeRatio = 0.8;        // Screen coverage ratio
```

#### Color Thresholds
```csharp
public const double OrangeThreshold = 0.6;        // 60% elapsed -> orange
public const double RedThreshold = 0.8;           // 80% elapsed -> red
```

#### Animation Constants
```csharp
public const double BlinkMinOpacity = 0.3;        // Minimum blink opacity
public const double BlinkMaxOpacity = 1.0;        // Maximum blink opacity
public const int BlinkAnimationDuration = 1000;   // Blink cycle duration (ms)
```

#### Notification Settings
```csharp
public const int NotificationDuration = 5000;     // Notification display time (ms)
public const int NotificationCleanupDelay = 6000; // Cleanup delay (ms)
```

---

## Resource Management

### Resources Class
**Namespace**: `RemainingTimeMeter.Properties`  
**File**: `Properties/Resources.Designer.cs`

Generated class for accessing localized resources.

#### Key Resources
```csharp
public static string AppTitle { get; }           // "残り時間メーター"
public static string TimeSetting { get; }        // "時間設定"
public static string Minutes { get; }            // "分"
public static string Seconds { get; }            // "秒"
public static string DisplayMonitor { get; }     // "表示モニター"
public static string Position { get; }           // "位置"
public static string PositionRight { get; }      // "右端"
public static string PositionLeft { get; }       // "左端"
public static string PositionTop { get; }        // "上端"
public static string PositionBottom { get; }     // "下端"
public static string Start { get; }              // "開始"
public static string Pause { get; }              // "一時停止"
public static string Resume { get; }             // "再開"
public static string Stop { get; }               // "停止"
public static string Timer { get; }              // "タイマー"
public static string Error { get; }              // "エラー"
```

#### Error Messages
```csharp
public static string InvalidMinutesValue { get; }    // Input validation errors
public static string InvalidSecondsValue { get; }    
public static string PleaseSetTimeCorrectly { get; }
public static string ErrorOccurred { get; }          // "{0}エラーが発生しました"
```

#### Notification Messages
```csharp
public static string TimeUpMessage { get; }      // "時間です！{0}分{1}秒経過しました！"
```

#### Display Formatting
```csharp
public static string DisplayFormat { get; }         // "ディスプレイ {0} - {1}x{2}"
public static string DisplayFormatPrimary { get; }  // "ディスプレイ {0} - {1}x{2} (メイン)"
```

---

## Extension Points

### Custom Positioning
To add new timer positions:

1. **Add enum value** to `TimerPosition`
2. **Update ParsePosition()** in `TimerWindow.cs`
3. **Add case** to `SetupWindowPosition()` switch statement
4. **Add resource** strings for new position
5. **Update XAML** ComboBox items

### Custom Notifications
To implement custom notification systems:

1. **Inherit** from base notification interface
2. **Implement** `ShowNotification(string title, string message)`
3. **Add fallback** logic in `ShowTimeUpNotification()`
4. **Handle exceptions** appropriately

### Logging Extensions
To add custom logging destinations:

1. **Extend Logger class** with new methods
2. **Implement** custom log writers
3. **Add configuration** options
4. **Maintain fallback** behavior

---

## Error Handling Patterns

### Exception Hierarchy
```csharp
// UI-related exceptions
InvalidOperationException     // UI state errors
ArgumentException            // Parameter validation
XamlParseException          // UI loading errors

// System-related exceptions  
Win32Exception              // Windows API failures
UnauthorizedAccessException // Permission errors
OutOfMemoryException        // Critical system errors
```

### Error Recovery Strategy
1. **Log error** with appropriate level
2. **Attempt graceful degradation** when possible
3. **Show user-friendly message** for actionable errors
4. **Fallback to alternatives** (e.g., MessageBox for notifications)
5. **Continue operation** if non-critical

### Best Practices
- Use specific exception types
- Provide meaningful error messages
- Log sufficient context for debugging
- Implement fallback mechanisms
- Avoid exposing technical details to users