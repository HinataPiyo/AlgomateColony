# AlgomateColony ドキュメント整備計画書

## 調査の前提と表記

- 本書はリポジトリの現在のソース・アセット・既存資料を確認したうえで、今後のドキュメント整備を計画するものです。個別の仕様書や設計書はこの段階では作成しません。
- **【確認済み】** は、現在のソースコード、シリアライズ済みアセット、設定ファイル、既存資料から確認した事実です。
- **【資料記載】** は、既存ドキュメントに記載されているものの、実装と一致するとは限らない情報です。
- **【提案】** は、今後の資料化・設計検討に向けた提案です。
- **【要確認】** は、コードだけでは実際のプレイ挙動やUnity Inspector上の設定を確定できない事項です。
- ファイル名の `Warkshop`、`Deposite`、`Upgread` などは、コード・アセット上の実際の表記を尊重して記載しています。

## 1. プロジェクトの現状

### 1.1 ゲーム概要

- **【確認済み】** リポジトリ／ソリューション名は `AlgorithmColony`、既存資料に記載されているゲームタイトルは「AlgomateColony（アルゴメイトコロニー）」です。
- **【資料記載】** プレイヤーがロボットに命令を与え、資源を集めてコロニーを発展させるシミュレーションゲームとして説明されています。プレイヤーがキャラクターを直接操作するのではなく、コマンド入力と拠点運営を中心とする意図も記されています。
- **【確認済み】** Unity 6 `6000.0.23f1`、C#、URP 17.0.3を使用しています。主要なビルド対象Sceneは `Assets/Scenes/GameScene.unity` です。
- **【確認済み】** `Assets/Scripts` 以下にC#スクリプト81件があります。主要なカテゴリは `Robot`、`Facility`、`So`、`UI`、`GameSystem`、`Manager&Controller`、`Command`、`Consts` です。
- **【確認済み】** Unityアセットとして、ScriptableObjectのデータ資産、Prefab、Scene、Animation、Tile等が存在します。主なデータ資産は `Assets/ScriptableObject` にあり、`RobotData.asset` は `Assets/Scripts/So/Robot/` に置かれています。

### 1.2 実装済み・部分実装の機能

| 領域 | リポジトリから確認できた状態 |
|---|---|
| ロボット生成 | `GameManager.Start()` が `RobotFactory.CreateRobot()` を呼び、Prefabからロボットを1体生成します。Inspectorのテスト用フラグから追加生成する経路もあります。 |
| コマンド入力・検証 | `RobotCommandUIController` が入力を改行単位に分割し、`CommandHandler` が `Move`、`Gather`、`Deposit` の引数を検証します。定義データは `Resources` のJSONから `CommandLoader` が読み込みます。 |
| コマンド実行 | `RobotCommandExecute` が入力済みコマンドを順に処理し、`RobotController` の状態を切り替えます。 |
| 探索・移動 | `RobotSearch` がPhysics2Dの範囲検索を行い、見つからない場合は検索半径を広げます。`RobotMovement` がウェイポイント移動とRaycastによる障害物回避を行います。 |
| 経路生成 | `PathfindingSystem.GeneratePath()` は開始点と終点を直線補間したウェイポイントを返します。A*等の地形・グラフに基づく経路探索は、このクラスからは確認できません。 |
| 採集 | `RobotGather` が採集時間、装備による速度補正、ロボット所持スロットへの資源追加を扱います。資源の種類・採集条件は `MaterialSO` と `BaseMaterial` を参照します。 |
| 倉庫 | `RobotDeposit` が倉庫の近接確認とロボット所持品からの移送を行い、`WarehouseController` が倉庫内データと表示を管理します。 |
| 拠点レベルアップ | `LocationController` が必要素材を照合し、倉庫から素材を消費して `SystemControlSO.LocationLevelUp()` を呼びます。現行の `LocationLevelupUnlock.asset` は次の解放データを1件持っています。 |
| 加工施設の解放 | レベル0からのレベルアップ時に、`SystemControlSO.LocationLevelUp()` がデータ内のPrefabを生成します。現行アセットが参照するPrefabは加工施設です。 |
| 加工UI | `ProcessingController` が加工品候補と必要素材を表示し、作成ボタンの有効・無効を判定します。一方、確認したコードには作成ボタンの実行リスナーや加工品生成・素材消費の処理が見当たらず、加工機能全体が完成しているとは判断できません。 |
| チュートリアル | `TutorialController` が `TutorialSO` のタスクを表示・進行し、移動、採集、格納、レベルアップに関連する処理から進捗通知を受け取ります。アセット上のタスクには拠点レベルアップまでが記載されています。 |
| UI・施設パネル | `FacilityManager` が設定、拠点、倉庫、充電室、加工施設、ロボット状態のCanvas表示を管理します。 |
| マップ・資源配置 | `MapGeneration` が300×300のTilemapを生成し、`MaterialObjSpawnController` が資源Prefabをランダム配置します。ロボットの移動は別途Transform・Physics2Dを利用しています。 |

