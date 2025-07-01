using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public static TutorialController insrance;
    [SerializeField] SystemControlSO scSO;
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
    int current_BigListNumber;
    int current_FineListNumber;
    [SerializeField] AnimEndFlag_AnimStat animEndFlag;
    bool fineText_allActiv;

    public int BigTaskNumber { get{ return current_BigListNumber; } }

    void Awake()
    {
        insrance = this;
        
    }


    void Start()
    {
        tTaskList = tutorialSO.tutorialTasks;
        task_text.text = "";
        // チュートリアルをまだ実行していなければ
        if(tutorialSO.tutorialEndFlag == false || tutorialSO.resetTutorial == true)
        {
            tutorialSO.ResetTutorial();
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
                current_BigListNumber = ii;

                // 大きなタスクの説明文が全て表示されたら
                yield return new WaitUntil(() => tTaskList[ii].textAllActiv);

                if(ii == tTaskList.Count - 1)
                {
                    tTaskList[tTaskList.Count - 1].completionFlag = true;
                }

                yield return new WaitForSeconds(2f);


                // 小さなタスクを表示させていく
                for(int qq = 0; qq < tTaskList[ii].fineTasks.Count; qq++)
                {
                    panel.SetActive(true);
                    panel_anim.SetTrigger("PanelOpen");     // パネルを開くアニメーション

                    yield return new WaitForSeconds(1f);
                    SetText(tTaskList[ii].fineTasks[qq].taskExp, tTaskList[ii].fineTasks[qq].textPos);
                    current_FineListNumber = qq;
                    
                    // テキストが全て表示し終わったか確認する
                    yield return new WaitUntil(() => fineText_allActiv);
                    // 細かいタスクが完了するまで待機
                    yield return new WaitUntil(() => tTaskList[ii].fineTasks[qq].completionFlag);

                    panel_anim.SetTrigger("PanelClose");     // パネルを開くアニメーション

                    yield return new WaitUntil(() => animEndFlag.panelCloseFlag);
                    
                    panel.SetActive(false);
                    animEndFlag.panelCloseFlag = false;
                    task_text.text = "";

                    if(tTaskList[ii].completionFlag == true)
                    {
                        yield return new WaitForSeconds(3f);
                    }
                }
            }
            yield return null;
        }

        // チュートリアルが終了したことを知らせる
        tutorialSO.tutorialEndFlag = true;

        yield break;
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
        fineText_allActiv = false;
        foreach (char c in fullText)
        {
            task_text.text += c; // 1文字ずつ追加
            yield return new WaitForSeconds(typeSpeed); // 指定の速度で待機
        }

        fineText_allActiv = true;
    }

    public void TutorialCheck(int _bigNum, int _fineNum)
    {
        if(tTaskList[_bigNum].fineTasks[_fineNum].completionFlag == false)
        {
            tTaskList[_bigNum].fineTasks[_fineNum].completionFlag = true;
        }
    }

    public void BigTaskCheck(int _bigNum)
    {
        if(tTaskList[_bigNum].completionFlag == false)
        {
            tTaskList[_bigNum].completionFlag = true;
        }
    }
}
