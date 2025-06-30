using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// アルゴメイトのステータスから見れるツールスロットの設定
/// 各スロットにアタッチされるスクリプトs
/// </summary>
public class RobotStatusToolSlot : MonoBehaviour
{
    [SerializeField] EquipmentType.DATA data;
    public Image icon;          // アイコンの画像
    public Button button;       // アイコンをクリック

    public void SetText_ToolValue(EquipmentType.DATA _data)
    {
        data = _data;
        icon.sprite = _data.icon;
        icon.enabled = true;
    }

    void Start()
    {
        button.onClick.AddListener(OnClick_SelectButton);
        button.interactable = false;
    }

    /// <summary>
    /// スロットをクリックしたときcontrollerに自身の番号を送る
    /// </summary>
    public void OnClick_SelectButton()
    {
        SoundManager.instance.PlayAudio("ButtonClick");
        EquipmentManager.instance.SetActiv_SelectSlots_Tool();

        foreach (var _slot in EquipmentManager.instance.EquipSlot)
        {
            _slot.Check_SelectSlot(SELECT_EQUIPMENTSLOT.TOOL, 0);
        }

        EquipmentManager.instance.ToolController.SetText_EquipmentInfo();                // スクロールバーのスロットの設定
        EquipmentManager.instance.SetActive_Equipment_ScrollView(true);   // 非表・示表示の設定
    }


}