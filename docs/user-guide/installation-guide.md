# Installation Guide

## System Requirements

### Minimum Requirements
- **Operating System**: Windows 10 version 1809 or later, Windows 11
- **Architecture**: x64 (64-bit) or x86 (32-bit)
- **Memory**: 512 MB RAM
- **Storage**: 100 MB free disk space
- **Display**: Minimum resolution 1024×768

### Recommended Requirements
- **Memory**: 2 GB RAM or more
- **Storage**: 1 GB free disk space
- **Display**: 1920×1080 or higher resolution
- **Multiple displays**: For multi-monitor features

## Download Options

### Framework-Dependent Version (Recommended)
**Smaller file size, requires .NET runtime**

1. **64-bit Windows**: `RemainingTimeMeter-framework-dependent-win-x64.exe` (~5MB)
2. **32-bit Windows**: `RemainingTimeMeter-framework-dependent-win-x86.exe` (~5MB)

**Requirements**: .NET 8.0 Desktop Runtime must be installed

### Self-Contained Version
**Larger file size, no additional requirements**

1. **64-bit Windows**: `RemainingTimeMeter-self-contained-win-x64.exe` (~150MB)
2. **32-bit Windows**: `RemainingTimeMeter-self-contained-win-x86.exe` (~150MB)

**Requirements**: None (runtime included)

## Installation Steps

### Option 1: Framework-Dependent Installation

#### Step 1: Download the Application
1. Go to the [Releases page](https://github.com/yokenzan/remaining-time-meter/releases)
2. Download the appropriate framework-dependent version for your system:
   - For 64-bit Windows: `RemainingTimeMeter-framework-dependent-win-x64.exe`
   - For 32-bit Windows: `RemainingTimeMeter-framework-dependent-win-x86.exe`

#### Step 2: Install .NET 8.0 Desktop Runtime (if needed)
1. Try running the downloaded executable
2. If you get a runtime error, you'll be directed to download .NET 8.0
3. Visit [.NET 8.0 Download Page](https://dotnet.microsoft.com/download/dotnet/8.0)
4. Download and install ".NET Desktop Runtime 8.0" for your architecture
5. Restart your computer after installation

#### Step 3: Run the Application
1. Double-click the downloaded executable
2. The application should start immediately
3. No additional installation steps required

### Option 2: Self-Contained Installation

#### Step 1: Download the Application
1. Go to the [Releases page](https://github.com/yokenzan/remaining-time-meter/releases)
2. Download the appropriate self-contained version:
   - For 64-bit Windows: `RemainingTimeMeter-self-contained-win-x64.exe`
   - For 32-bit Windows: `RemainingTimeMeter-self-contained-win-x86.exe`

#### Step 2: Run the Application
1. Double-click the downloaded executable
2. The application will start immediately
3. No additional installation or runtime required

## Determining Your Windows Architecture

### Method 1: System Information
1. Press `Windows + R` to open Run dialog
2. Type `msinfo32` and press Enter
3. Look for "System Type" in the System Summary
   - **x64-based PC**: Use 64-bit version
   - **x86-based PC**: Use 32-bit version

### Method 2: Settings App
1. Open Windows Settings (`Windows + I`)
2. Go to System → About
3. Under "Device specifications", look at "System type"
   - **64-bit operating system**: Use 64-bit version
   - **32-bit operating system**: Use 32-bit version

### Method 3: Control Panel
1. Open Control Panel
2. Go to System and Security → System
3. Look at "System type"

## First Launch

### Language Selection
The application automatically detects your system language and uses:
- **Japanese** for Japanese systems
- **English** for English systems
- **Chinese (Simplified)** for Chinese (China) systems
- **Chinese (Traditional)** for Chinese (Taiwan/Hong Kong) systems
- **English** as fallback for other languages

### Initial Configuration
1. **Time Setting**: Default is 5 minutes 00 seconds
2. **Display Selection**: Primary display is selected by default
3. **Position**: Right edge is selected by default
4. **Start**: Click the Start button to begin your first timer

## Portable Installation

The application is fully portable and doesn't require installation:

1. **Create Application Folder**: 
   ```
   C:\Tools\RemainingTimeMeter\
   ```

2. **Place Executable**: Move the downloaded .exe file to this folder

3. **Create Desktop Shortcut** (optional):
   - Right-click on the .exe file
   - Select "Create shortcut"
   - Move the shortcut to your desktop

4. **Add to PATH** (optional for command-line usage):
   - Add the application folder to your system PATH
   - Run from command line with `RemainingTimeMeter`

## Troubleshooting Installation Issues

### Issue: "Windows protected your PC" message
**Solution**: This is Windows SmartScreen protection
1. Click "More info"
2. Click "Run anyway"
3. This occurs because the executable isn't digitally signed

### Issue: ".NET Runtime not found" error
**Solution**: Install .NET 8.0 Desktop Runtime
1. Visit [.NET 8.0 Download](https://dotnet.microsoft.com/download/dotnet/8.0)
2. Download ".NET Desktop Runtime 8.0"
3. Install and restart your computer

### Issue: Application won't start
**Solutions**:
1. **Check architecture**: Ensure you downloaded the correct version (x64/x86)
2. **Run as administrator**: Right-click → "Run as administrator"
3. **Check antivirus**: Temporarily disable antivirus and try again
4. **Windows updates**: Ensure Windows is up to date

### Issue: High DPI display problems
**Solution**: Windows should handle DPI scaling automatically
1. If text appears too small/large:
   - Right-click the .exe file
   - Properties → Compatibility tab
   - Check "Override high DPI scaling behavior"
   - Select "System" or "System (Enhanced)"

### Issue: Multiple displays not detected
**Solutions**:
1. **Refresh displays**: Windows + P → Extend
2. **Update graphics drivers**: Update your graphics card drivers
3. **Display settings**: Ensure displays are properly configured in Windows settings

## Uninstallation

Since this is a portable application:

1. **Close the application** if it's running
2. **Delete the executable file** from your chosen location
3. **Remove shortcuts** (desktop, start menu) if created
4. **Clean up settings** (optional):
   - Settings are stored in Windows user profile
   - Usually no manual cleanup needed

## Advanced Installation Options

### Silent Installation for Organizations
For deployment in enterprise environments:

```cmd
REM Copy to standard location
copy "RemainingTimeMeter-framework-dependent-win-x64.exe" "C:\Program Files\RemainingTimeMeter\"

REM Create Start Menu shortcut
powershell -Command "& {$WshShell = New-Object -comObject WScript.Shell; $Shortcut = $WshShell.CreateShortcut('C:\ProgramData\Microsoft\Windows\Start Menu\Programs\Remaining Time Meter.lnk'); $Shortcut.TargetPath = 'C:\Program Files\RemainingTimeMeter\RemainingTimeMeter-framework-dependent-win-x64.exe'; $Shortcut.Save()}"
```

### Network Drive Installation
The application can run from network drives:

1. Copy the executable to a network location
2. Ensure users have read/execute permissions
3. Create shortcuts pointing to the network location
4. Note: Performance may be slower over network

## Version Updates

### Checking for Updates
1. Visit the [Releases page](https://github.com/yokenzan/remaining-time-meter/releases)
2. Compare version numbers with your current installation
3. Download newer version if available

### Updating Process
1. Close the current application
2. Download the new version
3. Replace the old executable with the new one
4. Start the new version

### Automatic Updates
Currently, the application doesn't include automatic update functionality. Manual updates are required.