using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// メイン処理
/// </summary>
public class ChargingBatteryPanel : MonoBehaviour
{
    UpdateTime_Class updateTime = new UpdateTime_Class();
    [SerializeField] ChargingBatterySO cbSO;
    [SerializeField] TextMeshProUGUI possible_chargeAmount_text;

    [SerializeField] Transform robotslots_parent;               // スロットを格納する親オブジェクト
    [SerializeField] GameObject robotslot_prefab;
    [SerializeField] List<ChargingBatteryRobotSlot> robotSlots = new List<ChargingBatteryRobotSlot>();     // 生成したスロットを格納する

    [Header("スロットを押したときに実行するもの")]
    [SerializeField] GameObject putout_panel;                   // 充電が完了したロボットのパネルを押すと表示されるパネル
    [SerializeField] Button yes_button;
    [SerializeField] Button no_button;
    ChargingBatteryRobotSlot cbrSlot;

    [Header("ロボットの出現位置とPrefab")]
    [SerializeField] GameObject robot_prefab;
    [SerializeField] Transform spawn_pos;

    // 一時的に格納しておくもの（スロットから送られてくる）
    RobotController robot;
    BaseStatus robotBase;

    [SerializeField] bool flag;


    void Start()
    {
        // リスナー登録
        yes_button.onClick.AddListener(YesOnClick);
        no_button.onClick.AddListener(NoOnClick);

        SetActive_PutOutPanel(false);
        Creat_RobotSlot();
    }

    void Update()
    {
        if(updateTime.UpdateTime() == true)
        {
            // 充電可能数を確認する
            possible_chargeAmount_text.text = $"{Check_InSlot()}/{cbSO.possible_chargeAmount}";
            Creat_RobotSlot();
        }
    }

    // ※充電回数がMAXの場合で充電施設に入れないようにする
    /// <summary>
    /// RobotSlotを生成するメソッド
    /// </summary>
    void Creat_RobotSlot()
    {
        for(int ii = 0; ii < cbSO.possible_chargeAmount; ii++)
        {
            // 既に生成されてあるスロットの数より充電可能数が大きければ
            if(cbSO.possible_chargeAmount > robotSlots.Count)
            {
                // スロットを生成する
                GameObject _slot = Instantiate(robotslot_prefab, robotslots_parent);
                ChargingBatteryRobotSlot cbSlot = _slot.GetComponent<ChargingBatteryRobotSlot>();

                // スロットの初期化
                Initialization_RobotSlot(cbSlot);

                // 生成したスロットをリストに追加する
                robotSlots.Add(cbSlot);
            }
        }
    }

    /// <summary>
    /// 生成したスロットの初期化
    /// </summary>
    void Initialization_RobotSlot(ChargingBatteryRobotSlot _cbSlot)
    {
        _cbSlot.icon.sprite = null;
        _cbSlot.robotName_text.text = "";
        _cbSlot.timeRemaining_text.text = "0";
        _cbSlot.rechargeCount_text.text = "0/0";
        _cbSlot.timeRemaining_text.color = Color.white;

        // 自身のスクリプトを渡す
        _cbSlot.Set_ChargingBatteryPanel(this);
        _cbSlot.inSlot = false;

        _cbSlot.slotText_parent.SetActive(false);
    }

    /// <summary>
    /// 現在の充電可能数を調べる
    /// </summary>
    /// <returns></returns>
    int Check_InSlot()
    {
        int count = 0;
        foreach(var _slot in robotSlots)
        {
            if(_slot.robotbase._runk != "")
            {
                count++;
            }
        }

        return count;
    }

    /// <summary>
    /// スロットからロボットの情報を一時的に格納する
    /// </summary>
    public void Set_RobotData(BaseStatus _robotBase, ChargingBatteryRobotSlot _cbrSlot)
    {
        cbrSlot = _cbrSlot;
        robotBase = _robotBase;
    }

    void YesOnClick()
    {
        GameObject _robotObj = Instantiate(robot_prefab, new Vector2(spawn_pos.position.x, spawn_pos.position.y - 1.5f), Quaternion.identity);
        RobotController _script = _robotObj.GetComponent<RobotController>();
        _script.Initialize(robotBase);

        Initialization_RobotSlot(cbrSlot);     // スロット内をクリアする
        SetActive_PutOutPanel(false);       // Yes or No ボタンパネルを非アクティブ状態にする
    }

    void NoOnClick() { SetActive_PutOutPanel(false); }

    public void SetActive_PutOutPanel(bool flag) { putout_panel.SetActive(flag);}
}
