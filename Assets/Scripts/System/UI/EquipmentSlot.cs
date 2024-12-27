using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
    EquipmentController equipmentController;
    [SerializeField] EquipmentSO equipmentSO;
    [SerializeField] EquipmentSO.EQUIPMENT_VALUE value;
    [SerializeField] UNLOCK_EQUIPMENT_SLOT slotNo;
    public Image icon;          // アイコンの画像
    public Button button;       // アイコンをクリック

    public void SetEquipmentController(EquipmentController cont) { equipmentController = cont; }
    public UNLOCK_EQUIPMENT_SLOT GetUnlockSlotNomber() { return slotNo; }
    public EquipmentSO.EQUIPMENT_VALUE GetEquipmentValue() { return value; }
    public void SetText_EquipmentValue(EquipmentSO.EQUIPMENT_VALUE _value)
    {
        value = _value;
        icon.sprite = _value.icon;
        icon.enabled = true;
    }

    private void Start() {
        button.onClick.AddListener(OnClick_SelectButton);
        button.interactable = false;
    }
    
    /// <summary>
    /// スロットをクリックしたときcontrollerに自身の番号を送る
    /// </summary>
    public void OnClick_SelectButton() {
        equipmentController.SetActive_Equipment_ScrollView(true);
        equipmentController.SetSelectNomber(slotNo);
    }


}