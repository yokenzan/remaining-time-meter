# Timer Input Enhancement Specification

## Overview

Enhance the timer setup UI to be more mouse-friendly for users who are already in fullscreen presentation mode and need to quickly configure the timer using only mouse interactions.

## Current Limitations

1. **Keyboard dependency**: Current UI requires typing in separate minute/second fields
2. **Cumbersome during presentations**: Users in fullscreen mode prefer mouse-only interaction
3. **Limited input format**: No support for quick numeric input like "0500" → "05:00"
4. **Maximum time unclear**: Current validation allows up to 59 seconds, needs 99:99 support

## Enhanced Requirements

### 1. Mouse-Friendly Controls
- **Quick Time Buttons**: Preset time buttons (1min, 5min, 10min, 15min, 30min, 60min)
- **Increment/Decrement Buttons**: +/- buttons for fine adjustment
- **Large Click Targets**: All interactive elements should be easily clickable

### 2. Flexible Input Format
- **Single Field Input**: Allow "0500" input that automatically formats to "05:00"
- **Multiple Formats**: Support various input formats:
  - `0500` → `05:00` (5 minutes)
  - `1030` → `10:30` (10 minutes 30 seconds)
  - `90` → `01:30` (90 seconds = 1 minute 30 seconds)
  - `5` → `05:00` (5 minutes)

### 3. Extended Time Range
- **Maximum Time**: Support up to 99 minutes 99 seconds (99:99)
- **Validation**: Ensure seconds don't exceed 99 (will auto-convert to minutes)

### 4. Visual Feedback
- **Real-time Preview**: Show formatted time as user types
- **Clear Display**: Large, readable time display
- **Visual States**: Highlight active controls and validation states

## UI Design Specification

### Layout Options

#### Option A: Enhanced Current Layout
```
Time Setting: [05:00] [+1] [-1] [+10] [-10]
Quick Times: [1min] [5min] [10min] [15min] [30min] [60min]
```

#### Option B: Compact Time Picker
```
Time: [MM:SS Display]
      [▲] [▲]  ← Increment buttons
      [▼] [▼]  ← Decrement buttons
      Min Sec

Quick: [1] [5] [10] [15] [30] [60] minutes
```

#### Option C: Hybrid Approach (Recommended)
```
Time Input: [    MMSS    ] → [05:00]
            [Quick Entry Field]  [Formatted Display]

Adjustments: [-10] [-1] [+1] [+10] minutes
             [-30] [-5] [+5] [+30] seconds

Quick Times: [1min] [5min] [10min] [15min] [30min] [60min]
```

## Implementation Details

### Input Processing Logic
```csharp
private (int minutes, int seconds) ParseTimeInput(string input)
{
    // Remove any non-numeric characters
    string numericOnly = Regex.Replace(input, @"[^\d]", "");
    
    if (int.TryParse(numericOnly, out int value))
    {
        if (value <= 99) // Single or double digit - treat as minutes
        {
            return (value, 0);
        }
        else if (value <= 9999) // 3-4 digits - treat as MMSS
        {
            int minutes = value / 100;
            int seconds = value % 100;
            
            // Convert excess seconds to minutes
            if (seconds >= 60)
            {
                minutes += seconds / 60;
                seconds = seconds % 60;
            }
            
            // Ensure within limits
            if (minutes > 99) minutes = 99;
            
            return (minutes, seconds);
        }
        else // 5+ digits - treat as total seconds
        {
            int totalSeconds = Math.Min(value, 99 * 60 + 99); // Max 99:99
            return (totalSeconds / 60, totalSeconds % 60);
        }
    }
    
    return (5, 0); // Default fallback
}
```

### Quick Time Buttons
```csharp
private void QuickTimeButton_Click(object sender, RoutedEventArgs e)
{
    if (sender is Button button && button.Tag is int minutes)
    {
        SetTime(minutes, 0);
    }
}

private void SetTime(int minutes, int seconds)
{
    // Ensure within bounds
    minutes = Math.Min(Math.Max(0, minutes), 99);
    seconds = Math.Min(Math.Max(0, seconds), 99);
    
    // Convert excess seconds to minutes
    if (seconds >= 60)
    {
        minutes += seconds / 60;
        seconds = seconds % 60;
    }
    
    // Update UI
    UpdateTimeDisplay(minutes, seconds);
}
```

### Increment/Decrement Logic
```csharp
private void AdjustTime(int minuteDelta, int secondDelta)
{
    var (currentMinutes, currentSeconds) = GetCurrentTime();
    
    int newMinutes = currentMinutes + minuteDelta;
    int newSeconds = currentSeconds + secondDelta;
    
    // Handle second overflow/underflow
    if (newSeconds >= 60)
    {
        newMinutes += newSeconds / 60;
        newSeconds = newSeconds % 60;
    }
    else if (newSeconds < 0)
    {
        int minutesToBorrow = (-newSeconds + 59) / 60;
        newMinutes -= minutesToBorrow;
        newSeconds += minutesToBorrow * 60;
    }
    
    // Ensure within bounds
    newMinutes = Math.Min(Math.Max(0, newMinutes), 99);
    if (newMinutes == 0 && newSeconds <= 0)
    {
        newSeconds = 0; // Don't allow negative time
    }
    
    SetTime(newMinutes, newSeconds);
}
```

## User Experience Flow

### Scenario 1: Quick Setup During Presentation
1. User opens timer app while in presentation mode
2. Clicks "10min" quick button → Timer set to 10:00
3. Clicks "Start" → Timer begins immediately

### Scenario 2: Custom Time Entry
1. User clicks in time input field
2. Types "0730" → Display shows "07:30"
3. Clicks "+1" button twice → Display shows "09:30"
4. Clicks "Start" → Timer begins

### Scenario 3: Fine Adjustment
1. User sees current time is "05:00"
2. Clicks "+1 min" three times → "08:00"
3. Clicks "+30 sec" → "08:30"
4. Clicks "Start" → Timer begins

## Accessibility Considerations

### Mouse-Only Operation
- All functionality accessible via mouse clicks
- No required keyboard input
- Large click targets (minimum 44px)

### Visual Clarity
- High contrast for time display
- Clear button labels
- Visual feedback for button presses

### Error Prevention
- Real-time validation and formatting
- Prevent invalid time entry
- Clear visual indication of current time

## Implementation Priority

### Phase 1: Core Functionality
1. Single time input field with smart parsing
2. Basic increment/decrement buttons
3. Quick time preset buttons
4. Updated validation for 99:99 maximum

### Phase 2: Enhanced UX
1. Real-time formatting preview
2. Visual polish and animations
3. Keyboard shortcuts (optional)
4. Touch-friendly sizing

### Phase 3: Advanced Features
1. Custom preset configuration
2. Time input history
3. Advanced keyboard shortcuts
4. Context-sensitive defaults