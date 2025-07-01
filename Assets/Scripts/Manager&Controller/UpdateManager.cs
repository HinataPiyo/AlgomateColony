using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// ゲーム全体で共有される、一定間隔でイベントを発生させるタイマー
/// </summary>
public class UpdateManager : MonoBehaviour
 {
     // シングルトンインスタンス
    public static UpdateManager Instance { get; private set; }

    /// <summary>
    /// 1秒ごとに発生するグローバルなイベント
    /// </summary>
    public static event Action OnUpdateTick;

    [Tooltip("イベントを発生させる間隔（秒）")]
    const float tickInterval = 1.0f;

    void Awake()
    {
        // シングルトンの設定
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーンをまたいで存在させる
        }
    }

    void Start()
    {
        // タイマー用のコルーチンを開始
        StartCoroutine(TickCoroutine());
    }

    IEnumerator TickCoroutine()
    {
        // 無限ループで実行し続ける
        while (true)
        {
            // 指定した時間だけ待機する
            yield return new WaitForSeconds(tickInterval);

            // イベントを購読している全てのメソッドを呼び出す
            // ?.Invoke() は、購読者がいなくてもエラーにならない安全な呼び出し方
            OnUpdateTick?.Invoke();
        }
    }
}