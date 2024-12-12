using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RobotStatusPanelManager : MonoBehaviour
{
    public static RobotStatusPanelManager instance;

    [Header("コンポーネント")]
    [SerializeField] Robot _robot;
    [SerializeField] BaseStatus _base;
    [SerializeField] Button backButton;
    [SerializeField] Button commandButton;
    [SerializeField] RectTransform robotStatusPanel;

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

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        backButton.onClick.AddListener(BackButtonOnClick);
        commandButton.onClick.AddListener(CommandButtonClick);
        rSlot = slotsParent.GetComponentsInChildren<RobotSlot>();

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
    }

    private void Update()
    {
        if(_base != null && _robot != null)
        {
            runk_text.text = "" + _base._runk;
            maxRecharge_text.text = "" + _base.recharge_MAX;
            currentRecharge_text.text = "" + _robot.GetCurrentRecharge();
            generalStatus_text.text = "" + _base.totalScore.ToString("F1");
            currentEnergy_text.text = "" + _robot.GetCurrentEnergy().ToString("F2");
            maxEnergy_text.text = "" + _base.maxEnergy;
            moveSpeed_text.text = "" + _base.moveSpeed;
            gatherStrength_text.text = "" + _base.gatherSterngth;
            gatherRate_text.text = "" + _base.gatherRate;
        }

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
    public void SetRobotStatus(Robot robot)
    {
        if(robot != null) {
            _robot = robot;
            _base = robot.GetBaseStatus();
            _baseSlot = robot.GetSlot();
        } else {
            _robot = null;
        }
    }

    /// <summary>
    /// Backボタンを押したときの処理
    /// </summary>
    void BackButtonOnClick()
    {
        FacilityManager.instance.CanvasEnabled(CanvasName.RobotStatus, false);
        SetHeight(450f);
    }

    private void CommandButtonClick() { SetHeight(1000f); }

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
