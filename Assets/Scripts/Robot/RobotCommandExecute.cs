using System.Collections;
using UnityEngine;

// ロボット自身につけるスクリプト
// コマンド実行処理
public class RobotCommandExecute : MonoBehaviour
{
    CommandDictionary cmdDic;
    RobotController robotCont;
    string[] proctext;      // 処理内容を格納

    public string[] Set_ProcText { set{ proctext = value; } }

    bool stateEndFlag;
    public bool StateEndFlag { set{ stateEndFlag = value; } }

    private void Start() {
        robotCont = GetComponent<RobotController>();
        cmdDic = new CommandDictionary();
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
        while(true)
        {
            for(int ii = 0; ii < proctext.Length; ii++)
            {
                P(proctext[ii]);

                yield return new WaitUntil(() => stateEndFlag);

                // 最後の行の処理を行ったら
                if(proctext[ii] == proctext[proctext.Length - 1])
                {
                    // コルーチンを抜ける
                    yield break;
                }
            }
            yield return null;
        }
        
    }

    void P(string _pText)
    {
        string specifytext = CommandDictionary.MatchParenthesesCommand(_pText);

        // move_toのコマンドを使用されていた場合
        if(_pText == CommandDictionary.moveTo + $"({specifytext});")
        {
            robotCont.ObjectName = specifytext;      // ()内に書かれた名前を設定する
            Debug.Log($"{specifytext}に移動します。");
            robotCont.ChangeState(RobotController.State.Search);
        }

        // gatherのコマンドを使用されていた場合
        if(_pText == CommandDictionary.gatherTo + $"({specifytext});")
        {
            switch(specifytext)
            {
                case "rock":
                    Debug.Log($"{specifytext}を取集します。");
                    break;
                case "tree":
                    Debug.Log($"{specifytext}を取集します。");
                    break;
            }
        }
    }
}