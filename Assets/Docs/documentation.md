# 開発者向け技術資料（統合先）

このページにあったコマンド、ロボット実行、管理クラス、拠点進行、保守メモを、現行コードを再確認したうえで共通ドキュメントへ統合しました。

- 現行ゲーム仕様と主要クラスの責務：[GameSpecification.md](../../Documentation/GameSpecification.md)
- 機能ごとの実装成熟度と根拠：[ImplementationStatus.md](../../Documentation/ImplementationStatus.md)
- コマンド／移動／採集／格納／拠点進行の処理図：[LogicFlows.md](../../Documentation/LogicFlows.md)
- 将来の拡張と依存関係：[FutureRoadmap.md](../../Documentation/FutureRoadmap.md)

**重要な訂正：** 旧版にあった「`Loop();` による循環動作に対応」という記述は、入力検証と `RobotCommandExecute.ExecuteCommand()` に `Loop` の実装がないため、現行利用可能機能としては確認できません。新しい資料では実行コルーチンに残る分岐と利用可能なコマンドを区別しています。

Scene／Prefab上のInspector参照とUnity Editorでの通し動作は、この統合資料のコード調査だけでは保証しません。要確認事項は各リンク先で明示しています。
