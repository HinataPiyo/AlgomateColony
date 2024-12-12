using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class RobotCommandController : MonoBehaviour
{
    CommandDictionary commandDic;
    CONTROL_SYNTAX current_controlsyntax;
    [SerializeField] Transform inputParent;
    [SerializeField] TMP_InputField[] inputField;
    bool procField_flag;

    [SerializeField] bool applyflag;
    public struct SummaryProc
    {
        public TMP_InputField _input;
        public CONTROL_SYNTAX thisControlSyntax;
        public string[] proctext;
    }

    public enum CONTROL_SYNTAX
    {
        ERR = -2,   // 入力されていない
        PROC = -1,
        NONE,
        IF,
        ELSE,
        ELSE_IF,
        FOR,
        WHILE,
        SWICH,
    }

    private void Start()
    {
        inputField = inputParent.GetComponentsInChildren<TMP_InputField>();
        commandDic = new CommandDictionary();
    }

    private void Update() {
        if(applyflag) Running(); applyflag = false;
    }

    void Running()
    {
        
    }

    /// <summary>
    /// コマンドを実行する
    /// </summary>
    void Run()
    {
        foreach(var input in inputField)
        {
            SummaryProc sp;

            sp._input = input;
            sp.thisControlSyntax = CheckControlSyntaxText(sp._input);       // 入力された制御構文を確認する
            if(sp.thisControlSyntax != CONTROL_SYNTAX.PROC)
            {
                // 制御構文を設定する
                current_controlsyntax = CheckControlSyntaxText(sp._input);
                sp.proctext = null;

                // 制御構文のInputField内が空だったり間違った入力をしていたら処理を行わない
                Debug.Log("制御構文が設定されていません。");
                if(current_controlsyntax == CONTROL_SYNTAX.ERR) return;
            }
            else
            {   // 処理を行うInputFieldだったら
                sp.proctext = CheckTextCommand(sp._input);
            }

            InputCommand(sp);
        }
    }

    /// <summary>
    /// コマンドをプログラミング言語に変換
    /// </summary>
    /// <param name="sp"></param>
    public void InputCommand(SummaryProc sp)
    {
        if(sp.proctext != null)
        {
            // ProcInputFieldを一行ずつ確認する
            for(int ii = 0; ii < sp.proctext.Length; ii++)
            {
                // もし制御構文がNONEだった場合
                if(current_controlsyntax == CONTROL_SYNTAX.NONE)
                {
                    // 一行目まで実行できる
                    if(ii == 0)
                    {
                        // コマンドがしっかりと動くものであれば
                        if(commandDic.CheckCommand(sp.proctext[ii]) == true)
                        {
                            // ()の中を確認する
                            string specifytext = commandDic.MatchCommand(sp.proctext[ii]);
                            // コマンドを実行する
                            commandDic.CommandToExecution(sp.proctext[ii],specifytext);
                            Debug.Log("出力します。");
                        }
                        else
                        {
                            Debug.Log("出力できません。コマンドが間違えています。");
                        }
                    }
                    else
                    {
                        Debug.Log("出力できません。");
                    }
                }
            }
        }

        Debug.Log("sp._input : "  + sp._input );
        Debug.Log("sp.thisControlSyntax : " + sp.thisControlSyntax);
    }

    /// <summary>
    /// 入力された制御構文を確認する
    /// </summary>
    /// <param name="_input"></param>
    /// <returns></returns>
    CONTROL_SYNTAX CheckControlSyntaxText(TMP_InputField _input)
    {
        CONTROL_SYNTAX cs = CONTROL_SYNTAX.ERR;
        if(_input.CompareTag("Proc")) cs = CONTROL_SYNTAX.PROC;
        else if(_input.CompareTag("ControlSyntax"))
        {
            switch (_input.text)
            {
                case "none:":
                    cs = CONTROL_SYNTAX.NONE;
                    break;
                case "if:":
                    cs = CONTROL_SYNTAX.IF;
                    break;
                case "else:":
                    cs = CONTROL_SYNTAX.ELSE;
                    break;
                case "else if:":
                    cs = CONTROL_SYNTAX.ELSE_IF;
                    break;
                case "for:":
                    cs = CONTROL_SYNTAX.FOR;
                    break;
                case "while:":
                    cs = CONTROL_SYNTAX.WHILE;
                    break;
                case "swich:":
                    cs = CONTROL_SYNTAX.SWICH;
                    break;
            }
        }
        return cs;
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

        return lines;
    }

    
}

public class CommandDictionary
{
    // 処理終了コマンド
    public string proc_end = "proc_end;"; 
    // メインコマンド
    const string move_to = "move_to";
    const string gather = "gather";

    // 例 : Locationと入力されたらLocationと返す
    public Dictionary<string, string> moveName = new Dictionary<string, string>()
    {
        { "Location", "Location"}, { "stone", "stone"}, { "tree", "tree"}
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
    public bool CheckCommand(string _proctext) 
    {
        foreach(var mn in moveName.Keys)
        {
            if(_proctext == move_to + $"({mn});") { return true;}
        }
        if(_proctext == move_to + materialName) { return true; }
        if(_proctext == gather + materialName) { return true; }

        return false;
    }

    /// <summary>
    /// 最終フェーズのコマンドの実行を行う
    /// </summary>
    /// <param name="proctext"></param>
    /// <param name="specifytext"></param>
    public void CommandToExecution(string proctext, string specifytext)
    {
        foreach(var mn in moveName.Keys)
        {
            // move_toのコマンドを使用されていた場合
            if(proctext == move_to + $"({mn});")
            {
                switch(specifytext)
                {
                    case "Location":
                        Debug.Log("最終フェーズのコマンドの実行を行います。");
                        break;
                }
            }
        }
        
        if(proctext == move_to + materialName)
        {
            switch(specifytext)
            {
                case "Location":
                    Debug.Log("最終フェーズのコマンドの実行を行います。");
                    break;
            }
        }
    }


    // 条件コマンド
}