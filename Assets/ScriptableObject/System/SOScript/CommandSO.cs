using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;


[CreateAssetMenu(menuName = "CommandList", fileName = "CommandSO")]
public class CommandSO : ScriptableObject
{
    [Header("コマンド一覧で表示するテキスト")] public ComanndsDetail[] cmdsDetail;
    // メインコマンド
    public const string moveTo = "MoveTo";
    public const string gatherTo = "GatherTo";
    public const string depositTo = "DepositTo";

    // 例 : Locationと入力されたらLocationと返す
    Dictionary<string, string> moveName = new Dictionary<string, string>()
    {
        { "location", "location"}, { "warehouse", "warehouse" },
        { "rock", "rock"}, { "tree", "tree"}, { "ironore", "ironore" }
    };

    Dictionary<string, string> gatherName = new Dictionary<string, string>()
    {
        { "rock", "rock"}, { "tree", "tree"}, { "ironore", "ironore" }
    };

    /// <summary>
    /// 括弧の中の文字列を取得する
    /// </summary>
    /// <param name="_name"></param>
    /// <returns></returns>
    public static string MatchParenthesesCommand(string _name)
    {
        // 括弧内の文字列を取得し、; や空白も削除する
        Match match = Regex.Match(_name, @"\(([^)]+)\)");

        if (match.Success)
        {
            string result = match.Groups[1].Value.Trim();  // 括弧内の空白を削除
            Debug.Log("括弧内 : " + result);
            return result;
        }
        else
        {
            Debug.Log("括弧の中身を見つけられませんでした。");
        }

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
        // MoveToコマンドのチェック
        if (_proctext.StartsWith(moveTo))
        {
            string _move = MatchParenthesesCommand(_proctext);
            if (_move != null && moveName.ContainsKey(_move))
            {
                return _proctext;  // 有効なMoveToコマンド
            }
        }

        // GatherToコマンドのチェック
        if (_proctext.StartsWith(gatherTo))
        {
            string _gather = MatchParenthesesCommand(_proctext);
            if (_gather != null && gatherName.ContainsKey(_gather))
            {
                return _proctext;  // 有効なGatherToコマンド
            }
        }

        // DepositToコマンドのチェック（個数も含めて）
        if (_proctext.StartsWith(depositTo))
        {
            string depositCommand = MatchParenthesesCommand(_proctext);
            if (depositCommand != null)
            {
                // 正規表現で個数を確認する
                string[] args = depositCommand.Split(',');
                if (args.Length == 2)
                {
                    string mate = args[0].Trim();
                    string quantityStr = args[1].Trim();

                    if (gatherName.ContainsKey(mate) && int.TryParse(quantityStr, out int quantity))
                    {
                        return _proctext;  // 有効なDepositToコマンド（オブジェクト名, 個数）
                    }
                }
            }
        }

        return null;  // 無効なコマンド
    }
}