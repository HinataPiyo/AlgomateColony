using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "TutorialSO", menuName = "System/TutorialSO")]
public class TutorialSO : ScriptableObject
{
    [Header("treu : チュートリアル終了, false : チュートリアルがまだ")]
    public bool tutorialEndFlag;
    [Header("treu : チュートリアルを初期化, false : 初期化しない")]
    public bool resetTutorial;
    // チュートリアルで行うTaskをリストにまとめておく
    public List<TutorialTask> tutorialTasks = new List<TutorialTask>();

    [System.Serializable]
    public class TutorialTask
    {
        [Header("大きなタスク")]
        public Vector2 focusPos;         // 見てほしいところなどにフォーカスする時の位置
        public bool textAllActiv = false;       // テキストがすべて表示されたら
        public bool completionFlag = false;     // タスクが完了したことを知らせる
        [TextArea(2, 5)] public string taskExp;          // タスクの説明

        [Space(20), Header("小さなタスク")]
        public List<FineTask> fineTasks = new List<FineTask>();

        [System.Serializable]
        public class FineTask
        {
            public Vector2 focusPos;         // 見てほしいところなどにフォーカスする時の位置
            public Vector2 textPos;
            public bool completionFlag;     // タスクが完了したことを知らせる
            [TextArea(2, 5)] public string taskExp;          // タスクの説明
        }
    }

    /// <summary>
    /// チュートリアルを初期化する
    /// </summary>
    public void ResetTutorial()
    {
        tutorialEndFlag = false;
        foreach (var tuto in tutorialTasks)
        {
            tuto.completionFlag = false;
            tuto.textAllActiv = false;

            foreach (var fine in tuto.fineTasks)
            {
                fine.completionFlag = false;
            }
        }
    }
}