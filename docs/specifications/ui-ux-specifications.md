# UI/UX Specifications

## 1. Design Philosophy

### 1.1 Core Principles
- **Minimalism**: Clean, unobtrusive interface that doesn't interfere with presentations
- **Clarity**: Clear visual hierarchy and intuitive controls
- **Responsiveness**: Immediate feedback for all user interactions
- **Accessibility**: Support for different screen sizes and DPI settings

### 1.2 Design Goals
- Provide instant visual time awareness
- Minimize screen real estate usage
- Maintain consistency across different display configurations
- Ensure visibility without distraction

## 2. Visual Design System

### 2.1 Color Palette
```
Primary Colors:
- Progress Green: #008000 (RGB: 0, 128, 0)
- Warning Orange: #FFA500 (RGB: 255, 165, 0)
- Alert Red: #FF0000 (RGB: 255, 0, 0)
- Pause Blue: #483D8B (RGB: 72, 61, 139) - DarkSlateBlue

Background:
- Transparent: rgba(0, 0, 0, 0)
- Semi-transparent Black: rgba(0, 0, 0, 0.3) for control panels
```

### 2.2 Typography
```
Main Window:
- Title: 24pt, Bold, System Font
- Labels: 12pt, Regular, System Font
- Input Fields: 12pt, Regular, System Font

Timer Window:
- Time Display: 16pt, Bold, System Font
- Buttons: 12pt, Regular, System Font
```

### 2.3 Spacing and Layout
```
Margins:
- Window margins: 20px
- Control spacing: 10px
- Button spacing: 5px

Dimensions:
- Timer bar thickness: 20px
- Expanded control panel: 200px × 150px
- Button minimum size: 60px × 30px
```

## 3. Main Configuration Window

### 3.1 Layout Structure
```
┌─────────────────────────────────┐
│          App Title              │
│                                 │
│   [Time Setting]                │
│   [5] 分 [00] 秒               │
│                                 │
│   [Display Monitor]             │
│   [Primary Display ▼]           │
│                                 │
│   [Position]                    │
│   [Right Edge ▼]                │
│                                 │
│        [Start Button]           │
└─────────────────────────────────┘
```

### 3.2 Interactive Elements
- **Time Input Fields**: 
  - Auto-select text on focus
  - Numeric validation
  - Tab navigation support
- **Dropdown Menus**:
  - Clear option labeling
  - Keyboard navigation
- **Start Button**:
  - Prominent placement
  - Visual feedback on click

### 3.3 Window Properties
```
Size: 400px × 350px
Position: Center screen
Resizable: No
Icon: Application icon
Title Bar: Standard Windows title bar
```

## 4. Timer Display Window

### 4.1 Normal State Design
```
Right/Left Position (Vertical):
┌──┐
│██│ ← Progress bar (fills bottom-to-top)
│██│
│░░│ ← Remaining space (transparent)
│░░│
└──┘
Width: 20px, Height: Screen height × 0.8

Top/Bottom Position (Horizontal):
┌──────────────┐
│██████░░░░░░░░│ ← Progress fills left-to-right
└──────────────┘
Width: Screen width × 0.8, Height: 20px
```

### 4.2 Hover State Design
```
Expanded Control Panel:
┌─────────────┐
│    5:23     │ ← Time display
│             │
│ [Pause] [Stop] │ ← Control buttons
│   [Close]   │
└─────────────┘
```

### 4.3 Visual States
- **Running**: Progress color based on elapsed time
- **Paused**: Dark slate blue color, static display
- **Hover**: Expanded control panel with buttons
- **Blinking**: Red color with opacity animation (80-100% elapsed)

## 5. Progress Visualization

### 5.1 Progress Bar Behavior
```
Vertical Bars (Left/Right edges):
- Fill direction: Bottom → Top
- Full height: Screen height × 0.8
- Progress height = Full height × (elapsed/total)

Horizontal Bars (Top/Bottom edges):
- Fill direction: Left → Right  
- Full width: Screen width × 0.8
- Progress width = Full width × (elapsed/total)
```

### 5.2 Color Transitions
```
Time Remaining Zones:
- 100% - 40% remaining: Green (#008000)
- 40% - 20% remaining: Orange (#FFA500)  
- 20% - 0% remaining: Red (#FF0000) + Blinking
- Paused state: Dark Slate Blue (#483D8B)
```

### 5.3 Animation Effects
```css
Blinking Animation (CSS equivalent):
@keyframes blink {
  0% { opacity: 0.3; }
  50% { opacity: 1.0; }
  100% { opacity: 0.3; }
}
Duration: 1000ms
Iteration: Infinite
```

## 6. Responsive Design

### 6.1 DPI Scaling
- Automatic detection of system DPI settings
- Consistent physical size across different DPI levels
- Logical pixel calculations for precise positioning

### 6.2 Multi-Monitor Support
```
Display Handling:
1. Enumerate all connected displays
2. Calculate logical bounds for each display
3. Position timer within selected display bounds
4. Handle DPI differences between displays
```

### 6.3 Screen Size Adaptations
```
Minimum Requirements:
- Screen width: 1024px
- Screen height: 768px
- Timer bar maintains 20px thickness
- Control panel minimum 150px × 100px
```

## 7. Interaction Design

### 7.1 Mouse Interactions
```
Timer Window:
- Hover: Show control panel (immediate)
- Leave: Hide control panel (immediate)
- Click buttons: Execute action + visual feedback

Main Window:
- Click input fields: Auto-select text
- Tab navigation: Logical flow through controls
- Enter key: Start timer (when focused on Start button)
```

### 7.2 Keyboard Shortcuts
```
Main Window:
- Tab: Navigate between controls
- Enter: Start timer
- Escape: Close application

Timer Window:
- Space: Pause/Resume (when control panel visible)
- Escape: Stop timer and return to main window
```

### 7.3 Touch Support
- Touch-friendly button sizes (minimum 44px)
- Hover state triggered by touch
- Accessible touch targets for control panel

## 8. Accessibility Features

### 8.1 Visual Accessibility
- High contrast color combinations
- Clear visual hierarchy
- Consistent iconography and labeling
- Scalable UI elements

### 8.2 Localization Support
```
Supported Languages:
- Japanese (ja-JP): Primary language
- English (en): Default fallback
- Simplified Chinese (zh-CN)
- Traditional Chinese (zh-TW)

Text Elements:
- All UI text externalized to resource files
- Automatic language detection
- Consistent terminology across languages
```

### 8.3 Error Communication
- Clear, actionable error messages
- Visual error indicators
- Non-blocking error notifications
- Graceful degradation for failed operations

## 9. Performance Considerations

### 9.1 UI Responsiveness
- 60 FPS target for animations
- <100ms response time for interactions
- Efficient redraw operations
- Minimal CPU usage during idle state

### 9.2 Memory Efficiency
- Lazy loading of resources
- Proper disposal of graphics resources
- Minimal texture memory usage
- Efficient window management

## 10. Platform Integration

### 10.1 Windows Integration
- Native Windows look and feel
- System theme compatibility
- Proper z-order management (always on top)
- Integration with Windows notification system

### 10.2 Hardware Considerations
- Support for multiple graphics adapters
- High refresh rate display compatibility
- Hardware acceleration when available
- Graceful fallback for limited hardware