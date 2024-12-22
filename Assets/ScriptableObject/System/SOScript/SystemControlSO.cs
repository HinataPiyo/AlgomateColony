using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "SystemControlSO", menuName = "SystemControlSO")]
public class SystemControlSO : ScriptableObject 
{
    [SerializeField] private NextLevelUnlockedSO nluSO;
    [SerializeField] private int LocationLevel;     // 拠点のレベルを設定
    [SerializeField] List<StatusLimited> statuslimited = new List<StatusLimited>();


    public int GetLocationLevel() { return LocationLevel; }
    public NextLevelUnlockedSO GetNextLevelUnlockedSO() { return nluSO; }
    public List<StatusLimited> GetStatusLimiteds() { return statuslimited; }

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
/// ステータスの上限Upの値を決める
/// </summary>
[System.Serializable]
public struct StatusLimited
{
    public StatusParam[] statusParam;

    [System.Serializable]
    public struct StatusParam
    {
        public STATUS_SELECT selectStatus;
        public float statusLimited_value;       // 上限突破するステータスの値
    }
}
