using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentSO", menuName = "CreatScriptableObject/EquipmentSO")]
public class EquipmentSO : ScriptableObject
{
    /// 【装備の設定】
    // ----------------------------------------------------------------------------------------------
    /// 初期の段階では１ロボットにつき装備は一つしか装備できないが
    /// 進んでいくと装備スロットが解放され２つ以上装備できるようにする...とか
    /// 装備していないロボットが例えば木を切りに行くとすると、採取速度がかなり落ちる様にする...とか
    /// 装備は共通して、一つしか存在しないものとする。他のロボットが装着しても能力が引き継がれる。
    // ----------------------------------------------------------------------------------------------

    public Sprite stop_sprite;
    public EQUIPMENT_VALUE[] equipment_values;

    // 各々の装備のステータスを設定する
    [System.Serializable]
    public struct EQUIPMENT_VALUE
    {
        public EQUIPMENT_NAME equipment_name;   // 装備の名前
        public Sprite icon;
        public string _name;            // 装備の名前
        public int level;               // 装備のレベル
        public string statusup_name;    // 何のステータスが上昇するか
        public float statusup_value;   // GatherRateに適応する値
        public float levelupPitch;      // レベル上昇時の上り幅
    }

    /// <summary>
    /// 装備の合計値からGatherRateステータスを引くので、
    /// 全ての装備の合計値を求めて返す
    /// </summary>
    /// <returns></returns>
    public float GetEquipmentTotalValue(EQUIPMENT_NAME _name)
    {
        foreach(var _equipment in equipment_values)
        {
            if(_equipment.equipment_name == _name)
            {
                switch(_name)
                {
                    case EQUIPMENT_NAME.NONE:
                        return 0;
                    case EQUIPMENT_NAME.DRIL:
                        return _equipment.statusup_value;
                    case EQUIPMENT_NAME.ARM:
                        return _equipment.statusup_value;
                    case EQUIPMENT_NAME.CHAINSAW:
                        return _equipment.statusup_value;
                }
            }
        }

        return 0;       // 合計値を返す
    }

    
}

[System.Flags]
public enum EQUIPMENT_NAME
{
    BATTERY = -1 << -1, 
    NONE = 0 << 0,
    DRIL = 1 << 1,
    ARM = 1 << 2,
    CHAINSAW = 1 << 3,
}

public enum UNLOCK_EQUIPMENT_SLOT
{
    NONE = -1,
    ONE,
    TWO,
    THREE,
}
