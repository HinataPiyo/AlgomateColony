using UnityEngine;
using UnityEngine.UI;

public class BatterySlot : MonoBehaviour
{
    [SerializeField] BatteryData.BATTERY_STATUS value;
    public Image icon;          // アイコンの画像
    public Button button;       // アイコンをクリック

    public void SetText_BatteryValue(BatteryData.BATTERY_STATUS _value)
    {
        value = _value;
        icon.sprite = _value.icon;
        icon.enabled = true;
    }

    private void Start() {
        button.onClick.AddListener(OnClick_SelectButton);
    }
    
    /// <summary>
    /// スロットをクリックしたときcontrollerに自身の番号を送る
    /// </summary>
    public void OnClick_SelectButton() {
        SoundManager.instance.PlayAudio("ButtonClick");
        EquipmentManager.instance.SetActiv_SelectSlots_Battery();

        foreach(var _slot in EquipmentManager.instance.GetBatterySelectSlot())
        {
            _slot.Check_SelectSlot(SELECT_EQUIPMENTSLOT.BATTERY, 0);
        }

        EquipmentManager.instance.GetRobotBatteryController().SetText_EquipmentInfo();      // スクロールバーのスロットの設定
        EquipmentManager.instance.SetActive_Battery_ScrollView(true);               // 非表・示表示の設定
    }
}