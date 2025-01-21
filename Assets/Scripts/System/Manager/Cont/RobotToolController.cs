using UnityEngine;

public class RobotToolController : MonoBehaviour
{
    [SerializeField] EquipmentSO equipmentSO;                                // 装備の核、スクリプタブルオブジェクト
    EquipmentSO.EQUIPMENT_STATUS[] e_status;                // 装備のステータスをまとめる場所
    
    [Header("装備")]
    [SerializeField] ToolSlot equipmentSlots;          // 装備スロットのスクリプト(個々)



    private void Start()
    {
        e_status = equipmentSO.equipment_values;

        equipmentSlots.icon.sprite = null;
        equipmentSlots.icon.enabled = false;
    }

    /// <summary>
    /// 装備のInfoパネルにあるテキストなどの設定
    /// </summary>
    /// <param name="_name"></param>
    public void SetText_EquipmentInfo()
    {
        for(int ii = 0; ii < e_status.Length; ii++)
        {
            EquipmentManager.instance.GetEquipmentSelectSlot()[ii].SetText_EquipmentInfo_Equipment(e_status[ii]);
        }
    }

    /// <summary>
    /// 装備を選んだあと装備スロットに設定する処理
    /// また、"RobotBaseStatus"の個々の"Equipment_Value"を上書き
    /// </summary>
    /// <param name="selectslot_Nomber">選択された装備欄のスロット</param>
    /// <param name="_value"></param>
    public void SetEquipmentSlot(EquipmentSO.EQUIPMENT_STATUS _value)
    {
        EquipmentManager.instance.GetRobotStatus().equipment_value = _value;
        equipmentSlots.SetText_ToolValue(_value);
    }

    /// <summary>
    /// 装備スロットのボタンの"Interactable"の設定と"Icon"の表示・非表示
    /// </summary>
    public void SetButtonInteractable()
    {
        equipmentSlots.button.interactable = true;

        // スロットに画像が設定されていたら
        if(equipmentSlots.icon.sprite == null)
        {
            equipmentSlots.icon.enabled = false;
        }
        else    // スロットに画像が設定されていなければ
        {
            equipmentSlots.icon.enabled = true;
        }
    }

    public ToolSlot GetToolSlot() { return equipmentSlots; }
    
}