using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarehouseController : MonoBehaviour
{
    [SerializeField] WarehouseSO warehouseSO;
    FacilityManager fm;
    List<WarehouseSO.BASE_WAREHOUSE_SLOT> wlist;

    [SerializeField] Button back_button;

    [SerializeField] Transform maincanvas_warehouse;
    HaveMaterialSlot[] haveMateSlots;
    [SerializeField] Transform warehouseSlot_parent;
    WarehouseSlot[] warehouseSlots;

    UpdateTime_Class updateTime = new UpdateTime_Class();
    void Start() {
        // コンポーネントの取得
        fm = GetComponent<FacilityManager>();
        wlist = warehouseSO.GetBaseWarehouseSlot_List();
        haveMateSlots = maincanvas_warehouse.GetComponentsInChildren<HaveMaterialSlot>();
        warehouseSlots = warehouseSlot_parent.GetComponentsInChildren<WarehouseSlot>();

        back_button.onClick.AddListener(ButtonOnClick_Back);

        SetSlot_WarehouseInventory();       // 倉庫内を更新する
    }

    void Update() {
        if(updateTime.UpdateTime() == true)
        {
            Check_WarehouseMaterialAmount();    // 倉庫内の素材を確認する
            SetSlot_WarehouseInventory();       // テスト
        }
    }

    /// <summary>
    /// 倉庫内を更新する
    /// </summary>
    void SetSlot_WarehouseInventory()
    {
        foreach(var _slot in warehouseSlots)
        {
            if(_slot.GetMaterialSO() == null) continue;
            else _slot.ClearSlot();
        }

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
        for(int ii = 0; ii < wlist.Count; ii++)
        {
            switch(wlist[ii].mateSO.serialNum)
            {
                // テキストの順番は固定なので番号を振ってあげる
                case 1:     // 石
                    haveMateSlots[0].SetHaveMaterial(wlist[ii].mateAmount);
                    break;
                case 2:     // 木
                    haveMateSlots[1].SetHaveMaterial(wlist[ii].mateAmount);
                    break;
            }
        }
    }

    /// <summary>
    /// 倉庫に素材を入れる。また、シリアル番号が同一だった加算する
    /// </summary>
    /// <param name="_mateSO"></param>
    /// <param name="_amo"></param>
    public void SetWarehouseSlot(MaterialSO _mateSO, uint _amo)
    {
        WarehouseSO.BASE_WAREHOUSE_SLOT warehouse_slot = new WarehouseSO.BASE_WAREHOUSE_SLOT();

        // 構造体に値を設定
        warehouse_slot.mateSO = _mateSO;
        warehouse_slot.mateAmount = _amo;

        for(int ii = 0; ii < wlist.Count; ii++)
        {
            // 追加しようとしている素材のシリアル番号と既にある素材のシリアル番号が一致しているか調べる
            if(wlist[ii].mateSO.serialNum == _mateSO.serialNum)
            {
                wlist[ii].mateAmount += _amo;
                SetSlot_WarehouseInventory();       // 倉庫内を更新する
                break;
            }
            // シリアル番号が一致していなければ　かつ　最後までスロットを調べた後
            else if(wlist[ii].mateSO.serialNum != _mateSO.serialNum
            && ii == wlist.Count - 1)
            {
                // 新規なのでリストに追加してあげる
                wlist.Add(warehouse_slot);
                SetSlot_WarehouseInventory();       // 倉庫内を更新する
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

    /// <summary>
    /// Backボタンを押したときの処理
    /// </summary>
    void ButtonOnClick_Back()
    {
        fm.CanvasEnabled(CanvasName.Warehouse, false);
    }

    public WarehouseSO GetWarehouseSO() { return warehouseSO;}
}
