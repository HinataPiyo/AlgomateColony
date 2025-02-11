using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TutorialSO", menuName = "TutorialSO")]
public class TutorialSO : ScriptableObject
{
    public bool tutorialFlag;       // treu : チュートリアル終了, false : チュートリアルがまだ　
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
}