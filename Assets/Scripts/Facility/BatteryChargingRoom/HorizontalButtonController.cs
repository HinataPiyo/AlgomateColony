using System.Linq;
using UnityEngine;

public class HorizontalButtonController : MonoBehaviour
{
    ButtonSlotVarticalHorizontal[] slot_script;
    BatteryChargingRoomManager.HorizontalButtonCohesion[] horizontal_panels;
    
    void Start()
    {
        HorizontalButtonClick_Proc(0);
    }

    public void Set_HorizontalButton(ButtonSlotVarticalHorizontal[] _slots,
    BatteryChargingRoomManager.HorizontalButtonCohesion[] _panels, string[] _buttanNames)
    {
        horizontal_panels = _panels;
        slot_script = _slots;
        for(int ii = 0; ii < _slots.Length; ii++)
        {

            // スロットに番号を振り分けておく
            _slots[ii].slotNo = ii;

            // ラムダ式でボタンスロットをリスナーに登録しておく
            int _index = ii;
            _slots[ii].button.onClick.AddListener(() => HorizontalButtonClick_Proc(_index));

        }

        for(int qq = 0; qq < _panels[0].buttonName.Length; qq++)
        {
            _panels[0].buttonName[qq].text = _buttanNames[qq];
        }
    }

    public void HorizontalButtonClick_Proc(int _slotnum)
    {
        SoundManager.instance.PlayAudio("ButtonClick");
        for(int ii = 0; ii < slot_script.Length; ii++)
        {
            if(slot_script[ii].gameObject.activeSelf == false) continue;

            if(ii == _slotnum)
            {
                // "横"ボタンを押下したボタンに合わせてパネルを表示する。
                horizontal_panels[0].horizontal_panels[_slotnum].gameObject.SetActive(true);
            }
            else
            {
                if(ii < horizontal_panels[0].horizontal_panels.Length)
                {
                    // 他のパネルを非アクティブ状態にする
                    horizontal_panels[0].horizontal_panels[ii].gameObject.SetActive(false);
                }
            }
        }
    }
}