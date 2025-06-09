package main

import (
	"syscall"
	"unsafe"
)

const (
	// Window styles
	WS_OVERLAPPED  = 0x00000000
	WS_POPUP       = 0x80000000
	WS_CHILD       = 0x40000000
	WS_MINIMIZE    = 0x20000000
	WS_VISIBLE     = 0x10000000
	WS_DISABLED    = 0x08000000
	WS_CLIPSIBLINGS = 0x04000000
	WS_CLIPCHILDREN = 0x02000000
	WS_MAXIMIZE    = 0x01000000
	WS_CAPTION     = 0x00C00000
	WS_BORDER      = 0x00800000
	WS_DLGFRAME    = 0x00400000
	WS_VSCROLL     = 0x00200000
	WS_HSCROLL     = 0x00100000
	WS_SYSMENU     = 0x00080000
	WS_THICKFRAME  = 0x00040000
	WS_GROUP       = 0x00020000
	WS_TABSTOP     = 0x00010000

	// Extended window styles
	WS_EX_TOPMOST        = 0x00000008
	WS_EX_TRANSPARENT    = 0x00000020
	WS_EX_LAYERED        = 0x00080000
	WS_EX_TOOLWINDOW     = 0x00000080
	WS_EX_NOACTIVATE     = 0x08000000

	// Messages
	WM_CREATE     = 0x0001
	WM_DESTROY    = 0x0002
	WM_PAINT      = 0x000F
	WM_TIMER      = 0x0113
	WM_LBUTTONUP  = 0x0202
	WM_RBUTTONUP  = 0x0205
	WM_KEYDOWN    = 0x0100

	// Keys
	VK_SPACE  = 0x20
	VK_ESCAPE = 0x1B

	// Colors
	COLOR_WINDOW = 5

	// Layer window attributes
	LWA_ALPHA = 0x00000002
)

var (
	user32             = syscall.NewLazyDLL("user32.dll")
	kernel32           = syscall.NewLazyDLL("kernel32.dll")
	gdi32              = syscall.NewLazyDLL("gdi32.dll")

	procRegisterClass   = user32.NewProc("RegisterClassW")
	procCreateWindow    = user32.NewProc("CreateWindowExW")
	procDefWindowProc   = user32.NewProc("DefWindowProcW")
	procGetMessage      = user32.NewProc("GetMessageW")
	procTranslateMessage = user32.NewProc("TranslateMessage")
	procDispatchMessage = user32.NewProc("DispatchMessageW")
	procPostQuitMessage = user32.NewProc("PostQuitMessage")
	procSetTimer        = user32.NewProc("SetTimer")
	procKillTimer       = user32.NewProc("KillTimer")
	procGetDC          = user32.NewProc("GetDC")
	procReleaseDC      = user32.NewProc("ReleaseDC")
	procSetLayeredWindowAttributes = user32.NewProc("SetLayeredWindowAttributes")
	procGetSystemMetrics = user32.NewProc("GetSystemMetrics")
	procSetWindowPos   = user32.NewProc("SetWindowPos")
	procMessageBox     = user32.NewProc("MessageBoxW")
	procInvalidateRect = user32.NewProc("InvalidateRect")
	procCreatePopupMenu = user32.NewProc("CreatePopupMenu")
	procAppendMenu     = user32.NewProc("AppendMenuW")
	procTrackPopupMenu = user32.NewProc("TrackPopupMenu")
	procGetCursorPos   = user32.NewProc("GetCursorPos")
	procDestroyMenu    = user32.NewProc("DestroyMenu")

	procCreateSolidBrush = gdi32.NewProc("CreateSolidBrush")
	procFillRect        = user32.NewProc("FillRect")
	procSelectObject    = gdi32.NewProc("SelectObject")
	procDeleteObject    = gdi32.NewProc("DeleteObject")

	procGetModuleHandle = kernel32.NewProc("GetModuleHandleW")
)

type WNDCLASS struct {
	Style         uint32
	LpfnWndProc   uintptr
	CbClsExtra    int32
	CbWndExtra    int32
	HInstance     uintptr
	HIcon         uintptr
	HCursor       uintptr
	HbrBackground uintptr
	LpszMenuName  *uint16
	LpszClassName *uint16
}

type MSG struct {
	Hwnd    uintptr
	Message uint32
	WParam  uintptr
	LParam  uintptr
	Time    uint32
	Pt      POINT
}

type POINT struct {
	X, Y int32
}

