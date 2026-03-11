# プロジェクト技術ドキュメント

このページは開発者向けの構成説明です。  

- プレイヤー向けマニュアル: [README.md](./README.md)
- 世界観・企画背景: [story.md](./story.md)

## 1. 構成サマリ

主要システムは、マネージャー群・ロボット挙動・コマンド解析・施設UIで構成されています。

主な流れ:

1. CommandHandler がコマンド文字列を検証する。
2. RobotCommandExecute がコマンド文をロボット状態へ対応付ける。
3. RobotController と関連コンポーネントが移動・収集・格納を実行する。
4. FacilityManager がパネル表示と遷移を管理する。

## 2. 主要コンポーネント（実装済み）

### 2.1 コマンドシステム

- Assets/Scripts/Command/CommandHandler.cs
  - 対応コマンド識別子: Move, Gather, Deposit
  - CommandLoader のデータを使って引数を検証
- Assets/Scripts/Command/CommandLoader.cs
  - Resources 配下の JSON からコマンド定義を読み込む
  - MoveCommands、GatherCommands、DepositCommands を公開

### 2.2 ロボット実行

- Assets/Scripts/Robot/RobotCommandExecute.cs
  - 解析済みコマンドを順番に実行
  - Move/Gather/Deposit の状態遷移を処理
  - Loop(); による循環動作に対応
- Assets/Scripts/Consts/RobotData.cs (BaseStatus)
  - ロボット基礎ステータス、バッテリー・装備・アクセサリ情報
  - インベントリ枠は最大5

### 2.3 ゲーム・施設管理

- Assets/Scripts/Manager&Controller/GameManager.cs
  - シングルトンのルートマネージャー
  - RobotFactory で初期ロボットを生成
  - 左上UI（プレイヤー名、拠点レベル）を更新
- Assets/Scripts/Manager&Controller/FacilityManager.cs
  - キャンバス表示を制御（Setting/Location/Warehouse/BatteryRoom/Warkshop/RobotStatus）
  - 開いているパネルの Escape 閉じ動作を管理

### 2.4 拠点進行

- Assets/Scripts/Facility/Location/LocationController.cs
  - レベルアップ必要素材を表示
  - レベルごとの解放情報を反映
- Assets/Scripts/So/Database/LocationLevelupUnlock.cs
  - レベルごとの解放情報を管理する ScriptableObject データベース
  - 必要素材と解放状態パラメータを定義

### 2.5 UIショートカット入力

- Assets/Scripts/UI/InputCommand.cs
  - 補助コマンド cmd:location、cmd:setting を受け付ける
  - Tab補完: c -> cmd:、cmd:l -> cmd:location、cmd:s -> cmd:setting

## 3. 現状の制約

- 実行コマンドは検証系・実行系ともに3種類が中心です。
- 企画上の将来要素は、現行ランタイムに未接続のものがあります。
- 互換性のため命名揺れを残している箇所があります（例: Warkshop）。

## 4. 実装済みと予定の切り分け

現在実装:

- Move/Gather/Deposit のコマンド実行フロー
- ロボット基礎ステータスとインベントリ処理
- 施設パネル切替と拠点レベル関連UI

予定または部分:

- if/for/while などの高度なプログラミング構文
- より広い生産チェーンと高度な施設
- 拡張された進行・経済システム

## 5. 保守メモ

- 新規コマンド追加時は、検証処理と実行処理を必ず両方更新してください。
- プレイ可能範囲が変わったら README と story の説明も同期してください。
- 拠点進行を変更する際は ScriptableObject 側の前提整合を確認してください。
