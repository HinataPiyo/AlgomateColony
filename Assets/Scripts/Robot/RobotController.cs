using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

// ロボット自身にアタッチするスクリプト
public class RobotController : MonoBehaviour
{
    public enum State
    {
        DoNon,      // 何もしない
        Search,     // 資源を探す
        Move,       // 移動する
        GatherResource, // 収集する
        Recharge,   // 充電する
        NonEnergy,
        Deposit,
    }

    [Header("ステート")]
    State currentState;
    bool stateEnter;
    float stateTime = 0;
    
    [Header("スライダー")]
    [SerializeField] GameObject energySlider;       // Prefab
    GameObject eSlider;     // 位置を調整するため
    Slider _eslider;        // スライダーを更新するため(充電ゲージ)
    [SerializeField] GameObject gatherSlider;   // Prefab
    GameObject gSlider;     // 位置を調整するため(収集ゲージ)

    [Header("コンポーネント")]
    [SerializeField] BatteryData batteryData;
    [SerializeField] BaseStatus _base;

    RobotGather robotGather;
    RobotMovement robotMove;
    RobotSearch robotSearch;
    RobotBattery robotBattery;
    RobotCommandExecute robotCmdExecute;
    RobotDeposit robotDeposit;

    Canvas sliderCanvas;
    Animator robot_anim;

    // Searchで取得してきたhitInfoを格納しておく為の変数
    Collider2D hitInfo;
    [SerializeField] TextMeshPro robotname_text;

    string objectName;
    string[] depsiteName;
    public string[] DepsiteName { get{ return depsiteName; } set{ depsiteName = value; }}
    public string ObjectName { get{return objectName;} set{ objectName = value; } }
    public RobotCommandExecute Get_RobotCommandExecute { get{ return robotCmdExecute; } }
#region ステート管理
    void LateUpdate()
    {
        if(stateTime != 0)
        {
            stateEnter = false;
        }
    }

    // ステートの切り替え
    public void ChangeState(State newState)
    {
        currentState = newState;
        stateEnter = true;
        stateTime = 0;
    }
#endregion

    private void Update()
    {

        // ロボットの充電がなくなったか調べる
        robotBattery.Check_CurrentEnergy();

        // ステート管理
        stateTime += Time.deltaTime;
        switch(currentState)
        {
            case State.DoNon:
                if(stateEnter)
                {
                    Debug.Log(_base.robotName + "が待機状態になりました");
                    LogController.instance.SetLog(_base, "待機状態です");
                    robotMove.DoNonPosition();
                    gSlider.SetActive(false);   // 非アクティブ状態にする
                }
                break;
            case State.Search:
                // オブジェクトを探す
                hitInfo = robotSearch.Search(objectName);
                break;
            case State.Move:
                if(stateEnter)
                {
                    LogController.instance.SetLog(_base, "移動を開始しました");
                }
                // 移動処理
                robotMove.Moveing();
                break;
            case State.GatherResource:
                if(stateEnter)
                {
                    // 収集開始
                    robotGather.StartCoroutine_GatherResource();
                }
                break;
            case State.Recharge:
                if(stateEnter)
                {
                    // ロボットにバッテリー交換が必要か否か確認する
                    robotBattery.Check_NeedRecharge();
                }

                // バッテリーを充電する
                robotBattery.RechargeBattery();
                break;
            case State.NonEnergy:
                if(stateEnter)
                {
                    // 充電がなくなったら時の処理
                    robotBattery.NonEnergy();
                }
                break;
            case State.Deposit:
                if(stateEnter)
                {
                    // アイテムを倉庫に入れる
                    robotDeposit.Deposite();
                }
                break;
        }
        
        // スライダーの位置を設定する
        Set_SliderPosition();
    }

    /// <summary>
    /// 他から "再度生成" するときに使用するメソッド
    /// </summary>
    /// <param name="_basestatus"></param>
    public void Set_BaseStatus(BaseStatus _basestatus)
    {
        _base = _basestatus;
        Initialize();
    }

