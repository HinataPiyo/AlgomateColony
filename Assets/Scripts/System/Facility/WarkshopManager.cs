using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarkshopManager : MonoBehaviour
{
    [Header("戻るボタン")]
    [SerializeField] Button back_button;

    [Header("パネル")]
    [SerializeField] GameObject processing_panel;
    [SerializeField] GameObject accessory_panel;

    [Header("パネルを切り替えるボタン")]
    [SerializeField] Transform changePanel_parent;
    ButtonSlotVarticalHorizontal[] changePanel_slot;

    [Header("倉庫リスト")] 
    [SerializeField] List<WarehouseSO.MATERIAL_WAREHOUSE_SLOT> wlist;

    private void Awake() {
     WarehouseController wc = GetComponent<WarehouseController>();
        wlist = wc.GetWarehouseSO().GetMaterial_WarehouseList();   
    }
    private void Start()
    {

        back_button.onClick.AddListener(ButtonOnClick_Back);

        changePanel_slot = changePanel_parent.GetComponentsInChildren<ButtonSlotVarticalHorizontal>();

        for(int pp = 0; pp < changePanel_slot.Length; pp++)
        {
            changePanel_slot[pp].slotNo = pp;
            changePanel_slot[pp].Initialize_Warkshop(this);

            switch(pp)
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

    public List<WarehouseSO.MATERIAL_WAREHOUSE_SLOT> GetWarehouseList() { return wlist; }
}