### 1.3 未実装・構想段階、または実装を確認できない機能

- **【資料記載】** `if`、`for`、`while`、関数、複数ロボット協調、研究・クエスト、他コロニーとの交易・競争、エネルギー・修理・防衛などが今後の案として挙げられています。これらを実装済みと示す根拠は確認できません。
- **【資料記載】** READMEは、加工工場や倉庫を「今後実装予定」とも記載しています。一方、倉庫の資源移送・管理コードと加工施設UIの一部は既に存在します。READMEの「実装」の範囲を、UI／基礎処理／完成機能に分けて更新する必要があります。
- **【確認済み】** `RobotCommandExecute` には最終行が `Loop();` の場合に先頭へ戻す分岐があります。しかし、`CommandHandler` の検証対象には `Loop` がなく、`ExecuteCommand()` にも `Loop();` の実処理がありません。現状のUI経由で利用可能なループ機能とは確認できず、既存技術資料の「Loop対応」は不正確です。
- **【要確認】** 複数ロボットはファクトリーメソッドから生成可能ですが、通常起動時の操作UI・選択・同時実行が複数体に対応するかは、SceneとPrefabの実行確認が必要です。
- **【要確認】** セーブ／ロード、永続化、実際の配布ビルドでの動作保証範囲は、この調査だけでは確定していません。

### 1.4 開発者メモとの相違・補正

- `AlgorithmColony_Memo.md` はロボットのタイル単位移動と記載しますが、確認した移動コードはTransform移動、直線補間ウェイポイント、Physics2D判定を用いています。Tilemapの上を移動する設計・挙動と同一とは断定できません。
- 開発者メモには各ロボットの `RobotData` による管理とありますが、ランタイムの主要データ型として確認できるのは `Assets/Scripts/Consts/RobotData.cs` 内の `BaseStatus` です。また、別途 `Assets/Scripts/So/Robot/RobotData.asset` があるため、同名データの役割は整理が必要です。
- 基本情報にある `Move(target);`、`Gather(target);`、`Deposit(target, amount);` のうち、実装上の `Deposit` はアイテム名と数量を受け取ります。倉庫は近くにあることを別途確認するため、コマンド引数の `target` を倉庫として指定する設計とは異なります。
- `SystemControlSO` の説明データには充電室・加工室が含まれていますが、現行の `MoveCommands.json` の対象一覧は `location`、`warehouse`、`rock`、`tree`、`ironOre` です。資料表示と入力検証で対象一覧が一致していません。
- `Assets/ScriptableObject/Database/LocationLevelupUnlock.asset` はレベルアップ先データを1件持つ構成であり、現行データから確認できるのはレベル0から1への進行・加工施設Prefab生成です。「チュートリアル実装済み」という説明はありますが、チュートリアル完了とゲーム全体の完成度は分けて記載します。

## 2. 主要システムとクラスの一覧

