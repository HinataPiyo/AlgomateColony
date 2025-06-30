using UnityEngine;

[CreateAssetMenu(fileName = "BatteryDatabase", menuName = "Database/BatteryDatabase")]
public class BatteryDatabase : ScriptableObject
{
    [SerializeField] BatteryType.DATA[] battery_values;
    public BatteryType.DATA[] DB => battery_values;
}