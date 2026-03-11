# Project Documentation / プロジェクト技術ドキュメント

このページは開発者向けの構成説明です。  
This page describes the current implementation for developers.

- プレイヤー向けマニュアル: [README.md](./README.md)
- 世界観・企画背景: [story.md](./story.md)

## 1. Architecture Snapshot / 構成サマリ

Main systems are organized around managers, robot behaviors, command parsing, and facility UIs.  
主要システムは、マネージャー群・ロボット挙動・コマンド解析・施設UIで構成されています。

Primary flow / 主な流れ:

1. Command strings are validated by CommandHandler.
2. RobotCommandExecute maps command text to robot states.
3. RobotController and sub-components perform movement/gather/deposit.
4. FacilityManager handles panel visibility and transitions.

## 2. Core Components (Implemented) / 主要コンポーネント（実装済み）

### 2.1 Command System / コマンドシステム

- Assets/Scripts/Command/CommandHandler.cs
  - Supported command identifiers: Move, Gather, Deposit
  - Performs argument validation through CommandLoader data
- Assets/Scripts/Command/CommandLoader.cs
  - Loads command entries from Resources JSON
  - Exposes MoveCommands, GatherCommands, DepositCommands

### 2.2 Robot Execution / ロボット実行

- Assets/Scripts/Robot/RobotCommandExecute.cs
  - Sequentially executes parsed commands
  - Handles Move/Gather/Deposit state transitions
  - Supports Loop(); for cyclic behavior
- Assets/Scripts/Consts/RobotData.cs (BaseStatus)
  - Robot base stats, battery/equipment/accessory data
  - Inventory slots: 5 max

### 2.3 Game and Facility Management / ゲーム・施設管理

- Assets/Scripts/Manager&Controller/GameManager.cs
  - Singleton root manager
  - Creates initial robots via RobotFactory
  - Updates top-left UI fields (player name, location level)
- Assets/Scripts/Manager&Controller/FacilityManager.cs
  - Controls canvas visibility (Setting/Location/Warehouse/BatteryRoom/Warkshop/RobotStatus)
  - Handles Escape close behavior for open panels

### 2.4 Location Progression / 拠点進行

- Assets/Scripts/Facility/Location/LocationController.cs
  - Displays level-up requirements
  - Reflects unlock information per level
- Assets/Scripts/So/Database/LocationLevelupUnlock.cs
  - ScriptableObject database for per-level unlock metadata
  - Defines required materials and unlock status params

### 2.5 UI Shortcut Commands / UIショートカット入力

- Assets/Scripts/UI/InputCommand.cs
  - Accepts helper commands: cmd:location, cmd:setting
  - Tab completions: c -> cmd:, cmd:l -> cmd:location, cmd:s -> cmd:setting

## 3. Current Design Constraints / 現状の制約

- Only three executable robot command types are implemented in validator and executor paths.
- Future-oriented concepts exist in planning docs but are not all present in runtime behavior.
- Some names include legacy spelling (example: Warkshop) and are kept for compatibility.

- 実行コマンドは検証系・実行系ともに3種類が中心です。
- 企画上の将来要素は、現行ランタイムに未接続のものがあります。
- 互換性のため命名揺れを残している箇所があります（例: Warkshop）。

## 4. Implemented vs Planned / 実装済みと予定の切り分け

Implemented now / 現在実装:

- Move/Gather/Deposit command flow
- Basic robot stats and inventory handling
- Facility panel toggles and location-level related UI

Planned or partial / 予定または部分:

- Rich programming syntax (if/for/while)
- Broader production chain and advanced buildings
- Larger progression/economy ecosystem

## 5. Maintenance Notes / 保守メモ

- When adding a new command, update both validation and execution paths.
- Keep README and story docs aligned when gameplay scope changes.
- Verify ScriptableObject data assumptions when changing level progression.

- 新規コマンド追加時は、検証処理と実行処理を必ず両方更新してください。
- プレイ可能範囲が変わったら README と story の説明も同期してください。
- 拠点進行を変更する際は ScriptableObject 側の前提整合を確認してください。
