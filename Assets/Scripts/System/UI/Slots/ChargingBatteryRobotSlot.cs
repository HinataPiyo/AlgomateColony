using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChargingBatteryRobotSlot : MonoBehaviour
{
    [SerializeField] SystemControlSO scSO;
    [SerializeField] Button button;
    ChargingBatteryController cbCont;
    public BaseStatus robotbase;
    public Image icon;
    public TextMeshProUGUI robotName_text;
    public TextMeshProUGUI timeRemaining_text;
    public TextMeshProUGUI rechargeCount_text;
    [SerializeField] Sprite[] change_icon;       // 0 : 通常のスプライト, 1 : 充電切れのスプライト

    public bool inSlot;    // ロボットがスロットの中に入っているのかの確認

    bool complet_flag;

    private void Start()
    {
        button.onClick.AddListener(OnClick_Slot);
    }

    public void Set_ChargingBatteryController(ChargingBatteryController _script) { cbCont = _script; }
    public void InSlot(BaseStatus _robotBase)
    {
        inSlot = true;
        robotbase = _robotBase;

        // ロボットに名前が設定されていなければ
        if(robotbase.robotName == "")
        {
            robotName_text.text = "未設定";
        }
        else    // 名前が設定されていれば
        {
            robotName_text.text = robotbase.robotName;
        }
        
        rechargeCount_text.text = $"{robotbase.currentRecharged}/{robotbase.recharge_MAX}";
    }

    private void Update()
    {
        if(inSlot == true)
        {
            GetCompletTime();
            icon.enabled = true;

            // 充電が完了していなければ
            if(complet_flag == false)
            {
                // アイコンの画像を変更する
                icon.sprite = change_icon[1];

                // 最大値より現在の値が小さければ
                if(robotbase.currentEnergy <= robotbase.maxEnergy)
                {
                    // 現在の充電量 = 現在の充電量 + (0.002 / 2) * アップグレード用係数
                    robotbase.currentEnergy += (Time.deltaTime / 2) * scSO.GetBatteryChargingTime();
                }
                else    // 最大値を超えたら
                {
                    // 最大値に設定する
                    robotbase.currentEnergy = robotbase.maxEnergy;

                    // 充電回数をカウントアップする
                    robotbase.currentRecharged++;
                    rechargeCount_text.text = $"{robotbase.currentRecharged}/{robotbase.recharge_MAX}";
                }
            }
            else    // 充電が完了したら
            {
                // アイコンの画像を変更する
                icon.sprite = change_icon[0];

                // スロットを押せるようにする
                button.interactable = true;
            }
        }
        else
        {
            // スロットを押せないようにする
            button.interactable = false;
        }
    }

    /// <summary>
    /// 充電が終わてちるかどうか調べる
    /// </summary>
    float GetCompletTime()
    {
        float _time;
        _time = robotbase.maxEnergy - robotbase.currentEnergy;

        // 充電が完了していたら
        if(0 == _time)
        {
            timeRemaining_text.color = Color.green;
            timeRemaining_text.text = "完了";
            complet_flag = true;
        }
        else    // 充電がまだできるなら
        {
            timeRemaining_text.color = Color.white;
            timeRemaining_text.text = "" + _time.ToString("F1");
            complet_flag = false;
        }

        return _time;
    }


    // スロットをクリックしたとき
    void OnClick_Slot()
    {
        SoundManager.instance.PlayAudio("ButtonClick");
        cbCont.Set_RobotData(robotbase, this);
        cbCont.SetActive_PutOutPanel(true);
    }
}