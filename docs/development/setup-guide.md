# Development Setup Guide

## Prerequisites

### Required Software
1. **.NET 8.0 SDK**
   - Download from [Microsoft .NET](https://dotnet.microsoft.com/download/dotnet/8.0)
   - Minimum version: 8.0.0
   - Includes runtime, compiler, and development tools

2. **Visual Studio 2022** (Recommended) or **Visual Studio Code**
   - Visual Studio Community (free) with ".NET desktop development" workload
   - OR Visual Studio Code with C# extension

3. **Git**
   - For version control and repository management
   - Download from [git-scm.com](https://git-scm.com/)

### Optional Tools
1. **Windows Subsystem for Linux (WSL)** - For cross-platform development
2. **Docker Desktop** - For containerized builds
3. **GitHub CLI** - For enhanced GitHub integration

## Repository Setup

### Clone the Repository
```bash
git clone https://github.com/yokenzan/remaining-time-meter.git
cd remaining-time-meter
```

### Project Structure Overview
```
remaining-time-meter/
├── wpf/                          # WPF Application
│   ├── App.xaml                  # Application entry point
│   ├── MainWindow.xaml           # Main configuration window
│   ├── TimerWindow.xaml          # Timer display window
│   ├── Models/                   # Data models
│   ├── Helpers/                  # Utility classes
│   ├── Properties/               # Resources and settings
│   └── RemainingTimeMeter.csproj # Project file
├── docs/                         # Documentation
├── .github/                      # GitHub workflows
├── LICENSE                       # MIT License
├── README.md                     # Project overview
└── TODO.md                       # Development roadmap
```

## Development Environment Setup

### Visual Studio 2022 Setup

#### Install Required Workloads
1. Open Visual Studio Installer
2. Select ".NET desktop development" workload
3. Ensure these components are included:
   - .NET 8.0 SDK
   - WPF project templates
   - IntelliCode
   - Git tools

#### Configure Development Settings
1. **Code Style**: Tools → Options → Text Editor → C# → Code Style
   - Enable EditorConfig support
   - Configure StyleCop analyzers

2. **Debugging**: Tools → Options → Debugging
   - Enable "Suppress JIT optimization on module load"
   - Configure exception handling

### Visual Studio Code Setup

#### Install Required Extensions
```bash
# Install C# extension
code --install-extension ms-dotnettools.csharp

# Install additional helpful extensions
code --install-extension ms-dotnettools.vscode-dotnet-runtime
code --install-extension formulahendry.code-runner
code --install-extension ms-vscode.vscode-json
```

#### Configure Workspace Settings
Create `.vscode/settings.json`:
```json
{
    "dotnet.defaultSolution": "wpf/RemainingTimeMeter.csproj",
    "files.exclude": {
        "**/bin": true,
        "**/obj": true
    },
    "omnisharp.enableRoslynAnalyzers": true,
    "omnisharp.enableEditorConfigSupport": true
}
```

#### Configure Tasks and Launch
Create `.vscode/tasks.json`:
```json
{
    "version": "2.0.0",
    "tasks": [
        {
            "label": "build",
            "command": "dotnet",
            "type": "process",
            "args": ["build", "${workspaceFolder}/wpf/RemainingTimeMeter.csproj"],
            "group": "build",
            "presentation": {
                "reveal": "silent"
            },
            "problemMatcher": "$msCompile"
        },
        {
            "label": "run",
            "command": "dotnet",
            "type": "process",
            "args": ["run", "--project", "${workspaceFolder}/wpf/RemainingTimeMeter.csproj"],
            "group": "build"
        }
    ]
}
```

## Build and Run

### Command Line Build
```bash
# Navigate to WPF project directory
cd wpf

# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run the application
dotnet run
```

### IDE Build
- **Visual Studio**: Press F5 or click "Start Debugging"
- **VS Code**: Use Ctrl+Shift+P → "Tasks: Run Task" → "build"

### Build Configurations
```bash
# Debug build (default)
dotnet build --configuration Debug

# Release build
dotnet build --configuration Release

# Publish for distribution
dotnet publish --configuration Release --runtime win-x64 --self-contained false
```

## Development Workflow

### Code Style and Quality

#### StyleCop Configuration
The project uses StyleCop.Analyzers for code style enforcement:
- Configuration: `wpf/stylecop.json`
- Rules are enforced during build
- Fix warnings before committing

#### Code Formatting
```bash
# Format code (if dotnet-format is installed)
dotnet format

# Install dotnet-format tool
dotnet tool install -g dotnet-format
```

### Debugging

#### Debugging in Visual Studio
1. Set breakpoints in code
2. Press F5 to start debugging
3. Use debugging windows:
   - Locals
   - Watch
   - Call Stack
   - Output

#### Debugging in VS Code
1. Install C# extension
2. Set breakpoints
3. Press F5 or use Run and Debug panel
4. Configure launch.json if needed

#### Common Debugging Scenarios
```csharp
// Add debug logging
Logger.Debug($"Timer started with {totalSeconds} seconds");

// Conditional breakpoints
#if DEBUG
    System.Diagnostics.Debugger.Break();
#endif

// Debug window positioning
Logger.Debug($"Window position: {Left}, {Top}, Size: {Width}x{Height}");
```

### Testing

#### Unit Testing Setup
```bash
# Add test project (if needed)
dotnet new xunit -n RemainingTimeMeter.Tests
dotnet add RemainingTimeMeter.Tests reference RemainingTimeMeter.csproj

# Run tests
dotnet test
```

#### Manual Testing Checklist
- [ ] Application startup and shutdown
- [ ] Timer functionality (start, pause, stop)
- [ ] Multi-display detection and selection
- [ ] DPI scaling on different displays
- [ ] Window positioning at all four edges
- [ ] Color transitions and animations
- [ ] Notification system
- [ ] Internationalization (language switching)

### Version Control

#### Git Workflow
```bash
# Create feature branch
git checkout -b feature/new-feature

# Make changes and commit
git add .
git commit -m "Add new feature: description"

# Push to remote
git push origin feature/new-feature

# Create pull request on GitHub
```

#### Commit Message Convention
```
type(scope): description

Examples:
feat(timer): add pause/resume functionality
fix(ui): correct DPI scaling on secondary displays
docs(readme): update installation instructions
refactor(helpers): extract display utility methods
```

## Building for Distribution

### Framework-Dependent Build
```bash
cd wpf

# Windows x64
dotnet publish -c Release -r win-x64 --self-contained false -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true

# Windows x86
dotnet publish -c Release -r win-x86 --self-contained false -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true
```

### Self-Contained Build
```bash
# Windows x64
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true

# Windows x86  
dotnet publish -c Release -r win-x86 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true
```

### Makefile Usage
```bash
# Build all variants
make build

# Publish all variants
make publish

# Clean build artifacts
make clean

# Format code
make format
```

## Development Tips

### Performance Profiling
```csharp
// Use Stopwatch for performance measurement
var stopwatch = Stopwatch.StartNew();
// ... code to measure
stopwatch.Stop();
Logger.Debug($"Operation took {stopwatch.ElapsedMilliseconds}ms");
```

### Memory Management
```csharp
// Implement IDisposable properly
public void Dispose()
{
    Dispose(true);
    GC.SuppressFinalize(this);
}

protected virtual void Dispose(bool disposing)
{
    if (disposing)
    {
        timer?.Dispose();
        // Dispose other managed resources
    }
}
```

### WPF-Specific Debugging
```csharp
// Debug binding issues
Logger.Debug($"Binding value: {GetValue(PropertyName)}");

// Debug visual tree
Logger.Debug($"Visual children count: {VisualTreeHelper.GetChildrenCount(this)}");

// Debug DPI issues
var dpi = VisualTreeHelper.GetDpi(this);
Logger.Debug($"DPI: {dpi.DpiScaleX}x{dpi.DpiScaleY}");
```

## Troubleshooting Development Issues

### Common Build Errors

#### Error: SDK not found
```bash
# Check installed SDKs
dotnet --list-sdks

# Install .NET 8.0 SDK if missing
# Download from Microsoft .NET website
```

#### Error: Package restore failed
```bash
# Clear NuGet cache
dotnet nuget locals all --clear

# Restore packages
dotnet restore
```

#### Error: StyleCop violations
```bash
# View specific violations
dotnet build --verbosity normal

# Auto-fix some issues
dotnet format
```

### Runtime Issues

#### DPI scaling problems
- Test on different DPI settings (100%, 125%, 150%, 200%)
- Use `DisplayHelper.GetLogicalScreenBounds()` for calculations
- Verify with `VisualTreeHelper.GetDpi()`

#### Multi-monitor issues
- Test with different monitor configurations
- Verify display enumeration in `GetDisplayInformation()`
- Check coordinate calculations

#### Timer accuracy
- Use `DispatcherTimer` for UI updates
- Avoid blocking operations in timer events
- Test with different system loads

## Contributing Guidelines

### Code Standards
1. Follow existing code style and conventions
2. Add XML documentation for public APIs
3. Include appropriate logging
4. Handle exceptions appropriately
5. Write self-documenting code

### Pull Request Process
1. Create feature branch from main
2. Implement changes with tests
3. Update documentation as needed
4. Submit pull request with clear description
5. Address review feedback

### Documentation Updates
- Update relevant documentation for API changes
- Include screenshots for UI changes
- Update README for new features
- Maintain changelog/release notes