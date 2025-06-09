use tauri::{Manager, PhysicalPosition, PhysicalSize, Window};

#[tauri::command]
async fn close_app(window: Window) {
    window.close().unwrap();
}

#[tauri::command]
async fn show_notification(title: String, body: String) -> Result<(), String> {
    #[cfg(target_os = "windows")]
    {
        use std::process::Command;
        let _ = Command::new("powershell")
            .args([
                "-Command",
                &format!("New-BurntToastNotification -Text '{}', '{}'", title, body)
            ])
            .output();
    }
    Ok(())
}

#[tauri::command]
async fn change_position(window: Window, position: String) -> Result<(), String> {
    let screen_width = 1920; // デフォルト値、実際には取得すべき
    let screen_height = 1080;
    
    match position.as_str() {
        "right" => {
            window.set_size(PhysicalSize::new(50, screen_height)).map_err(|e| e.to_string())?;
            window.set_position(PhysicalPosition::new(screen_width - 50, 0)).map_err(|e| e.to_string())?;
        },
        "left" => {
            window.set_size(PhysicalSize::new(50, screen_height)).map_err(|e| e.to_string())?;
            window.set_position(PhysicalPosition::new(0, 0)).map_err(|e| e.to_string())?;
        },
        "top" => {
            window.set_size(PhysicalSize::new(screen_width, 50)).map_err(|e| e.to_string())?;
            window.set_position(PhysicalPosition::new(0, 0)).map_err(|e| e.to_string())?;
        },
        "bottom" => {
            window.set_size(PhysicalSize::new(screen_width, 50)).map_err(|e| e.to_string())?;
            window.set_position(PhysicalPosition::new(0, screen_height - 50)).map_err(|e| e.to_string())?;
        },
        _ => return Err("Invalid position".to_string()),
    }
    
    Ok(())
}

#[cfg_attr(mobile, tauri::mobile_entry_point)]
pub fn run() {
  tauri::Builder::default()
    .setup(|app| {
      if cfg!(debug_assertions) {
        app.handle().plugin(
          tauri_plugin_log::Builder::default()
            .level(log::LevelFilter::Info)
            .build(),
        )?;
      }
      
      let window = app.get_webview_window("main").unwrap();
      
      // 初期位置を画面右端に設定
      let _ = window.set_position(PhysicalPosition::new(1870, 0));
      
      Ok(())
    })
    .invoke_handler(tauri::generate_handler![close_app, show_notification, change_position])
    .run(tauri::generate_context!())
    .expect("error while running tauri application");
}
