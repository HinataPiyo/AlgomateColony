using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgreadBatteryRobotSlot : MonoBehaviour
{
    [SerializeField] Button button;
    BaseStatus robotbase;
    [SerializeField] Image robotIcon;
    [SerializeField] TextMeshProUGUI robotName_text;
    [SerializeField] TextMeshProUGUI batteryName_text;
    [SerializeField] Image batteryImage;
    [SerializeField] Sprite[] robot_icon;       // 0 : 通常のスプライト, 1 : 充電切れのスプライト

    public BaseStatus RobotBase { get{ return robotbase; } }

    private void Start()
    {
        button.onClick.AddListener(OnClick_Slot);
    }

    public void InSlot(BaseStatus _robotBase)
    {
        robotbase = _robotBase;

        robotIcon.sprite = robot_icon[0];                           // 通常の画像を設定する
        batteryName_text.text = robotbase.battery_status._name;     // バッテリーの名前を設定する
        batteryImage.sprite = robotbase.battery_status.icon;        // バッテリーの画像を設定する

        // TODO ロボット自身に例えば"ロボット1"と、出現している数だけ数字を入れてあげるようにする
        // ロボットに名前が設定されていなければ
        if(robotbase.robotName == "")
        {
            robotName_text.text = "未設定";
        }
        else    // 名前が設定されていれば
        {
            robotName_text.text = robotbase.robotName;
        }
    }

    public void ClearSlot()
    {
        robotbase = null;

        robotIcon.sprite = null;          
        batteryImage.sprite = null;
        batteryName_text.text = "";
        robotName_text.text = "";
    }

    // スロットをクリックしたとき
    void OnClick_Slot()
    {
        SoundManager.instance.PlayAudio("ButtonClick");
        // cbCont.Set_RobotData(robotbase, this);
        // cbCont.SetActive_PutOutPanel(true);
    }
}