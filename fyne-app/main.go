package main

import (
	"fmt"
	"time"

	"fyne.io/fyne/v2"
	"fyne.io/fyne/v2/app"
	"fyne.io/fyne/v2/container"
	"fyne.io/fyne/v2/widget"
	"fyne.io/fyne/v2/layout"
)

type Timer struct {
	totalSeconds   int
	currentSeconds int
	isRunning      bool
	progressBar    *widget.ProgressBar
	timeLabel      *widget.Label
	ticker         *time.Ticker
	done           chan bool
}

func NewTimer() *Timer {
	return &Timer{
		totalSeconds:   300, // 5分
		currentSeconds: 0,
		isRunning:      false,
		progressBar:    widget.NewProgressBar(),
		timeLabel:      widget.NewLabel("05:00"),
		done:           make(chan bool),
	}
}

func (t *Timer) updateDisplay() {
	progress := float64(t.currentSeconds) / float64(t.totalSeconds)
	t.progressBar.SetValue(progress)
	
	remaining := t.totalSeconds - t.currentSeconds
	if remaining < 0 {
		remaining = 0
	}
	minutes := remaining / 60
	seconds := remaining % 60
	t.timeLabel.SetText(fmt.Sprintf("%02d:%02d", minutes, seconds))
}

func (t *Timer) start() {
	if !t.isRunning {
		t.isRunning = true
		t.ticker = time.NewTicker(time.Second)
		
		go func() {
			for {
				select {
				case <-t.ticker.C:
					t.currentSeconds++
					t.updateDisplay()
					
					if t.currentSeconds >= t.totalSeconds {
						t.stop()
						// タイマー終了の通知
						dialog := widget.NewLabel("時間終了!")
						dialog.Resize(dialog.MinSize())
						return
					}
				case <-t.done:
					return
				}
			}
		}()
	}
}

func (t *Timer) stop() {
	if t.isRunning {
		t.isRunning = false
		if t.ticker != nil {
			t.ticker.Stop()
		}
		t.done <- true
	}
}

func (t *Timer) reset() {
	t.stop()
	t.currentSeconds = 0
	t.updateDisplay()
}

func (t *Timer) setTime(minutes int) {
	t.totalSeconds = minutes * 60
	t.reset()
}

func main() {
	myApp := app.New()
	myWindow := myApp.NewWindow("シークバー的タイムキーパー")
	
	// ウィンドウサイズを設定
	myWindow.Resize(fyne.NewSize(400, 100))
	myWindow.SetFixedSize(true)
	
	timer := NewTimer()
	timer.updateDisplay()
	
	// ボタンを作成
	startBtn := widget.NewButton("開始", func() {
		timer.start()
	})
	
	pauseBtn := widget.NewButton("一時停止", func() {
		timer.stop()
	})
	
	resetBtn := widget.NewButton("リセット", func() {
		timer.reset()
	})
	
	setTimeBtn := widget.NewButton("時間設定", func() {
		entry := widget.NewEntry()
		entry.SetText("5")
		
		dialog := container.NewVBox(
			widget.NewLabel("タイマー時間を分で入力:"),
			entry,
			container.NewHBox(
				widget.NewButton("OK", func() {
					if minutes := entry.Text; minutes != "" {
						var min int
						if _, err := fmt.Sscanf(minutes, "%d", &min); err == nil && min > 0 {
							timer.setTime(min)
						}
					}
				}),
				widget.NewButton("キャンセル", func() {
					// ダイアログを閉じる
				}),
			),
		)
		
		// 簡易的なダイアログ表示
		dialog.Resize(dialog.MinSize())
	})
	
	closeBtn := widget.NewButton("×", func() {
		myApp.Quit()
	})
	
	// プログレスバーと時間表示
	progressContainer := container.NewBorder(
		nil, nil, nil, timer.timeLabel,
		timer.progressBar,
	)
	
	// ボタンコンテナ
	buttonContainer := container.New(
		layout.NewHBoxLayout(),
		startBtn,
		pauseBtn,
		resetBtn,
		setTimeBtn,
		closeBtn,
	)
	
	// メインコンテナ
	content := container.NewVBox(
		progressContainer,
		buttonContainer,
	)
	
	myWindow.SetContent(content)
	myWindow.ShowAndRun()
}