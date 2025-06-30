using UnityEngine;

/// <summary>
/// ロボットのツール関連を管理するクラス
/// ロボット自身にアタッチする
/// </summary>
public class RobotToolController : MonoBehaviour
{
    EquipmentDatabase equipmentDB;      // 装備のデータベース
    
    [Header("装備スロット")]
    [SerializeField] RobotStatusToolSlot equipmentSlots;          // 装備スロットのスクリプト

    public RobotStatusToolSlot ToolSlot => equipmentSlots;

    private void Start()
    {
        equipmentDB = DataManager.instance.EquipmentDB;

        equipmentSlots.icon.sprite = null;
        equipmentSlots.icon.enabled = false;
    }

    /// <summary>
    /// 装備のInfoパネルにあるテキストなどの設定
    /// </summary>
    /// <param name="_name"></param>
    public void SetText_EquipmentInfo()
    {
        for(int ii = 0; ii < equipmentDB.DB.Length; ii++)
        {
            EquipmentManager.instance.EquipSlot[ii].SetText_EquipmentInfo_Equipment(equipmentDB.DB[ii]);
        }
    }

    /// <summary>
    /// 装備を選んだあと装備スロットに設定する処理
    /// また、"RobotBaseStatus"の個々の"Equipment_Value"を上書き
    /// </summary>
    /// <param name="selectslot_Nomber">選択された装備欄のスロット</param>
    /// <param name="_value"></param>
    public void SetEquipmentSlot(EquipmentType.DATA _value)
    {
        EquipmentManager.instance.RobotStatus.equipment_value = _value;
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
    
}