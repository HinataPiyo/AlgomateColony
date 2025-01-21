using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WarehouseController : MonoBehaviour
{
    UpdateTime_Class updateTime = new UpdateTime_Class();
    
    [SerializeField] WarehouseSO warehouseSO;
    FacilityManager fm;
    List<WarehouseSO.MATERIAL_WAREHOUSE_SLOT> wlist;
    List<AccessoryData> acceData_list;

    [Header("戻る")]
    [SerializeField] Button back_button;

    [Header("MainCanvasに表示させるモノ")]
    [SerializeField] GameObject slot_prefab;
    [SerializeField] Transform maincanvas_parent;
    [SerializeField] List<HaveMaterialSlot> maincanvas_wlistSlots = new List<HaveMaterialSlot>();
    [Header("パネルの高さ")] 
    [SerializeField] RectTransform panel_rect;
    [SerializeField] int hieght_panel;

    [Header("倉庫内のスロット/素材")]
    [SerializeField] Transform warehouseSlot_parent;
    WarehouseSlot[] warehouseSlots;

    [Header("倉庫内のスロット / アクセサリー")]
    [SerializeField] Transform warehouse_AcceParent;
    WarehouseSlot[] slot_acce;

    [Header("パネルを変更するときのボタン")]
    [SerializeField] Transform changeButton_parent;
    ButtonSlotVarticalHorizontal[] changeButtons;
    [SerializeField] GameObject[] changePael;

    void Start() {
        // コンポーネントの取得
        fm = GetComponent<FacilityManager>();
        wlist = warehouseSO.GetMaterial_WarehouseList();
        acceData_list = warehouseSO.GetAccessory_WarehouseList();
        warehouseSlots = warehouseSlot_parent.GetComponentsInChildren<WarehouseSlot>();
        slot_acce = warehouse_AcceParent.GetComponentsInChildren<WarehouseSlot>();
        changeButtons = changeButton_parent.GetComponentsInChildren<ButtonSlotVarticalHorizontal>();

        back_button.onClick.AddListener(ButtonOnClick_Back);

        SetSlot_MaterialInventory();       // 倉庫内を更新する

        // パネルを変更するボタンの名前変更
        for(int pp = 0; pp < changeButtons.Length; pp++)
        {
            changeButtons[pp].slotNo = pp;
            changeButtons[pp].Initialize_Warehouse(this);

            switch(pp)
            {
                case 0:
                    changeButtons[pp].button_name.text = "素材";
                    break;
                case 1:
                    changeButtons[pp].button_name.text = "アクセサリー";
                    break;
            }
        }

        changePael[0].SetActive(true);
        changePael[1].SetActive(false);
    }

    void Update() {
        if(updateTime.UpdateTime() == true)
        {
            // テストらしい
            SetSlot_MaterialInventory();
            SetSlot_AccessoryInventory();

            CreatSlot_MainCanvas_HaveMaterial();    // スロットを生成する
        }
    }

    /// <summary>
    /// 倉庫内のアイテムが入ってるスロットの量に合わせて、スロットを生成、ListからRemoveを行う
    /// </summary>
    void CreatSlot_MainCanvas_HaveMaterial()
    {
        // メインキャンバスに表示されているスロットが倉庫内の所持してるスロットの数が小さければ
        if(maincanvas_wlistSlots.Count < wlist.Count)
        {
            // 差分を調べる
            int diff = wlist.Count - maincanvas_wlistSlots.Count;

            // その差分の分だけforを回す
            for(int ii = 0; ii < diff; ii++)
            {
                // Prefabを複製する
                GameObject slot_clone = Instantiate(slot_prefab, maincanvas_parent);
                HaveMaterialSlot haveSlot = slot_clone.GetComponent<HaveMaterialSlot>();
                maincanvas_wlistSlots.Add(haveSlot);
                SetHeight(hieght_panel);
            }
        }

        if(maincanvas_wlistSlots.Count > wlist.Count)
        {
            for(int ii = maincanvas_wlistSlots.Count - 1; ii >= wlist.Count; ii--)
            {
                Destroy(maincanvas_wlistSlots[ii].gameObject);
                maincanvas_wlistSlots.Remove(maincanvas_wlistSlots[ii]);
                SetHeight(- hieght_panel);
            }
        }
        Check_WarehouseMaterialAmount();        // アイテムをセットする
    }

#region 素材の倉庫の処理
    /// <summary>
    /// 倉庫内を更新する(素材倉庫)
    /// </summary>
    void SetSlot_MaterialInventory()
    {
        // 一度スロット全体をクリアな状態にする
        foreach(var _slot in warehouseSlots)
        {
            if(_slot.GetMaterialSO() == null) continue;
            else _slot.ClearSlot();
        }

        // SOのリストと倉庫のスロットを適合させる
        for (int ii = 0; ii < warehouseSlots.Length; ii++)
        {
            // リストの上限を超えないようにする
            if(ii < wlist.Count)
            {
                // スロットに素材が入っていなかったら
                if(warehouseSlots[ii].GetMaterialSO() == null)
                {
                    warehouseSlots[ii].AddMaterialToSlot(wlist[ii]);
                }

                if(warehouseSlots[ii].GetHaveAmount() == 0)
                {
                    // リストから除外する
                    wlist.Remove(wlist[ii]);
                }
            }
        }
    }

    /// <summary>
    /// 倉庫の素材を取得しテキストに反映する
    /// ※倉庫のスロットは同一の素材を2個以上存在させてはならない。
    /// 合算されないため
    /// </summary>
    void Check_WarehouseMaterialAmount()
    {
        // 生成されたスロット分forを回す
        for(int ii = 0; ii < maincanvas_wlistSlots.Count; ii++)
        {
            maincanvas_wlistSlots[ii].ClearSlot();      // 一度中身を空にする
            if(ii < wlist.Count)
            {
                maincanvas_wlistSlots[ii].SetHaveMaterial(wlist[ii].mateSO, wlist[ii].mateAmount);      // アイテムをセット
            }
        }
    }

    /// <summary>
    /// 倉庫に素材を入れる。また、シリアル番号が同一だった加算する
    /// </summary>
    /// <param name="_mateSO"></param>
    /// <param name="_amo"></param>
    public void SetMaterial_WarehouseSlot(MaterialSO _mateSO, uint _amo)
    {
        WarehouseSO.MATERIAL_WAREHOUSE_SLOT warehouse_slot = new WarehouseSO.MATERIAL_WAREHOUSE_SLOT();

        // 構造体に値を設定
        warehouse_slot.mateSO = _mateSO;
        warehouse_slot.mateAmount = _amo;

        for(int ii = 0; ii < wlist.Count; ii++)
        {
            // 追加しようとしている素材のシリアル番号と既にある素材のシリアル番号が一致しているか調べる
            if(wlist[ii].mateSO.serialNum == _mateSO.serialNum)
            {
                wlist[ii].mateAmount += _amo;
                SetSlot_MaterialInventory();       // 倉庫内を更新する
                break;
            }
            // シリアル番号が一致していなければ　かつ　最後までスロットを調べた後
            else if(wlist[ii].mateSO.serialNum != _mateSO.serialNum
            && ii == wlist.Count - 1)
            {
                // 新規なのでリストに追加してあげる
                wlist.Add(warehouse_slot);
                SetSlot_MaterialInventory();       // 倉庫内を更新する
                break;
            }
        }
    }

    /// <summary>
    /// 素材を消費する
    /// </summary>
    /// <param name="_mateSO"></param>
    /// <param name="_useAmo"></param>
    public void UseMaterial(MaterialSO _mateSO, uint _useAmo)
    {
        for(int ii = 0; ii < wlist.Count; ii++)
        {
            if(wlist[ii].mateSO.serialNum == _mateSO.serialNum)
            {
                // 使用量が所持量より大きければ
                if(wlist[ii].mateAmount >= _useAmo)
                {
                    // 素材を消費する
                    wlist[ii].mateAmount -= _useAmo;
                }
                else
                {
                    Debug.Log($"素材が足りません。現在の所持数は {wlist[ii].mateAmount} です。");
                }
            }
        }
    }
#endregion

#region アクセサリー
    /// <summary>
    /// 倉庫内を更新する(アクセサリー倉庫)
    /// </summary>
    void SetSlot_AccessoryInventory()
    {
        foreach(var _slot in slot_acce)
        {
            if(_slot.GetAccessoryData() == null) continue;
            else _slot.ClearSlot();
        }

        for (int ii = 0; ii < slot_acce.Length; ii++)
        {
            // リストの上限を超えないようにする
            if(ii < acceData_list.Count)
            {
                slot_acce[ii].AddAccessorySlot(acceData_list[ii]);

                if(slot_acce[ii].GetAccessoryData() == null)
                {
                    // リストから除外する
                    acceData_list.Remove(acceData_list[ii]);
                }
            }
        }
    }

    /// <summary>
    /// 倉庫にアクセサリーを入れる。
    /// </summary>
    /// <param name="_mateSO"></param>
    /// <param name="_amo"></param>
    public void SetAccessory_WarehouseSlot(AccessoryData _mateSO, uint _amo)
    {
        for(int ii = 0; ii < wlist.Count; ii++)
        {
            // 新規なのでリストに追加してあげる
            acceData_list.Add(_mateSO);

            // 倉庫内を更新する
            SetSlot_AccessoryInventory();
        }
    }

    /// <summary>
    /// アクセサリーを使用する
    /// </summary>
    /// <param name="_mateSO"></param>
    /// <param name="_useAmo"></param>
    public void UseAccessory()
    {
        // !   アクセサリーを選択したときの処理（倉庫のスロット・ロボットのスロット）
        for(int ii = 0; ii < acceData_list.Count; ii++)
        {
        }
    }

#endregion


    /// <summary>
    /// パネルの高さの幅を変える
    /// </summary>
    /// <param name="newHeight"></param>
    void SetHeight(float newHeight)
    {
        // 現在のsizeDeltaの幅を保持して高さのみ変更
        Vector2 size = panel_rect.sizeDelta;
        size.y += newHeight;
        panel_rect.sizeDelta = size;
    }

    /// <summary>
    /// パネルを変更するボタンを押したときの処理
    /// </summary>
    /// <param name="_num"></param>
    public void ChangePanel(int _num)
    {
        switch(_num)
        {
            case 0:
                changePael[0].SetActive(true);

                changePael[1].SetActive(false);
                break;
            case 1:
                changePael[0].SetActive(false);

                changePael[1].SetActive(true);
                break;
        }
    }

    /// <summary>
    /// Backボタンを押したときの処理
    /// </summary>
    void ButtonOnClick_Back()
    {
        fm.CanvasEnabled(CanvasName.Warehouse, false);
    }

    public WarehouseSO GetWarehouseSO() { return warehouseSO;}
}