| サブシステム | 主なクラス／データ | 責務と関係 |
|---|---|---|
| 起動・生成 | `GameManager`、`RobotFactory`、`RobotController` | `GameManager` が初期ロボット生成と上部UI更新を開始し、`RobotFactory` が `Robot_0_Root.prefab` を生成、`RobotController` が `BaseStatus` と関連コンポーネントを初期化します。 |
| コマンド入力・検証 | `RobotCommandUIController`、`CommandHandler`、`CommandLoader`、`MoveCommands.json`、`GatherCommands.json`、`DepositCommands.json` | 入力行の分割、コマンド名・引数の検証、JSON定義ロードを担います。実行系とは `ProcText` を介して接続します。 |
| ロボット状態・実行 | `RobotController`、`RobotCommandExecute`、`IRobotInitializable` | `RobotController` がロボット状態を保持・切り替え、移動・探索・採集・充電・格納コンポーネントを呼び出します。`RobotCommandExecute` が状態完了通知を待ちながらコマンド列を進めます。 |
| 移動・探索 | `RobotSearch`、`RobotMovement`、`PathfindingSystem`、`ObstacleAvoidanceHelper` | Physics2Dで対象を見つけ、経路ウェイポイントと障害物回避を用いて移動します。`PathfindingSystem` は現状、直線の分割点生成を担当します。 |
| 採集・アイテム | `RobotGather`、`BaseMaterial`、`MaterialSO`、`BaseStatus.Slot` | `MaterialSO` が素材の定義を保持し、`BaseMaterial` がワールド上資源の耐久・採集量、`RobotGather` が採集処理、`BaseStatus` がロボット所持品を管理します。 |
| 倉庫・施設 | `WarehouseController`、`RobotDeposit`、`LocationController`、`ProcessingController`、`WarkshopController`、`BatteryChargingRoomManager` | 倉庫在庫、格納、拠点レベルアップ表示・素材消費、加工・充電施設UIを扱います。ロジックの完成度は施設ごとに異なります。 |
| 進行・共有データ | `DataManager`、`SystemControlSO`、`LocationLevelupUnlock`、`TutorialController`、`TutorialSO`、`UpdateManager` | シングルトン参照とScriptableObject経由で進行・マスターデータを提供し、チュートリアル進行や一定間隔のUI更新を行います。 |
| UI・選択・ログ | `FacilityManager`、`RobotStatusPanelManager`、`InputCommand`、`CommandList`、各種Slotクラス、`LogController` | Canvas切替、補助コマンド、コマンド説明、ロボット状態・倉庫表示、操作ログを担います。 |
| マップ・資源出現 | `MapGeneration`、`MaterialObjSpawnController`、Tilemap／素材Prefab | Tilemap背景とワールド上資源Prefabを生成します。生成資源はPrefab上の `BaseMaterial` と `MaterialSO` に依存します。 |

### 2.1 依存関係の概略

```text
GameManager ──> RobotFactory ──> Robot_0_Root.prefab
     │                                 │
     │                                 └─> RobotController
     │                                      ├─> RobotCommandExecute
     │                                      ├─> RobotSearch ──> RobotMovement ──> PathfindingSystem
     │                                      ├─> RobotGather ──> BaseMaterial ──> MaterialSO
     │                                      ├─> RobotDeposit ──> WarehouseController
     │                                      └─> RobotBattery / BaseStatus
     │
RobotCommandUIController ──> CommandHandler ──> CommandLoader ──> Resources/*.json
                └────────────────────────────> RobotCommandExecute

DataManager ──> SystemControlSO / 各Database ScriptableObject
LocationController ──> WarehouseController + LocationLevelupUnlock
SystemControlSO.LocationLevelUp() ──> NEXT_UNLOCK.creatObj (現行データは加工施設Prefab)
FacilityManager ──> 各施設Canvas
TutorialController ──> TutorialSO
```

この図はコード上の主要な参照・呼び出し関係を表したものです。Inspector上のScene、Prefab、ScriptableObject参照の全件対応表ではありません。

### 2.2 データの生成・保持・参照・更新

