using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RobotCommandController : MonoBehaviour
{
    public static RobotCommandController instance;
    CommandDictionary commandDic;
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

    private void Start()
    {
        commandDic = new CommandDictionary();
        executeButton.onClick.AddListener(OnClick_CommandExecute);
        checkButton.onClick.AddListener(OnClick_CommandCheck);

        executeButton.interactable = false;     // 実行ボタンを最初は押せない状態にする
    }

    /// <summary>
    /// コマンドがしっかりと動作するか確認する
    /// </summary>
    /// <param name="proctext"></param>
    public bool InputCommand(string[] proctext)
    {
        int counter = 0;
        if(proctext != null)
        {
            // ProcInputFieldを一行ずつ確認する
            for(int ii = 0; ii < proctext.Length; ii++)
            {
                // string _proctext = commandDic.CheckCommand(proctext[ii]);

                // ()の中を確認する
                if(proctext[ii] != null)
                {
                    string text = CommandDictionary.MatchParenthesesCommand(proctext[ii]);

                    if(text != null)
                    {
                        counter++;
                    }
                    else
                    {
                        Debug.Log($"({proctext[ii]})コマンドが間違えています。");
                    }
                }
                else
                {
                    Debug.Log($"({proctext[ii]})コマンドが間違えています。");
                }
            }

            if(counter == proctext.Length)
            {
                return true;
            }
        }

        return false;
    }

    void OnClick_CommandCheck()
    {
        robotCmdExecute.Set_ProcText = null;
        robotCmdExecute.StateEndFlag  = false;

        proctext = CheckTextCommand(inputField);        // 行ごとに文字列を取得する
        complete = InputCommand(proctext);              // コマンドが正確か否かを判断する

        if(complete == true)
        {
            robotCmdExecute.Set_ProcText = proctext;
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

public class CommandDictionary
{
    // メインコマンド
    public const string moveTo = "MoveTo";
    public const string gatherTo = "GatherTo";

    // 例 : Locationと入力されたらLocationと返す
    Dictionary<string, string> moveName = new Dictionary<string, string>()
    {
        { "location", "location"},
        { "rock", "rock"}, { "tree", "tree"}
    };

    Dictionary<string, string> gatherName = new Dictionary<string, string>()
    {
        { "rock", "rock"}, { "tree", "tree"}
    };

    /// <summary>
    /// 括弧の中の文字列を取得する
    /// </summary>
    /// <param name="_name"></param>
    /// <returns></returns>
    public static string MatchParenthesesCommand(string _name)
    {
        Match match = Regex.Match(_name, @"\((.*?)\)");

        if (match.Success)
        {
            string result = match.Groups[1].Value;
            // Regex.Split
            Debug.Log(result);
            return result;
        }
        else
        {
            Debug.LogError("括弧の中身を見つけられませんでした。");
        }

        return null;
    }

    /// <summary>
    /// 括弧内のコマンドをセパレートする処理
    /// </summary>
    /// <param name="_text"></param>
    /// <returns></returns>
    public static string MatchSeparateCommand(string _text)
    {

        return null;
    }

    /// <summary>
    /// コマンドがちゃんと動くものか確かめる
    /// Dictionaryで設定された単語が誤字脱字なく書かれているか確認する
    /// </summary>
    /// <param name="_proctext"></param>
    /// <returns></returns>
    public string CheckCommand(string _proctext) 
    {
        // ()内のコマンドを取得する　
        foreach(var mn in moveName.Keys)        // 移動処理
        {
            if(_proctext == moveTo + $"({mn});") { return _proctext; }
        }

        foreach(var gn in gatherName.Keys)      // 収集処理
        {
            if(_proctext == gatherTo + $"({gn});") { return _proctext; }
        }

        return null;
    }
}