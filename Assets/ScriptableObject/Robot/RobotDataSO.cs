using System.Collections.Generic;
using System.Linq;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRobotData", menuName = "RobotSO/Robot Data")]
public class RobotDataSO : ScriptableObject
{
    
}


[System.Serializable]
public class BaseStatus
{
    public struct Score { public float total; }
    const float moveMin = 1f, moveMax = 8f; 
    const int rechargeMin = 10, rechargeMax = 100;
    const float energyMin = 50f, energyMax = 100f;
    const float gatherStrMin = 1f, gatherStrMax = 10f;
    const float gatherRtMin = 3f, gatherRtMax = 10f;
    public string _runk;
    public float totalScore;
    public string robotName;        // ロボットの名前
    public float moveSpeed;         // 移動速度
    public int recharge_MAX;            // 充電回数（最大値）
    public float maxEnergy;         // 最大エネルギー
    public float gatherSterngth;    // 収集力
    public float gatherRate;        // 資源収集速度
    const int STATUS_MAX = 5;

    // ロボット各々のインベントリ
    [Header("インベントリ")]
    const int MAX_SLOT = 5;
    public Slot[] slots = new Slot [MAX_SLOT];

    /// <summary>
    /// スロットを生成する
    /// </summary>
    public void GeneratSlots()
    {
        for(int ii = slots.Length - 1; ii >= 0; ii--)
        {
            slots[ii] = new Slot();
        }
    }

    /// <summary>
    /// true : 全てスタックMax, false : どれかがスタック可能
    /// </summary>
    /// <returns></returns>
    public bool CheckAllStackMax()
    {
        for(int ii = slots.Length - 1; ii >= 0; ii--)
        {
            // スロットの中にアイテムが存在していたら
            if(slots[ii].mateSO != null)
            {
                // スタック数がマックスだった場合
                if(slots[ii].CheckStackMax() == true)
                {
                    continue;
                }
                else    // スタックがまだ可能だった場合
                {
                    return false;
                }
            }
            else    // 0番目以外のスロットがnullだった場合,スタック可能なのにAllMaxという判定になってしまう
            {
                return false;
            }
        }

        // スタック数がマックスだった場合にtrueを返す
        return true;
    }


    /// <summary>
    /// インベントリの個々のスロットを生成
    /// </summary>
    [System.Serializable]
    public class Slot
    {
        public MaterialSO mateSO = null;            // スロットの中に入っいるアイテム 
        public int ITEM_STACK_MAX = 100;            // アイテムのスタック数
        public int itemStackAmount = 0;             // 資源のスタック数

        /// <summary>
        /// true : スタックMax, false : スタック可能
        /// </summary>
        /// <returns></returns>
        public bool CheckStackMax()
        {
            if(itemStackAmount >= ITEM_STACK_MAX) return true;
            return false;
        }
    }

    /// <summary>
    /// ランダムにステータスを決める
    /// </summary>
    public void RandomStatusProc()
    {
        // 強化余地を与えるため、0.8倍してあげてナーフする
        robotName = "";
        moveSpeed = Mathf.Round(Random.Range(moveMin, moveMax) * 0.8f * 10) / 10;
        recharge_MAX = Random.Range(rechargeMin, rechargeMax);
        maxEnergy = Mathf.Round(Random.Range(energyMin, energyMax) * 0.8f * 10) / 10;
        gatherSterngth = Mathf.Round(Random.Range(gatherStrMin, gatherStrMax) * 0.8f * 10) / 10;
        gatherRate = Mathf.Round(Random.Range(gatherRtMin, gatherRtMax) * 0.8f * 10) / 10;
    }

    /// <summary>
    /// 総合ステータスを求める関数
    /// </summary>
    public void TotalStatus()
    {
        List<Score> _scores = new List<Score>();

        // 各ステータス値を配列にまとめる
        float[] statuses = { moveSpeed, (float)recharge_MAX, maxEnergy, gatherSterngth, gatherRate };

        for (int ii = 0; ii < STATUS_MAX; ii++)
        {
            float _status = statuses[ii];

            // スコア計算を簡略化
            _scores.Add(CalculateScore(_status));
        }

        // 合計スコアを計算し、ランクを決定
        totalScore = DecideRunk(_scores);
    }

    // スコア計算用関数
    private Score CalculateScore(float _status)
    {
        Score score = new Score();
        score.total = _status;      // ステータス値をそのまま代入
        return score;
    }

    // ランク決定用関数
    private float DecideRunk(List<Score> _scores)
    {
        // ステータス全体の総和を求める
        float sumtotal = moveMax + energyMax + gatherRtMax + gatherStrMax + rechargeMax;
        float total = 0f;

        foreach (var score in _scores)
        {
            // 新しく生成されたステータスを足していく
            total += score.total;
        }

        // 比率を求める
        float ratio = total / sumtotal;

        if (ratio > 0.9f) _runk = "- S -";
        else if (ratio <= 0.9 && ratio > 0.8f) _runk = "- A -";
        else if (ratio <= 0.8 && ratio > 0.7f) _runk = "- B -";
        else if (ratio <= 0.7 && ratio > 0.6f) _runk = "- C -";
        else if (ratio <= 0.6 && ratio > 0.5f) _runk = "- D -";
        else if(ratio <= 0.5) _runk = "- E -";

        // 結果を記録
        return Mathf.Round(total * 10f) / 10f;
    }

}