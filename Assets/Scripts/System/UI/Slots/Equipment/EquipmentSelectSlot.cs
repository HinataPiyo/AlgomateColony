using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSelectSlot : MonoBehaviour
{
    [SerializeField] EquipmentSO.EQUIPMENT_STATUS equipment_value;
    [SerializeField] AccessoryData accessory_value;
    [SerializeField] BatteryData.BATTERY_STATUS battery_value; 

    SELECT_EQUIPMENTSLOT select_equipmentslot;
    int accessory_slotNo;

    [Header("ボタン")]
    [SerializeField] Button select_button;

    [Header("画像")]
    [SerializeField] Image icon;
    
    [Header("テキスト")]
    [SerializeField] TextMeshProUGUI equipment_name;
    [SerializeField] TextMeshProUGUI equipment_level;
    [SerializeField] TextMeshProUGUI equipment_statusup_name;
    [SerializeField] TextMeshProUGUI equipment_statusup_value;
    void Start()
    {
        select_button.onClick.AddListener(OnClick_SelectButton);
    }

    /// <summary>
    /// スロットやテキストの設定
    /// </summary>
    /// <param name="_value"></param>
    /// <param name="_scriot"></param>
    public void SetText_EquipmentInfo_Equipment(EquipmentSO.EQUIPMENT_STATUS _value)
    {
        equipment_value = _value;
        icon.sprite = _value.icon;
        equipment_name.text = _value._name;
        equipment_level.text = "" + _value.level;
        equipment_statusup_name.text = _value.statusup_name;
        equipment_statusup_value.text = "+ " + _value.statusup_value;
    }

    /// <summary>
    /// スロットやテキストの設定
    /// </summary>
    /// <param name="_value"></param>
    /// <param name="_scriot"></param>
    public void SetText_EquipmentInfo_Accessory(AccessoryData _value)
    {
        accessory_value = _value;
        icon.sprite = _value.icon;
        equipment_name.text = _value._name;
        equipment_level.text = "" + _value.level;
        equipment_statusup_name.text = _value.statusup_name;
        equipment_statusup_value.text = "+ " + _value.statusup_value;
    }

    public void Check_SelectSlot(SELECT_EQUIPMENTSLOT _select, int _slotNo)
    {
        select_equipmentslot = _select;
        accessory_slotNo = _slotNo;
    }

    /// <summary>
    /// ボタンが押されたら、装備欄のスロットに設定する
    /// </summary>
    public void OnClick_SelectButton()
    {
        SoundManager.instance.PlayAudio("ButtonClick");
        switch(select_equipmentslot)
        {
            case SELECT_EQUIPMENTSLOT.TOOL:
                EquipmentManager.instance.GetToolController().SetEquipmentSlot(equipment_value);
                break;
            case SELECT_EQUIPMENTSLOT.ACCESSORY:
                EquipmentManager.instance.GetAccessoryController().SetEquipmentSlot(accessory_value, accessory_slotNo);
                break;
        }
    }



}

public enum SELECT_EQUIPMENTSLOT
{
    TOOL,
    ACCESSORY,
}
