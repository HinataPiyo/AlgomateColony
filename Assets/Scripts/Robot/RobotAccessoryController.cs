using UnityEngine;

// ロボットにつけるスクリプト
public class RobotAccessoryController : MonoBehaviour
{
    // 手動で設定してある
    [SerializeField] AccessorySlot[] accessorySlots;

    private void Start() {
        // 番号を割り振る
        for(int ii = 0; ii < accessorySlots.Length; ii++)
        {
            accessorySlots[ii].SetAccessoryNum(ii);
        }
    }

    /// <summary>
    /// 装備のInfoパネルにあるテキストなどの設定
    /// </summary>
    /// <param name="_name"></param>
    public void SetText_AccessoryInfo()
    {
        for(int ii = 0; ii < EquipmentManager.instance.AccessoryStatusSlot.Length; ii++)
        {
            EquipmentManager.instance.EquipSlot[ii].
            SetText_EquipmentInfo_Accessory(EquipmentManager.instance.AccessoryStatusSlot[ii]);
        }
    }

    /// <summary>
    /// 装備を選んだあと装備スロットに設定する処理
    /// また、"RobotBaseStatus"の個々の"Equipment_Value"を上書き
    /// </summary>
    /// <param name="selectslot_Nomber">選択された装備欄のスロット</param>
    /// <param name="_acceData"></param>
    public void SetEquipmentSlot(AccessoryData _acceData, int _slotNo)
    {
        EquipmentManager.instance.RobotStatus.acceData_list[_slotNo] = _acceData;
        accessorySlots[_slotNo].SetText_AccessoryValue(_acceData);
    }


    public void SetButtonInteractable(BaseStatus _robotbase)
    {
        switch(_robotbase.unlock_accessory_slot)
        {
            case UNLOCK_ACCESSORY_SLOT.ZERO:
                Check_UnlockSlot(0);
                break;
            case UNLOCK_ACCESSORY_SLOT.ONE:
                Check_UnlockSlot(1);
                break;
            case UNLOCK_ACCESSORY_SLOT.TWO:
                Check_UnlockSlot(2);
                break;
        }
    }

    void Check_UnlockSlot(int _slotNo)
    {
        for(int ii = 0; ii < accessorySlots.Length; ii++)
        {
            if(ii < _slotNo)
            {
                // 押せないときの画像が設定されていれば
                if(accessorySlots[ii].icon.sprite == DataManager.instance.AccessoryDB.StopSprite)
                {
                    // スロットの中身を空にする
                    accessorySlots[ii].icon.sprite = null;
                }

                // 画像が設定されていなかった場合
                if(accessorySlots[ii].icon.sprite == null)
                {
                    accessorySlots[ii].icon.enabled = false;
                }
                else
                {
                    accessorySlots[ii].icon.enabled = true;
                }

                // ボタンを押せるようにする
                accessorySlots[ii].button .interactable = true;
            }
            else
            {
                // 押せないときに表示する画像を設定する
                accessorySlots[ii].icon.sprite = DataManager.instance.AccessoryDB.StopSprite;
                accessorySlots[ii].icon.enabled = true;

                // ボタンを押せるようにする
                accessorySlots[ii].button .interactable = false;
            }
        }
    }

    public AccessorySlot[] GetAccessorySlots() { return accessorySlots;}
    
}