type PAINTSTRUCT struct {
	Hdc         uintptr
	FErase      int32
	RcPaint     RECT
	FRestore    int32
	FIncUpdate  int32
	RgbReserved [32]byte
}

type RECT struct {
	Left, Top, Right, Bottom int32
}

// Timer state
type TimerState struct {
	TotalSeconds   int
	CurrentSeconds int
	IsRunning      bool
	WindowHandle   uintptr
}

var timerState = &TimerState{
	TotalSeconds:   300, // 5分
	CurrentSeconds: 0,
	IsRunning:      false,
}

const (
	TIMER_ID = 1
	SCREEN_WIDTH = 1920  // 仮の値、実際は動的取得
	SCREEN_HEIGHT = 1080 // 仮の値、実際は動的取得
	SEEKBAR_HEIGHT = 8
	
	// Menu IDs
	MENU_RESET = 1001
	MENU_EXIT  = 1002
	MENU_SET_TIME = 1003
	
	// Menu flags
	MF_STRING = 0x00000000
	MF_SEPARATOR = 0x00000800
	TPM_RIGHTBUTTON = 0x0002
	
	// Message types
	WM_COMMAND = 0x0111
)

func main() {
	// ウィンドウクラスを登録
	className := syscall.StringToUTF16Ptr("SeekbarTimer")
	
	hInstance, _, _ := procGetModuleHandle.Call(0)
	
	wndClass := WNDCLASS{
		LpfnWndProc:   syscall.NewCallback(windowProc),
		HInstance:     hInstance,
		LpszClassName: className,
		HbrBackground: COLOR_WINDOW + 1,
	}
	
	procRegisterClass.Call(uintptr(unsafe.Pointer(&wndClass)))
	
	// 画面サイズを取得
	screenWidth, _, _ := procGetSystemMetrics.Call(0) // SM_CXSCREEN
	screenHeight, _, _ := procGetSystemMetrics.Call(1) // SM_CYSCREEN
	
	// ウィンドウを作成（画面下端にシークバー状）
	hwnd, _, _ := procCreateWindow.Call(
		WS_EX_TOPMOST|WS_EX_LAYERED|WS_EX_TOOLWINDOW|WS_EX_NOACTIVATE,
		uintptr(unsafe.Pointer(className)),
		uintptr(unsafe.Pointer(syscall.StringToUTF16Ptr("シークバー的タイムキーパー"))),
		WS_POPUP|WS_VISIBLE,
		0, // x
		uintptr(screenHeight-SEEKBAR_HEIGHT), // y (画面下端)
		screenWidth, // width (画面幅)
		SEEKBAR_HEIGHT, // height
		0, 0, hInstance, 0,
	)
	
	if hwnd == 0 {
		panic("Failed to create window")
	}
	
	timerState.WindowHandle = hwnd
	
	// ウィンドウの透明度を設定（80%不透明）
	procSetLayeredWindowAttributes.Call(hwnd, 0, 200, LWA_ALPHA)
	
	// メッセージループ
	var msg MSG
	for {
		ret, _, _ := procGetMessage.Call(uintptr(unsafe.Pointer(&msg)), 0, 0, 0)
		if ret == 0 { // WM_QUIT
			break
		}
		procTranslateMessage.Call(uintptr(unsafe.Pointer(&msg)))
		procDispatchMessage.Call(uintptr(unsafe.Pointer(&msg)))
	}
}

func windowProc(hwnd uintptr, msg uint32, wParam uintptr, lParam uintptr) uintptr {
	switch msg {
	case WM_CREATE:
		// タイマーを開始（1秒間隔）
		procSetTimer.Call(hwnd, TIMER_ID, 1000, 0)
		return 0
		
	case WM_TIMER:
		if wParam == TIMER_ID && timerState.IsRunning {
			timerState.CurrentSeconds++
			if timerState.CurrentSeconds >= timerState.TotalSeconds {
				// タイマー終了
				timerState.IsRunning = false
				showAlert("時間終了！")
			}
			// 再描画を要求
			procInvalidateRect.Call(hwnd, 0, 1)
		}
		return 0
		
	case WM_PAINT:
		drawSeekbar(hwnd)
		return 0
		
	case WM_LBUTTONUP:
		// 左クリックで開始/停止
		timerState.IsRunning = !timerState.IsRunning
		procInvalidateRect.Call(hwnd, 0, 1)
		return 0
		
	case WM_RBUTTONUP:
		// 右クリックでメニュー表示
		showContextMenu(hwnd)
		return 0
		
	case WM_KEYDOWN:
		switch wParam {
		case VK_SPACE:
			timerState.IsRunning = !timerState.IsRunning
		case VK_ESCAPE:
			procPostQuitMessage.Call(0)
		}
		procInvalidateRect.Call(hwnd, 0, 1)
		return 0
		
	case WM_COMMAND:
		switch wParam & 0xFFFF {
		case MENU_RESET:
			timerState.CurrentSeconds = 0
			timerState.IsRunning = false
			procInvalidateRect.Call(hwnd, 0, 1)
		case MENU_EXIT:
			procPostQuitMessage.Call(0)
		case MENU_SET_TIME:
			showTimeDialog()
		}
		return 0
		
	case WM_DESTROY:
		procKillTimer.Call(hwnd, TIMER_ID)
		procPostQuitMessage.Call(0)
		return 0
	}
	
	ret, _, _ := procDefWindowProc.Call(hwnd, uintptr(msg), wParam, lParam)
	return ret
}

