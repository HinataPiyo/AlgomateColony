using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class DataType
{
    /// <summary>
    /// アクセサリー/加工品などを作成するときの必要素材
    /// </summary>
    [System.Serializable]
    public struct NEED_MATERIAL
    {
        [Tooltip("素材のデータ")] public MaterialSO mateSO;
        [Tooltip("必要個数")] public int needAmo;
    }

    /// <summary>
    /// 倉庫に格納するスロット
    /// </summary>
    [System.Serializable]
    public class WAREHOUSE_SLOT
    {
        public MaterialSO mateSO;
        public int hasAmount;
    }

    /// <summary>
    /// 潜在能力を確保するクラス
    /// 潜在能力はすべてのアルゴメイトに適用される
    /// </summary>
    [System.Serializable]
    public class POTENTIAL
    {
        [Header("ロボットの潜在能力")]
        public float moveSpeedMax;
        public float rechargeMax;
        public float energyMax;
        public float gatherStrengthMax;
        public float gatherRateMax;
    }

    /// <summary>
    /// 素材の所持数を必要素材のテキストに反映させる
    /// </summary>
    public static void Sync_HaveMaterialToText(WarkshopNeedMaterialSlot[] need, List<WAREHOUSE_SLOT> warehouseSlot)
    {
        for(int ii = 0; ii < need.Length; ii++)
        {
            // 倉庫内を全て見る
            for(int qq = 0; qq < warehouseSlot.Count; qq++)
            {
                // 必要素材と倉庫の素材のシリアル番号が同一だった場合
                if(need[ii].GetMaterialSO()?.serialNum == warehouseSlot[qq].mateSO.serialNum)
                {
                    if (ii <= warehouseSlot.Count) return;
                    // 素材の所持数を反映させる
                    need[ii].SetStockAmount(warehouseSlot[ii].hasAmount);
                    break;
                }
            }
        }
    }
}

/// <summary>
/// アクセサリーのアンロック状態
/// </summary>
public enum UNLOCK_ACCESSORY_SLOT { ZERO, ONE, TWO }

[System.Flags]
public enum STATUS_TYPE
{
    NONE = -1,
    MoveSpeedMax = 0 << 0,
    RechargeMax = 1 << 1,
    EnergyMax = 2 << 2,
    GatherStrengthMax = 3 << 3,
    GatherRateMax = 4 << 4,
}

