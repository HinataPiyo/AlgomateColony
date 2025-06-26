using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// コマンドを入力する箇所を管理するクラス
/// </summary>
[RequireComponent(typeof(RobotCommandExecute))]
public class RobotCommandUIController : MonoBehaviour
{
    [Header("確認ボタン")]
    [SerializeField] Button checkButton;
    [SerializeField] TextMeshProUGUI check_text;

    [Header("実行ボタン")]
    [SerializeField] Button executeButton;
    [SerializeField] TextMeshProUGUI execute_text;

    [Header("コードを入力するフィールド")]
    [SerializeField] TMP_InputField inputField;

    [Header("チェック完了フラグ")]
    [SerializeField] bool complete;

    [Header("コーディング内容")]
    [SerializeField] string[] proctext;

    RobotCommandExecute robotCmdExecute;
    public RobotCommandExecute Set_RobotCommandExecute { set { robotCmdExecute = value; } }
    public TMP_InputField InputCommandField { get { return inputField; } }

    void Awake()
    {
        executeButton.onClick.AddListener(ExecuteButtonOnClick);
        checkButton.onClick.AddListener(CommandCheckButtonOnClick);

        executeButton.interactable = false; // 実行ボタンを最初は押せない状態にする
    }

    /// <summary>
    /// コマンドがしっかりと動作するか確認する
    /// </summary>
    /// <param name="proctext">ユーザーが入力したコマンドの配列</param>
    public bool InputCommand(string[] proctext)
    {
        // もし、何も入力されていなかった場合
        if (string.IsNullOrEmpty(inputField.text))
        {
            UpdateButtonState(false, "失敗", Color.red);
            return false;
        }

        // コマンドの検証
        int validCommandCount = 0;
        foreach (var com in proctext)       // 入力されたコマンドを一つずつ確認する
        {
            // コマンドが正しいかどうかを確認する
            string _command = CommandHandler.CheckCommand(com);

            // コマンドが正しい場合
            if (_command != null)
            {
                validCommandCount++;    // 正しいコマンドの数をカウントする
            }
            else    // コマンドが間違っていた場合
            {
                Debug.Log($"[ {com} ]コマンドが間違えています。");
            }
        }

        // 全てのコマンドが正しい場合
        if (validCommandCount == proctext.Length)
        {
            UpdateButtonState(true, "成功", Color.green);
            return true;
        }

        // 一つでもコマンドが間違っていた場合
        UpdateButtonState(false, "失敗", Color.red);
        return false;
    }

    /// <summary>
    /// ボタンの状態を更新する
    /// </summary>
    /// <param name="isEnabled">正しいか正しくないか</param>
    /// <param name="message">テキスト</param>
    /// <param name="color">配色</param>
    private void UpdateButtonState(bool isEnabled, string message, Color color)
    {
        executeButton.interactable = isEnabled;
        check_text.text = message;
        check_text.color = color;
    }

    /// <summary>
    /// チェックボタンを押したときの処理
    /// </summary>
    void CommandCheckButtonOnClick()
    {
        ResetRobotCommandExecute();                 // ロボットのコマンドをリセットする

        proctext = CheckTextCommand(inputField);    // 行ごとに文字列を取得する
        complete = InputCommand(proctext);          // コマンドが正確か否かを判断する

        // コマンドが正確な場合
        if (complete)
        {
            robotCmdExecute.ProcText = proctext;    // ロボットのコマンドを設定する
            executeButton.interactable = true;      // 実行ボタンを押せる状態にする
        }
    }

    /// <summary>
    /// チェックボタンをリセットする
    /// </summary>
    public void Reset_Buttons()
    {
        UpdateButtonState(false, "チェック", Color.white);
    }

    /// <summary>
    /// コマンドを実行する
    /// </summary>
    void ExecuteButtonOnClick()
    {
        // コマンドを実行する
        robotCmdExecute.StartCoroutine_CommandToExecution();
        RobotStatusPanelManager.instance.BackButtonOnClick();
    }

    /// <summary>
    /// ロボットのコマンドをリセットする
    /// </summary>
    void ResetRobotCommandExecute()
    {
        // InputFieldとは別にstring[]で行ごとに管理している変数をnullにする
        robotCmdExecute.ProcText = null;
        robotCmdExecute.StateEndFlag = false;
    }

    /// <summary>
    /// 一行ずつコマンドを確認する
    /// </summary>
    string[] CheckTextCommand(TMP_InputField _input)
    {
        // 改行ごとに文字列を取得する
        string[] lines = _input.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

        // 行ごとに文字列を取得する
        for (int i = 0; i < lines.Length; i++)
        {
            Debug.Log($"Line {i + 1}: {lines[i]}");
        }

        // チュートリアルの条件を確認する
        CheckTutorialCondition(lines);
        return lines;
    }

    /// <summary>
    /// チュートリアルの条件を確認する
    /// </summary>
    /// <param name="lines">コマンド</param>
    private void CheckTutorialCondition(string[] lines)
    {
        if (lines.Length > 0 && lines[0] == "MoveTo(location);")
        {
            TutorialController.insrance.TutorialCheck(0, 2);
        }
    }
}


