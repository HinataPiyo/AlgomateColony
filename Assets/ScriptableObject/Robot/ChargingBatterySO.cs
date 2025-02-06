using UnityEngine;

/// <summary>
/// スクリプタブルオブジェクトの作成
/// </summary>
[CreateAssetMenu(fileName = "ChargingBatterySO", menuName = "CreatScriptableObject/ChargingBatterySO")]
public class ChargingBatterySO : ScriptableObject
{
    public int possible_chargeAmount = 1;
}