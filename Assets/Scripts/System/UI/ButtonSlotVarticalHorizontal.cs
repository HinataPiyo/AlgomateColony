using TMPro;
using UnityEngine;
using UnityEngine.UI;

// ボタンのBaseScript...だと思う
public class ButtonSlotVarticalHorizontal : MonoBehaviour
{
    public Button button;
    public int slotNo;
    public TextMeshProUGUI button_name;


    WarehouseController wareC;
    WarkshopManager warkC;
    GameSettingController gSettingCont;

    public void Initialize_Warehouse(WarehouseController _wareC)
    {
        wareC = _wareC;
        button.onClick.AddListener(OnClick_ChangePanel_Warehouse);
    }
    /// <summary>
    /// ProcessingControllerで行う初期化処理
    /// </summary>
    public void Initialize_Warkshop(WarkshopManager _warkC)
    {
        warkC = _warkC;
        button.onClick.AddListener(OnClick_ChangePanel_Warkshop);
    }

    public void Initialize_GameSetting(GameSettingController _gsCont)
    {
        gSettingCont = _gsCont;
        button.onClick.AddListener(OnClick_ChangePanel_GameSetting);
    }

    void OnClick_ChangePanel_Warehouse()
    {
        SoundManager.instance.PlayAudio("ButtonClick");
        wareC.ChangePanel(slotNo);
    }

    /// <summary>
    /// Warkshopのパネルを切り替える
    /// </summary>
    void OnClick_ChangePanel_Warkshop()
    {
        SoundManager.instance.PlayAudio("ButtonClick");
        warkC.ChangePanel(slotNo);
    }

    void OnClick_ChangePanel_GameSetting()
    {
        SoundManager.instance.PlayAudio("ButtonClick");
        gSettingCont.ChangePanel(slotNo);
    }
}