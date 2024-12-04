using UnityEngine;
using TMPro;

public class IMEBlocker : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;

    private void Start()
    {
        // IME無効化用の検証イベントを登録
        inputField.onValidateInput += ValidateChar;
    }

    private char ValidateChar(string text, int charIndex, char addedChar)
    {
        // 非ASCII文字を無効化
        return (addedChar <= 127) ? addedChar : '\0';
    }
}