    /// <summary>
    /// 生成時に行う処理(新規生成)
    /// </summary>
    public void Initialize()
    {
        _base = new BaseStatus();               // 自身にクラスを生成
        _base.RandomStatusProc();               // ランダムでステータスを決める
        _base.GeneratInventorySlots();          // インベントリを生成
        _base.GenerateEquipmentSlots();         // 装備スロットを生成
        _base.GeneratAccessorySlots();          // アクセサリースロットを生成
        _base.GenerateBatterySlots();           // バッテリースロットを生成
        _base.TotalStatus();                    // 総合ステータスを生成

        MemberInit();
    }

    void MemberInit()
    {
        sliderCanvas = GameManager.instance.sliderCanvas;   // スライダーを表示するキャンバスを取得
        // スライダーの生成 / バッテリーを示すスライダー
        eSlider = Instantiate(energySlider, new Vector2(transform.position.x, transform.position.y + 0.5f), Quaternion.identity, sliderCanvas.transform);
        // 資源収集を行うときのスライダー
        gSlider = Instantiate(gatherSlider, new Vector2(transform.position.x, transform.position.y + 0.6f), Quaternion.identity, sliderCanvas.transform);

        // コンポーネントの取得
        _eslider = eSlider.GetComponent<Slider>();
        robot_anim = GetComponentInChildren<Animator>();
        robotGather = GetComponent<RobotGather>();
        robotMove = GetComponent<RobotMovement>();
        robotSearch = GetComponent<RobotSearch>();
        robotBattery = GetComponent<RobotBattery>();
        robotCmdExecute = GetComponent<RobotCommandExecute>();
        robotDeposit = GetComponent<RobotDeposit>();


        // 初期化処理を開始
        _base.battery_status = batteryData.battery_values[0];   // 一番弱いバッテリーを最初に装着させておく
        _base.StatusUp_EnergyMax();
        _base.currentEnergy = _base.maxEnergy;  // 充電をMaxにする
        _base.base_MaxEnergy = _base.maxEnergy; // 最大充電量を別の変数に格納しておく
        _eslider.maxValue = _base.maxEnergy;    // スライダーの最大値を個々の充電量を反映
        _eslider.value = _eslider.maxValue;     // スライダーのvalueをMaxに設定
        ChangeState(State.DoNon);               // ステートを何もしない状態にする

        // 最初に実行される初期化処理
        robotGather.Initialize(this);             // 収集用スクリプト
        robotMove.Initialize(this);               // 移動用スクリプト
        robotSearch.Initialize(this,robotMove);   // 資源探し用スクリプト
        robotBattery.Initialize(this);            // バッテリー用スクリプト
        robotDeposit.Initialize(this);

        _base.TotalStatus();                    // 総合ステータスを生成

        // 非アクティブ状態系は最後に実行
        gSlider.SetActive(false);   // 非アクティブ状態にする
    }

    void Set_SliderPosition()
    {
        if(eSlider != null)
        {
            // 充電量のスライダーの位置を更新
            eSlider.transform.position =
            new Vector3(transform.position.x, transform.position.y + 0.5f);
        }

        if(gSlider != null)
        {
            // 収集速度のスライダーの位置を更新
            gSlider.transform.position =
            new Vector3(transform.position.x, transform.position.y + 0.65f);
        }
    }

    public void SetRobotName(string _name)
    {
        _base.robotName = _name;
        robotname_text.text = _base.robotName;
    }

    public BaseStatus GetBaseStatus() { return _base; }
    public BaseStatus.Slot[] GetSlot() { return _base.slots; }

    public GameObject GetGatherSliderObject() { return gSlider; }
    public Collider2D GetHitInfo() { return hitInfo; }

    public Slider GetEnergySlider() { return _eslider; }

    public Animator GetRobotAnim() { return robot_anim; }

    public State GetCurrentStat() { return currentState; }
}
