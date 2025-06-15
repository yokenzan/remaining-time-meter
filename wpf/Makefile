# Makefile for RemMeter WPF Application

# Project settings
PROJECT_NAME = RemMeter
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

# Format code
.PHONY: format
format:
	dotnet format $(CSPROJ_FILE)
	dotnet format whitespace $(CSPROJ_FILE)

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
	@echo "  format    - Format code"
	@echo "  help      - Show this help"