using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BatteryChargingRoomManager : MonoBehaviour
{
    HorizontalButtonController hbCont;
    ChargingBatteryPanel cbCont;


    [Header("ChargingBatteryキャンバスの設定")]
    [SerializeField] Button backButton;                 // 戻るボタン

    [Header("横ボタンを押したときに切り替えるパネル")]
    [SerializeField] Transform horizontalButton_parent;
    [SerializeField] HorizontalButtonCohesion[] horizontal_panels;
    ButtonSlotVarticalHorizontal[] horizontal_slots;
    [SerializeField] string[] buttonName;

    // 横ボタンのまとまり用ストラクト
    [System.Serializable]
    public struct HorizontalButtonCohesion
    {
        // まとまりの親オブジェクト
        public Transform panels_paent;
        // まとまりの中にある別のパネル
        public GameObject[] horizontal_panels;
        public TextMeshProUGUI[] buttonName;
    }

    private void Start() {
        backButton.onClick.AddListener(ButtonOnClick_Back);
        // コンポーネントの取得
        hbCont = GetComponent<HorizontalButtonController>();
        cbCont = GetComponent<ChargingBatteryPanel>();
        horizontal_slots = horizontalButton_parent.GetComponentsInChildren<ButtonSlotVarticalHorizontal>();
        
        // 横に並んでるボタンを設定する
        hbCont.Set_HorizontalButton(horizontal_slots, horizontal_panels, buttonName);
    }
    
    private void Update()
    {
    }

    /// <summary>
    /// Backボタンを押したときの処理
    /// </summary>
    void ButtonOnClick_Back()
    {
        FacilityManager.instance.CanvasEnabled(CanvasName.BatteryRoom, false);
    }

    public ChargingBatteryPanel GetChargingBatteryController() { return cbCont;}
}