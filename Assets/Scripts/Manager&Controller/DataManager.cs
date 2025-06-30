using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SOなどのデータを一元管理するためのシングルトンクラス
/// </summary>
public class DataManager : MonoBehaviour
{
    public static DataManager instance { get; private set; }

    [Tooltip("ゲーム進行に関するSO")]
    [SerializeField] SystemControlSO systemControlSO;

    [Tooltip("アクセサリーのデータベース")]
    [SerializeField] AccessoryDatabase accessoryDatabase;

    [Tooltip("加工品のデータベース")]
    [SerializeField] ProcessingDatabase processingDatabase;

    [Tooltip("装備のデータベース")]
    [SerializeField] EquipmentDatabase equipmentDatabase;

    [Tooltip("バッテリーのデータベース")]
    [SerializeField] BatteryDatabase batteryDatabase;

    [Tooltip("拠点の次のレベルになった時のデータ")]
    [SerializeField] LocationLevelupUnlock locationLevelupUnlock;

    [SerializeField] ChargingBatterySO chargingBatterySO;

    [Tooltip("潜在能力")]
    [SerializeField] DataType.POTENTIAL potential;

    // ゲッター
    public DataType.POTENTIAL PotentialTB => potential;

    public SystemControlSO SystemControlSO => systemControlSO;
    public AccessoryDatabase AccessoryDB => accessoryDatabase;
    public ProcessingDatabase ProcessingDB => processingDatabase;
    public EquipmentDatabase EquipmentDB => equipmentDatabase;
    public BatteryDatabase BatteryDB => batteryDatabase;
    public LocationLevelupUnlock levelupUnlockTB => locationLevelupUnlock;
    public ChargingBatterySO ChargingBatterySO => chargingBatterySO;

    // メンバ変数
    Dictionary<STATUS_TYPE, System.Action<float>> _potentialSetters;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        _potentialSetters = new Dictionary<STATUS_TYPE, System.Action<float>>()
        {
            { STATUS_TYPE.MoveSpeedMax, v => potential.moveSpeedMax += v },
            { STATUS_TYPE.RechargeMax, v => potential.rechargeMax += v },
            { STATUS_TYPE.EnergyMax, v => potential.energyMax += v },
            { STATUS_TYPE.GatherStrengthMax, v => potential.gatherStrengthMax += v },
            { STATUS_TYPE.GatherRateMax, v => potential.gatherRateMax += v },
        };
    }

    /// <summary>
    /// 潜在能力を上昇させる
    /// </summary>
    /// <param name="type">ステータス名</param>
    /// <param name="val">値</param>
    public void SetPotential(STATUS_TYPE type, float val)
    {
        if (_potentialSetters.TryGetValue(type, out var setter))
        {
            setter(val);
        }
    }

}