1. `RobotFactory` がロボットPrefabを生成し、`RobotController.SpawnInit()` が `BaseStatus`、所持スロット、装備・アクセサリ・バッテリー情報を初期化します。
2. `RobotController` がロボットの状態を保持し、関連コンポーネントが `BaseStatus` を介してエネルギーや所持アイテム等を更新します。
3. `MaterialSO` が素材定義、`BaseMaterial` がワールド上の資源状態を持ちます。採集した資源はロボット所持枠に追加されます。
4. 格納時に `RobotDeposit` が `WarehouseController.SetMaterial_WarehouseSlot()` を呼び、倉庫管理クラスのリストを更新します。倉庫・ロボット間の在庫は別々のデータ構造です。
5. 拠点レベルアップ時に `LocationController` が必要数を照合し、倉庫在庫を消費した後、`SystemControlSO.LocationLevelUp()` がレベルを更新し、登録されたPrefabを生成します。
6. `DataManager` は各種データベースSOをInspector参照で保持し、各システムがシングルトンから取得します。コマンド定義だけは `Resources.Load<TextAsset>()` によるJSONロードです。

### 2.3 設計上の論点（根拠付き）

- **コマンド文法と実行の二重管理：** 検証は `CommandHandler`、実行時の分岐は `RobotCommandExecute.ExecuteCommand()` に分かれています。追加コマンドで両方の更新が必要です。検証は `StartsWith` と括弧抽出を中心としており、文法全体の解析とは異なります。
- **ループの実装・資料不一致：** `RobotCommandExecute` に `Loop();` 用の最終行判定がありますが、検証マップとコマンド実行分岐には対応がありません。技術資料の「Loop対応」は修正対象です。
- **経路探索の呼称：** `PathfindingSystem` の実装は `Vector2.Lerp` による直線分割です。障害物回避は `RobotMovement` のRaycast処理であり、地図に基づく探索アルゴリズムと分けて説明する必要があります。
- **密結合：** `RobotController` が多数のコンポーネントを直接取得・呼び出し、複数システムが `DataManager.instance`、`FacilityManager.instance`、`TutorialController.insrance` 等に依存しています。Scene／Prefabの参照設定も動作に影響します。
- **データ責務の分散：** ロボット所持品、倉庫在庫、ScriptableObject上の素材定義、進行データが別クラスに保持されています。更新順・消費・表示同期を説明する資料が必要です。
- **施設機能の成熟度差：** UI、必要素材判定、施設Prefab解放が存在しても、実際の加工処理などが完成しているとは限りません。機能一覧は「実装済み／部分実装／構想」に分ける必要があります。
- **命名・用語の揺れ：** `Warkshop`、`Deposite`、`Upgread` 等の実ファイル名・識別子と、資料中の一般的な `Workshop`、`Deposit`、`Upgrade` 表記が混在します。既存識別子を誤記せず、説明上の正規表記を別途定義するのが望ましいです。
- **Scene／Prefab参照の追跡性：** コードはSerializeField経由のInspector依存を多く持ちます。Scene、Prefab、SOの参照全体を資料にする際は、GUIDとInspector設定をUnity上でも照合する必要があります。

## 3. 主要処理フローの概要

### 3.1 コマンド入力から実行まで

1. `RobotCommandUIController` が入力欄を改行で分割します。
2. 各行を `CommandHandler.CheckCommand()` に渡し、`CommandLoader` が読み込んだJSON定義を参照して妥当性を検証します。
3. 全行が有効なら `RobotCommandExecute.ProcText` に保存し、実行操作でコルーチンを開始します。
4. `RobotCommandExecute` が行単位でコマンドを解釈し、`RobotController.ChangeState()` により `Search`、`GatherResource`、`Deposit` 等へ遷移します。
5. 各処理が終了時に `StateEndFlag` を立て、実行コルーチンが次のコマンドへ進みます。

**補足：** 現行のコマンド入力検証対象は `Move`、`Gather`、`Deposit` です。`cmd:location` と `cmd:setting` は別系統のUI補助入力で、ロボットの行動コマンドではありません。

