using UnityEngine;

public class RobotBatteryController : MonoBehaviour
{
    [SerializeField] BatteryData batteryData;
    BatteryData.BATTERY_STATUS[] b_status;

    [Header("バッテリー")]
    [SerializeField] BatterySlot b_slot;

    BaseStatus robotBase;

    private void Start()
    {
        b_status = batteryData.battery_values;

        b_slot.icon.sprite = null;
        b_slot.icon.enabled = false;
    }

    /// <summary>
    /// 装備のInfoパネルにあるテキストなどの設定
    /// </summary>
    /// <param name="_name"></param>
    public void SetText_EquipmentInfo()
    {
        for(int ii = 0; ii < b_status.Length; ii++)
        {
            EquipmentManager.instance.GetBatterySelectSlot()[ii].SetText_EquipmentInfo_Battery(b_status[ii]);
        }
    }

    /// <summary>
    /// 装備を選んだあと装備スロットに設定する処理
    /// また、"RobotBaseStatus"の個々の"Equipment_Value"を上書き
    /// </summary>
    /// <param name="selectslot_Nomber">選択された装備欄のスロット</param>
    /// <param name="_value"></param>
    public void SetEquipmentSlot(BatteryData.BATTERY_STATUS _value)
    {
        // バッテリーを装備したらRobotClassに格納される
        EquipmentManager.instance.GetRobotStatus().battery_status = _value;

        // バッテリーを表示するスロットのテキストやステータスなどの設定
        b_slot.SetText_BatteryValue(_value);

        EquipmentManager.instance.GetRobotStatus().StatusUp_EnergyMax();
    }

    /// <summary>
    /// 装備スロットのボタンの"Interactable"の設定と"Icon"の表示・非表示
    /// </summary>
    public void SetButtonInteractable()
    {
        b_slot.button.interactable = true;

        // スロットに画像が設定されていたら
        if(b_slot.icon.sprite == null)
        {
            b_slot.icon.enabled = false;
        }
        else    // スロットに画像が設定されていなければ
        {
            b_slot.icon.enabled = true;
        }
    }

    public BatterySlot GetToolSlot() { return b_slot; }
}