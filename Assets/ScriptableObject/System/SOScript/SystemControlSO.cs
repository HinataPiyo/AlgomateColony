using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SystemControlSO", menuName = "CreatScriptableObject/SystemControlSO")]
public class SystemControlSO : ScriptableObject 
{
    public string playerName;
    [SerializeField] NextLevelUnlockedSO nluSO;
    [SerializeField] int LocationLevel;                 // 拠点のレベルを設定
    [SerializeField] float upgread_chargingTime;        // ロボットの充電を早くする為のUpgread要素
    [SerializeField] POTENTIAL potential_class;         // 潜在能力のクラス

    public List<BaseStatus> robot_list = new List<BaseStatus>();

    public int GetLocationLevel() { return LocationLevel; }
    public float GetBatteryChargingTime() { return upgread_chargingTime; }
    public NextLevelUnlockedSO GetNextLevelUnlockedSO() { return nluSO; }

    public POTENTIAL GetPotential() { return potential_class; }

    /// <summary>
    /// STATUS_SELECTの名前を日本語に変換
    /// </summary>
    public string StatusSelectName(STATUS_SELECT _statusSelect)
    {
        switch(_statusSelect)
        {
            case STATUS_SELECT.MoveSpeedMax:
                return "移動速度";
            case STATUS_SELECT.RechargeMax:
                return "充電回数";
            case STATUS_SELECT.EnergyMax:
                return "バッテリー容量";
            case STATUS_SELECT.GatherStrengthMax:
                return "収集力";
            case STATUS_SELECT.GatherRateMax:
                return "収集速度";
        }
        return null;
    }

    /// <summary>
    /// 潜在能力を確保するクラス
    /// </summary>
    [System.Serializable]
    public class POTENTIAL
    {
        [Header("ロボットの潜在能力")]
        public float MOVESPEED_MAX;
        public float RECHARGE_MAX;
        public float ENERGY_MAX;
        public float GATHERSTRENGTH_MAX;
        public float GATHERRATE_MAX;
        
    }

    /// <summary>
    /// Locationのレベルを上げたときに処理される関数
    /// </summary>
    public void LocationLevelUp()
    {
        BASE_NEXT_UNLOCK.StatusParam[] _statusParams = null;
        if(LocationLevel < nluSO.GetBaseNextUnlocks_List().Count)
        {
            // 現在のレベルに合わせたStatusParamを取得する
            _statusParams = nluSO.GetBaseNextUnlocks_List()[LocationLevel].statusParam;
        }

        if(_statusParams != null)
        {
            for(int ii = 0; ii < _statusParams.Length; ii++)
            {
                // 上昇させたいステータスを決める
                switch(_statusParams[ii].selectStatus)
                {
                    case STATUS_SELECT.MoveSpeedMax:
                        potential_class.MOVESPEED_MAX += _statusParams[ii].statusLimited_value;
                        break;
                    case STATUS_SELECT.RechargeMax:
                        potential_class.RECHARGE_MAX += _statusParams[ii].statusLimited_value;
                        break;
                    case STATUS_SELECT.EnergyMax:
                        potential_class.ENERGY_MAX += _statusParams[ii].statusLimited_value;
                        break;
                    case STATUS_SELECT.GatherStrengthMax:
                        potential_class.GATHERSTRENGTH_MAX += _statusParams[ii].statusLimited_value;
                        break;
                    case STATUS_SELECT.GatherRateMax:
                        potential_class.GATHERRATE_MAX += _statusParams[ii].statusLimited_value;
                        break;
                }
            }
        }

        // アンロックさせる内容の処理が終わったら
        LocationLevel++;        // 拠点のレベルを上げる
    }

    
}

[System.Flags]
public enum STATUS_SELECT
{
    NONE = -1,
    MoveSpeedMax = 0 << 0,
    RechargeMax = 1 << 1,
    EnergyMax = 2 << 2,
    GatherStrengthMax = 3 << 3,
    GatherRateMax = 4 << 4,
}


/// <summary>
/// UIの更新を1フレームごとに行わないため
/// </summary>
public class UpdateTime_Class
{
    const float update_AbsTime = 1.0f;      // UIの更新を1フレームごとに行わないための時間
    float processTime = 0f;                 // 経過時間

    /// <summary>
    /// UIの更新を1フレームごとに行わないための処理
    /// </summary>
    public bool UpdateTime()
    {
        // 経過時間を更新
        processTime += Time.deltaTime;

        // 設定時間より経過時間の方が大きくなったら
        if (processTime > update_AbsTime)
        {
            processTime = 0f; // 条件を満たしたらリセット
            return true;
        }

        return false;
    }
}