### 3.2 移動・探索

1. `Move(target)` を受けると、ロボット状態が `Search` になります。
2. `RobotSearch.Search()` が現在地周辺をPhysics2Dで検索し、指定タグの対象がなければ探索半径を0.6ずつ増やします。
3. 対象が見つかると `RobotMovement` にTransformを渡し、状態を `Move` にします。
4. `PathfindingSystem` が開始点から目標までの直線を10区間のウェイポイントに分けます。`RobotMovement` は経路点へ移動し、前方Raycastで障害物があれば `ObstacleAvoidanceHelper` を利用します。
5. 対象との到達判定後、待機状態に戻り、コマンド完了を通知します。

**事実上の用語整理：** 資料では「経路探索」よりも「直線ウェイポイント生成＋局所Raycast障害物回避」と記載し、A*等の探索との混同を避けます。

### 3.3 採集・格納

1. `Gather(target)` が `RobotController` の採集状態を開始します。採集対象は探索済みColliderとタグを使って判定します。
2. `RobotGather` が `MaterialSO.gatherTime` とロボットの採集ステータス・装備補正を参照し、採集ゲージを更新します。
3. 採集完了時に `BaseMaterial.TakeDamage()` を呼び、得た素材を `BaseStatus.Slot[]` に加算します。所持枠は `BaseStatus` 上で5枠、スロット単位の最大数は100として定義されています。
4. `Deposit(material, amount)` は `RobotDeposit` に渡り、倉庫が近くに存在すること、対象素材と所持量が条件を満たすことを確認します。
5. `WarehouseController` に在庫を加算し、ロボット所持数を減らします。

### 3.4 拠点レベルアップ・施設解放

1. `LocationController` が `LocationLevelupUnlock.NextUnlock` の必要素材と倉庫在庫を表示・比較します。
2. 条件成立時にレベルアップ操作を有効化し、操作時に必要素材を倉庫から消費します。
3. `SystemControlSO.LocationLevelUp()` が、現在レベルに対応するデータのPrefab等を処理し、拠点レベルを加算します。
4. 現行データではレベル0の次に加工施設Prefabを生成する設定が確認できます。チュートリアルSOにも拠点レベルアップまでの段階があります。
5. 加工施設の解放後にどの加工機能が利用可能かは、`ProcessingController` のUIと実行処理を分けて評価する必要があります。

## 4. 既存ドキュメントの評価

| 資料 | 内容・有用性 | 整備上の注意 |
|---|---|---|
| [README.md](./README.md) | ゲーム概要、操作、プレイ可能範囲、補助コマンド、将来予定を紹介。リポジトリの入口として有用です。 | 実装済み倉庫処理・加工UIの存在と、加工処理の未確認／未完成を区別し、範囲説明をコードと照合してください。 |
| [Assets/Docs/README.md](./Assets/Docs/README.md) | プレイヤー向けマニュアル。基本ループ、コマンド例、現状と今後予定を説明しています。 | README.mdとの重複が大きいため、今後は片方を概要・導線、片方を操作マニュアルと役割分担します。 |
| [Assets/Docs/documentation.md](./Assets/Docs/documentation.md) | コマンド、ロボット、マネージャー、拠点進行の短い技術概要。 | `Loop();` 対応の記述は検証・実行コードと不一致です。実装詳細、データの流れ、クラス依存、Scene／Prefab参照は不足しています。 |
| [Assets/Docs/story.md](./Assets/Docs/story.md) | 世界観、体験の核、将来拡張を説明し、企画と現在の体験を分けようとしています。 | 企画意図の基準資料として活用できます。実装機能の説明は技術資料に委ね、構想を実装済みと誤読させないラベルを維持します。 |
| [AlgorithmColony_Memo.md](./AlgorithmColony_Memo.md) | ロボット、マップ、施設、ゲーム進行、拡張案を広く記録した開発メモです。 | 現行仕様・将来案・アイデアが混在し、タイル移動や一部コマンドの記述は実装と一致しません。内容を削除せず、検証後に正式資料へ移管または「構想」へ分類します。 |

