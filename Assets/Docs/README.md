# Algorithm Colony Manual / アルゴリズム・コロニー マニュアル

このページはプレイヤー向けの操作説明です。  
This page is a player-facing manual.

- 開発者向けアーキテクチャ: [documentation.md](./documentation.md)
- 世界観・企画背景: [story.md](./story.md)

## 1. Game Overview / ゲーム概要

Algorithm Colony is a colony simulation game where you program robots from a top-down management perspective.  
アルゴリズム・コロニーは、神の視点でロボットへ命令を与え、資源収集と運営を自動化していくシミュレーションゲームです。

## 2. Core Loop / 基本ループ

1. Select a robot on the map.  
   マップ上のロボットを選択する。
2. Enter commands and execute behavior.  
   コマンドを入力して行動を実行する。
3. Gather and deposit resources to progress.  
   資源を収集・格納して進行する。
4. Upgrade Location and unlock next steps.  
   拠点レベルを上げて次の要素を解放する。

## 3. Controls / 操作方法

- Robot Selection / ロボット選択:
  - Click a robot to inspect and command it.  
    ロボットをクリックして状態確認と命令を行います。
- Command Input / コマンド入力:
  - Use command lines such as Move, Gather, Deposit.  
    Move、Gather、Deposit などのコマンドを入力します。
- Facility Panels / 施設パネル:
  - Open facility UI from in-world objects or command shortcuts.  
    ワールド上の施設クリック、またはショートカット入力で施設UIを開きます。

## 4. Available Commands (Implemented) / 現在実装済みコマンド

- Move(target)
- Gather(target)
- Deposit(material, amount)

Example / 例:

```csharp
Move(ironOre);
Gather(ironOre);
Move(tree);
Gather(tree);
Move(warehouse);
Deposit(ironOre, 30);
Deposit(tree, 30);
```

## 5. Input Helper Commands / 左下入力の補助コマンド

Implemented helper commands / 実装済み:

- cmd:setting
- cmd:location

Tab completion / Tab補完:

- Input c then press Tab to complete to cmd:  
  c を入力して Tab で cmd: に補完されます。

## 6. Current Playable Scope / 現在プレイ可能な範囲

- Basic robot command execution (Move/Gather/Deposit)
- Resource gather and deposit workflow
- Location panel and level-up related UI
- Basic status and facility UI operation

- ロボットへの基本命令（Move/Gather/Deposit）
- 資源の収集と格納
- 拠点パネルとレベルアップ関連UI
- 基本的なステータス・施設UI操作

## 7. Notes / 注意事項

If a robot gets stuck on an object, retry execution or move to a different target once.  
ロボットがオブジェクトに引っかかった場合は、再実行するか別対象へ一度移動させてください。

## 8. Planned Features (Not Yet Implemented) / 今後予定（未実装）

- Advanced programming constructs (if/for/while)
- Expanded facilities and production lines
- Expanded automation and progression systems

- 高度なプログラミング構文（if/for/while）
- 施設や生産ラインの拡張
- 自動化と進行システムの拡張
