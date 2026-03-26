# IME Edge Bar

A minimal Windows taskbar-edge indicator that shows the current **IME state** (ON / OFF) via background colour, while an arrow inside the bar tracks the **mouse pointer position** along the edge.

---

## Features

- **Colour-coded IME state** — background colour changes instantly when the IME is toggled
- **Mouse-position arrow** — a filled triangle points to the current cursor X (or Y) coordinate so you always know where on the screen the pointer is
- **Per-pixel transparency** — configure independent colour and opacity (0 – 100 %) for IME ON and IME OFF states; the arrow is always drawn fully opaque regardless of opacity
- **Four edge positions** — attach the bar to the top, bottom, left, or right edge of the monitor
- **Adjustable thickness** — 4 – 30 px
- **Multi-monitor aware** — the bar follows the mouse to the active monitor and repositions automatically when display settings (resolution, DPI scale, taskbar) change
- **Never steals focus** — uses `WS_EX_NOACTIVATE`; typing is never interrupted
- **Hidden from Alt+Tab** — uses `WS_EX_TOOLWINDOW`
- **System tray icon** — reflects the current IME state colour; right-click for the context menu
- **Global hotkey** `Ctrl+Alt+B` — toggle bar visibility
- **Settings persisted** — saved as JSON in `%APPDATA%\ImeEdgeBar\settings.json`

## Default Settings

| Item | Default |
|---|---|
| Position | Top edge |
| Thickness | 8 px |
| IME ON colour | Green, 100 % opacity |
| IME OFF colour | Red, 100 % opacity |

## Requirements

- Windows 10 or later
- [.NET 8 Desktop Runtime](https://dotnet.microsoft.com/download/dotnet/8.0)

## Installation

1. Download the latest release of `ImeEdgeBar.exe` from the [Releases](https://github.com/itagagaki/ime-edge-bar/releases) page.
2. Run `ImeEdgeBar.exe`.
3. The bar appears at the top edge of the monitor. A tray icon is shown in the notification area.

## Usage

| Action | How |
|---|---|
| Open Settings | Right-click tray icon → **設定...** |
| Toggle visibility | `Ctrl+Alt+B` or right-click tray icon → **表示 / 非表示** |
| Exit | Right-click tray icon → **終了** |

> **Tip:** Setting one IME state to 0 % opacity (fully transparent) while giving the other a visible colour is enough to distinguish both states — the arrow remains visible at all times.

## Building from Source

```
# Requires .NET 8 SDK
git clone https://github.com/itagagaki/ime-edge-bar.git
cd ImeEdgeBar
dotnet build -c Release
```

Or open `ImeEdgeBar.sln` in Visual Studio 2022 or later and build normally.

To regenerate `app.ico`:

```powershell
pwsh -File ImeEdgeBar/generate-icon.ps1
```

## Publish (single-file, framework-dependent)

```
cd ImeEdgeBar
dotnet publish -c Release -r win-x64 -p:SelfContained=false -p:PublishSingleFile=true
```

---
---

# IME Edge Bar

モニターの端に表示される細いバーで、**IME の状態**（ON / OFF）を背景色で示しつつ、バー内の矢印で**マウスポインターの位置**を視覚的に追跡するツールです。

---

## 機能

- **IME 状態の色表示** — IME を切り替えると背景色が即座に変化
- **マウス位置の矢印** — 現在のカーソル X 座標（または Y 座標）を指す三角形を描画。画面上のどこにポインターがあるかを常に把握できる
- **ピクセル単位の透過** — IME ON・OFF それぞれに色と不透明度（0 〜 100 %）を独立設定可能。矢印は不透明度に関わらず常に完全不透明で描画
- **4 辺への配置** — モニターの上辺・下辺・左辺・右辺から選択
- **幅の調整** — 4 〜 30 px
- **マルチモニター対応** — マウスが移動したモニターへバーが追従。解像度・DPI スケール・タスクバーの変更後も自動で再配置
- **フォーカスを奪わない** — `WS_EX_NOACTIVATE` により入力が中断されない
- **Alt+Tab に表示されない** — `WS_EX_TOOLWINDOW` を使用
- **タスクトレイアイコン** — 現在の IME 状態の色を反映。右クリックでコンテキストメニュー
- **グローバルホットキー** `Ctrl+Alt+B` — バーの表示 / 非表示を切り替え
- **設定の保存** — `%APPDATA%\ImeEdgeBar\settings.json` に JSON 形式で自動保存

## 初期設定

| 項目 | 初期値 |
|---|---|
| 表示位置 | 上辺 |
| バーの幅 | 8 px |
| IME ON の色 | 緑、不透明度 100 % |
| IME OFF の色 | 赤、不透明度 100 % |

## 動作環境

- Windows 10 以降
- [.NET 8 デスクトップ ランタイム](https://dotnet.microsoft.com/download/dotnet/8.0)

## インストール

1. [Releases](https://github.com/itagagaki/ime-edge-bar/releases) ページから最新版の `ImeEdgeBar.exe` をダウンロードする。
2. `ImeEdgeBar.exe` を実行する。
3. モニターの上辺にバーが表示され、通知領域にトレイアイコンが追加される。

## 使い方

| 操作 | 方法 |
|---|---|
| 設定を開く | トレイアイコンを右クリック → **設定...** |
| 表示 / 非表示 | `Ctrl+Alt+B` またはトレイアイコン右クリック → **表示 / 非表示** |
| 終了 | トレイアイコン右クリック → **終了** |

> **ヒント:** 一方の IME 状態を不透明度 0 %（完全透明）にして、もう一方に色を付けるだけで両状態を区別できます。矢印はどちらの状態でも常に表示されます。

## ソースからビルド

```
# .NET 8 SDK が必要
git clone https://github.com/itagagaki/ime-edge-bar.git
cd ImeEdgeBar
dotnet build -c Release
```

または `ImeEdgeBar.slnx` を Visual Studio 2022 以降で開いて通常どおりビルドしてください。

`app.ico` を再生成する場合：

```powershell
pwsh -File ImeEdgeBar/generate-icon.ps1
```

## パブリッシュ（単一ファイル・フレームワーク依存型）

```
cd ImeEdgeBar
dotnet publish -c Release -r win-x64 -p:SelfContained=false -p:PublishSingleFile=true
```
