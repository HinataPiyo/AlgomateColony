using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 倉庫の管理を行うクラス
/// 素材やアクセサリーのスロット管理、UI更新、アイテムの追加・消費を行う
/// </summary>
public class WarehouseController : MonoBehaviour
{
    UpdateTime_Class updateTime = new UpdateTime_Class();

    [SerializeField] WarehouseSO warehouseSO; // 倉庫データ
    FacilityManager fm; // ファシリティマネージャー
    List<WarehouseSO.MATERIAL_WAREHOUSE_SLOT> wlist; // 素材倉庫リスト
    List<AccessoryData> acceData_list; // アクセサリー倉庫リスト

    [Header("戻るボタン")]
    [SerializeField] Button back_button;

    [Header("メインキャンバスに表示するスロット")]
    [SerializeField] GameObject slot_prefab;
    [SerializeField] Transform maincanvas_parent;
    [SerializeField] List<HaveMaterialSlot> maincanvas_wlistSlots = new List<HaveMaterialSlot>();

    [Header("パネルの高さ設定")]
    [SerializeField] RectTransform panel_rect;
    [SerializeField] int hieght_panel;

    [Header("倉庫内のスロット/素材")]
    [SerializeField] Transform warehouseSlot_parent;
    WarehouseSlot[] warehouseSlots;

    [Header("倉庫内のスロット/アクセサリー")]
    [SerializeField] Transform warehouse_AcceParent;
    WarehouseSlot[] slot_acce;

    [Header("パネル変更ボタン")]
    [SerializeField] Transform changeButton_parent;
    ButtonSlotVarticalHorizontal[] changeButtons;
    [SerializeField] GameObject[] changePael;

    /// <summary>
    /// 初期化処理
    /// </summary>
    void Start()
    {
        // コンポーネントの取得
        fm = GetComponent<FacilityManager>();
        wlist = warehouseSO.GetMaterial_WarehouseList();
        acceData_list = warehouseSO.GetAccessory_WarehouseList();
        warehouseSlots = warehouseSlot_parent.GetComponentsInChildren<WarehouseSlot>();
        slot_acce = warehouse_AcceParent.GetComponentsInChildren<WarehouseSlot>();
        changeButtons = changeButton_parent.GetComponentsInChildren<ButtonSlotVarticalHorizontal>();

        // ボタンのイベント登録
        back_button.onClick.AddListener(ButtonOnClick_Back);

        // 倉庫内を初期化
        SetSlot_MaterialInventory();

        // パネル変更ボタンの設定
        for (int pp = 0; pp < changeButtons.Length; pp++)
        {
            changeButtons[pp].slotNo = pp;
            changeButtons[pp].Initialize_Warehouse(this);

            // ボタン名を設定
            changeButtons[pp].button_name.text = pp == 0 ? "素材" : "アクセサリー";
        }

        // 初期パネル設定
        changePael[0].SetActive(true);
        changePael[1].SetActive(false);
    }

    /// <summary>
    /// フレームごとの更新処理
    /// </summary>
    void Update()
    {
        if (updateTime.UpdateTime())
        {
            // 倉庫内を更新
            SetSlot_MaterialInventory();
            SetSlot_AccessoryInventory();

            // メインキャンバスのスロットを生成
            CreatSlot_MainCanvas_HaveMaterial();
        }
    }

    /// <summary>
    /// メインキャンバスに表示するスロットを生成
    /// </summary>
    void CreatSlot_MainCanvas_HaveMaterial()
    {
        // スロット数の差分を計算し、必要に応じて生成または削除
        int diff = wlist.Count - maincanvas_wlistSlots.Count;

        if (diff > 0)
        {
            for (int ii = 0; ii < diff; ii++)
            {
                GameObject slot_clone = Instantiate(slot_prefab, maincanvas_parent);
                HaveMaterialSlot haveSlot = slot_clone.GetComponent<HaveMaterialSlot>();
                maincanvas_wlistSlots.Add(haveSlot);
                SetHeight(hieght_panel);
            }
        }
        else if (diff < 0)
        {
            for (int ii = maincanvas_wlistSlots.Count - 1; ii >= wlist.Count; ii--)
            {
                Destroy(maincanvas_wlistSlots[ii].gameObject);
                maincanvas_wlistSlots.RemoveAt(ii);
                SetHeight(-hieght_panel);
            }
        }

        // 倉庫内の素材をスロットに反映
        Check_WarehouseMaterialAmount();
    }

