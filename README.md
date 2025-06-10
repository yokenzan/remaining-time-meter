# ProgressBar TimerKeeper

プレゼンテーション等の際に、直感的に残り時間を把握できるお役立ちタイムキーパー

## 概要

残り時間を視覚的に把握できるプログレスバー型タイマーです。画面端に細長く表示されるため、投影中の資料の邪魔になりません。

## 主要機能

- **時間設定**: 分と秒を個別に指定可能（既定は5分00秒）
- **4つの配置位置**: 右端・左端・上端・下端から選択可能
- **マルチディスプレー対応**: 表示するディスプレーを選択可能（既定は主画面）
- **視覚的な進捗表示**: 
  - 左右配置時: 下から上へ進捗表示
  - 上下配置時: 左から右へ進捗表示
- **段階的な色変化**: 開始時は緑色、60%経過でオレンジ、80%経過で赤色（点滅）
- **一時停止・再開機能**: ホバー時のコントロールパネルから操作
- **時間切れ通知**: メッセージボックスでお知らせ
- **Always on top**: 他のウィンドウの上に常に表示

## 仕様

### デザイン仕様
- **通常時**: 20px幅の細長いデザイン
- **ホバー時**: 拡大してコントロールパネルを表示
  - 上配置時: 下方向に伸長
  - 下配置時: 上方向に伸長
  - 左配置時: 右方向に伸長
  - 右配置時: 左方向に伸長
- **一時停止時**: メーターは青色(DarkSlateBlue)に変化

### 技術仕様
- **フレームワーク**: .NET 8.0 + WPF
- **対象OS**: Windows 10/11
- **言語**: C#
- **特徴**: ネイティブWindows UI、高パフォーマンス、マルチディスプレー対応

### システム要件
- .NET 8.0 SDK以上
- Windows 10/11
- マルチディスプレー環境（オプション）

## 実装版

### WPF版 (Windows専用)
📁 `wpf-progressbar-timerkeeper/`

#### ダウンロード
GitHub Releasesから実行ファイルをダウンロードできます：
- Windows x64版
- Windows x86版

#### ビルド・実行方法

##### ビルド
```bash
cd wpf-progressbar-timerkeeper
dotnet build
```

##### 実行
```bash
cd wpf-progressbar-timerkeeper
dotnet run
```

##### 発行（単一実行ファイル作成）
```bash
cd wpf-progressbar-timerkeeper
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

## 使い方

1. アプリケーションを起動
2. 分と秒を入力（例：5分30秒）
3. **表示ディスプレーを選択**（既定では主画面が選択済み）
4. 配置位置を選択（右端・左端・上端・下端）
5. 「開始」ボタンでタイマースタート
6. タイマーバーにマウスを置くとコントロールパネルが表示
7. 一時停止・再開・停止・閉じるの操作が可能

## ファイル構成

### WPF版
- `ProgressBarTimerKeeper.csproj` - プロジェクトファイル
- `App.xaml` / `App.xaml.cs` - アプリケーションエントリポイント
- `MainWindow.xaml` / `MainWindow.xaml.cs` - メイン設定ウィンドウ
- `TimerWindow.xaml` / `TimerWindow.xaml.cs` - タイマー表示ウィンドウ

## ライセンス

MIT License 