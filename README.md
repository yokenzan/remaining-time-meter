# 残り目盛り / RemMeter

プレゼンテーション等の際に、直感的に残り時間を把握できるお役立ちタイムキーパーです。

## 概要

視覚的に持ち時間の消費を把握できるタイマーです。画面端に細長く表示されるため、プレゼンの資料投影中でも邪魔になりません。

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
📁 `wpf/`

#### 📥 ダウンロード
GitHub Releasesから2種類の実行ファイルをダウンロードできます：

##### 🎯 推奨：Framework-dependent版（サイズ小）
- **RemMeter-framework-dependent-win-x64.exe** - 64bit Windows用
- **RemMeter-framework-dependent-win-x86.exe** - 32bit Windows用

**要件**: .NET 8.0 Desktop Runtimeが必要
- 未インストールの場合、アプリ実行時に自動でダウンロードページに案内されます
- [.NET 8.0 Desktop Runtime ダウンロード](https://dotnet.microsoft.com/download/dotnet/8.0)

##### 🔧 Self-contained版（サイズ大、ランタイム不要）
- **RemMeter-self-contained-win-x64.exe** - 64bit Windows用
- **RemMeter-self-contained-win-x86.exe** - 32bit Windows用

#### ビルド・実行方法

##### ビルド
```bash
cd wpf
dotnet build
```

##### 実行
```bash
cd wpf
dotnet run
```

```bash
cd wpf
# 64bit版(Framework-dependent)
dotnet publish -c Release -r win-x64 --self-contained false -p:PublishSingleFile=true
# 32bit版(Framework-dependent)
dotnet publish -c Release -r win-x86 --self-contained false -p:PublishSingleFile=true
# 64bit版(Self-contained)
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
# 32bit版(Self-contained)
dotnet publish -c Release -r win-x86 --self-contained true -p:PublishSingleFile=true
```

## 使い方

1. アプリケーションを起動
2. 分と秒を入力（例：5分30秒）
3. 表示ディスプレーを選択（既定では主画面が選択済み）
4. 配置位置を選択（右端・左端・上端・下端）
5. 「開始」ボタンでタイマースタート
6. タイマーバーにマウスを置くとコントロールパネル表示
  - 一時停止・再開・停止・閉じるの操作が可能

## ファイル構成

### WPF版
- `RemMeter.csproj` - プロジェクトファイル
- `App.xaml` / `App.xaml.cs` - アプリケーションエントリポイント
- `MainWindow.xaml` / `MainWindow.xaml.cs` - メイン設定ウィンドウ
- `TimerWindow.xaml` / `TimerWindow.xaml.cs` - タイマー表示ウィンドウ

## ライセンス

MIT License 