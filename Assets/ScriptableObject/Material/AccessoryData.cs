using UnityEngine;

[CreateAssetMenu(fileName = "AccessoryData", menuName = "AccessoryData")]
public class AccessoryData : ScriptableObject
{
    public Sprite icon;
    public int serialNum;
    public string _name;            // 装備の名前
    public int level;               // 装備のレベル
    public string statusup_name;    // 何のステータスが上昇するか(アビリティの説明)
    public string exp;
    public float statusup_value;    // GatherRateに適応する値
    public float levelupPitch;      // レベル上昇時の上り幅
}