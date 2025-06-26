using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public static class CommandHandler
{
    public static readonly string Move = "Move";
    public static readonly string Gather = "Gather";
    public static readonly string Deposit = "Deposit";

    // コマンド名と検証ロジックのマッピング
    private static readonly Dictionary<string, Func<string, bool>> CommandValidators = new Dictionary<string, Func<string, bool>>()
    {
        { Move,    arg => CommandLoader.instance.MoveCommands.Exists(e => e.command == arg) },
        { Gather,  arg => CommandLoader.instance.GatherCommands.Exists(e => e.command == arg) },
        { Deposit, DepositFunc },
        // 新しいコマンドはここに追加
    };

    /// <summary>
    /// 括弧の中の文字列を取得する
    /// </summary>
    public static string MatchParenthesesCommand(string _name)
    {
        Match match = Regex.Match(_name, @"\(([^)]+)\)");
        if (match.Success)
            return match.Groups[1].Value.Trim();
        return null;
    }

    /// <summary>
    /// コマンドが有効かどうか判定する（拡張性重視版）
    /// </summary>
    public static string CheckCommand(string _proctext)
    {
        foreach (var pair in CommandValidators)
        {
            if (_proctext.StartsWith(pair.Key))
            {
                string arg = MatchParenthesesCommand(_proctext);
                if (arg == null) return null;
                if (pair.Value(arg))
                    return _proctext;
                return null;
            }
        }
        return null; // 未対応コマンド
    }

    /// <summary>
    /// Depositのような長い処理は分ける
    /// </summary>
    static bool DepositFunc(string arg)
    {
        var args = arg.Split(',');
        if (args.Length != 2) return false;
        string mate = args[0].Trim();
        string quantityStr = args[1].Trim();
        bool isValidMate = CommandLoader.instance.DepositCommands.Exists(e => e.command == mate);
        bool isValidQuantity = int.TryParse(quantityStr, out _);
        return isValidMate && isValidQuantity;
    }
}