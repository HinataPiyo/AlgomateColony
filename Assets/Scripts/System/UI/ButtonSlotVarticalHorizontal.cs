using TMPro;
using UnityEngine;
using UnityEngine.UI;

// ボタンのBaseScript...だと思う
public class ButtonSlotVarticalHorizontal : MonoBehaviour
{
    public Button button;
    public int slotNo;
    public TextMeshProUGUI button_name;


    WarkshopManager wc;
    GameSettingController gSettingCont;
    /// <summary>
    /// ProcessingControllerで行う初期化処理
    /// </summary>
    public void Initialize_Warkshop(WarkshopManager _wc)
    {
        wc = _wc;
        button.onClick.AddListener(OnClick_ChangePanel_Warkshop);
    }

    public void Initialize_GameSetting(GameSettingController _gsCont)
    {
        gSettingCont = _gsCont;
        button.onClick.AddListener(OnClick_ChangePanel_GameSetting);
    }

    /// <summary>
    /// Warkshopのパネルを切り替える
    /// </summary>
    void OnClick_ChangePanel_Warkshop()
    {
        SoundManager.instance.PlayAudio("ButtonClick");
        wc.ChangePanel(slotNo);
    }

    void OnClick_ChangePanel_GameSetting()
    {
        SoundManager.instance.PlayAudio("ButtonClick");
        gSettingCont.ChangePanel(slotNo);
    }
}