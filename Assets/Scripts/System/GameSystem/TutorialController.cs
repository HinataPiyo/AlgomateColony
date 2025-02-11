using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [SerializeField] TutorialSO tutorialSO;
    List<TutorialSO.TutorialTask> tTaskList;
    [Header("チュートリアルパネル")]
    [SerializeField] GameObject panel;
    [SerializeField] Animator panel_anim;
    [SerializeField] TextMeshProUGUI task_text;     // 自由に動かすテキスト

    [Header("大きなタスクのスクリプトを取得")]
    [SerializeField] BigTaskSlot bigTask;           // 左上に表示させる

    [Header("文字が表示される速度")]
    [SerializeField] float typeSpeed = 0.05f;       // 文字が表示される速度
    string fullText; // 全文
    Vector2 textPos;
    Coroutine typingCoroutine;
    int currentListNumber;
    [SerializeField] AnimEndFlag_AnimStat animEndFlag;


    void Start()
    {
        tTaskList = tutorialSO.tutorialTasks;
        task_text.text = "";

        // チュートリアルをまだ実行していなければ
        if(tutorialSO.tutorialFlag == false)
        {
            // チュートリアル開始
            StartCoroutine(TutorialProgress());
        }

        panel.SetActive(false);
    }

    IEnumerator TutorialProgress()
    {
        // 最後のチュートリアルが終了するまでループ
        while(tTaskList[tTaskList.Count - 1].completionFlag == false)
        {
            for(int ii = 0; ii < tTaskList.Count; ii++)
            {
                // 大きなタスクを先に表示
                bigTask.SetText(tTaskList[ii]);
                currentListNumber = ii;

                // 大きなタスクの説明文が全て表示されたら
                yield return new WaitUntil(() => tTaskList[ii].textAllActiv);

                yield return new WaitForSeconds(2f);


                // 小さなタスクを表示させていく
                foreach(var _fine in tTaskList[ii].fineTasks)
                {
                    panel.SetActive(true);
                    panel_anim.SetTrigger("PanelOpen");     // パネルを開くアニメーション

                    yield return new WaitForSeconds(1f);
                    SetText(_fine.taskExp, _fine.textPos);
                    // 細かいタスクが完了するまで待機
                    yield return new WaitUntil(() => _fine.completionFlag);

                    panel_anim.SetTrigger("PanelClose");     // パネルを開くアニメーション

                    yield return new WaitUntil(() => animEndFlag.panelCloseFlag);
                    
                    panel.SetActive(false);
                    animEndFlag.panelCloseFlag = false;
                    task_text.text = "";
                }
            }
            yield return null;
        }

        // チュートリアルが終了したことを知らせる
        tutorialSO.tutorialFlag = true;
    }

    /// <summary>
    /// 文字をセットしてアニメーションを開始
    /// </summary>
    /// <param name="text">表示する文字列</param>
    public void SetText(string text, Vector2 pos)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine); // すでに動いている場合は停止
            task_text.text = "";
            task_text.text = fullText;
        }

        fullText = text;
        textPos = pos;
        typingCoroutine = StartCoroutine(TypeText());
    }


    /// <summary>
    /// 文字を1文字ずつ表示するコルーチン
    /// </summary>
    IEnumerator TypeText()
    {
        task_text.text = "";        // テキストを空にする
        task_text.rectTransform.anchoredPosition = textPos;     // テキストの表示位置を設定
        foreach (char c in fullText)
        {
            task_text.text += c; // 1文字ずつ追加
            yield return new WaitForSeconds(typeSpeed); // 指定の速度で待機
        }
    }

    
}
