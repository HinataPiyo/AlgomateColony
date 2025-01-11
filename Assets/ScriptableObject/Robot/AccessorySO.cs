using UnityEngine;

[CreateAssetMenu(fileName = "AccessorySO", menuName = "CreatScriptableObject/AccessorySO")]
public class AccessorySO : ScriptableObject
{
    // 【アクセサリーの設定】
    public Sprite stop_sprite;
    public ACCESSORY_STATUS[] accessory_status;
    [System.Serializable]
    public struct ACCESSORY_STATUS
    {
        public ACCESSORY_NAME accessory_name;   // 装備の名前
        public Sprite icon;
        public string _name;            // 装備の名前
        public int level;               // 装備のレベル
        public string statusup_name;    // 何のステータスが上昇するか(アビリティの説明)
        public float statusup_value;   // GatherRateに適応する値
        public float levelupPitch;      // レベル上昇時の上り幅
    }
}

public enum ACCESSORY_NAME
{
    NONE,
    NAME_1,
    NAME_2,
}

public enum UNLOCK_ACCESSORY_SLOT
{
    ZERO,
    ONE,
    TWO,
}