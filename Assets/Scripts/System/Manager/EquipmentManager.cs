using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager instance;
    ToolController toolC;
    RobotAccessoryController accessoryC;

    [SerializeField] EquipmentSO equipmentSO;
    [SerializeField] AccessorySO accessorySO;

    BaseStatus robotbase;                                   // ロボットのベースステータス

    AccessorySO.ACCESSORY_STATUS[] a_status;                // アクセサリーのステータスをまとめる場所(スロット)


    [Header("選択画面")]
    [SerializeField] GameObject equipment_ScrollView;       // 装備選択画面のスクロールビュー
    [SerializeField] GameObject Select_AccessorySlot_Prefab;       // アクセサリースロットを生成する為のPrefab
    [SerializeField] Transform equipmentselect_parent;      // 装備選択スロットの親オブジェクト
    EquipmentSelectSlot[] e_select_slots;                   // 装備選択スロット(個々)
    [SerializeField] Button back_button;                    // 戻るボタン

    public List<GameObject> select_objs = new List<GameObject>();

    private void Awake() {
        if(instance == null) instance = this;
        else { Destroy(this); }
    }

    private void Start() {
        toolC = GetComponent<ToolController>();
        accessoryC = GetComponent<RobotAccessoryController>();

        a_status = accessorySO.accessory_status;

        for(int ii = 0; ii < a_status.Length; ii++)
        {
            // SOで作成したアクセサリーの数に合わせて選択スロットを生成する
            GameObject obj = Instantiate(Select_AccessorySlot_Prefab, transform.position, Quaternion.identity, equipmentselect_parent);
            select_objs.Add(obj);
        }
        
        // もし生成したスロット数が装備の最大数より小さければ追加で生成する
        if(select_objs.Count < equipmentSO.equipment_values.Length)
        {
            int index = equipmentSO.equipment_values.Length - select_objs.Count;
            for(int ii = 0; ii < index; ii++)
            {
                // SOで作成したアクセサリーの数に合わせて選択スロットを生成する
                GameObject obj = Instantiate(Select_AccessorySlot_Prefab, transform.position, Quaternion.identity, equipmentselect_parent);
                select_objs.Add(obj);
            }
        }

        e_select_slots = equipmentselect_parent.GetComponentsInChildren<EquipmentSelectSlot>();

        back_button.onClick.AddListener(OnClick_BackButton);
        SetActive_Equipment_ScrollView(false);
    }

    /// <summary>
    /// ロボットを押された時に初めに処理される
    /// </summary>
    /// <param name="_robotBase"></param>
    public void Check_UnlockEquipmentSlot(BaseStatus _robotBase)
    {
        // 選択されたロボットのステータスを取得する
        robotbase = _robotBase;

        // 
        toolC.GetToolSlot().SetText_ToolValue(_robotBase.equipment_value);

        for(int ii = 0; ii < accessoryC.GetAccessorySlots().Length; ii++)
        {
            accessoryC.GetAccessorySlots()[ii].SetText_AccessoryValue(_robotBase.accessories_value[ii]);
        }
        
        toolC.SetButtonInteractable();
        accessoryC.SetButtonInteractable(robotbase);
    }

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
            if(ii < accessorySO.accessory_status.Length)
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

    public void SetActive_Equipment_ScrollView(bool flag) { equipment_ScrollView.SetActive(flag); }
    public void OnClick_BackButton()
    {
        SoundManager.instance.PlayAudio("Back_2");
        SetActive_Equipment_ScrollView(false);
    }

    public BaseStatus GetRobotStatus() { return robotbase; }
    public RobotAccessoryController GetAccessoryController() { return accessoryC; }
    public ToolController GetToolController() { return toolC; }

    public EquipmentSelectSlot[] GetEquipmentSelectSlot() { return e_select_slots; }
    public AccessorySO.ACCESSORY_STATUS[] GetAccessoryStatusSlot() { return a_status;}
}