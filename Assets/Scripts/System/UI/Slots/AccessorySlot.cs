using UnityEngine;
using UnityEngine.UI;

public class AccessorySlot : MonoBehaviour
{
    [SerializeField] AccessoryData accessory_value;
    int this_slotNumber;
    public Image icon;          // アイコンの画像
    public Button button;       // アイコンをクリック

    public void SetText_AccessoryValue(AccessoryData status)
    {
        if(status != null)
        {
            icon.sprite = status.icon;
            icon.enabled = true;
        }
        else
        {
            icon.enabled = false;
        }
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

        EquipmentManager.instance.SetActiv_SelectSlots_Accessory();

        foreach(var _slot in EquipmentManager.instance.GetEquipmentSelectSlot())
        {
            _slot.Check_SelectSlot(SELECT_EQUIPMENTSLOT.ACCESSORY, this_slotNumber);
        }

        EquipmentManager.instance.GetAccessoryController().SetText_AccessoryInfo();
        EquipmentManager.instance.SetActive_Equipment_ScrollView(true);   // 非表・示表示の設定
    }

    public void SetAccessoryNum(int num) { this_slotNumber = num; }

}