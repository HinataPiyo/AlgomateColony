using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class ProcessingController : MonoBehaviour
{
    WarkshopManager warkManager;
    UpdateTime_Class updateTime = new UpdateTime_Class();
    
    
    [Header("加工品を選択するスロット")]
    [SerializeField] GameObject warkshopSlot_prefab;      // 設定されているアクセサリーの量分生成するため
    [SerializeField] Transform warkshopSlot_parent;       // Prefabを生成する為の親オブジェクトとなる（スクロールバー）
    [SerializeField] List<WarkshopSlot> processingSlots_list = new List<WarkshopSlot>();
    [Space(10), Header("選択したときのスロット（Infoパネル）")]
    [SerializeField] AccessorySO processingSO;
    [SerializeField] Image select_icon;
    [SerializeField] TextMeshProUGUI selectName_text;
    [SerializeField] TextMeshProUGUI selectExp_text;

    [Space(10), Header("必要素材のスロット")]
    [SerializeField] Transform needMate_parent;
    [SerializeField] WarkshopNeedMaterialSlot[] needMate_slots;
    bool OverSet_MaterialList;      // true : 必要素材リストを超えた,false : まだ超えていない



    [Header("作成ボタン")]
    [SerializeField] Button creat_button;
    // sin は -1 ~ 1 の間
    // cos は -1 ~ 1 の間
    // tan は -∞ ~ ∞の間
    // ラジアン(角度) を渡すと比率を返してくれる->シータΘ

    void Start()
    {
        select_icon.enabled = false;
        selectName_text.text = "";
        selectExp_text.text = "";

        warkManager = GetComponent<WarkshopManager>();
        needMate_slots = needMate_parent.GetComponentsInChildren<WarkshopNeedMaterialSlot>();

        // リストに作成している加工品分回す
        for(int ii = 0; ii < processingSO.processing_status.Length; ii++)
        {
            // 加工品のスロットを作成
            GameObject _slot = Instantiate(warkshopSlot_prefab, warkshopSlot_parent);
            // スロットに番号を設定
            WarkshopSlot warkshop_cs = _slot.GetComponent<WarkshopSlot>();
            warkshop_cs.SetProcessing_NumAndScript(this, processingSO.processing_status[ii]);
            // リストに追加
            processingSlots_list.Add(warkshop_cs);
        }

        // 必要素材の表示するスロットを非アクティブ状態にする
        for(int ii = 0; ii < needMate_slots.Length; ii++)
        {
            needMate_slots[ii].SetSlotMaterial(null,0);
            needMate_slots[ii].gameObject.SetActive(false);
        }

        // 最初は0番目の加工品を表示する
        SetProcessing_SelectsButton(processingSO.processing_status[0]);
        Sync_HaveMaterialToText();
    }

    void Update()
    {
        if(updateTime.UpdateTime() == true)
        {
            // 必要素材がそろっているか確認する
            Check_CompletionAllMaterials();
            Sync_HaveMaterialToText();
        }        
    }


    /// <summary>
    /// ボタンが押された時にInfoパネルの設定を行う
    /// </summary>
    /// <param name="_selectNumber"></param>
    public void SetProcessing_SelectsButton(AccessorySO.PROCESSING_STATUS _processingSO)
    {
        // Infoパネルの設定
        select_icon.enabled = true;
        select_icon.sprite = _processingSO.mateSO.icon;
        selectName_text.text = _processingSO.mateSO.materialName;
        selectExp_text.text = _processingSO.mateSO.exp;


        // 必要素材スロットの表示
        for(int ii = 0; ii < needMate_slots.Length; ii++)
        {
            needMate_slots[ii].gameObject.SetActive(true);

            // 上記で設定されたアクセサリーのステータスに入ってある
            // 必要素材の配列分forを回す
            if(ii < _processingSO.need_mate_list.Length)
            {
                // 必要素材スロット(個々)
                needMate_slots[ii].SetSlotMaterial(
                    _processingSO.need_mate_list[ii].mateSO,   // 素材のデータ
                    _processingSO.need_mate_list[ii].needAmo   // 必要個数
                );
            }
            else
            {
                needMate_slots[ii].SetSlotMaterial(null,0);
            }
        }
    }

    /// <summary>
    /// 全てのスロットにある素材の必要個数が所持数より上回っているか確かめる
    /// </summary>
    void Check_CompletionAllMaterials()
    {
        bool check_needAmoOverFlag = true;
        foreach(var _slot in needMate_slots)
        {
            // スロットのどれかが必要個数より下回っていた場合ループを抜ける
            if(_slot.Check_OverNeedAmo() == false)
            {
                check_needAmoOverFlag = false;
            }
        }

        // 全てのスロットの必要個数より所持数のほうが多かった場合
        // かつ　必要素材のリストのカウントがレベルより下回っていない場合
        if(check_needAmoOverFlag == true && OverSet_MaterialList == false)
        {
            // レベルアップボタンを押せるようにする
            creat_button.interactable = true;
        }
        else if(check_needAmoOverFlag == false && OverSet_MaterialList == true)
        {
            creat_button.interactable = false;
        }
    }

    /// <summary>
    /// 素材の所持数を必要素材のテキストに反映させる
    /// </summary>
    void Sync_HaveMaterialToText()
    {
        for(int ii = 0; ii < needMate_slots.Length; ii++)
        {
            for(int qq = 0; qq < warkManager.GetWarehouseList().Count; qq++)
            {
                // 必要素材と倉庫の素材のシリアル番号が同一だった場合
                if(needMate_slots[ii].GetMaterialSO()?.serialNum == warkManager.GetWarehouseList()[qq].mateSO.serialNum)
                {
                    // 素材の所持数を反映させる
                    needMate_slots[ii].SetStockAmount(warkManager.GetWarehouseList()[ii].mateAmount);
                    break;
                }
            }
        }
    }


    public void Interactable_CreatButton(bool flag) { creat_button.interactable = flag; }
}
