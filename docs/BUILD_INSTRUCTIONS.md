# Windows実行ファイルのビルド方法

このプロジェクトはElectronアプリケーションです。Windows実行ファイルをビルドするには以下の手順に従ってください。

## WSL/Linux環境の場合

WSL環境では直接Windowsバイナリをビルドできないため、以下のいずれかの方法を使用してください：

### 方法1: Windows環境でビルド（推奨）

1. Windows環境でNode.jsをインストール
2. プロジェクトフォルダをWindows環境にコピー
3. 以下のコマンドを実行：

```bash
npm install
npm run dist
```

4. `dist`フォルダに実行ファイルが生成されます

### 方法2: GitHub Actionsを使用

`.github/workflows/build.yml`ファイルを作成してGitHub Actionsでビルドすることも可能です。

## 必要な準備

Windowsでビルドする場合、以下が必要です：
- Node.js (v14以上)
- npm または yarn
- Windows 10/11

## ビルドコマンド

```bash
# 依存関係のインストール
npm install

# Windows用実行ファイルの作成
npm run dist
```

## 出力ファイル

ビルドが成功すると、以下のファイルが生成されます：
- `dist/シークバー的タイムキーパー Setup {version}.exe` - インストーラー
- `dist/win-unpacked/` - ポータブル版

## トラブルシューティング

- アイコンファイルがない場合は、デフォルトのElectronアイコンが使用されます
- ビルドエラーが発生した場合は、`node_modules`を削除して再インストールしてください