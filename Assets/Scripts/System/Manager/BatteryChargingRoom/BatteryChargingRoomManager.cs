using UnityEngine;
using UnityEngine.UI;

public class BatteryChargingRoomManager : MonoBehaviour
{
    VerticalButtonController vbCont;
    HorizontalButtonController hbCont;

    ChargingBatteryController cbCont;

    [Header("ChargingBatteryキャンバスの設定")]
    [SerializeField] Button backButton;                 // 戻るボタン

    [Header("縦ボタンを押したときに切り替えるパネル")]
    [SerializeField] GameObject[] vertical_panels;
    [SerializeField] Transform verticalButton_parent;
    ButtonSlotVarticalHorizontal[] vertical_slots;

    [Header("横ボタンを押したときに切り替えるパネル")]
    [SerializeField] Transform horizontalButton_parent;
    [SerializeField] HorizontalButtonCohesion[] horizontal_panels;
    ButtonSlotVarticalHorizontal[] horizontal_slots;

    // 横ボタンのまとまり用ストラクト
    [System.Serializable]
    public struct HorizontalButtonCohesion
    {
        // まとまりの親オブジェクト
        public Transform panels_paent;
        // まとまりの中にある別のパネル
        public GameObject[] horizontal_panels;
        public string[] buttonName;
    }

    private void Start() {
        backButton.onClick.AddListener(ButtonOnClick_Back);
        // コンポーネントの取得
        vbCont = GetComponent<VerticalButtonController>();
        hbCont = GetComponent<HorizontalButtonController>();
        cbCont = GetComponent<ChargingBatteryController>();
        vertical_slots = verticalButton_parent.GetComponentsInChildren<ButtonSlotVarticalHorizontal>();
        horizontal_slots = horizontalButton_parent.GetComponentsInChildren<ButtonSlotVarticalHorizontal>();
        
        // 縦に並んでるボタンを設定する
        vbCont.Set_VarticalButton(vertical_slots, vertical_panels);
        hbCont.Set_HorizontalButton(horizontal_slots, horizontal_panels);
        
        vbCont.ButtonClick_Proc(0);     // 最初の画面設定
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

    public ChargingBatteryController GetChargingBatteryController() { return cbCont;}
}