既存資料は基本的な導入・企画・技術概要をすでにカバーしているため、同じ概要資料を増やすより、現状仕様の確定、処理フロー、アセット参照、設計判断の不足を埋める方針が適切です。

## 5. ドキュメント体系の提案

作成場所は既存の `Assets/Docs/` を基本とします。既存4資料は削除・上書きせず、内容確認と差分レビューを経て必要な箇所だけ更新します。重複を避けるため、「ゲームの確定仕様」「実装の説明」「世界観・構想」「ポートフォリオ紹介」の責務を分けます。

| 優先度／ドキュメント名（提案） | 目的・想定読者 | 記載内容 | 参照するコード・既存資料 | 関係・作成時の確認事項 |
|---|---|---|---|---|
| **P0 現行仕様・実装範囲** `Assets/Docs/current-scope.md` | 開発者、テスター、採用担当者が「今遊べること」を誤解なく把握する。 | ゲーム概要、起動Scene、チュートリアル範囲、実装済み／部分実装／未実装一覧、操作、コマンド引数と制約。 | 既存README 2種、`TutorialSO.asset`、`CommandHandler`、`RobotCommandUIController`、`LocationController`、`SystemControlSO`、`LocationLevelupUnlock.asset` | 既存READMEを入口にする場合は本書を現状の詳細表とし、README側には要約とリンクのみ置く。実行ビルドでの手順、クリア条件、施設解放後の状態を要確認。 |
| **P0 アーキテクチャ・データ設計** `Assets/Docs/architecture.md` | 開発者・技術評価者がクラス責務とデータの保持場所を理解する。 | サブシステム図、主要クラスの責務、依存関係、Singleton／Inspector参照、データ生成・保持・更新、ScriptableObject一覧と用途、Scene／Prefab構成。 | `Assets/Scripts/**`、`Assets/ScriptableObject/**`、`Assets/Prefab/**`、`GameScene.unity`、`Robot_0_Root.prefab`、既存`documentation.md` | 本計画の「アーキテクチャ概要」を根拠に、Inspector上の実参照をUnityで照合する。全クラス列挙ではなく責務グループ＋主要クラス一覧を基本とする。 |
| **P1 ロジックフロー** `Assets/Docs/logic-flows.md` | 実装者が主要な操作をコード追跡し、変更時の影響範囲を理解する。 | コマンド入力→検証→実行、探索→移動、採集→所持品、格納→倉庫、レベルアップ→Prefab生成、チュートリアル通知を図示。失敗・完了条件も記載。 | `CommandHandler`、`CommandLoader`、`RobotCommandUIController`、`RobotCommandExecute`、`RobotController`、`RobotSearch`、`RobotMovement`、`PathfindingSystem`、`RobotGather`、`RobotDeposit`、`WarehouseController`、`LocationController`、`SystemControlSO` | `architecture.md` の責務・依存図を再掲せず、時系列と分岐に集中。現行 `PathfindingSystem` を「経路探索」と呼べる範囲を明確にする。 |
| **P1 アセット／設定カタログ** `Assets/Docs/asset-catalog.md` | Unity開発者が主要Scene、Prefab、SO、JSON、設定の参照元を特定する。 | Build Settings対象Scene、Prefab構成、SO継承とデータ、JSONロード名、素材・加工品・レベルアップ設定、ScriptableObject参照表。 | `Assets/Scenes/**`、`Assets/Prefab/**`、`Assets/ScriptableObject/**`、`Assets/Resources/**`、`ProjectSettings/EditorBuildSettings.asset`、`Packages/manifest.json` | Scene／Prefab内部の全UI部品は重要箇所のみ記載。GUIDとInspector参照が実行時に一致するか確認し、変更頻度の高い値を重複転記しない。 |
| **P2 保守性・技術的課題** `Assets/Docs/engineering-notes.md` | 開発者・技術評価者が現状の制約、リスク、テスト観点を把握する。 | 責務境界、密結合、コマンド解析方式、インベントリ／倉庫の同期、命名規則、既知の制約、回帰確認項目。改善案は実装事実と分けて記載。 | `Assets/Scripts/**`、既存`documentation.md`、該当ScriptableObject・Prefab | 本書はコードの不具合一覧ではなく、根拠と影響を伴う保守メモに限定。優先度・対応状況はIssue等で別管理し、設計提案を現行仕様に混ぜない。 |
| **P2 ゲーム仕様・デザイン意図** 既存 `Assets/Docs/story.md` と `Assets/Docs/README.md` を役割分担 | 企画・開発者はゲームの体験価値と仕様を、プレイヤーは操作を理解する。 | Storyは世界観・コアループ・意図・将来像。マニュアルは操作、入力形式、失敗条件、現在のプレイ範囲。 | 既存story／README、`TutorialSO.asset`、`SystemControlSO.asset`、コマンド定義、現行仕様書 | 新規企画書を増やす前に既存storyへ統合できるか判断。構想の記述は必ず「未実装／検討中」と明記する。ゲームルールと演出上の意図を切り分ける。 |
| **P3 今後の展望・拡張ロードマップ** `Assets/Docs/roadmap.md` | 開発者・ポートフォリオ読者に、構想と段階的な発展方針を伝える。 | コマンド言語、複数ロボット、資源加工、研究・交易等の候補、依存関係、優先判断、未決事項。 | `AlgorithmColony_Memo.md`、`Assets/Docs/story.md`、`Assets/Scripts/So/Database/**`、現行仕様・技術課題資料 | 実装済み機能の説明と明確に分離。確定ロードマップではなく、未決定項目・仮説・判断基準を記し、実現時期を根拠なく約束しない。 |
| **P3 ポートフォリオ紹介** `README.md` の整理（新規重複資料は作らない） | 採用担当者、技術者、プレイヤーに短時間で作品の魅力と技術的工夫を伝える。 | 作品概要、ゲームプレイの核、プレイ可能範囲、技術的ハイライト、画面／短い動画、担当範囲、制約と今後、詳細資料へのリンク。 | 現行README、current-scope、architecture、logic-flows、story、ビルド設定 | READMEを入口ページとして使い、詳説を各資料へ委譲する。スクリーンショット・動画、開発者の担当範囲、配布可能なビルド、第三者アセット表記は要確認。 |

