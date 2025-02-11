using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RobotCommandController : MonoBehaviour
{
    public static RobotCommandController instance;
    [SerializeField] CommandSO commandSO;
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
    public RobotCommandExecute Set_RobotCommandExecute { set{robotCmdExecute = value;} }
    public TMP_InputField InputCommandField { get{return inputField;} }

    private void Start()
    {
        executeButton.onClick.AddListener(OnClick_CommandExecute);
        checkButton.onClick.AddListener(OnClick_CommandCheck);

        executeButton.interactable = false;     // 実行ボタンを最初は押せない状態にする
    }

    public void Reset_Buttons()
    {
        executeButton.interactable = false;
        check_text.text = "チェック";
        check_text.color = Color.white;
    }

    /// <summary>
    /// コマンドがしっかりと動作するか確認する
    /// </summary>
    /// <param name="proctext"></param>
    public bool InputCommand(string[] proctext)
    {
        if (string.IsNullOrEmpty(inputField.text))
        {
            UpdateButtonState(false, "失敗", Color.red);
            return false;
        }

        int validCommandCount = 0;
        foreach (var command in proctext)
        {
            string commandText = commandSO.CheckCommand(command);
            if (commandText != null)
            {
                validCommandCount++;
            }
            else
            {
                Debug.Log($"[ {command} ]コマンドが間違えています。");
            }
        }

        if (validCommandCount == proctext.Length)
        {
            UpdateButtonState(true, "成功", Color.green);
            return true;
        }

        UpdateButtonState(false, "失敗", Color.red);
        return false;
    }

    private void UpdateButtonState(bool isEnabled, string message, Color color)
    {
        executeButton.interactable = isEnabled;
        check_text.text = message;
        check_text.color = color;
    }


    void OnClick_CommandCheck()
    {
        robotCmdExecute.ProcText = null;
        robotCmdExecute.StateEndFlag  = false;

        proctext = CheckTextCommand(inputField);        // 行ごとに文字列を取得する
        complete = InputCommand(proctext);              // コマンドが正確か否かを判断する

        if(complete == true)
        {
            robotCmdExecute.ProcText = proctext;
            executeButton.interactable = true;
        }
    }

    void OnClick_CommandExecute()
    {
        // コマンドを実行する
        robotCmdExecute.StartCoroutine_CommandToExecution();
        
        Debug.Log("出力します。");
    }

    /// <summary>
    /// 一行ずつコマンドを確認する
    /// </summary>
    /// <param name="_input"></param>
    /// <returns></returns>
    string[] CheckTextCommand(TMP_InputField _input)
    {
        string _proctext = _input.text;

        // 改行ごとにテキストを分割
        string[] lines = _proctext.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

        // 各行をログに表示
        for (int i = 0; i < lines.Length; i++)
        {
            Debug.Log($"Line {i + 1}: {lines[i]}");
        }

        return lines;
    }
}