func drawSeekbar(hwnd uintptr) {
	hdc, _, _ := procGetDC.Call(hwnd)
	defer procReleaseDC.Call(hwnd, hdc)
	
	// 画面サイズを取得
	screenWidth, _, _ := procGetSystemMetrics.Call(0)
	
	// 進捗率を計算
	progress := float64(timerState.CurrentSeconds) / float64(timerState.TotalSeconds)
	if progress > 1.0 {
		progress = 1.0
	}
	
	progressWidth := int32(float64(screenWidth) * progress)
	
	// 背景（グレー）
	backgroundBrush, _, _ := procCreateSolidBrush.Call(0x404040) // ダークグレー
	rect := RECT{0, 0, int32(screenWidth), SEEKBAR_HEIGHT}
	procFillRect.Call(hdc, uintptr(unsafe.Pointer(&rect)), backgroundBrush)
	procDeleteObject.Call(backgroundBrush)
	
	// プログレスバー（グラデーション風）
	if progressWidth > 0 {
		var color uint32
		if progress < 0.5 {
			color = 0x00FF00 // 緑
		} else if progress < 0.8 {
			color = 0x00FFFF // 黄
		} else {
			color = 0x0000FF // 赤
		}
		
		progressBrush, _, _ := procCreateSolidBrush.Call(uintptr(color))
		progressRect := RECT{0, 0, progressWidth, SEEKBAR_HEIGHT}
		procFillRect.Call(hdc, uintptr(unsafe.Pointer(&progressRect)), progressBrush)
		procDeleteObject.Call(progressBrush)
	}
}

func showAlert(message string) {
	messagePtr := syscall.StringToUTF16Ptr(message)
	titlePtr := syscall.StringToUTF16Ptr("タイマー")
	procMessageBox.Call(0, uintptr(unsafe.Pointer(messagePtr)), uintptr(unsafe.Pointer(titlePtr)), 0)
}

func showContextMenu(hwnd uintptr) {
	// ポップアップメニューを作成
	hMenu, _, _ := procCreatePopupMenu.Call()
	
	// メニュー項目を追加
	procAppendMenu.Call(hMenu, MF_STRING, MENU_RESET, uintptr(unsafe.Pointer(syscall.StringToUTF16Ptr("リセット"))))
	procAppendMenu.Call(hMenu, MF_SEPARATOR, 0, 0)
	procAppendMenu.Call(hMenu, MF_STRING, MENU_SET_TIME, uintptr(unsafe.Pointer(syscall.StringToUTF16Ptr("時間設定..."))))
	procAppendMenu.Call(hMenu, MF_SEPARATOR, 0, 0)
	procAppendMenu.Call(hMenu, MF_STRING, MENU_EXIT, uintptr(unsafe.Pointer(syscall.StringToUTF16Ptr("終了"))))
	
	// カーソル位置を取得
	var pt POINT
	procGetCursorPos.Call(uintptr(unsafe.Pointer(&pt)))
	
	// メニューを表示
	procTrackPopupMenu.Call(hMenu, TPM_RIGHTBUTTON, uintptr(pt.X), uintptr(pt.Y), 0, hwnd, 0)
	
	// メニューを破棄
	procDestroyMenu.Call(hMenu)
}

func showTimeDialog() {
	// 簡易的な入力ダイアログ（実際にはより複雑なダイアログが必要）
	showAlert("時間設定: スペースキー=開始/停止, 右クリック=メニュー, Esc=終了")
}