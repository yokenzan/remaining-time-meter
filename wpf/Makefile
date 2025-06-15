# Makefile for RemainingTimeMeter WPF Application

# Project settings
PROJECT_NAME = RemainingTimeMeter
CSPROJ_FILE = $(PROJECT_NAME).csproj
OUTPUT_DIR = bin
PUBLISH_DIR = publish
RUNTIME = win-x64
CONFIGURATION = Release

# Default target
.PHONY: all
all: build

# Build the application
.PHONY: build
build:
	dotnet build $(CSPROJ_FILE) --configuration $(CONFIGURATION)

# Build in debug mode
.PHONY: debug
debug:
	dotnet build $(CSPROJ_FILE) --configuration Debug

# Run the application
.PHONY: run
run: build
	dotnet run --project $(CSPROJ_FILE) --configuration $(CONFIGURATION)

# Run in debug mode
.PHONY: run-debug
run-debug: debug
	dotnet run --project $(CSPROJ_FILE) --configuration Debug

# Publish the application as single executable
.PHONY: publish
publish:
	dotnet publish $(CSPROJ_FILE) \
		--configuration $(CONFIGURATION) \
		--runtime $(RUNTIME) \
		--self-contained true \
		--output $(PUBLISH_DIR) \
		--property:PublishSingleFile=true \
		--property:PublishTrimmed=false \
		--property:IncludeNativeLibrariesForSelfExtract=true

# Publish debug version with logging
.PHONY: publish-debug
publish-debug:
	dotnet publish $(CSPROJ_FILE) \
		--configuration Debug \
		--runtime $(RUNTIME) \
		--self-contained true \
		--output bin/Debug/$(RUNTIME) \
		--property:PublishSingleFile=false \
		--property:PublishTrimmed=false

# Clean build artifacts
.PHONY: clean
clean:
	dotnet clean $(CSPROJ_FILE)
	rm -rf $(OUTPUT_DIR)
	rm -rf $(PUBLISH_DIR)
	rm -rf obj

# Restore NuGet packages
.PHONY: restore
restore:
	dotnet restore $(CSPROJ_FILE)

# Run tests (if any exist)
.PHONY: test
test:
	dotnet test $(CSPROJ_FILE)

# Format code (all file types)
.PHONY: format
format: format-dotnet format-prettier

# Format .NET files (C#, XML, project files)
.PHONY: format-dotnet
format-dotnet:
	dotnet format $(CSPROJ_FILE)
	dotnet format whitespace $(CSPROJ_FILE)

# Format other files (YAML, JSON, MD)
.PHONY: format-prettier
format-prettier:
	@if command -v prettier >/dev/null 2>&1; then \
		prettier --write "../.github/workflows/*.yml" "*.json" || true; \
	else \
		echo "prettier not installed, skipping non-.NET file formatting"; \
	fi

# Check code formatting (all file types)
.PHONY: format-check
format-check: format-check-dotnet format-check-prettier

# Check .NET file formatting
.PHONY: format-check-dotnet
format-check-dotnet:
	dotnet format $(CSPROJ_FILE) --verify-no-changes
	dotnet format whitespace $(CSPROJ_FILE) --verify-no-changes

# Check other file formatting
.PHONY: format-check-prettier
format-check-prettier:
	@if command -v prettier >/dev/null 2>&1; then \
		prettier --check "../.github/workflows/*.yml" "*.json" || true; \
	else \
		echo "prettier not installed, skipping non-.NET format check"; \
	fi

# Show help
.PHONY: help
help:
	@echo "Available targets:"
	@echo "  all       - Build the application (default)"
	@echo "  build     - Build in Release mode"
	@echo "  debug     - Build in Debug mode"
	@echo "  run       - Build and run in Release mode"
	@echo "  run-debug - Build and run in Debug mode"
	@echo "  publish   - Create single executable in publish/ directory"
	@echo "  publish-debug - Create debug executable with logging in bin/Debug/win-x64/"
	@echo "  clean     - Remove build artifacts"
	@echo "  restore   - Restore NuGet packages"
	@echo "  test      - Run tests"
	@echo "  format    - Format all files (C#, XML, YAML, JSON)"
	@echo "  format-dotnet - Format .NET files only"
	@echo "  format-prettier - Format YAML/JSON files only" 
	@echo "  format-check - Check all file formatting"
	@echo "  format-check-dotnet - Check .NET file formatting"
	@echo "  format-check-prettier - Check YAML/JSON formatting"
	@echo "  help      - Show this help"