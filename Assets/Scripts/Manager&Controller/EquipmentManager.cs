using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager instance;
    RobotToolController toolC;
    RobotAccessoryController accessoryC;
    RobotBatteryController batteryC;

    // ロボットのベースステータス
    BaseStatus robotbase;

    [Header("Robotのステータス画面で表示する装備など")]
    [SerializeField] EquipmentSO equipmentSO;
    [SerializeField] AccessorySO accessorySO;
    [SerializeField] BatteryData batteryData;

    // アクセサリーのステータスをまとめる場所(スロット)
    AccessoryData[] acceDatas;

    [SerializeField] Button back_button;                    // 戻るボタン
    [SerializeField] GameObject backButton_obj;

    [Header("選択画面 / 装備")]
    [SerializeField] GameObject equipment_ScrollView;       // 装備選択画面のスクロールビュー
    [SerializeField] GameObject select_slot_Prefab;         // 装備スロットを生成する為のPrefab(装備)
    [SerializeField] Transform equipmentselect_parent;      // 装備選択スロットの親オブジェクト
    EquipmentSelectSlot[] e_select_slots;                   // 装備選択スロット(個々)

    public List<GameObject> select_objs = new List<GameObject>();

    [SerializeField] BatterySlot b_slot;    // バッテリー用スロット（装備スロット）


    private void Awake() {
        if(instance == null) instance = this;
        else { Destroy(this); }
    }

    private void Start()
    {
        // コンポーネントを取得
        toolC = GetComponent<RobotToolController>();
        accessoryC = GetComponent<RobotAccessoryController>();
        batteryC = GetComponent<RobotBatteryController>();

        // ボタンをリスナーに登録
        back_button.onClick.AddListener(OnClick_BackButton);
        
        // もし生成したスロット数が装備の最大数より小さければ追加で生成する(装備)
        if(select_objs.Count < equipmentSO.equipment_values.Length)
        {
            int index = equipmentSO.equipment_values.Length - select_objs.Count;
            for(int ii = 0; ii < index; ii++)
            {
                // SOで作成したアクセサリーの数に合わせて選択スロットを生成する
                GameObject obj = Instantiate(select_slot_Prefab, equipmentselect_parent);
                select_objs.Add(obj);
            }
        }

        // 装備スロットを取得
        e_select_slots = equipmentselect_parent.GetComponentsInChildren<EquipmentSelectSlot>();

        // パネル類を非表示
        backButton_obj.SetActive(false);
        SetActive_Equipment_ScrollView(false);
    }

    /// <summary>
    /// ロボットを押された時に初めに処理される（ロボットのステータス画面が表示されるとき）
    /// 全ての装備スロットを確認する
    /// RobotStatusPanelManagerで処理が実行される
    /// </summary>
    /// <param name="_robotBase"></param>
    public void Check_EquipmentSlots(BaseStatus _robotBase)
    {
        // 選択されたロボットのステータスを取得する
        robotbase = _robotBase;

        // バッテリースロットを設定する
        Set_BatterySlot();

        // ロボットが装備しているツールをスロットに反映させる
        toolC.GetToolSlot().SetText_ToolValue(_robotBase.equipment_value);

        // アクセサリーの処理
        for(int ii = 0; ii < accessoryC.GetAccessorySlots().Length; ii++)
        {
            accessoryC.GetAccessorySlots()[ii].SetText_AccessoryValue(_robotBase.acceData_list[ii]);
        }

        // スロットのボタンが押せるかどうか調べる
        toolC.SetButtonInteractable();
        accessoryC.SetButtonInteractable(robotbase);
    }

    /// <summary>
    /// 装備スロットにバッテリーを反映させる
    /// </summary>
    public void Set_BatterySlot()
    {
        // ロボットが装備しているバッテリーを装備スロットに反映させる
        b_slot.SetSlot_BatteryStatus(robotbase.battery_status);
    }

#region スロットを表示/非表示にするか調べる
    /// <summary>
    /// アクセサリースロットを押下した場合SOで設定している
    /// アクセサリーの数だけスロットをアクティブ状態にする
    /// </summary>
    public void SetActiv_SelectSlots_Accessory()
    {
        // 生成したスロット分forを回す
        for(int ii = 0; ii < select_objs.Count; ii++)
        {
            // アクセサリーの数分アクティブ状態にする
            if(ii < acceDatas.Length)
            {
                select_objs[ii].SetActive(true);
            }
            else    // それ以外を非アクティブ状態にする
            {
                select_objs[ii].SetActive(false);
            }
        }
    }

    public void SetActiv_SelectSlots_Tool()
    {
        // 生成したスロット分forを回す
        for(int ii = 0; ii < select_objs.Count; ii++)
        {
            // 装備の数分アクティブ状態にする
            if(ii < equipmentSO.equipment_values.Length)
            {
                select_objs[ii].SetActive(true);
            }
            else    // それ以外を非アクティブ状態にする
            {
                select_objs[ii].SetActive(false);
            }
        }
    }
#endregion

#region ロボットの装備スロットを押下した時にパネルを表示/非表示にするか設定する
    /// <summary>
    /// ツールパネルの非表示 / 表示
    /// </summary>
    /// <param name="flag"></param>
    public void SetActive_Equipment_ScrollView(bool flag)
    {
        // パネルを表示
        equipment_ScrollView.SetActive(flag);

        // ツールパネルが表示状態だった場合
        if(equipment_ScrollView.activeSelf == true) 
        {
            // バックボタンを表示
            backButton_obj.SetActive(true);
        }
        else
        {
            // バックボタンを非表示
            backButton_obj.SetActive(false);
        }
    }

#endregion

    /// <summary>
    /// 戻るボタン(X)を押したときの処理
    /// </summary>
    public void OnClick_BackButton()
    {
        SoundManager.instance.PlayAudio("SelectObject");

        SetActive_Equipment_ScrollView(false);
    }

    public BaseStatus GetRobotStatus() { return robotbase; }
    public RobotAccessoryController GetAccessoryController() { return accessoryC; }
    public RobotToolController GetToolController() { return toolC; }
    public RobotBatteryController GetRobotBatteryController() { return batteryC; }

    public EquipmentSelectSlot[] GetEquipmentSelectSlot() { return e_select_slots; }
    public AccessoryData[] GetAccessoryStatusSlot() { return acceDatas;}
}