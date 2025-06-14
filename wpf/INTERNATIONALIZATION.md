# Internationalization Testing

This document explains how to test the internationalization features of the Remaining Time Meter application.

## Supported Languages

The application supports the following languages:

- **English** (`en-US`) - Default
- **Japanese** (`ja-JP`) - 日本語
- **Simplified Chinese** (`zh-CN`) - 简体中文
- **Traditional Chinese** (`zh-TW`) - 繁體中文

## Testing Methods

### Method 1: Command Line Arguments

You can force a specific language when starting the application:

```bash
# Test English
RemainingTimeMeter.exe --lang en-US

# Test Japanese
RemainingTimeMeter.exe --lang ja-JP

# Test Simplified Chinese
RemainingTimeMeter.exe --lang zh-CN

# Test Traditional Chinese
RemainingTimeMeter.exe --lang zh-TW

# Short form also works
RemainingTimeMeter.exe -l ja-JP
```

### Method 2: Environment Variable

Set the `TIMER_LANG` environment variable before starting the application:

```bash
# Windows Command Prompt
set TIMER_LANG=zh-CN
RemainingTimeMeter.exe

# Windows PowerShell
$env:TIMER_LANG="zh-TW"
.\RemainingTimeMeter.exe

# Linux/WSL
export TIMER_LANG=ja-JP
./RemainingTimeMeter.exe
```

### Method 3: System Locale (Default Behavior)

If no language override is specified, the application automatically detects and uses the system locale if supported.

## What Gets Translated

When testing different languages, verify that the following elements change:

### Main Window
- Window title
- Button labels ("Start")
- Position labels ("Right edge", "Left edge", etc.)
- Display monitor text
- Quick time button labels

### Timer Window
- Control panel buttons ("Pause", "Resume", "Stop")
- Time up notification message

### Error Messages
- Input validation errors
- System error dialogs

## Testing Procedure

1. **Start with default language** (your system locale)
2. **Test each override method** with different language codes
3. **Verify all UI text changes** immediately on startup
4. **Test timer functionality** works identically in all languages
5. **Test notifications** show in the correct language
6. **Verify error handling** shows localized error messages

## Troubleshooting

### Invalid Language Code
If you specify an unsupported language code, the application will:
- Log a warning message
- Fall back to system locale or English
- Continue running normally

### Missing Translations
If a translation is missing, the application will:
- Fall back to the English text
- Log the missing resource (in debug builds)
- Continue running without errors

## Development Notes

The language override is processed in `App.xaml.cs` during application startup, before any windows are created. This ensures all UI elements use the correct language from the beginning.