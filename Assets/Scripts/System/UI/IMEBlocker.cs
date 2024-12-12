using UnityEngine;
using TMPro;

public class IMEBlocker : MonoBehaviour
{
    [SerializeField] private TMP_InputField maincanvasInput;
    [SerializeField] private TMP_InputField robot_ProcInput;

    private void Start()
    {
        // IME無効化用の検証イベントを登録
        // maincanvasInput.onValidateInput += ValidateChar;
        robot_ProcInput.lineType = TMP_InputField.LineType.MultiLineNewline;

    }

     private void Update()
     {
         Debug.Log(robot_ProcInput.isFocused);
    //     // Enterキーが押された場合の処理
    //     if (Input.GetKeyDown(KeyCode.Return) && Input.GetKey(KeyCode.LeftShift))
    //     {
    //         Debug.Log("キーが押されました。");
    //         if (robot_ProcInput.isFocused == true)
    //         {
    //             // 改行を挿入
    //             robot_ProcInput.text += "\n";
    //             Debug.Log("改行されました。");

    //             // キャレット位置を末尾に移動
    //             robot_ProcInput.caretPosition = robot_ProcInput.text.Length;
    //         }
    //     }
     }

    private char ValidateChar(string text, int charIndex, char addedChar)
    {
        // 非ASCII文字を無効化
        return (addedChar <= 127) ? addedChar : '\0';
    }
}
