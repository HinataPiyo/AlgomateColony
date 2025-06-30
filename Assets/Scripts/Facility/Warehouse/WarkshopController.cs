using UnityEngine;
using UnityEngine.UI;

public class WarkshopController: MonoBehaviour
{
    [Header("戻るボタン")]
    [SerializeField] Button back_button;

    [Header("パネル")]
    [SerializeField] GameObject processing_panel;
    [SerializeField] GameObject accessory_panel;

    [Header("パネルを切り替えるボタン")]
    [SerializeField] Transform changePanel_parent;
    ButtonSlotVarticalHorizontal[] changePanel_slot;

    void Awake()
    {
        back_button.onClick.AddListener(ButtonOnClick_Back);

        changePanel_slot = changePanel_parent.GetComponentsInChildren<ButtonSlotVarticalHorizontal>();

    }
    
    void Start()
    {
        for (int pp = 0; pp < changePanel_slot.Length; pp++)
        {
            changePanel_slot[pp].slotNo = pp;
            changePanel_slot[pp].Initialize_Warkshop(this);

            switch (pp)
            {
                case 0:
                    changePanel_slot[pp].button_name.text = "材料";
                    break;
                case 1:
                    changePanel_slot[pp].button_name.text = "アクセサリー";
                    break;
            }
        }

        processing_panel.SetActive(true);
        accessory_panel.SetActive(false);
    }

    public void ChangePanel(int _num)
    {
        switch(_num)
        {
            case 0:
                processing_panel.SetActive(true);

                accessory_panel.SetActive(false);
                break;
            case 1:
                processing_panel.SetActive(false);

                accessory_panel.SetActive(true);
                break;
        }
    }

    /// <summary>
    /// Backボタンを押したときの処理
    /// </summary>
    void ButtonOnClick_Back()
    {
        FacilityManager.instance.CanvasEnabled(CanvasName.Warkshop, false);
    }

}