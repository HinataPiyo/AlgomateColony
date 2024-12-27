using System.Collections.Generic;
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
    const float gatherRtMin = 10f, gatherRtMax = 1f;
    
    [Header("ベースステータス")]
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
    public Slot[] slots = new Slot [MAX_SLOT];
    const int MAX_SLOT = 5;
    public enum SLOT_STACK
    {
        STACK_TRUE,
        STACK_MAX,
        ALL_STACK_MAX
    }

    [Header("装備")]
    public EQUIPMENT_NAME select_equipment_name;
    public EquipmentSO.EQUIPMENT_VALUE[] equipment_value = new EquipmentSO.EQUIPMENT_VALUE[MAX_EQUIPMENT_SLOTS];
    const int MAX_EQUIPMENT_SLOTS = 3;
    [Header("装備できるスロット数")]
    public UNLOCK_EQUIPMENT_SLOT unlock_equipment_slot;

    
    public void GenerateEquipmentSlots()
    {
        for(int ii = 0; ii < MAX_EQUIPMENT_SLOTS; ii++)
        {
            equipment_value[ii] = new EquipmentSO.EQUIPMENT_VALUE();
        }
    }

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
    public SLOT_STACK CheckStackMax()
    {
        for(int ii = 0; ii < slots.Length; ii++)
        {
            // スロットの中にアイテムが存在していたら
            if(slots[ii].mateSO != null)
            {
                // スタック数がマックスだった場合
                if(slots[ii].CheckStackMax() == true)
                {
                    return SLOT_STACK.STACK_MAX;
                }
                else if(slots[ii].CheckStackMax() == false)    // スタックがまだ可能だった場合
                {
                    return SLOT_STACK.STACK_TRUE;
                }
            }
            else        // スロットの中にアイテムが存在していなかったら
            {
                return SLOT_STACK.STACK_TRUE;
            }
        }

        // スタック数がマックスだった場合にSTACK_MAXを返す
        return SLOT_STACK.STACK_MAX;
    }

    public SLOT_STACK CheckAllStackMax()
    {
        int value = 0;
        for(int ii = 0; ii < slots.Length; ii++)
        {
            // スタック数がMaxなら
            if(slots[ii].CheckStackMax() == true)
            {
                value++;
            }
        }

        if(value == slots.Length)
        {
            return SLOT_STACK.ALL_STACK_MAX;
        }

        return SLOT_STACK.STACK_TRUE;

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
        // 装備によって強化できるものは最低値にする
        // ロボットなのでランダム性はなるべく避ける
        robotName = "";
        moveSpeed = moveMin;
        maxEnergy = energyMin;
        recharge_MAX = rechargeMin;
        gatherSterngth = gatherStrMin;
        gatherRate = gatherRtMin;
    }

    /// <summary>
    /// 総合ステータスを求める関数
    /// </summary>
    public void TotalStatus()
    {
        List<Score> _scores = new List<Score>();

        // 各ステータス値を配列にまとめる
        float[] statuses = { moveSpeed, (float)recharge_MAX, maxEnergy, gatherSterngth};

        for (int ii = 0; ii < STATUS_MAX -1; ii++)
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
        float sumtotal = moveMax + energyMax + gatherStrMax + rechargeMax;
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
    

    
    /// <summary>
    /// 収集速度ステータスの上昇
    /// </summary>
    /// <param name="_equipmentSO"></param>
    public void UpdateGatherRate(EquipmentSO _equipmentSO)
    {
        gatherRate = gatherRtMin - _equipmentSO.GetEquipmentTotalValue(select_equipment_name);
    }
}


