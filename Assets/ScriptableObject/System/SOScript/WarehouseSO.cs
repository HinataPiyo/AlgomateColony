using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WarehouseSO", menuName = "CreatScriptableObject/WarehouseSO")]
public class WarehouseSO : ScriptableObject
{
    // 一つ一つのスロットをまとめたリスト
    [SerializeField] List<BASE_WAREHOUSE_SLOT> base_warehouse_slots = new List<BASE_WAREHOUSE_SLOT>();

    public List<BASE_WAREHOUSE_SLOT> GetBaseWarehouseSlot_List() { return base_warehouse_slots; }

    /// <summary>
    /// 倉庫の一つ一つのスロット
    /// </summary>
    [System.Serializable]
    public class BASE_WAREHOUSE_SLOT
    {
        public MaterialSO mateSO;
        public uint mateAmount;
    }
}