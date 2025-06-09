package main

import (
	"embed"
	"fmt"
	"log"
	"net"
	"net/http"
	"os"
	"os/exec"
	"runtime"
	"strconv"
	"syscall"
)

//go:embed timer.html
var content embed.FS

func main() {
	// 利用可能なポートを見つける
	listener, err := net.Listen("tcp", ":0")
	if err != nil {
		log.Fatal(err)
	}
	port := listener.Addr().(*net.TCPAddr).Port
	listener.Close()

	// HTTPサーバーを起動
	http.Handle("/", http.FileServer(http.FS(content)))
	
	server := &http.Server{
		Addr: ":" + strconv.Itoa(port),
	}

	go func() {
		log.Printf("サーバー起動: http://localhost:%d", port)
		if err := server.ListenAndServe(); err != nil && err != http.ErrServerClosed {
			log.Fatal(err)
		}
	}()

	// ブラウザでアプリケーションを開く
	url := fmt.Sprintf("http://localhost:%d/timer.html", port)
	openBrowser(url)

	// プログラムを終了させないように待機
	select {}
}

func openBrowser(url string) {
	var err error
	switch runtime.GOOS {
	case "linux":
		err = exec.Command("xdg-open", url).Start()
	case "windows":
		err = exec.Command("rundll32", "url.dll,FileProtocolHandler", url).Start()
	case "darwin":
		err = exec.Command("open", url).Start()
	default:
		err = fmt.Errorf("unsupported platform")
	}
	if err != nil {
		log.Printf("ブラウザを開けませんでした: %v", err)
		log.Printf("手動で以下のURLを開いてください: %s", url)
	}
}

func init() {
	if runtime.GOOS == "windows" {
		// Windowsでコンソールウィンドウを非表示にする
		console := syscall.NewLazyDLL("kernel32.dll").NewProc("GetConsoleWindow")
		if console.Find() == nil {
			show := syscall.NewLazyDLL("user32.dll").NewProc("ShowWindow")
			if show.Find() == nil {
				hwnd, _, _ := console.Call()
				if hwnd != 0 {
					show.Call(hwnd, 0) // SW_HIDE
				}
			}
		}
	}
}