using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RobotStatusPanelManager : MonoBehaviour
{
    public static RobotStatusPanelManager instance;

    [Header("コンポーネント")]
    [SerializeField] SystemControlSO scSO;
    [SerializeField] RobotController _robot;
    [SerializeField] BaseStatus _base;
    [SerializeField] Button backButton;
    [SerializeField] Button commandButton;
    [SerializeField] RectTransform robotStatusPanel;
    [SerializeField] GameObject robotCodingObj;
    RobotCommandController robotCmdCont;

    [Header("親のオブジェ")]
    [SerializeField] Transform slotsParent;
    [Header("スロット")]
    [SerializeField] RobotSlot[] rSlot;
    [SerializeField] BaseStatus.Slot[] _baseSlot;

    [Header("各々の数値テキスト")]
    [SerializeField] TextMeshProUGUI runk_text;
    [SerializeField] TextMeshProUGUI maxRecharge_text;
    [SerializeField] TextMeshProUGUI currentRecharge_text;
    [SerializeField] TextMeshProUGUI generalStatus_text;
    [SerializeField] TextMeshProUGUI maxEnergy_text;
    [SerializeField] TextMeshProUGUI currentEnergy_text;
    [SerializeField] TextMeshProUGUI moveSpeed_text;
    [SerializeField] TextMeshProUGUI gatherStrength_text;
    [SerializeField] TextMeshProUGUI gatherRate_text;
    [Header("潜在能力テキスト")]
    [SerializeField] TextMeshProUGUI maxRecharge_potentialtext;
    [SerializeField] TextMeshProUGUI maxEnergy_potentialtext;
    [SerializeField] TextMeshProUGUI maxMovespeed_potentialtext;
    [SerializeField] TextMeshProUGUI maxGatherStrength_potentialtext;
    [SerializeField] TextMeshProUGUI maxGatherRate_potentialtext;
    

    const int ROBOT_STATUSPANEL_WIDTH_CLOSE = 450;
    const int ROBOT_STATUSPANEL_WIDTH_OPEN = 850;



    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        backButton.onClick.AddListener(BackButtonOnClick);
        commandButton.onClick.AddListener(CommandButtonClick);
        rSlot = slotsParent.GetComponentsInChildren<RobotSlot>();
        robotCmdCont = GetComponent<RobotCommandController>();

        ResetText();

        // CodingPanelを非アクティブ状態にする
        robotCodingObj.SetActive(false);
        SetHeight(ROBOT_STATUSPANEL_WIDTH_CLOSE);
    }

    void ResetText()
    {
        foreach(var _rSlot in rSlot)
        {
            _rSlot.icon.sprite = null;
            _rSlot.icon.enabled = false;
            _rSlot.stackAmo_text.text = "0";
        }

        runk_text.text = "";
        maxRecharge_text.text = "";
        currentEnergy_text.text = "";
        generalStatus_text.text = "";
        currentEnergy_text.text = "";
        gatherStrength_text.text = "";
        maxEnergy_text.text = "";
        moveSpeed_text.text = "";
        gatherRate_text.text = "";

        // 潜在能力テキスト
        maxRecharge_potentialtext.text = "";
        maxEnergy_potentialtext.text = "";
        maxMovespeed_potentialtext.text = "";
        maxGatherStrength_potentialtext.text = "";
        maxGatherRate_potentialtext.text = "";
    }

    private void Update()
    {
        if(_base != null && _robot != null)
        {
            runk_text.text = "" + _base._runk;
            maxRecharge_text.text = "" + _base.recharge_MAX;
            currentRecharge_text.text = "" + _base.currentRecharged;
            generalStatus_text.text = "" + _base.totalScore.ToString("F1");
            currentEnergy_text.text = "" + _base.currentEnergy.ToString("F2");
            maxEnergy_text.text = "" + _base.maxEnergy;
            moveSpeed_text.text = "" + _base.moveSpeed;
            gatherStrength_text.text = "" + _base.gatherSterngth;
            gatherRate_text.text = "" + _base.gatherRate;
        }

        // 潜在能力テキスト
        maxRecharge_potentialtext.text = $"({scSO.GetPotential().RECHARGE_MAX})";
        maxEnergy_potentialtext.text = $"({scSO.GetPotential().ENERGY_MAX})";
        maxMovespeed_potentialtext.text = $"({scSO.GetPotential().MOVESPEED_MAX})";
        maxGatherStrength_potentialtext.text = $"({scSO.GetPotential().GATHERSTRENGTH_MAX})";
        maxGatherRate_potentialtext.text = $"({scSO.GetPotential().GATHERRATE_MAX})";


        if(_baseSlot != null)
        {
            SetSlot();      // スロット内に画像を入れる
        }
    }

    void SetSlot()
    {
        for(int ii = _baseSlot.Length - 1; ii >= 0; ii--)
        {
            // Robotがアイテムを所持していなければ
            if(_baseSlot[ii].mateSO == null)
            {
                rSlot[ii].icon.sprite = null;
                rSlot[ii].stackAmo_text.text = "0";
            }
            else    // アイテムを所持していれば
            {
                rSlot[ii].icon.enabled = true;
                rSlot[ii].icon.sprite = _baseSlot[ii].mateSO.icon;
                rSlot[ii].stackAmo_text.text = "" + _baseSlot[ii].itemStackAmount;
            }
        }
    }

    /// <summary>
    /// Robotをクリックしたら呼び出される関数
    /// 加え、StatusPanelのBackを押したら"robot"を"null"に設定する
    /// </summary>
    /// <param name="robot"></param>
    public void SetRobotStatus(RobotController robot)
    {
        if(robot != null) {
            _robot = robot;
            _base = robot.GetBaseStatus();
            _baseSlot = robot.GetSlot();

            // ロボット自身にアタッチしてあるスクリプトを取得する
            robotCmdCont.Set_RobotCommandExecute = robot.gameObject.GetComponent<RobotCommandExecute>();
            EquipmentManager.instance.Check_EquipmentSlots(_base);
        } else {
            _robot = null;
        }
    }

    /// <summary>
    /// Backボタンを押したときの処理
    /// </summary>
    void BackButtonOnClick()
    {
        ResetText();
        FacilityManager.instance.CanvasEnabled(CanvasName.RobotStatus, false);
        robotCodingObj.SetActive(false);    // CodingPanelを非アクティブ状態にする
        SetHeight(ROBOT_STATUSPANEL_WIDTH_CLOSE);
    }

    private void CommandButtonClick()
    {
        robotCodingObj.SetActive(true);    // CodingPanelをアクティブ状態にする
        SetHeight(ROBOT_STATUSPANEL_WIDTH_OPEN);
    }

    /// <summary>
    /// パネルの高さの幅を変える
    /// </summary>
    /// <param name="newHeight"></param>
    void SetHeight(float newWidth)
    {
        // 現在のsizeDeltaの幅を保持して高さのみ変更
        Vector2 size = robotStatusPanel.sizeDelta;
        size.x = newWidth;
        robotStatusPanel.sizeDelta = size;
    }
}
