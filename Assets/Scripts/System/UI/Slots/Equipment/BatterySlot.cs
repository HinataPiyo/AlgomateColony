using UnityEngine;
using UnityEngine.UI;

public class BatterySlot : MonoBehaviour
{
    [SerializeField] BatteryData.BATTERY_STATUS value;
    public Image icon;          // アイコンの画像
    public Button button;       // アイコンをクリック

    public void SetSlot_BatteryStatus(BatteryData.BATTERY_STATUS _value)
    {
        value = _value;
        icon.sprite = _value.icon;
        icon.preserveAspect = true;     // 元画像に合わせる
        icon.enabled = true;
    }

    private void Start() {
        // button.onClick.AddListener(OnClick_SelectButton);
    }
    
    /// <summary>
    /// スロットをクリックしたときcontrollerに自身の番号を送る
    /// </summary>
    public void OnClick_SelectButton() {
        SoundManager.instance.PlayAudio("ButtonClick");
    }
}