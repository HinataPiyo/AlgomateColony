using UnityEngine;

public static class BatteryType
{
    [System.Serializable]
    public struct DATA
    {
        public string _name;
        public LEVEL battery_level;
        public float energyMax_Up;
        public string statusup_name;
        public Sprite icon;
    }

    /// <summary>
    /// バッテリーのレベル（数字が小さいほど強い）
    /// </summary>
    public enum LEVEL
    { TIER_ONE, TIER_TWO, TIER_THREE }
}