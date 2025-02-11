using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BigTaskSlot : MonoBehaviour
{
    [SerializeField] Toggle completToggle;
    [SerializeField] TextMeshProUGUI task_text;
    [SerializeField] float typeSpeed = 0.05f; // 文字が表示される速度
    TutorialSO.TutorialTask task;

    string fullText;

    public void SetText(TutorialSO.TutorialTask _task)
    {
        task = _task;
        fullText = _task.taskExp;
        StartCoroutine(TypeText());
    }

    /// <summary>
    /// 文字を1文字ずつ表示するコルーチン
    /// </summary>
    IEnumerator TypeText()
    {
        task_text.text = "";        // テキストを空にする
        foreach (char c in fullText)
        {
            task_text.text += c; // 1文字ずつ追加
            yield return new WaitForSeconds(typeSpeed); // 指定の速度で待機
        }

        task.textAllActiv = true;
    }
}