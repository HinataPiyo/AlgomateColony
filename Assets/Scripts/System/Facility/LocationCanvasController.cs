using TMPro;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class LocationCanvasController : MonoBehaviour
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
    SystemControlSO scSO;       // ゲーム進行を管理するSO
    NextLevelUnlockedSO nextUnlockSO;
    

    [Header("Locationパネルの設定")]
    [SerializeField] RectTransform location_panel;
    [SerializeField] Button backButton;
    [SerializeField] TextMeshProUGUI locationLevel_text;

    [Header("必要素材")]
    [SerializeField] Transform materialSlot_parent;
    LocationMaterialSlot[] mateSlots;      // 素材を表示させるスロット
    [Header("必要素材をまとめて格納してあるSO"), SerializeField] NeedMaterialSO needMateSO;
    int oldlevel = -1;

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

    

    void Start()
    {
        // コンポーネントの取得
        fm = GetComponent<FacilityManager>();
        mateSlots = materialSlot_parent.GetComponentsInChildren<LocationMaterialSlot>();
        scSO = GameManager.instance.GetSystemControlSO();
        nextUnlockSO = scSO.GetNextLevelUnlockedSO();

        CheckSet_NeedMaterial(scSO.GetLocationLevel());
        
        // リスナー登録
        backButton.onClick.AddListener(BackButtonOnClick);
    }

    private void Update() {
        CheckSet_NeedMaterial(scSO.GetLocationLevel());
    }

    /// <summary>
    /// 拠点のレベルに合わせて素材を変える
    /// </summary>
    void CheckSet_NeedMaterial(int location_level)
    {
        locationLevel_text.text = "" + location_level;
        SetNeedMate(location_level);
        TextSet_NextLevelUnlock(location_level);
    }

#region 必要素材をスロットに適応させる処理
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

    /// <summary>
    /// Backボタンを押したときの処理
    /// </summary>
    void BackButtonOnClick()
    {
        fm.CanvasEnabled(CanvasName.Location, false);
    }

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
        // 拠点のレベルが変わったか調べる
        if(Check_ChangeLocationLevel(location_level) == true)
        {
            // 拠点のレベルが変わったらスロットを初期化する
            foreach(var _slot in mateSlots)
            {
                Debug.Log("初期化されました");
                _slot.SetSlotMaterial(null, 0);
            }
            oldlevel = location_level;
        }

        NeedMaterialSO.NEED_MATERIAL_ROOT needmate_root;        // 必要素材のリスト
        
        // numがリストのカウント数より小さければ
        if(location_level < needMateSO.need_mate_root.Count)
        {
            needmate_root = needMateSO.need_mate_root[location_level];
        }
        else
        {
            Check_SlotInMaterial();
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
#endregion


#region アンロックの表示処理
    /// <summary>
    /// Unlock画面のテキストやスロット設定
    /// </summary>
    void TextSet_NextLevelUnlock(int location_level)
    {
        StatusLimited statusLimited;

        if(location_level < scSO.GetStatusLimiteds().Count)
        {
            statusLimited = scSO.GetStatusLimiteds()[location_level];
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
        StatusLimited.StatusParam[] statusParam = statusLimited.statusParam;

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

                slTexts[ii].statusName_texts.text = statusParam[ii].statusName;
                slTexts[ii].statusValue_value.text = "" + statusParam[ii].statusLimited_value;
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

    
}
