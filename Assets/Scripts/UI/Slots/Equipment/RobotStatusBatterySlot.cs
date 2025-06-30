using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// アルゴメイトのステータスから見れるバッテリースロットの設定
/// 各スロットにアタッチされるスクリプト
/// </summary>
public class RobotStatusBatterySlot : MonoBehaviour
{
    [SerializeField] BatteryType.DATA data;
    public Image icon;          // アイコンの画像
    public Button button;       // アイコンをクリック

    public void SetSlot_BatteryStatus(BatteryType.DATA _data)
    {
        data = _data;
        icon.sprite = _data.icon;
        icon.preserveAspect = true;     // 元画像に合わせる
        icon.enabled = true;
    }

    private void Start()
    {
        // button.onClick.AddListener(OnClick_SelectButton);
    }

    /// <summary>
    /// スロットをクリックしたときcontrollerに自身の番号を送る
    /// </summary>
    public void OnClick_SelectButton()
    {
        SoundManager.instance.PlayAudio("ButtonClick");
    }
}