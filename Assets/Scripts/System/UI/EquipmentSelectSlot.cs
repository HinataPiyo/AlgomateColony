using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSelectSlot : MonoBehaviour
{
    [SerializeField] EquipmentSO.EQUIPMENT_VALUE equipment_value;
    [SerializeField] UNLOCK_EQUIPMENT_SLOT select_slotNo;
    EquipmentController equipmentController;

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

    public void SetText_EquipmentInfo(EquipmentSO.EQUIPMENT_VALUE _value, EquipmentController _scriot)
    {
        if(equipmentController == null)
        {
            equipmentController = _scriot;
        }

        equipment_value = _value;
        icon.sprite = _value.icon;
        equipment_name.text = _value._name;
        equipment_level.text = "" + _value.level;
        equipment_statusup_name.text = _value.statusup_name;
        equipment_statusup_value.text = "" + _value.statusup_value;
    }

    /// <summary>
    /// ボタンが押されたら、装備欄のスロットに設定する
    /// </summary>
    public void OnClick_SelectButton()
    {
        equipmentController.SetEquipmentSlot(equipment_value);
    }

}
