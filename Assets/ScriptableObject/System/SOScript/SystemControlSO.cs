using System.Collections.Generic;
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
        public string statusName;               // 上限突破するステータスの名前
        public float statusLimited_value;       // 上限突破するステータスの値
    }
}
