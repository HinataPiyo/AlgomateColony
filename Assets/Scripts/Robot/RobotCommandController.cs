using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RobotCommandController : MonoBehaviour
{
    CommandDictionary commandDic;
    [SerializeField] TMP_InputField inputField;

    [SerializeField] bool applyflag;        // テスト
    public string[] proctext;


    private void Start()
    {
        commandDic = new CommandDictionary();
    }

    private void Update() {
        if(applyflag) Run(); applyflag = false;
    }

    /// <summary>
    /// コマンドを実行する
    /// </summary>
    void Run()
    {
        proctext = CheckTextCommand(inputField);        // 行ごとに文字列を取得する
        InputCommand(proctext);                         // コマンドが正確か否かを判断し、実行する
    }

    /// <summary>
    /// コマンドをプログラミング言語に変換
    /// </summary>
    /// <param name="sp"></param>
    public void InputCommand(string[] proctext)
    {
        if(proctext != null)
        {
            // ProcInputFieldを一行ずつ確認する
            for(int ii = 0; ii < proctext.Length; ii++)
            {
                string _proctext = commandDic.CheckCommand(proctext[ii]);
                if(_proctext == null)
                {
                    Debug.Log("()内のコマンドが設定されていません");
                }
                // コマンドがしっかりと動くものであれば
                if(_proctext != null)
                {
                    // ()の中を確認する
                    string specifytext = commandDic.MatchCommand(_proctext);
                    if(specifytext == null)
                    {
                        Debug.Log("()内のコマンドが間違えています。");
                    }
                    // コマンドを実行する
                    commandDic.CommandToExecution(proctext[ii],specifytext);
                    Debug.Log("出力します。");
                }
                else
                {
                    Debug.Log("出力できません。コマンドが間違えています。");
                }
            }
        }
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
    public bool procend_flag;
    // メインコマンド
    const string move_to = "move_to";
    const string gather = "gather";

    // 例 : Locationと入力されたらLocationと返す
    Dictionary<string, string> moveName = new Dictionary<string, string>()
    {
        { "Location", "Location"},
        { "stone", "stone"}, { "tree", "tree"}
    };

    Dictionary<string, string> gatherName = new Dictionary<string, string>()
    {
        { "stone", "stone"}, { "tree", "tree"}
    };

    public string materialName;

    /// <summary>
    /// 括弧の中の文字列を取得する
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public string MatchCommand(string text)
    {
        Match match = Regex.Match(text, @"\((.*?)\)");

        if (match.Success)
        {
            string result = match.Groups[1].Value;
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
    /// コマンドがちゃんと動くものか確かめる
    /// </summary>
    /// <param name="_proctext"></param>
    /// <returns></returns>
    public string CheckCommand(string _proctext) 
    {
        // ()内のコマンドを取得する　
        foreach(var mn in moveName.Keys)        // 移動処理
        {
            if(_proctext == move_to + $"({mn});") { return _proctext; }
        }

        foreach(var gn in gatherName.Keys)      // 収集処理
        {
            if(_proctext == gather + $"({gn});") { return _proctext; }
        }

        return null;
    }


    /// <summary>
    /// 最終フェーズのコマンドの実行を行う
    /// </summary>
    /// <param name="proctext"></param>
    /// <param name="specifytext"></param>
    public void CommandToExecution(string proctext, string specifytext)
    {
        // move_toのコマンドを使用されていた場合
        if(proctext == move_to + $"({specifytext});")
        {
            switch(specifytext)
            {
                case "Location":
                    Debug.Log($"{specifytext}に移動します。");
                    break;
            }
        }

        // gatherのコマンドを使用されていた場合
        if(proctext == gather + $"({specifytext});")
        {
            switch(specifytext)
            {
                case "stone":
                    Debug.Log($"{specifytext}に移動します。");
                    break;
                case "tree":
                    Debug.Log($"{specifytext}に移動します。");
                    break;
            }
        }
    }
}