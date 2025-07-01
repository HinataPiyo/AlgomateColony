using System.Collections;
using UnityEngine;

/// <summary>
/// ロボット自身につけるスクリプト
/// コマンド実行処理
/// </summary>
public class RobotCommandExecute : MonoBehaviour
{
    RobotController robotCont;
    string[] proctext;      // 処理内容を格納

    // 個々のロボットで処理を実行するために自身の処理内容を格納する
    public string[] ProcText { get { return proctext; } set { proctext = value; } }

    bool stateEndFlag;          // ステートが終了したらこのフラグが " true " になある
    public bool StateEndFlag { get { return stateEndFlag; } set { stateEndFlag = value; } }

    const string looping = "Loop();";

    /// <summary>
    /// ロボットを管理するクラスを取得する
    /// </summary>
    public void SetRobotController(RobotController ctrl)
    {
        robotCont = ctrl;
    }

    // コルーチン実行関数
    public void StartCoroutine_CommandToExecution()
    {
        StartCoroutine(CommandToExecution());
    }

    /// <summary>
    /// 最終フェーズのコマンドの実行を行う
    /// </summary>
    /// <param name="proctext"></param>
    /// <param name="specifytext"></param>
    IEnumerator CommandToExecution()
    {
        if (robotCont == null) yield break;
        if (proctext == null || proctext.Length == 0) yield break;

        LogController.instance.SetLog(robotCont.GetBaseStatus(), "コマンドを実行します");
        int commandIndex = 0;

        while (true)
        {
            stateEndFlag = false;

            if (proctext == null || proctext.Length == 0) yield break;

            ExecuteCommand(proctext[commandIndex]);
            yield return new WaitUntil(() => stateEndFlag);

            if (IsLastCommand(commandIndex))
            {
                if (proctext[commandIndex] == looping)
                {
                    commandIndex = 0; // ループ
                }
                else
                {
                    robotCont.ChangeState(RobotController.State.DoNon); // 終了
                    yield break;
                }
            }
            else
            {
                commandIndex++;
            }
        }
    }

    private bool IsLastCommand(int index)
    {
        if (proctext == null || proctext.Length == 0) return false;
        return index == proctext.Length - 1;
    }

    /// <summary>
    /// コマンドを実行する
    /// </summary>
    /// <param name="commandText"></param>
    void ExecuteCommand(string commandText)
    {
        string specifytext = CommandHandler.MatchParenthesesCommand(commandText);

        if (commandText == CommandHandler.Move + $"({specifytext});")
        {
            robotCont.ObjectName = specifytext;
            robotCont.ChangeState(RobotController.State.Search);
        }
        else if (commandText == CommandHandler.Gather + $"({specifytext});")
        {
            robotCont.ObjectName = specifytext;
            robotCont.ChangeState(RobotController.State.GatherResource);
        }
        else if (commandText == CommandHandler.Deposit + $"({specifytext});")
        {
            robotCont.DepsiteName = ParseDepositParameters(specifytext);
            robotCont.ChangeState(RobotController.State.Deposit);
        }
    }

    /// <summary>
    /// デポジットのパラメータを解析する
    /// </summary>
    string[] ParseDepositParameters(string parameters)
    {
        string[] splitResult = parameters.Split(',');

        for (int i = 0; i < splitResult.Length; i++)
        {
            splitResult[i] = splitResult[i].Trim();
        }

        return splitResult;
    }
}