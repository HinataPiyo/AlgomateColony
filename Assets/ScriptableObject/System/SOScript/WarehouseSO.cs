using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WarehouseSO", menuName = "CreatScriptableObject/WarehouseSO")]
public class WarehouseSO : ScriptableObject
{
    // 一つ一つのスロットをまとめたリスト
    [SerializeField] List<MATERIAL_WAREHOUSE_SLOT> mate_warehouse_slots = new List<MATERIAL_WAREHOUSE_SLOT>();
    [SerializeField] List<AccessoryData> acce_warehouse_slots = new List<AccessoryData>();

    public List<MATERIAL_WAREHOUSE_SLOT> GetMaterial_WarehouseList() { return mate_warehouse_slots; }
    public List<AccessoryData> GetAccessory_WarehouseList() { return acce_warehouse_slots; }

    /// <summary>
    /// 倉庫の一つ一つのスロット
    /// </summary>
    [System.Serializable]
    public class MATERIAL_WAREHOUSE_SLOT
    {
        public MaterialSO mateSO;
        public uint mateAmount;
    }
}