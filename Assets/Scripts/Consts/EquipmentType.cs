using UnityEngine;

public static class EquipmentType
{
    // 各々の装備のステータスを設定する
    [System.Serializable]
    public struct DATA
    {
        public TYPE equipment_name;   // 装備の名前
        public Sprite icon;
        public string _name;            // 装備の名前
        public int level;               // 装備のレベル
        public string statusup_name;    // 何のステータスが上昇するか
        public float statusup_value;   // GatherRateに適応する値
        public float levelupPitch;      // レベル上昇時の上り幅
    }

    public enum TYPE
    { BATTERY = -1, NONE, DRIL, ARM, CHAINSAW, }

    /// <summary>
    /// 収集対象のオブジェクトが装備しているツールと
    /// マッチしていれば本領発揮できるようにする関数
    /// </summary>
    /// <param name="data">ツールデータ</param>
    /// <param name="mate">収集対象</param>
    public static float GetEquipmentTotalValue(DATA data, MaterialSO mate)
    {
        bool isMatch = data.equipment_name == mate.equipmentToMatch;
        
        return isMatch ? data.statusup_value : 0.5f;
    }
}