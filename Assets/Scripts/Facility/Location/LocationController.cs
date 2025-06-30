using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LocationController : MonoBehaviour
{
    // スロットの行数を管理するenum
    public enum SlotLine
    {
        OneLine,
        TwoLine,
        ThreeLine,
        FourLine,
    }

    // スロットが増えたときにパネルの縦幅を大きくする値を設定しておく
    const int oneHight = 260, twoHight = 340, threeHight = 420, fourHight = 500;
    [Header("非素材のスロットの行数"), SerializeField] SlotLine current_slotline;

    FacilityManager fm;         // 施設のマネージャースクリプト
    WarehouseController wc;     // 倉庫のスクリプト
    SystemControlSO scSO;       // ゲーム進行を管理するSO
    NextLevelUnlockedSO nextUnlockSO;
    UpdateTime_Class updateTime = new UpdateTime_Class();
    

    [Header("Locationパネルの設定")]
    [SerializeField] RectTransform location_panel;      // 本体のパネル
    [SerializeField] Button locationLevelUp_button;     // レベルアップボタン
    [SerializeField] Button backButton;                 // 戻るボタン
    [SerializeField] TextMeshProUGUI locationLevel_text;    // レベルアップテキスト（押下できるかできないかを切り替える）

    [Header("必要素材")]
    NeedMaterialSO.NEED_MATERIAL_ROOT needmate_root;        // 必要素材のリストの宣言
    [SerializeField] Transform materialSlot_parent;         // LocationMaterialSlotの親のTransform
    LocationMaterialSlot[] mateSlots;      // 素材を表示させるスロット
    [Header("必要素材をまとめて格納してあるSO"), SerializeField] NeedMaterialSO needMateSO;
    int oldlevel = -1;
    bool OverSet_MaterialList;      // true : 必要素材リストを超えた,false : まだ超えていない

    [Space(20.0f),Header("スロット ・ テキストの設定")]
    [SerializeField] GameObject unlockSlot_obj;         // アンロックスロットの本体
    [SerializeField] TextMeshProUGUI allUnlocked_text;  // 「Allunlock」と書かれたテキスト
    [SerializeField] Image icon;                        // アンロックスロットのアイコン
    [SerializeField] TextMeshProUGUI objname_text;      // オブジェクトの名前
    [SerializeField] TextMeshProUGUI exp_text;          // 説明文
    [SerializeField] StatusLimitedTexts[] slTexts;      // ステータス上限突破のテキストと値のストラクト

    // ステータス上限突破のテキストと値の
    [System.Serializable]
    struct StatusLimitedTexts
    {
        public TextMeshProUGUI statusName_texts;
        public TextMeshProUGUI statusValue_value;
    }

    [Header("倉庫リスト")] 
    List<WarehouseSO.MATERIAL_WAREHOUSE_SLOT> wlist;

    

    void Start()
    {
        // コンポーネントの取得
        fm = GetComponent<FacilityManager>();
        wc = GetComponent<WarehouseController>();
        mateSlots = materialSlot_parent.GetComponentsInChildren<LocationMaterialSlot>();
        scSO = GameManager.instance.GetSystemControlSO();
        nextUnlockSO = scSO.GetNextLevelUnlockedSO();
        wlist = wc.GetWarehouseSO().GetMaterial_WarehouseList();

        // 最初に行う処理
        CheckSet_NeedMaterial(scSO.GetLocationLevel);     // 現在の必要個数を所持数と比べる
        locationLevelUp_button.interactable = false;        // ボタンの押下を出来ないようにする
        
        
        // リスナー登録
        locationLevelUp_button.onClick.AddListener(BottonOnClick_LocationLevelUp);
        backButton.onClick.AddListener(ButtonOnClick_Back);

        GameManager.instance.Set_PlayerName_LocationLevel(scSO.playerName, scSO.GetLocationLevel);
    }

    private void Update() {
        if(updateTime.UpdateTime() == true)
        {
            Check_CompletionAllMaterials();                         // 必要素材がそろっているか確認する
            CheckSet_NeedMaterial(scSO.GetLocationLevel);         // LocationLevelに応じて必要素材を変える
            Sync_HaveMaterialToText();                              // 素材の所持数を必要素材のテキストに反映させる
        }
    }

    /// <summary>
    /// 拠点のレベルに合わせて素材を変える
    /// </summary>
    void CheckSet_NeedMaterial(int location_level)
    {
        // １秒間に１回更新されるようにする
        if(Check_ChangeLocationLevel(location_level) == true)
        {
            locationLevel_text.text = "" + location_level;      // テキストのレベル表示を更新する
            // 左上のPlayerNameとLocationLevelを設定する
            GameManager.instance.Set_PlayerName_LocationLevel(scSO.playerName, scSO.GetLocationLevel);
            SetNeedMate(location_level);                        // 必要個数をスロットに設定する
            TextSet_NextLevelUnlock(location_level);            // 次のレベルでｒ
            oldlevel = location_level;
        }
    }

#region 必要素材をスロットに適応させる処理
    

    /// <summary>
    /// スロットの数に応じてパネルの縦幅を変える
    /// </summary>
    /// <param name="slotLine"></param>
    public void SetLocationHight(SlotLine slotLine)
    {
        current_slotline = slotLine;
        switch(current_slotline)
        {
            case SlotLine.OneLine:
                SetHeight(oneHight);
                break;
            case SlotLine.TwoLine:
                SetHeight(twoHight);
                break;
            case SlotLine.ThreeLine:
                SetHeight(threeHight);
                break;
            case SlotLine.FourLine:
                SetHeight(fourHight);
                break;
        }
    }

    /// <summary>
    /// パネルの高さの幅を変える
    /// </summary>
    /// <param name="newHeight"></param>
    void SetHeight(float newHeight)
    {
        // 現在のsizeDeltaの幅を保持して高さのみ変更
        Vector2 size = location_panel.sizeDelta;
        size.y = newHeight;
        location_panel.sizeDelta = size;
    }

    void SetNeedMate(int location_level)
    {
        // スロットの初期化
        foreach(var _slot in mateSlots)
        {
            Debug.Log("初期化されました");
            _slot.SetSlotMaterial(null, 0);
        }
        
        // numがリストのカウント数より小さければ
        if(location_level < needMateSO.need_mate_root.Count)
        {
            OverSet_MaterialList = false;
            needmate_root = needMateSO.need_mate_root[location_level];
        }
        else
        {
            Check_SlotInMaterial();
            // 必要素材を設定していないのにボタンを押されるのを防ぐ
            OverSet_MaterialList = true;
            Debug.Log($"拠点のレベルが素材リストのカウント数を超えました");
            return;
        }
        
        int maxneed_mate = needmate_root.need_materials.Length;

        for(int ii = 0; ii < maxneed_mate; ii++)
        {
            mateSlots[ii].SetSlotMaterial(
                needmate_root.need_materials[ii].mateSO, 
                needmate_root.need_materials[ii].needAmo
            );
        }

        Check_SlotInMaterial();
    }

    /// <summary>
    /// 拠点のレベルが変更されたか調べる
    /// </summary>
    /// <param name="num"></param>
    bool Check_ChangeLocationLevel(int num)
    {
        if(oldlevel != num)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// スロットの中に素材があるかどうか調べる
    /// </summary>
    void Check_SlotInMaterial()
    {
        int amo = mateSlots.Length;
        foreach(var _mateslot in mateSlots)
        {
            if(_mateslot.GetMaterialSO() == null)
            {
                _mateslot.gameObject.SetActive(false);
                amo--;
            }
            else
            {
                _mateslot.gameObject.SetActive(true);
            }
        }

        // スロットの数に応じてパネルの縦幅を変える
        if(amo <= 2) SetLocationHight(SlotLine.OneLine);
        if(amo > 2 && amo <= 4) SetLocationHight(SlotLine.TwoLine);
        if(amo > 4 && amo <= 6) SetLocationHight(SlotLine.ThreeLine);
        if(amo > 6 && amo <= 8) SetLocationHight(SlotLine.FourLine);
    }
#endregion


#region アンロックの表示処理
    /// <summary>
    /// Unlock画面のテキストやスロット設定
    /// </summary>
    void TextSet_NextLevelUnlock(int location_level)
    {
        BASE_NEXT_UNLOCK base_next_unlock;
        List<BASE_NEXT_UNLOCK> nextlevel_list = scSO.GetNextLevelUnlockedSO().GetBaseNextUnlocks_List();

        if(location_level < nextlevel_list.Count)
        {
            base_next_unlock = nextlevel_list[location_level];
        }
        else
        {
            // スロットのアクティブ状態を設定する
            UnlockSlot_Active(false);

            // 上限突破ステータステキストも非アクティブ状態にする
            foreach(var texts in slTexts)
            {
                texts.statusName_texts.gameObject.SetActive(false);
                texts.statusValue_value.gameObject.SetActive(false);
            }

            Debug.Log($"拠点のレベルが上限突破ステータスリストのカウント数を超えました");
            return;
        }

        
        // Unlockするためのオブジェクトのスクリプトを取得する
        BASE_NEXT_UNLOCK _baseNextUnlock = nextUnlockSO.GetBaseNextUnlocks_List()[location_level];
        BASE_NEXT_UNLOCK.StatusParam[] statusParam = base_next_unlock.statusParam;

        if(_baseNextUnlock != null)
        {
            // スロットのアクティブ状態を設定する
            UnlockSlot_Active(true);

            // スロットやテキストを設定する
            icon.sprite = _baseNextUnlock.icon;
            objname_text.text = _baseNextUnlock.name_text;
            exp_text.text = _baseNextUnlock.exp_text;
        }
        else
        {
            // スロットのアクティブ状態を設定する
            UnlockSlot_Active(false);
        }

        for(int ii = 0; ii < slTexts.Length; ii++)
        {
            // ステータスの上限突破のテキストを設定する
            if(ii < statusParam.Length)
            {
                // 中身が空っぽだったらテキストをアクティブ状態にする
                slTexts[ii].statusName_texts.gameObject.SetActive(true);
                slTexts[ii].statusValue_value.gameObject.SetActive(true);

                // enumの名前をswitchで日本語に変換する
                slTexts[ii].statusName_texts.text = $"{scSO.StatusSelectName(statusParam[ii].selectStatus)}";
                slTexts[ii].statusValue_value.text = "+" + statusParam[ii].statusLimited_value;
            }
            else    // 中身が空っぽだったらテキストを非アクティブ状態にする
            {
                slTexts[ii].statusName_texts.gameObject.SetActive(false);
                slTexts[ii].statusValue_value.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// アンロックスロットのアクティブ状態を設定する
    /// </summary>
    /// <param name="flag">
    /// true : icon, name, exp をアクティブ状態, allUnlocked_text を非アクティブ状態
    /// false : icon, name, exp を非アクティブ状態, allUnlocked_text をアクティブ状態
    /// </param>
    void UnlockSlot_Active(bool flag)
    {
        switch(flag)
        {
            case false:
                unlockSlot_obj.SetActive(false);
                allUnlocked_text.gameObject.SetActive(true);
                break;
            case true:
                unlockSlot_obj.SetActive(true);
                allUnlocked_text.gameObject.SetActive(false);
                break;
        }
    }

#endregion

    /// <summary>
    /// 全てのスロットにある素材の必要個数が所持数より上回っているか確かめる
    /// </summary>
    void Check_CompletionAllMaterials()
    {
        bool check_needAmoOverFlag = true;
        foreach(var _slot in mateSlots)
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
            locationLevelUp_button.interactable = true;
            TutorialController.insrance.TutorialCheck(2, 1);
        }
        else if(check_needAmoOverFlag == false && OverSet_MaterialList == true)
        {
            locationLevelUp_button.interactable = false;
        }
    }
    /// <summary>
    /// Locationのレベルを上げるボタンを押したときの処理
    /// </summary>
    void BottonOnClick_LocationLevelUp()
    {
        // 連続でボタンを押されるのを防ぐ
        locationLevelUp_button.interactable = false;
        TutorialController.insrance.TutorialCheck(2, 2);
        TutorialController.insrance.BigTaskCheck(2);
        
        // 倉庫の所持数を必要個数分だけ減らす
        for(int ii = 0; ii < needmate_root.need_materials.Length; ii++)
        {
            wc.UseMaterial(
                needmate_root.need_materials[ii].mateSO,
                needmate_root.need_materials[ii].needAmo
            );
        }
        
        // レベルアップ処理
        scSO.LocationLevelUp();

        // LocationLevelに応じて必要素材を変える
        CheckSet_NeedMaterial(scSO.GetLocationLevel);

        // テキストの反映
        Sync_HaveMaterialToText();

        SoundManager.instance.PlayAudio("LevelUp");
    }

    /// <summary>
    /// 素材の所持数を必要素材のテキストに反映させる
    /// </summary>
    void Sync_HaveMaterialToText()
    {
        for(int ii = 0; ii < mateSlots.Length; ii++)
        {
            for(int qq = 0; qq < wlist.Count; qq++)
            {
                // 必要素材と倉庫の素材のシリアル番号が同一だった場合
                if(mateSlots[ii].GetMaterialSO()?.serialNum == wlist[qq].mateSO.serialNum)
                {
                    // 素材の所持数を反映させる
                    mateSlots[ii].SetStockAmount(wlist[ii].mateAmount);
                }
            }
        }
    }

    /// <summary>
    /// Backボタンを押したときの処理
    /// </summary>
    void ButtonOnClick_Back()
    {
        fm.CanvasEnabled(CanvasName.Location, false);
    }
}