    #region 素材倉庫の処理

    /// <summary>
    /// 素材倉庫を更新
    /// </summary>
    void SetSlot_MaterialInventory()
    {
        // スロットをクリア
        foreach (var _slot in warehouseSlots)
        {
            if (_slot.GetMaterialSO() != null)
                _slot.ClearSlot();
        }

        // リストとスロットを同期
        for (int ii = 0; ii < warehouseSlots.Length; ii++)
        {
            if (ii < wlist.Count)
            {
                if (warehouseSlots[ii].GetMaterialSO() == null)
                    warehouseSlots[ii].AddMaterialToSlot(wlist[ii]);

                if (warehouseSlots[ii].GetHaveAmount() == 0)
                    wlist.RemoveAt(ii);
            }
        }
    }

    /// <summary>
    /// 倉庫内の素材をスロットに反映
    /// </summary>
    void Check_WarehouseMaterialAmount()
    {
        for (int ii = 0; ii < maincanvas_wlistSlots.Count; ii++)
        {
            maincanvas_wlistSlots[ii].ClearSlot();
            if (ii < wlist.Count)
                maincanvas_wlistSlots[ii].SetHaveMaterial(wlist[ii].mateSO, wlist[ii].mateAmount);
        }
    }

    /// <summary>
    /// 素材を倉庫に追加
    /// </summary>
    public void SetMaterial_WarehouseSlot(MaterialSO _mateSO, int _amo)
    {
        WarehouseSO.MATERIAL_WAREHOUSE_SLOT warehouse_slot = new WarehouseSO.MATERIAL_WAREHOUSE_SLOT();
        warehouse_slot.mateSO = _mateSO;
        warehouse_slot.mateAmount = _amo;

        if (wlist.Count == 0)
        {
            wlist.Add(warehouse_slot);
            SetSlot_MaterialInventory();
            return;  // ← 関数を終了（ループの有無に関係なく）
        }

        for (int ii = 0; ii < wlist.Count; ii++)
        {
            if (wlist[ii].mateSO.serialNum == _mateSO.serialNum)
            {
                wlist[ii].mateAmount += _amo;
                SetSlot_MaterialInventory();
                return;  // ← 関数を終了（新規追加の処理に進まない）
            }
        }

        // ループを抜けた後、新規追加
        wlist.Add(warehouse_slot);
        SetSlot_MaterialInventory();
    }

    /// <summary>
    /// 素材を消費
    /// </summary>
    public void UseMaterial(MaterialSO _mateSO, int _useAmo)
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
                    LogController.instance.SetLog(null, $"素材が足りません。現在の所持数は {wlist[ii].mateAmount} です。");
                }
            }
        }
    }

    #endregion

    #region アクセサリー倉庫の処理

    /// <summary>
    /// アクセサリー倉庫を更新
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
    /// アクセサリーを倉庫に追加
    /// </summary>
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
    /// アクセサリーを使用
    /// </summary>
    public void UseAccessory()
    {
        // !   アクセサリーを選択したときの処理（倉庫のスロット・ロボットのスロット）
        for(int ii = 0; ii < acceData_list.Count; ii++)
        {
        }
    }

    #endregion

    /// <summary>
    /// パネルの高さを変更
    /// </summary>
    void SetHeight(float newHeight)
    {
        // 現在のsizeDeltaの幅を保持して高さのみ変更
        Vector2 size = panel_rect.sizeDelta;
        size.y += newHeight;
        panel_rect.sizeDelta = size;
    }

    /// <summary>
    /// パネルを変更
    /// </summary>
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
    /// 戻るボタンの処理
    /// </summary>
    void ButtonOnClick_Back()
    {
        fm.CanvasEnabled(CanvasName.Warehouse, false);
    }

    /// <summary>
    /// 倉庫データを取得
    /// </summary>
    public WarehouseSO GetWarehouseSO()
    {
        return warehouseSO;
    }
}
