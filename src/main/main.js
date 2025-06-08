const { app, BrowserWindow, screen } = require('electron');

let mainWindow;

function createWindow() {
  // Get the primary display's work area (excluding taskbar)
  const { width, height } = screen.getPrimaryDisplay().workAreaSize;
  
  // Create window with vertical bar configuration (right edge by default)
  mainWindow = new BrowserWindow({
    width: 40,                    // Slim width when not hovered
    height: height,               // Full height of work area
    x: width - 40,               // Position at right edge
    y: 0,                        // Top of screen
    frame: false,                // Remove window frame
    transparent: true,           // Allow transparency
    alwaysOnTop: true,          // Always on top
    skipTaskbar: true,          // Don't show in taskbar
    resizable: false,           // Prevent resizing
    movable: false,             // Prevent moving
    focusable: true,            // Allow focus for input
    webPreferences: {
      nodeIntegration: true,
      contextIsolation: false
    }
  });
  
  // Load the HTML file
  mainWindow.loadFile('src/renderer/index.html');
  
  // Prevent window from being closed accidentally
  mainWindow.on('close', (event) => {
    if (!app.isQuitting) {
      event.preventDefault();
      mainWindow.hide();
    }
  });
  
  // Handle mouse events for expanding/collapsing
  mainWindow.on('blur', () => {
    // When window loses focus, restore to slim width
    const bounds = mainWindow.getBounds();
    if (bounds.width > 40) {
      mainWindow.setBounds({
        x: width - 40,
        y: 0,
        width: 40,
        height: height
      });
    }
  });
}

// App event handlers
app.whenReady().then(() => {
  createWindow();
  
  // Re-create window if all windows are closed (macOS)
  app.on('activate', () => {
    if (BrowserWindow.getAllWindows().length === 0) {
      createWindow();
    }
  });
});

// Quit when all windows are closed (except on macOS)
app.on('window-all-closed', () => {
  if (process.platform !== 'darwin') {
    app.quit();
  }
});

// Add IPC handlers for window positioning
const { ipcMain } = require('electron');

// Handle position change requests from renderer
ipcMain.on('change-position', (event, position) => {
  const { width, height } = screen.getPrimaryDisplay().workAreaSize;
  let bounds = {};
  
  switch(position) {
    case 'right':
      bounds = { x: width - 40, y: 0, width: 40, height: height };
      break;
    case 'left':
      bounds = { x: 0, y: 0, width: 40, height: height };
      break;
    case 'top':
      bounds = { x: 0, y: 0, width: width, height: 40 };
      break;
    case 'bottom':
      bounds = { x: 0, y: height - 40, width: width, height: 40 };
      break;
  }
  
  mainWindow.setBounds(bounds);
});

// Handle hover expansion requests
ipcMain.on('expand-window', (event, position, expand) => {
  const { width, height } = screen.getPrimaryDisplay().workAreaSize;
  let bounds = mainWindow.getBounds();
  
  if (expand) {
    switch(position) {
      case 'right':
        bounds = { x: width - 200, y: 0, width: 200, height: height };
        break;
      case 'left':
        bounds = { x: 0, y: 0, width: 200, height: height };
        break;
      case 'top':
        bounds = { x: 0, y: 0, width: width, height: 100 };
        break;
      case 'bottom':
        bounds = { x: 0, y: height - 100, width: width, height: 100 };
        break;
    }
  } else {
    switch(position) {
      case 'right':
        bounds = { x: width - 40, y: 0, width: 40, height: height };
        break;
      case 'left':
        bounds = { x: 0, y: 0, width: 40, height: height };
        break;
      case 'top':
        bounds = { x: 0, y: 0, width: width, height: 40 };
        break;
      case 'bottom':
        bounds = { x: 0, y: height - 40, width: width, height: 40 };
        break;
    }
  }
  
  mainWindow.setBounds(bounds);
});