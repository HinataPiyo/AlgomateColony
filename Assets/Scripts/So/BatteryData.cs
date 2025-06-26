using UnityEngine;

[CreateAssetMenu(fileName = "BatteryData", menuName = "BatteryData")]
public class BatteryData : ScriptableObject
{
    public BATTERY_STATUS[] battery_values;
    [System.Serializable]
    public struct BATTERY_STATUS
    {
        public string _name;
        public BATTERY_LEVEL battery_level;
        public float energyMax_Up;
        public string statusup_name;
        public Sprite icon;
    }
}


/// <summary>
/// バッテリーのレベル（数字が小さいほど強い）
/// </summary>
public enum BATTERY_LEVEL
{
    TIER_ONE,
    TIER_TWO,
    TIER_THREE,
}