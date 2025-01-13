using UnityEngine;
using UnityEngine.EventSystems;

public enum CanvasName
{
    Setting,        // 設定
    RobotStatus,    // ステータス
    Location,       // 拠点
    Warehouse,      // 倉庫
    BatteryRoom,    // 充電
    Warkshop,       // 加工
}

public class FacilityManager : MonoBehaviour
{
    public static FacilityManager instance;
    [SerializeField] Canvas settingCanvas;
    [SerializeField] Canvas locationCanvas;             // 拠点をクリックしたときに表示されるキャンバス
    [SerializeField] Canvas warehouseCanvas;
    [SerializeField] Canvas batteryRoomCanvas;
    [SerializeField] Canvas warkshopCanvas;
    Canvas robotStatusCanvas;
    LocationController lcCont;                    // 拠点キャンバスのスクリプト

    bool isOpenCnavas;      // Canvasが開いているか否か

    private void Awake()
    {
        if(instance == null){ instance = this; }
        else { Destroy(gameObject); }
    }
    void Start()
    {
        // コンポーネントの取得
        lcCont = GetComponent<LocationController>();
        robotStatusCanvas = RobotStatusPanelManager.instance.GetComponentInChildren<Canvas>();

        CanvasEnabled(CanvasName.Setting, false);       // 設定キャンバスを非表示にする
        CanvasEnabled(CanvasName.Location, false);      // 拠点キャンバスを非表示にする
        CanvasEnabled(CanvasName.Warehouse, false);     // 倉庫キャンバスを非表示にする
        CanvasEnabled(CanvasName.BatteryRoom, false);   // 充電施設のキャンバスを非表示にする
        CanvasEnabled(CanvasName.Warkshop, false);      // 加工施設のキャンバスを非表示にする
        CanvasEnabled(CanvasName.RobotStatus, false);   // ステータスキャンバス

        // 変数の初期化
        isOpenCnavas = false;
    }

    void Update()
    {
        if(isOpenCnavas == true && Input.GetKeyDown(KeyCode.Escape))
        {
            CanvasEnabled(CanvasName.Setting, false);       // 設定キャンバス
            CanvasEnabled(CanvasName.Location, false);      // 拠点キャンバス
            CanvasEnabled(CanvasName.Warehouse, false);     // 倉庫キャンバス
            CanvasEnabled(CanvasName.BatteryRoom, false);   // 充電施設のキャンバス
            CanvasEnabled(CanvasName.Warkshop, false);      // 加工施設のキャンバス
            CanvasEnabled(CanvasName.RobotStatus, false);   // ステータスキャンバス
        }
    }

    /// <summary>
    /// キャンバスの非表示・表示を行うメソッド
    /// </summary>
    /// <param name="canvasName">キャンバスの名前</param>
    /// <param name="flag">表示・非表示</param>
    public void CanvasEnabled(CanvasName canvasName, bool flag)
    {
        switch(canvasName)
        {
            // 設定キャンバスの設定
            case CanvasName.Setting:
                settingCanvas.enabled = flag;
                break;
            case CanvasName.RobotStatus:
                robotStatusCanvas.enabled = flag;
                break;
            // 拠点キャンバスの設定
            case CanvasName.Location:
                locationCanvas.enabled = flag;
                break;
            // 倉庫キャンバスの設定
            case CanvasName.Warehouse:
                warehouseCanvas.enabled = flag;
                break;
            // 充電施設
            case CanvasName.BatteryRoom:
                batteryRoomCanvas.enabled = flag;
                break;
            // 加工施設
            case CanvasName.Warkshop:
                warkshopCanvas.enabled = flag;
                break;
        }

        isOpenCnavas = flag;    // Canvasが開いているか否か
        EventSystem.current.SetSelectedGameObject(null);    // 何も選択されていない状態にする
    }

    public bool GetIsOpenCanvas() { return isOpenCnavas; }          // Canvasが開いているか否か
    public LocationController lcController() { return lcCont; }
}
