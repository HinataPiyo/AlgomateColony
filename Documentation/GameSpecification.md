# AlgomateColony 現行ゲーム仕様書（コード照合版）

## 1. この資料の扱い

本書は現行コードとScriptableObjectデータから確認できる仕様をまとめたものです。企画資料にある将来案は現行仕様に含めません。ソースに処理が存在することと、Unity Editor／配布ビルドで正常に通しプレイできることは区別します。

- **確認済み**：コード／アセット上で設定・処理が確認できる。
- **部分実装**：一部の経路はあるが、入力から結果までの完成や例外処理が確認できない。
- **要確認**：Inspector参照または実行時の挙動・設計意図の確認が必要。
- Unity Editor上の実プレイ検証は未実施です。

関連する呼び出し図は [LogicFlows.md](./LogicFlows.md)、機能別の成熟度は [ImplementationStatus.md](./ImplementationStatus.md) を参照してください。

## 2. ゲーム進行の概要

### 2.1 現行チュートリアルに記載された短期目標

`Assets/ScriptableObject/System/TutorialSO.asset` に記録されている順序は次のとおりです。

1. アルゴメイトを拠点へ移動させる。
2. 石を移動・採集する。
3. 倉庫へ移動して石を格納する。
4. 拠点パネルで必要素材をそろえ、レベルアップする。
5. チュートリアルを完了する。

このタスクデータが存在することは確認済みですが、実プレイで最後まで進められることは未確認です。チュートリアル完了後の最終目標は、コード・現行データから確定できません。

### 2.2 起動時のロボット

- `GameManager.Start()` は `RobotFactory.CreateRobot()` を呼びます。
- `RobotFactory` はInspector設定のPrefabを生成して `RobotController.SpawnInit()` を呼び、生成された `BaseStatus` を `GameManager.RobotList` に追加します。
- 通常起動時の初期生成呼び出しは1回です。テスト用 `CreatFlag` による追加生成経路はあります。
- 生成数、ゲーム開始時の配置、複数体を操作するUIが実用上サポートされるかは要確認です。

## 3. コマンド仕様

### 3.1 入力から実行まで

1. ロボット選択時に `RobotStatusPanelManager.SetRobotStatus()` が選択中のロボットを `RobotCommandUIController.SetRobot()` へ渡します。
2. 入力欄の複数行文字列を `RobotCommandUIController.CheckTextCommand()` が改行単位に分割し、空行を除外します。
3. 「チェック」操作で各行を `CommandHandler.CheckCommand()` に渡します。
4. 全行が検証成功した場合、行配列が選択中ロボットの `RobotCommandExecute.ProcText` に設定され、実行ボタンが有効になります。
5. 実行ボタンが `RobotCommandExecute.StartCoroutine_CommandToExecution()` を呼びます。実行クラスは1行ずつ状態遷移を起こし、`StateEndFlag` がtrueになるまで待機します。

### 3.2 現行コマンド

| コマンド | 構文例 | 検証・処理 |
|---|---|---|
| 移動 | `Move(rock);` | 引数が `MoveCommands.json` に存在することを検証。実行時は指定タグのオブジェクトを検索し、見つけると移動する。 |
| 採集 | `Gather(rock);` | 引数が `GatherCommands.json` に存在することを検証。実行時は直前等に `hitInfo` として保持された対象と引数タグが一致する必要がある。 |
| 格納 | `Deposit(rock, 30);` | 第1引数素材、第2引数整数を `DepositCommands.json` を参照して検証。実行時には倉庫を近傍検索する。格納対象の倉庫を引数で指定する構文ではない。 |

現在のJSONデータは次の候補を定義しています。

- Move：`location`、`warehouse`、`rock`、`tree`、`ironOre`
- Gather：`rock`、`tree`、`ironOre`
- Deposit素材：`rock`、`tree`、`ironOre`

JSONは `CommandLoader` が `Resources.Load<TextAsset>()` で読み込みます。JSONのロード失敗・構文失敗を明示的に処理するコードは確認できません。

### 3.3 検証と実行の差

