# プレイヤーマニュアル（統合先）

このページにあったゲーム概要、基本ループ、操作方法、コマンド例、補助入力、プレイ可能範囲、注意事項、未実装予定を、リポジトリの共通ドキュメントへ統合しました。

- ゲーム概要と現在のプレイ体験：[GameOverview.md](../../Documentation/GameOverview.md)
- 現行の操作・コマンド・ゲーム仕様：[GameSpecification.md](../../Documentation/GameSpecification.md)
- 実装済み／部分実装／未実装の区分：[ImplementationStatus.md](../../Documentation/ImplementationStatus.md)
- 主要処理の呼び出し順とフロー図：[LogicFlows.md](../../Documentation/LogicFlows.md)
- 将来構想：[FutureRoadmap.md](../../Documentation/FutureRoadmap.md)

旧記載からの主な整理：

- `Move`、`Gather`、`Deposit` の入力検証と実行上の制約を、現行コードに合わせて説明しています。
- `Deposit(material, amount)` の素材引数と、倉庫が周囲検索される仕様を明記しています。
- 倉庫管理・加工UIのコードが存在することと、各機能が完成していることを区別しています。
- 操作説明はUnity Editorでの通しプレイ検証済みを意味しません。未確認事項は各資料に記載しています。
