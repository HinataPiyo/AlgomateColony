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
    [SerializeField] BaseStatus _base;

    RobotGather robotGather;
    RobotMovement robotMove;
    RobotSearch robotSearch;
    RobotBattery robotBattery;

    Canvas sliderCanvas;
    Animator robot_anim;

    // Searchで取得してきたhitInfoを格納しておく為の変数
    Collider2D hitInfo;

    [Header("Test")]
    [SerializeField] bool flag;


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
        // テスト
        if(flag == true)
        {   
            ChangeState(State.Search);
            flag = false;
        }

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
                    robotMove.DoNonPosition();
                    gSlider.SetActive(false);   // 非アクティブ状態にする
                }
                break;
            case State.Search:
                // 資源を探す
                hitInfo = robotSearch.Material_Search();
                break;
            case State.Move:
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
        GameInit();
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
        _base.TotalStatus();                    // 総合ステータスを生成

        GameInit();
    }

    void GameInit()
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


         // 初期化処理を開始
        _base.currentEnergy = _base.maxEnergy;        // 充電をMaxにする
        _eslider.maxValue = _base.maxEnergy;    // スライダーの最大値を個々の充電量を反映
        _eslider.value = _eslider.maxValue;     // スライダーのvalueをMaxに設定
        ChangeState(State.DoNon);               // ステートを何もしない状態にする

        // 最初に実行される初期化処理
        robotGather.GameInit(this);             // 収集用スクリプト
        robotMove.GameInit(this);               // 移動用スクリプト
        robotSearch.GameInit(this,robotMove);   // 資源探し用スクリプト
        robotBattery.GameInit(this);            // バッテリー用スクリプト

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
            new Vector3(transform.position.x, transform.position.y + 0.6f);
        }
    }


    public BaseStatus GetBaseStatus() { return _base; }
    public BaseStatus.Slot[] GetSlot() { return _base.slots; }

    public GameObject GetGatherSliderObject() { return gSlider; }
    public Collider2D GetHitInfo() { return hitInfo; }

    public Slider GetEnergySlider() { return _eslider; }

    public Animator GetRobotAnim() { return robot_anim; }

    public State GetCurrentStat() { return currentState; }
}
