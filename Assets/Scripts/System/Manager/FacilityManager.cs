using UnityEngine;
using UnityEngine.EventSystems;

public enum CanvasName
{
    Setting,        // 設定
    RobotStatus,    // ステータス
    Location,       // 拠点
}

public class FacilityManager : MonoBehaviour
{
    public static FacilityManager instance;
    [SerializeField] Canvas settingCanvas;
    [SerializeField] Canvas locationCanvas;             // 拠点をクリックしたときに表示されるキャンバス
    Canvas robotStatusCanvas;
    LocationCanvasController lcCont;                    // 拠点キャンバスのスクリプト

    bool isOpenCnavas;      // Canvasが開いているか否か

    private void Awake()
    {
        if(instance == null){ instance = this; }
        else { Destroy(gameObject); }
    }
    void Start()
    {
        // コンポーネントの取得
        lcCont = GetComponent<LocationCanvasController>();
        robotStatusCanvas = RobotStatusPanelManager.instance.GetComponentInChildren<Canvas>();

        CanvasEnabled(CanvasName.Setting, false);       // 設定キャンバスを非表示にする
        CanvasEnabled(CanvasName.Location, false);      // 拠点キャンバスを非表示にする
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
        }

        isOpenCnavas = flag;    // Canvasが開いているか否か
        EventSystem.current.SetSelectedGameObject(null);    // 何も選択されていない状態にする
    }

    public bool GetIsOpenCanvas() { return isOpenCnavas; }          // Canvasが開いているか否か
    public LocationCanvasController lcController() { return lcCont; }
}
