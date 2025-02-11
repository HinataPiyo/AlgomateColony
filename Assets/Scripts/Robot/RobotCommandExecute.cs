using System.Collections;
using UnityEngine;

// ロボット自身につけるスクリプト
// コマンド実行処理
public class RobotCommandExecute : MonoBehaviour
{
    [SerializeField] CommandSO commandSO;
    RobotController robotCont;
    string[] proctext;      // 処理内容を格納

    // 個々のロボットで処理を実行するために自身の処理内容を格納する
    public string[] ProcText { get{ return proctext; } set{ proctext = value; } }

    bool stateEndFlag;          // ステートが終了したらこのフラグが " true " になある
    public bool StateEndFlag { get{ return stateEndFlag; } set{ stateEndFlag = value; } }

    const string looping = "Loop();";

    private void Start() {
        robotCont = GetComponent<RobotController>();
    }

    private void Update() {
        Debug.Log("flag : " + stateEndFlag);
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
        LogController.instance.SetLog(robotCont.GetBaseStatus(), "コマンドを実行します");
        int ii = 0;
        while (true)
        {
            stateEndFlag = false;

            P(proctext[ii]);
            yield return new WaitUntil(() => stateEndFlag);

            // 最後の行であればループするか終了
            if (ii == proctext.Length - 1)
            {
                if (proctext[ii] == looping)
                {
                    ii = 0; // ループ
                }
                else
                {
                    // 何もしない状態に移行する
                    robotCont.ChangeState(RobotController.State.DoNon);
                    yield break; // 終了
                }
            }
            else
            {
                ii++;
            }
        }
    }


    void P(string _pText)
    {
        string specifytext = CommandSO.MatchParenthesesCommand(_pText);

        if (_pText == CommandSO.moveTo + $"({specifytext});")
        {
            robotCont.ObjectName = specifytext;
            robotCont.ChangeState(RobotController.State.Search);
        }

        if (_pText == CommandSO.gatherTo + $"({specifytext});")
        {
            robotCont.ObjectName = specifytext;
            robotCont.ChangeState(RobotController.State.GatherResource);
        }

        if (_pText == CommandSO.depositTo + $"({specifytext});")
        {
            // DepositToコマンドの処理
            robotCont.DepsiteName = Deposite_NameAndAmount(specifytext);
            robotCont.ChangeState(RobotController.State.Deposit);
        }
    }


    string[] Deposite_NameAndAmount(string _text)
    {
        string[] splitResult = _text.Split(',');

        // 各要素の前後の空白を削除
        for (int i = 0; i < splitResult.Length; i++)
        {
            splitResult[i] = splitResult[i].Trim();
        }

        return splitResult;
    }
}