- `CommandHandler.CheckCommand()` は `StartsWith()`、括弧内文字列抽出、引数候補照合を行います。末尾セミコロンやコマンド全文の厳密な文法を検証しません。
- `RobotCommandExecute.ExecuteCommand()` は `Move(arg);`、`Gather(arg);`、`Deposit(arg);` と行全体が一致した場合だけ状態を変更します。
- そのため、チェックが成功しても実行分岐に一致しないコマンドがあり得ます。その場合、状態は変わらず `StateEndFlag` 待ちになる可能性があります。
- 空入力はUI上の「失敗」となります。空白のみの行、行頭空白、コマンド名の大文字小文字、行末空白、余分なセミコロンの組合せについては、完全なパーサーを持たないため注意が必要です。
- `Deposit` 数量は `int.TryParse()` のみで検査されるため、0と負数も検証を通ります。正値・最大値の検査はありません。
- `Loop();` は実行コルーチンに最終行時のループ分岐が残っていますが、コマンド検証および `ExecuteCommand()` の実処理に登録されていません。現在利用可能なコマンドとして扱いません。
- `cmd:setting` と `cmd:location` は `InputCommand` が扱う施設UIの補助入力であり、上記ロボット行動コマンドとは別機能です。

### 3.4 操作・施設パネル

- `ObjectClick.OnPointerClick()` は、Canvasが開いていないときの `location`、`warehouse`、`chargingroom`、`processingroom` タグのオブジェクトクリックから、対応する施設Canvasを開きます。
- `Robot` タグのオブジェクトをクリックすると、ロボット状態パネルを開き、選択したロボットを `RobotStatusPanelManager.SetRobotStatus()` に渡します。
- `FacilityManager` は設定、ロボット状態、拠点、倉庫、充電室、加工施設のCanvas表示を切り替えます。Canvasが開いているときにEscapeを押すと、各Canvasを閉じる処理があります。
- `InputCommand` は補助入力 `cmd:location` と `cmd:setting` を受け付けます。入力欄が未フォーカス時にTabでフォーカスし、入力中に `c` + Tab、`cmd:l` + Tab、`cmd:s` + Tab でそれぞれ `cmd:`、`cmd:location`、`cmd:setting` に補完するコードがあります。
- 旧プレイヤーマニュアルには、右上の設定ボタンからコマンド概要を確認できるとの案内があります。設定Canvasの現行表示内容はScene上で要確認です。
- 旧マニュアルは、ロボットがオブジェクトに引っかかった場合に再実行または別対象への移動を案内していました。これは回避策の案内であり、コードが移動復旧を保証する仕様ではありません。
- ワールド上のクリック操作にはCollider、EventSystem、Scene上の参照が関係します。実行時の配線が期待通りかはUnity上で要確認です。

## 4. アルゴメイトの行動

### 4.1 状態遷移

`RobotController` は少なくとも `DoNon`、`Search`、`Move`、`GatherResource`、`Recharge`、`NonEnergy`、`Deposit` の状態を持ちます。コマンド実行が状態を設定し、`RobotController.Update()` がその状態に応じたコンポーネントを呼び出します。

### 4.2 移動・対象探索

- `RobotSearch.Search(target)` は現在位置を中心に `Physics2D.OverlapCircleAll()` を実行し、LayerMask上のColliderから `target` と同じタグを検索します。
- 見つからない場合は探索半径を0.6加算します。コード上の最大半径や検索失敗の完了条件は確認できません。
- 見つかると `RobotMovement.Set_TargetPosition()` を呼び、RobotControllerを `Move` 状態へ変更します。
- 到達成功は `RobotMovement.ReachedDestination()` のOverlapCircleで対象Colliderを得て、そのTransformが対象Transformと一致することです。
- LayerMask、半径、障害物判定などはPrefab／SceneのInspector設定に依存します。

### 4.3 採集

- `RobotGather` は現在の `RobotController.GetHitInfo()` を使用します。採集コマンド自体が対象を検索し直す処理はありません。
- 指定名と対象Colliderのタグを照合します。
- `MaterialSO.gatherTime`、`BaseStatus.gatherRate`、装備補正により採集時間を進め、完了後に `BaseMaterial.TakeDamage(gatherStrength)` を実行します。
- 素材の `serialNum` を使って既存所持Slotまたは空Slotを探し、`MaterialSO` と `BaseMaterial.GetAmo()` の個数を加算します。
- `BaseStatus` のSlot配列は5枠、Slotごとの `ITEM_STACK_MAX` は100です。ただし採集加算時に上限超過を防止する処理は確認できません。
- 標準的な資源Prefabの `BaseMaterial` はHPが0以下になるとGameObjectを破棄します。破棄後に対象参照がnullとして扱われると、採集ループが終了しコマンド完了を通知する経路に入ります。

### 4.4 充電状態

`RobotController` には充電・エネルギー切れ状態が存在し、Prefab上のBaseStatusにもエネルギー情報があります。しかし、チュートリアルの標準採集・格納ループと異なる充電処理の全条件・施設連携・プレイ上のルールは本書では確定しません。充電施設の実挙動、利用可能なコマンド、通常の停止・復帰条件は要確認です。

