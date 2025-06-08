# シークバー的タイムキーパー

プレゼンテーション用のタイムキーパーアプリケーションです。画面端にシークバー状に表示され、時間経過を視覚的に把握できます。

## 機能

- **シークバー表示**: 時間経過を縦/横のバーで視覚的に表示
- **4方向配置**: 画面の右端・左端・上端・下端に配置可能
- **詳細時間設定**: 分と秒を指定可能（例：5分30秒）
- **Always on Top**: 他のアプリケーションの上に常に表示
- **視覚的警告**: 
  - 60%経過で黄色に変化
  - 80%経過で赤色に変化（点滅アニメーション）
- **通知機能**: 時間切れ時にシステム通知

## プロジェクト構成

```
seekbar-like-timer/
├── src/
│   ├── main/
│   │   └── main.js          # Electronメインプロセス
│   └── renderer/
│       └── index.html       # レンダラープロセス（UI）
├── assets/                  # アセットファイル
├── docs/
│   └── BUILD_INSTRUCTIONS.md # ビルド手順
├── .github/
│   └── workflows/
│       └── build.yml        # GitHub Actions設定
├── dist/                    # ビルド出力
├── node_modules/           # 依存関係
├── package.json            # プロジェクト設定
└── README.md              # このファイル
```

## 開発環境のセットアップ

```bash
# 依存関係のインストール
npm install

# 開発サーバーの起動
npm start
```

## ビルド

詳細なビルド手順は [docs/BUILD_INSTRUCTIONS.md](docs/BUILD_INSTRUCTIONS.md) を参照してください。

```bash
# Windows実行ファイルの作成
npm run dist
```

## 使用方法

1. アプリケーションを起動
2. 分と秒を入力（デフォルト：5分00秒）
3. 矢印ボタンで配置位置を選択
4. 「開始」ボタンでタイマー開始
5. バーにマウスを置くとコントロールパネルが表示

## 技術仕様

- **フレームワーク**: Electron 36.4.0
- **ビルドツール**: electron-builder
- **対応OS**: Windows 10/11
- **Node.js**: v14以上

## ライセンス

MIT License