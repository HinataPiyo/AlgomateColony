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
    public EQUIPMENT_STATUS[] equipment_values;

    // 各々の装備のステータスを設定する
    [System.Serializable]
    public struct EQUIPMENT_STATUS
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
    public float GetEquipmentTotalValue(EQUIPMENT_STATUS _status, MaterialSO _mateSO)
    {
        // 装備している装備の名前 と 取集している資源に設定してある対応した装備 が 一致していれば
        if(_status.equipment_name == _mateSO.EquipmentToMatch)
        {
            // 装備に合った値を返す
            switch(_status.equipment_name )
            {
                case EQUIPMENT_NAME.NONE:
                    return 0;
                case EQUIPMENT_NAME.DRIL:
                    return _status.statusup_value;
                case EQUIPMENT_NAME.ARM:
                    return _status.statusup_value;
                case EQUIPMENT_NAME.CHAINSAW:
                    return _status.statusup_value;
            }
        }

        return 0;       // 合計値を返す
    }
}

public enum EQUIPMENT_NAME
{
    BATTERY = -1, 
    NONE,
    DRIL,
    ARM,
    CHAINSAW,
}


