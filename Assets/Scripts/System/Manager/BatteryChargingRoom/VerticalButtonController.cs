using UnityEngine;

public class VerticalButtonController : MonoBehaviour
{
    HorizontalButtonController hbCont;
    ButtonSlotVarticalHorizontal[] slot_script;
    GameObject[] vertical_panel;
    [HideInInspector] public int current_verticalpanel_num;
    private void Start() {
        hbCont = GetComponent<HorizontalButtonController>();
        current_verticalpanel_num = -1;
    }

    public void Set_VarticalButton(ButtonSlotVarticalHorizontal[] _slots, GameObject[] _panels)
    {
        vertical_panel = _panels;
        slot_script = _slots;
        for(int ii = 0; ii < _slots.Length; ii++)
        {
            // スロットに番号を振り分けておく
            _slots[ii].slotNo = ii;

            int _index = ii;
            _slots[ii].button.onClick.AddListener(() => ButtonClick_Proc(_index));

            // ボタンのテキストを設定する
            _slots[ii].button_name.text = SettingText(ii);       // 順番に" SettingText "で返されたテキストを入力していく

            // テキストが空のスロットを非アクティブ状態にする
            if(_slots[ii].button_name.text == null)
            {
                _slots[ii].gameObject.SetActive(false);
            }
        }
    }


    /// <summary>
    /// 最初のボタンの設定
    /// </summary>
    string SettingText(int _index)
    {
        switch(_index)
        {
            case 0:
                return "スロット0";
            case 1:
                return "スロット1";
        }

        return null;
    }

    /// <summary>
    /// ボタンが押された時の処理
    /// パネルを非表示・表示するメソッド
    /// </summary>
    public void ButtonClick_Proc(int _num)
    {
        for(int ii = 0; ii < slot_script.Length; ii++)
        {
            // 非アクティブ状態のオブジェはスルー
            if(slot_script[ii].gameObject.activeSelf == false) continue;

            // ボタンの番号と入力された番号が一致していれば
            if(ii == _num)
            {
                // 入力されたボタンに合ったパネルをアクティブ状態にする
                vertical_panel[ii].gameObject.SetActive(true);
                current_verticalpanel_num = ii;
                hbCont.VerticalButtonClick_SetSlot(_num);
            }
            // 一致していなければ
            else
            {
                // パネルを非アクティブ状態にする
                vertical_panel[ii].gameObject.SetActive(false);
            }
        }
    }
}