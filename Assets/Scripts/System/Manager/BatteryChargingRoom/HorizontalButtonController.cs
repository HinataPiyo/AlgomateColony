using Unity.VisualScripting;
using UnityEngine;

public class HorizontalButtonController : MonoBehaviour
{
    ButtonSlotVarticalHorizontal[] slot_script;
    BatteryChargingRoomManager.HorizontalButtonCohesion[] horizontal_panels;
    VerticalButtonController vbCont;
    
    private void Start() {
        vbCont = GetComponent<VerticalButtonController>();
    }

    public void Set_HorizontalButton(ButtonSlotVarticalHorizontal[] _slots,
    BatteryChargingRoomManager.HorizontalButtonCohesion[] _panels)
    {
        horizontal_panels = _panels;
        slot_script = _slots;
        for(int ii = 0; ii < _slots.Length; ii++)
        {
            // スロットに番号を振り分けておく
            _slots[ii].slotNo = ii;

            // ラムダ式でボタンスロットをリスナーに登録しておく
            int _index = ii;
            _slots[ii].button.onClick.AddListener(() => HorizontalButtonClick_Proc(vbCont.current_verticalpanel_num, _index));
        }
    }


    /// <summary>
    /// "縦"ボタンが押された時の処理
    /// ボタンスロットのテキスト表示を変えるメソッド
    /// </summary>
    public void VerticalButtonClick_SetSlot(int _num)
    {
        for(int ii = 0; ii < slot_script.Length; ii++)
        {
            if(ii < horizontal_panels[_num].buttonName.Length)
            {
                slot_script[ii].gameObject.SetActive(true);                                 // 横ボタンをアクティブ状態にする
                horizontal_panels[_num].panels_paent.gameObject.SetActive(true);
                // ボタンのテキストを設定する（Inspectorで設定する）
                slot_script[ii].button_name.text = horizontal_panels[_num].buttonName[ii];
            }
            else
            {
                slot_script[ii].gameObject.SetActive(false);
            }


            // 親オブジェクトの非表示・表示を縦のボタンが押されたタイミングで処理する
            if(ii == _num)
            {
                HorizontalButtonClick_Proc(_num, 0);      // 最初の画面にする
            }
            else
            {
                if(ii < horizontal_panels.Length)
                {
                    // 親オブジェクトを非アクティブ状態にする
                    horizontal_panels[ii].panels_paent.gameObject.SetActive(false);
                }
            }
        }
    }


    public void HorizontalButtonClick_Proc(int _verticalbutton_num, int _slotnum)
    {
        SoundManager.instance.PlayAudio("ButtonClick");
        for(int ii = 0; ii < slot_script.Length; ii++)
        {
            if(slot_script[ii].gameObject.activeSelf == false) continue;

            if(ii == _slotnum)
            {
                // "横"ボタンを押下したボタンに合わせてパネルを表示する。
                horizontal_panels[_verticalbutton_num].horizontal_panels[_slotnum].gameObject.SetActive(true);
            }
            else
            {
                if(ii < horizontal_panels[_verticalbutton_num].horizontal_panels.Length)
                {
                    // 他のパネルを非アクティブ状態にする
                    horizontal_panels[_verticalbutton_num].horizontal_panels[ii].gameObject.SetActive(false);
                }
            }
        }
    }
}