### ドキュメント間の情報源ルール

- 実装事実はコード・Scene・Prefab・ScriptableObjectを一次情報とし、現行仕様書はそれを読者向けに要約します。
- ゲームの意図・世界観は既存のstoryを一次資料とし、技術資料に企画意図を重複させません。
- 今後の案はroadmapまたはstory内の構想節に集約し、マニュアルと現行仕様には混在させません。
- 同じクラス責務・数値・フローを複数資料に完全転記せず、詳細資料へのリンクで参照します。

## 6. 作成優先順位とその理由

| 順位 | 対応 | 理由 |
|---:|---|---|
| 1 | 現行仕様・実装範囲 | 「何が実装済みか」を誤解なく示す土台です。README、メモ、コード間の差を先に解消し、未実装機能の誤記を防ぎます。 |
| 2 | アーキテクチャ・データ設計 | クラス責務、依存関係、SOとランタイムデータの所在を整理し、保守性・設計力を説明する基礎にします。 |
| 3 | ロジックフロー | コマンド、移動、採集、格納、レベルアップがどのように連携するかを説明し、プロジェクトの中心的な技術体験を示します。 |
| 4 | アセット／設定カタログ | UnityのInspector参照やデータ定義を追跡しやすくし、Scene・Prefab・SOの構成を再現可能にします。 |
| 5 | 保守性・技術的課題 | 制約と改善余地を根拠付きで示し、未対応・部分実装を透明化します。先行資料がないと課題の影響範囲を確定しづらいため、この順序です。 |
| 6 | 既存マニュアル／storyの役割整理 | 既存資産を活かしてユーザー向け説明と企画意図を整理し、重複を抑えます。 |
| 7 | 展望・拡張ロードマップ | 現状アーキテクチャと制約を踏まえた現実的な拡張案にするため、仕様・設計資料の後に作成します。 |
| 8 | ポートフォリオ向けREADME整備 | 上記資料で事実関係を確定した後に、ゲームの魅力と設計上の工夫を簡潔に伝える入口として仕上げます。 |