## 5. 資源・所持品・倉庫

### 5.1 資源定義

`MaterialSO` は素材名、タグ名、serialNum、最大HP、採集時間、対応装備等を定義します。現行アセットには少なくとも石（`rock`）、木材（`tree`）、鉄鉱石（`ironOre`）があり、ワールド上Prefabの `BaseMaterial` が該当SOを参照します。

### 5.2 所持品と格納

- ロボット所持品は `BaseStatus.Slot[]` に保持します。
- 倉庫在庫は `WarehouseController` 内の `List<DataType.WAREHOUSE_SLOT>` に保持します。
- `RobotDeposit.Deposite()` は `warehouse` タグを半径検索し、所持Slotの素材タグと指定数量を照合します。
- 成功時に倉庫へ加算した後でロボットSlotを減算します。両更新をまとめるトランザクション、失敗時ロールバック、上限容量の拒否は確認できません。
- 倉庫表示用Slotとデータリストは `WarehouseController` が同期します。ロボットの所持Slot表示は `RobotStatusPanelManager` が更新します。
- UIの同期処理にindex参照のずれ得る実装があるため、表示値はデータリストの代替情報として扱わないでください。詳細は [LogicFlows.md](./LogicFlows.md) を参照してください。

## 6. 拠点レベルと施設解放

- 現行進行レベルは `SystemControlSO` が保持します。
- `LocationLevelupUnlock` の `NextUnlock` が次レベルの必要素材、表示情報、生成Prefab、潜在能力値を定義します。
- 現行 `LocationLevelupUnlock.asset` は1件の解放データを保持します。必要素材は石・鉄鉱石・木材の各30個で、レベル0からレベル1への解放先として加工室Prefabを参照し、生成位置は `(5, -2)` です。
- `LocationController` は素材Slotを表示し、1秒周期のUpdateManagerイベントで素材充足を確認してボタンのinteractableを変更します。
- ボタンクリック時、必要素材を倉庫から消費し、`SystemControlSO.LocationLevelUp()` を呼び、次レベルでデータ指定されたPrefabを生成します。現行コードではPrefab生成分岐はレベル0に限られます。
- `UseMaterial()` は消費の成功・失敗を呼び出し元へ返しません。クリック時に素材を再検証するガードもないため、表示上のボタン有効化だけでは消費成功を保証しません。
- レベル上限時の扱いはデータ不足時にnullの `statusParam` をforeachする可能性があるため、上限操作が正常に抑止されるか要確認です。
- 加工室Prefabを生成することと、加工レシピを実行して製品を生成することは別です。現行 `ProcessingController` は候補・必要素材表示と作成ボタンの有効化まで確認でき、ボタン押下時の加工処理は確認できません。

## 7. チュートリアル

- `TutorialController.Start()` は `TutorialSO` からタスクを取得し、未完了またはreset指定時にフラグをリセットして進行コルーチンを開始します。
- `TutorialProgress()` が大タスク・小タスクを順に表示します。説明文は `TypeText()` が一文字ずつ表示し、小タスク完了と閉じアニメーション終端を `WaitUntil` で待ちます。
- 操作・行動の各クラスが `TutorialCheck(bigIndex, fineIndex)` や `BigTaskCheck(bigIndex)` を呼びます。
- 現行 `TutorialSO.asset` には、アルゴメイトの拠点移動、採集と格納、拠点レベルアップ、完了のタスクがあります。
- 必要フラグがtrueにならない場合、またはアニメーション終端イベントが通知されない場合、コルーチンにタイムアウトがないため進行が止まります。
- `TutorialCheck()` はIndex範囲を検証しません。空のタスクリストも通常処理として安全に扱われません。
- Scene上の各UI、Animator、SO参照とアニメーション終端フラグの実接続は要確認です。

## 8. 機能成熟度一覧

