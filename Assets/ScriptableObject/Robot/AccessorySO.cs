using UnityEngine;

[CreateAssetMenu(fileName = "AccessorySO", menuName = "CreatScriptableObject/AccessorySO")]
public class AccessorySO : ScriptableObject
{
    // 【アクセサリーの設定】
    public Sprite stop_sprite;
    [Header("加工品の一覧")] public PROCESSING_STATUS[] processing_status;
    [Header("アクセサリーの一覧")] public NEED_ACCESSORY_STATUS[] need_accessory_status;
    [System.Serializable]
    public struct NEED_ACCESSORY_STATUS
    {
        public AccessoryData acceData;
        public NEED_MATERIAL[] need_mate_list;
    }

    /// <summary>
    /// 加工品のステータス
    /// </summary>
    [System.Serializable]
    public struct PROCESSING_STATUS
    {
        public MaterialSO mateSO;
        public NEED_MATERIAL[] need_mate_list;
    }

    /// <summary>
    /// アクセサリーを作成するときの必要素材
    /// </summary>
    [System.Serializable]
    public struct NEED_MATERIAL
    {
        public MaterialSO mateSO;       // 素材のデータ
        public uint needAmo;             // 必要個数
    }
}

public enum UNLOCK_ACCESSORY_SLOT
{
    ZERO,
    ONE,
    TWO,
}