using UnityEngine;
using UnityEngine.UI;

public class ToolSlot : MonoBehaviour
{
    [SerializeField] EquipmentSO.EQUIPMENT_STATUS value;
    public Image icon;          // アイコンの画像
    public Button button;       // アイコンをクリック

    public void SetText_ToolValue(EquipmentSO.EQUIPMENT_STATUS _value)
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
        SoundManager.instance.PlayAudio("ButtonClick");
        EquipmentManager.instance.SetActiv_SelectSlots_Tool();

        foreach(var _slot in EquipmentManager.instance.GetEquipmentSelectSlot())
        {
            _slot.Check_SelectSlot(SELECT_EQUIPMENTSLOT.TOOL, 0);
        }

        EquipmentManager.instance.GetToolController().SetText_EquipmentInfo();                // スクロールバーのスロットの設定
        EquipmentManager.instance.SetActive_Equipment_ScrollView(true);   // 非表・示表示の設定
    }


}