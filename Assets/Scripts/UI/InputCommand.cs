using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputCommand : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;

    [Header("コマンド")]
    private string cmd = "cmd:";
    private string location = "location";
    private string setting = "setting";

    private string confirmedText;
    [Header("補完候補")]
    private Dictionary<string, string> shortcuts = new Dictionary<string, string>
    {
        { "c", "cmd:" },
        { "cmd:l", "cmd:location" },
        { "cmd:s", "cmd:setting" }
    };

    private void Start() {
        inputField.onEndEdit.AddListener(EndEdit);
    }

    private void Update()
    {
        if (!inputField.isFocused && Input.GetKeyDown(KeyCode.Tab))
        {
            inputField.Select();
        }
        // 入力フィールドがフォーカスされている状態でTabキーを押した場合
        if (inputField.isFocused && Input.GetKeyDown(KeyCode.Tab))
        {
            string currentText = inputField.text;

            // ショートカット検索（降順で最も長いキーを優先してマッチ）
            foreach (var shortcut in shortcuts)
            {
                if (currentText == shortcut.Key)
                {
                    // 補完結果を設定
                    inputField.text = shortcut.Value;

                    // カーソルを末尾に移動
                    inputField.caretPosition = inputField.text.Length;

                    // デフォルトのタブキー挙動を無効化
                    EventSystem.current.SetSelectedGameObject(inputField.gameObject);
                    break; // 最初の一致で処理終了
                }
            }
        }
    }

    void EndEdit(string text)
    {
        // 入力フィールドの値を取得
        confirmedText = text;
        // パスコードの一致を確認
        if (!string.IsNullOrEmpty(confirmedText)
            && confirmedText == cmd + location)
        {
            // 正しいパスコードの場合
            inputField.text = string.Empty; // 入力をクリア
            FacilityManager.instance.CanvasEnabled(CanvasName.Location, true);
            Debug.Log("コマンドは正しいです。" + confirmedText);
        }
        else if(!string.IsNullOrEmpty(confirmedText)
            && confirmedText == cmd + setting)
        {
            // 正しいパスコードの場合
            inputField.text = string.Empty; // 入力をクリア
            FacilityManager.instance.CanvasEnabled(CanvasName.Setting, true);
            Debug.Log("コマンドは正しいです。" + confirmedText);
        }
        else
        {
            // 間違ったパスコードの場合
            inputField.text = string.Empty; // 入力をクリア
            Debug.Log("コマンドは間違えています。" + confirmedText);
        }
    }
}
