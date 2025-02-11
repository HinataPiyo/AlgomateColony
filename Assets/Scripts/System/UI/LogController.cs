using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LogController : MonoBehaviour
{
    public static LogController instance;
    [Header("ログを表示するTMP"), SerializeField] TextMeshProUGUI logField;
    [SerializeField] List<string> logs = new List<string>();
    [SerializeField] const int MaxLogCount = 10;
    private void Awake() {
        if(instance == null) instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        logField.text = "";
    }

    public void SetLog(BaseStatus _base, string _log)
    {
        string _t = $"{_base.robotName} : {_log}";
        logs.Add(_t);

        // 新しいログを追加する前に、最大ログ数を超えていれば先頭のログを削除
        if(logs.Count > MaxLogCount)
        {
            logs.RemoveAt(0); // 先頭のログを削除
        }

        // ログフィールドに現在のログリストを反映
        logField.text = string.Join("\n", logs); // リストを改行区切りで結合して表示
    } 
}
