#![cfg_attr(not(debug_assertions), windows_subsystem = "windows")]

use tauri::Manager;

#[tauri::command]
fn close_app(window: tauri::Window) {
    window.close().unwrap();
}

fn main() {
    tauri::Builder::default()
        .invoke_handler(tauri::generate_handler![close_app])
        .setup(|app| {
            let window = app.get_window("main").unwrap();
            
            // ウィンドウを画面上部に配置
            let _ = window.set_position(tauri::Position::Logical(tauri::LogicalPosition {
                x: 0.0,
                y: 0.0,
            }));
            
            Ok(())
        })
        .run(tauri::generate_context!())
        .expect("error while running tauri application");
}