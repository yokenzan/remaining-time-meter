# Technical Specifications

## 1. Architecture Overview

### 1.1 Technology Stack
- **Framework**: .NET 8.0 + WPF (Windows Presentation Foundation)
- **Language**: C# 12.0
- **Target Platform**: Windows 10/11 (x64, x86)
- **UI Framework**: XAML + Code-behind pattern

### 1.2 Application Architecture
```
RemainingTimeMeter/
├── Entry Point (App.xaml/App.xaml.cs)
├── Main Configuration Window (MainWindow)
├── Timer Display Window (TimerWindow)
├── Models/ (Data structures)
├── Helpers/ (Utility classes)
└── Properties/ (Resources & configuration)
```

## 2. Core Components

### 2.1 Application Entry Point
- **File**: `App.xaml`, `App.xaml.cs`
- **Purpose**: Application lifecycle management
- **Features**: 
  - Startup configuration
  - Global exception handling
  - Resource management

### 2.2 Main Configuration Window
- **File**: `MainWindow.xaml`, `MainWindow.xaml.cs`
- **Purpose**: Timer configuration interface
- **Key Functions**:
  - Timer duration input validation
  - Display enumeration and selection
  - Position selection
  - Timer window instantiation

### 2.3 Timer Display Window
- **File**: `TimerWindow.xaml`, `TimerWindow.xaml.cs`
- **Purpose**: Visual timer display and control
- **Key Functions**:
  - Progress visualization
  - Window positioning and sizing
  - Control panel management
  - Timer state management

## 3. Data Models

### 3.1 DisplayInfo
- **File**: `Models/DisplayInfo.cs`
- **Purpose**: Display configuration data
- **Properties**:
  ```csharp
  public class DisplayInfo
  {
      public int Width { get; set; }
      public int Height { get; set; }
      public int Left { get; set; }
      public int Top { get; set; }
      public bool IsPrimary { get; set; }
      public double ScaleX { get; set; }
      public double ScaleY { get; set; }
  }
  ```

### 3.2 TimerPosition
- **File**: `Models/TimerPosition.cs`
- **Purpose**: Timer positioning enumeration
- **Values**: `Right`, `Left`, `Top`, `Bottom`

## 4. Helper Classes

### 4.1 DisplayHelper
- **File**: `Helpers/DisplayHelper.cs`
- **Purpose**: DPI-aware display calculations
- **Key Methods**:
  - `GetLogicalScreenBounds()`: Converts physical to logical coordinates
  - DPI scaling calculations
  - Multi-monitor support

### 4.2 Logger
- **File**: `Logger.cs`
- **Purpose**: Application logging and diagnostics
- **Features**:
  - Multiple log levels (Debug, Info, Warning, Error)
  - File-based logging with rotation
  - Windows Event Log fallback
  - Exception handling and recovery

## 5. Configuration and Constants

### 5.1 Constants
- **File**: `Constants.cs`
- **Purpose**: Application-wide configuration values
- **Categories**:
  - UI dimensions and margins
  - Color thresholds
  - Animation parameters
  - Notification settings

### 5.2 Resource Management
- **Files**: `Properties/Resources.*`
- **Purpose**: Internationalization and localization
- **Supported Languages**:
  - English (default): `Resources.resx`
  - Japanese: `Resources.ja-JP.resx`
  - Simplified Chinese: `Resources.zh-CN.resx`
  - Traditional Chinese: `Resources.zh-TW.resx`

## 6. Window Management

### 6.1 Timer Window Positioning
```csharp
private void SetupWindowPosition()
{
    var logicalBounds = DisplayHelper.GetLogicalScreenBounds(targetDisplay, this);
    
    switch (position)
    {
        case TimerPosition.Right:
            // Position at right edge with margin
        case TimerPosition.Left:
            // Position at left edge with margin
        case TimerPosition.Top:
            // Position at top edge with margin
        case TimerPosition.Bottom:
            // Position at bottom edge with margin
    }
}
```

### 6.2 DPI Awareness
- Automatic DPI scaling detection
- Logical pixel calculations for consistent sizing
- Support for mixed-DPI multi-monitor setups

## 7. Timer Implementation

### 7.1 Timer Mechanism
```csharp
private readonly DispatcherTimer timer;
// Interval: 1 second
// Event: Timer_Tick - updates display and progress
```

### 7.2 Progress Calculation
```csharp
double progress = (double)(totalSeconds - remainingSeconds) / totalSeconds;
```

### 7.3 Visual Updates
- Progress bar size/position updates
- Color transitions based on progress thresholds
- Opacity animations for blinking effect

## 8. Exception Handling Strategy

### 8.1 Specific Exception Types
- `InvalidOperationException`: UI state errors
- `ArgumentException`: Invalid input parameters
- `Win32Exception`: Windows API failures
- `XamlParseException`: UI loading errors
- `OutOfMemoryException`: Critical system errors

### 8.2 Error Recovery
- Graceful degradation for non-critical failures
- User notification for actionable errors
- Automatic fallback mechanisms (e.g., MessageBox for failed notifications)

## 9. Build Configuration

### 9.1 Project Settings
```xml
<TargetFramework>net8.0-windows</TargetFramework>
<UseWPF>true</UseWPF>
<PublishSingleFile>true</PublishSingleFile>
<SelfContained>false</SelfContained>
<RuntimeIdentifier>win-x64</RuntimeIdentifier>
```

### 9.2 Build Variants
- **Framework-dependent**: Requires .NET 8.0 Runtime (~5MB)
- **Self-contained**: Includes runtime (~150MB)
- **Architectures**: x64, x86

### 9.3 Code Quality
- **StyleCop**: Code style analysis
- **Analyzers**: Static code analysis
- **XML Documentation**: Comprehensive API documentation

## 10. Performance Characteristics

### 10.1 Memory Usage
- Base application: ~15-20MB
- Timer operation: +5-10MB
- Resource cleanup on timer stop

### 10.2 CPU Usage
- Idle: <1% CPU
- Timer running: <2% CPU (1-second updates)
- UI interactions: Brief spikes <5%

### 10.3 Startup Performance
- Cold start: 1-2 seconds
- Warm start: <1 second
- Timer window creation: <500ms

## 11. Security Considerations

### 11.1 Permissions
- Standard user permissions sufficient
- No administrator privileges required
- No network access needed

### 11.2 Data Security
- No sensitive data storage
- Configuration stored in user profile
- Minimal system integration footprint

## 12. Deployment

### 12.1 Distribution Methods
- GitHub Releases (primary)
- Direct executable download
- No installer required (portable application)

### 12.2 System Requirements
- Windows 10 version 1809+ or Windows 11
- .NET 8.0 Desktop Runtime (framework-dependent builds)
- 100MB free disk space
- 512MB RAM minimum