| 機能 | 区分 | 根拠 |
|---|---|---|
| コマンド入力・検証 | 部分実装 | 3コマンドの入力・検証経路はあるが、検証と実行の文法が異なり、エラー詳細・タイムアウトがない。 |
| 移動 | 部分実装 | タグ検索、移動、到達判定はある。探索上限や失敗終了条件は確認できず、停止ケースに完了通知がない。 |
| 採集 | 部分実装 | HP・時間・所持Slotへの加算はあるが、満杯・不一致等で完了通知されない経路や容量上限処理の欠如がある。 |
| 倉庫格納 | 部分実装 | 成功時の倉庫加算・所持減算はあるが、異常経路の完了通知・数量制約・トランザクション保証がない。 |
| 経路探索 | 未実装（グラフ探索）／部分実装（経路生成と回避） | `PathfindingSystem` は直線補間ウェイポイントを生成し、移動時に局所Raycast回避を行う。A*等の探索は確認できない。 |
| 拠点レベルアップ | 部分実装 | 表示・消費・レベル増加はあるが、消費結果再確認と最大レベル時の安全な扱いに懸念がある。 |
| 加工施設解放 | 部分実装 | レベル0でPrefabをInstantiateするコードとアセット設定がある。Editor上で配置・利用可能になるかは未確認。 |
| 加工実行 | 未実装（確認できた範囲） | 加工候補と必要素材表示はあるが、作成操作から製品生成までの処理を確認できない。 |
| チュートリアル | 部分実装 | データ・UI進行と通知呼び出しはあるが、通しプレイ、全タスク通知、アニメーション参照は要確認。 |
| 複数ロボット運用 | 要確認 | 追加生成経路はあるが、複数選択・同時実行・割当運用は未検証。 |
| 白黒を基調とする全体ビジュアル | 要確認 | `BlackWhite.png` というUI画像はあるが、それだけでは作品全体の色設計を示せない。画面全体での確認が必要。 |

詳しい状態・未実装と構想の区別は [ImplementationStatus.md](./ImplementationStatus.md) を参照してください。

## 9. 主要クラスの責務（概要）

既存の開発者向け資料 `Assets/Docs/documentation.md` のクラス紹介を現行コードと照合し、責務の概要として整理します。

| 領域 | 主なクラス | 責務 |
|---|---|---|
| コマンド定義・検証 | `CommandLoader`、`CommandHandler` | Resources JSONからコマンド候補を読み込み、入力行を検証する。 |
| コマンドUI・実行 | `RobotCommandUIController`、`RobotCommandExecute` | 入力行の検証・ロボットへの設定、行単位の逐次実行を担う。 |
| ロボット行動 | `RobotController`、`RobotSearch`、`RobotMovement`、`RobotGather`、`RobotDeposit` | 状態管理、対象検索、移動、採集、倉庫格納を担う。 |
| ゲーム・パネル管理 | `GameManager`、`FacilityManager`、`RobotStatusPanelManager` | 起動時のロボット生成起動、Canvas切替、選択ロボットのステータス・コマンドUI連携を担う。 |
| 拠点進行・データ | `LocationController`、`DataManager`、`SystemControlSO`、`LocationLevelupUnlock` | 必要素材の表示、在庫消費要求、拠点レベル更新、レベル別データ参照に関わる。 |

これは主要クラスの概略であり、全依存関係やInspector上の参照図ではありません。処理順とデータ変化は [LogicFlows.md](./LogicFlows.md) を参照してください。

コマンド追加・変更時は検証 (`CommandHandler`) と実行 (`RobotCommandExecute`) の両方への影響確認が必要です。素材やレベル解放の変更では、ScriptableObject等の定義データとUI表示・消費処理の整合も確認対象になります。これは既存技術資料の保守メモを現行構成に合わせて引き継いだ注意点です。

## 10. 現行仕様と企画資料の差

- 現行の主要コマンドは `Move`、`Gather`、`Deposit` です。`if`、`for`、`while`、関数、`broadcast`、他コロニーとの交易などは企画メモ上の案であり、実装済みとして扱いません。
- `Deposit(material, amount)` の第1引数は倉庫ではなく素材タグです。倉庫を引数に取る構文ではありません。
- 企画メモはタイル単位の移動を記載していますが、コードはTransform移動、直線補間ウェイポイント、Physics2Dを使っています。
- Storyに記載された「直接操作より計画」等は企画意図として紹介できますが、開発者本人の制作動機やプレイヤー心理を確定する記述ではありません。

## 11. 開発者への確認事項

1. Unity Editorでチュートリアル開始からレベルアップ・加工室出現までの通しプレイが成功するか。
2. 移動対象・障害物・倉庫のLayerMask／タグ／ColliderがScene上でどう設定されているか。
3. 対象が検索されない・消失する・ロボットがエネルギー切れになる場合の期待動作。
4. 採集後の所持上限・超過時挙動、および倉庫上限の有無。
5. 加工室Prefabの生成後、どの操作が実際に利用可能か。
6. `Loop();` の実行分岐が残っている意図と、将来のループ構文仕様。
7. 拠点レベルの最大値、上限後のUI・操作。
8. チュートリアルのフォーカス位置、画面遷移、アニメーション終了通知のScene上の接続。
9. ゲーム全体の白黒ビジュアルを意図しているか、全体画面での実例があるか。
10. 複数アルゴメイトを通常プレイで制御することが現行要件か将来構想か。
