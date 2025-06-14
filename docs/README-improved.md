# 残り時間メーター / Remaining Time Meter

<div align="center">

![App Icon](wpf/tv_timekeeper_trimmed.png)

**Intuitive Visual Timer for Presentations and Time Management**

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET 8.0](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![Windows 10+](https://img.shields.io/badge/Windows-10%2B-blue.svg)](https://www.microsoft.com/windows)
[![Latest Release](https://img.shields.io/github/v/release/yokenzan/remaining-time-meter)](https://github.com/yokenzan/remaining-time-meter/releases)

[📥 Download](#download) • [📖 Documentation](#documentation) • [🚀 Quick Start](#quick-start) • [🌐 Website](#website)

</div>

---

## 🎯 Overview

The Remaining Time Meter is a sleek, unobtrusive visual timer designed for presentations, meetings, and time-sensitive activities. It displays a progress bar along screen edges that provides instant visual feedback about remaining time without interfering with your content.

### ✨ Key Features

- **🎨 Visual Progress Display**: Intuitive color-coded progress bar (Green → Orange → Red)
- **📐 Flexible Positioning**: Choose from 4 screen edge positions (Top, Bottom, Left, Right)
- **🖥️ Multi-Display Support**: Works seamlessly with multiple monitors
- **⏸️ Interactive Controls**: Pause, resume, and stop functionality via hover panel
- **🌍 International**: Support for Japanese, English, Chinese (Simplified & Traditional)
- **⚡ High Performance**: Minimal resource usage, always-on-top display
- **🔔 Smart Notifications**: Windows 10/11 native notifications with fallback

## 📥 Download

### 🎯 Recommended: Framework-Dependent (Smaller Download)
Perfect for most users - requires .NET 8.0 Runtime (automatically prompted if missing).

| Platform | Download | Size |
|----------|----------|------|
| Windows 64-bit | [📦 Download](https://github.com/yokenzan/remaining-time-meter/releases/latest/download/RemainingTimeMeter-framework-dependent-win-x64.exe) | ~5MB |
| Windows 32-bit | [📦 Download](https://github.com/yokenzan/remaining-time-meter/releases/latest/download/RemainingTimeMeter-framework-dependent-win-x86.exe) | ~5MB |

**Requirements**: [.NET 8.0 Desktop Runtime](https://dotnet.microsoft.com/download/dotnet/8.0) (free)

### 🔧 Self-Contained (No Dependencies)
Larger download but runs independently without any additional software.

| Platform | Download | Size |
|----------|----------|------|
| Windows 64-bit | [📦 Download](https://github.com/yokenzan/remaining-time-meter/releases/latest/download/RemainingTimeMeter-self-contained-win-x64.exe) | ~150MB |
| Windows 32-bit | [📦 Download](https://github.com/yokenzan/remaining-time-meter/releases/latest/download/RemainingTimeMeter-self-contained-win-x86.exe) | ~150MB |

## 🚀 Quick Start

### ⚡ 1-Minute Setup
1. **Download** the appropriate version for your system
2. **Run** the executable (no installation required)
3. **Configure** time, display, and position
4. **Start** your timer and present with confidence!

### 📱 Basic Usage
```
Main Window → Set Time (e.g., 5:30) → Choose Display → Select Position → Start
Timer Bar → Hover for Controls → Pause/Resume/Stop as needed
```

## 🖼️ Screenshots

<div align="center">

### Main Configuration Window
![Main Window](docs/images/main-window.png)

### Timer in Action
![Timer Positions](docs/images/timer-positions.png)

### Control Panel
![Control Panel](docs/images/control-panel.png)

</div>

## 🛠️ System Requirements

| Component | Minimum | Recommended |
|-----------|---------|-------------|
| **OS** | Windows 10 (1809+) | Windows 11 |
| **Architecture** | x64 or x86 | x64 |
| **Memory** | 512 MB RAM | 2 GB RAM |
| **Storage** | 100 MB free | 1 GB free |
| **Display** | 1024×768 | 1920×1080+ |
| **Runtime** | .NET 8.0 (framework-dependent only) | - |

## 📖 Documentation

### 👤 User Documentation
- [📋 Installation Guide](docs/user-guide/installation-guide.md) - Detailed setup instructions
- [📘 User Manual](docs/user-guide/user-manual.md) - Complete feature guide  
- [❓ FAQ](docs/user-guide/faq.md) - Common questions and answers
- [🛠️ Troubleshooting](docs/user-guide/troubleshooting.md) - Problem resolution

### 🔧 Technical Documentation
- [⚙️ Technical Specifications](docs/specifications/technical-specifications.md)
- [📋 Functional Requirements](docs/specifications/functional-requirements.md)
- [🎨 UI/UX Specifications](docs/specifications/ui-ux-specifications.md)
- [🌐 Website Specifications](docs/specifications/website-specifications.md)

### 👨‍💻 Developer Documentation
- [🏗️ Development Setup](docs/development/setup-guide.md) - Environment configuration
- [📚 API Reference](docs/api/api-reference.md) - Code documentation
- [🔀 Contributing Guide](CONTRIBUTING.md) - How to contribute
- [📝 Changelog](CHANGELOG.md) - Version history

## 🏗️ Architecture

### 🔧 Technology Stack
- **Framework**: .NET 8.0 + WPF
- **Language**: C# 12.0
- **UI**: XAML + Code-behind
- **Platform**: Windows 10/11

### 📁 Project Structure
```
remaining-time-meter/
├── wpf/                    # WPF Application
│   ├── Models/            # Data models
│   ├── Helpers/           # Utility classes  
│   ├── Properties/        # Resources & i18n
│   └── *.xaml/.cs        # UI components
├── docs/                  # Documentation
│   ├── specifications/   # Technical specs
│   ├── user-guide/      # User documentation
│   ├── development/     # Developer guides
│   └── api/            # API reference
└── .github/             # CI/CD workflows
```

## 🌐 Website

Planning to create a comprehensive website using static site generation:

- **Technology**: Jekyll + GitHub Pages (recommended)
- **Features**: Documentation, downloads, support, community
- **URL**: `https://username.github.io/remaining-time-meter`
- **Deployment**: Automated via GitHub Actions

[View Website Specifications](docs/specifications/website-specifications.md)

## 🛠️ Development

### 🏗️ Build from Source
```bash
# Clone repository
git clone https://github.com/yokenzan/remaining-time-meter.git
cd remaining-time-meter/wpf

# Restore dependencies
dotnet restore

# Build and run
dotnet run
```

### 📦 Create Distribution
```bash
# Framework-dependent (recommended)
dotnet publish -c Release -r win-x64 --self-contained false -p:PublishSingleFile=true

# Self-contained
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

### 🧪 Testing
```bash
# Run unit tests
dotnet test

# Code formatting
dotnet format

# Code analysis
dotnet build --verbosity normal
```

## 🤝 Contributing

We welcome contributions! Please see our [Contributing Guide](CONTRIBUTING.md) for details.

### 🐛 Reporting Issues
- [🐛 Bug Reports](https://github.com/yokenzan/remaining-time-meter/issues/new?template=bug_report.md)
- [💡 Feature Requests](https://github.com/yokenzan/remaining-time-meter/issues/new?template=feature_request.md)

### 🔄 Development Workflow
1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Make your changes
4. Add tests if applicable
5. Commit your changes (`git commit -m 'Add amazing feature'`)
6. Push to the branch (`git push origin feature/amazing-feature`)
7. Open a Pull Request

## 📋 Roadmap

See our [TODO.md](TODO.md) for current development priorities:

### ✅ Completed
- ✅ Internationalization (4 languages)
- ✅ DPI awareness and multi-display support
- ✅ Improved exception handling
- ✅ XAML styling and theming system

### 🔄 In Progress
- 🔄 Constants organization and refactoring
- 🔄 Comprehensive documentation
- 🔄 Website development

### 📋 Planned
- 📋 Plugin architecture
- 📋 Custom notification systems
- 📋 Advanced timer features
- 📋 Cross-platform support investigation

## 📊 Stats

- **Languages**: 4 supported (Japanese, English, Chinese)
- **Platforms**: Windows 10/11 (x64, x86)
- **File Size**: 5MB (framework-dependent) / 150MB (self-contained)
- **Memory Usage**: ~20MB during operation
- **Startup Time**: <2 seconds

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

```
MIT License - Free for personal and commercial use
Copyright (c) 2025 RemainingTimeMeter
```

## 🙏 Acknowledgments

- Windows Presentation Foundation (WPF) team for the excellent UI framework
- .NET team for the robust runtime and development tools
- Community contributors and testers
- Open source projects that inspired this tool

---

<div align="center">

**Made with ❤️ for presenters and time-conscious professionals**

[⭐ Star this repo](https://github.com/yokenzan/remaining-time-meter/stargazers) • [🍴 Fork it](https://github.com/yokenzan/remaining-time-meter/fork) • [📢 Share it](https://twitter.com/intent/tweet?text=Check%20out%20this%20awesome%20visual%20timer%20for%20presentations!&url=https://github.com/yokenzan/remaining-time-meter)

</div>