## 7. 不明点・要確認事項

1. **実プレイ確認：** チュートリアルの開始・完了条件、拠点レベルアップ、加工施設Prefab出現後のUI・操作をUnityで通し確認する。
2. **複数ロボット：** 追加生成時の選択、コマンドパネルとの紐付け、複数体の同時実行と状態表示を確認する。
3. **コマンド仕様：** 大文字小文字、空白、`;` の扱い、数量0・負値、存在しないタグ、対象未発見、採集対象消滅時の挙動を確認する。入力検証と実行側で受け付ける文法の差も明らかにする。
4. **ループ機能：** `Loop();` を利用可能にする意図があるのか、既存の特殊処理が残存コードなのかを開発者に確認する。
5. **経路探索の意図：** 現状の直線分割＋Raycast回避を当面の仕様とするのか、将来的なA*等の経路探索を予定するのか確認する。
6. **加工・施設：** 加工ボタンの処理、加工品データ、素材消費・生成、加工施設のScene上の既定配置／解放後配置を確認する。現行の解放データが1件のみである理由も確認する。
7. **データの永続性：** `SystemControlSO` 等に対するランタイム変更がセッション間で保存されるか、セーブ／ロード機能が別途存在するか確認する。
8. **アセット参照：** `GameScene.unity` の作業ツリー変更が存在するため、現在のScene状態を基準にしつつ、未コミットの変更を意図した設定として扱ってよいか確認する。Prefab、SO参照とScene上のInspector値をUnityで照合する。
9. **制作・ポートフォリオ情報：** 開発者の担当範囲、対象プラットフォーム、操作環境、配布可能なビルド、スクリーンショット／動画、外部アセットのクレジットを確認する。
10. **テスト方針：** リポジトリにはNavMeshComponents関連のEditMode／PlayModeテストプロジェクトが見えますが、ゲーム固有機能のテスト範囲・実行手順は別途確認する。

## 8. 次に着手する具体的なタスク

1. `current-scope.md` のドラフトを作り、Move／Gather／Deposit、チュートリアル、倉庫、拠点レベルアップ、加工施設について実装成熟度を分類する。
2. Unity Editorで `GameScene` を起動し、チュートリアル開始からレベル1・加工施設出現までを確認する。確認結果と再現手順を記録する。
3. `architecture.md` 用に、81件のC#スクリプトを責務別に分類し、主要クラスと呼び出し・データ依存をコード参照付きで確定する。
4. Scene・Prefab・ScriptableObjectのInspector参照を照合し、主要アセット間の対応表を作る。`GameScene.unity` の既存作業ツリー変更は保持し、意図を確認するまで編集しない。
5. `logic-flows.md` にコマンド検証、実行状態遷移、移動、採集、格納、レベルアップを図示する。Loopと経路探索の現在の仕様・制約も明記する。
6. READMEと3つの `Assets/Docs` 資料を、新規資料と重複しない範囲で段階的に整備する。既存情報の削除・上書きは行わず、差分をレビューしてから更新する。
7. 最後にroadmapとポートフォリオ入口を整え、実装済み・部分実装・構想の表示を全資料で統一する。

### 調査時の作業ツリー状態

調査開始時点で `Assets/Scenes/GameScene.unity`、複数の画像 `.meta`、URP設定、`ProjectSettings/ProjectSettings.asset` に既存の変更があり、`Assembly-CSharp.csproj.lscache` と `Build.zip` が未追跡でした。本計画の作成では、これらのファイルを変更・削除していません。Unity Editorでの実行確認はこの段階では行っていません。
