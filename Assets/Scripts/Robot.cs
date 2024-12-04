using UnityEngine;
// ロボット自身にアタッチするスクリプト
public class Robot : MonoBehaviour
{
    

    enum State
    {
        DoNon,      // 何もしない
        Move,       // 移動する
        Recharge,   // 充電する
    }

    [Header("ステート")]
    State currentState;
    bool stateEnter;
    float stateTime = 0;
    
    [Header("スライダー")]
    [SerializeField] GameObject energySlider;
    GameObject eSlider;

    [Header("ステータス")]
    Vector3 targetPosition;     // 移動先の目標座標
    float currentEnergy;        // 現在のエネルギー
    bool isMoving;              // 移動中かどうか
    bool hitMaterial;

    [Header("コンポーネント")]
    [SerializeField] BaseStatus bStatus;
    RobotDataSO rData;          // ロボットの設定データ
    Canvas sliderCanvas;
    Animator animator;

    [Header("プレイヤーを索敵する範囲")]
    [SerializeField] float infoRadius;
    [SerializeField] LayerMask layerMask;
    [SerializeField] Collider2D hitInfo;


#region ステート管理
    void LateUpdate()
    {
        if(stateTime != 0)
        {
            stateEnter = false;
        }
    }

    // ステートの切り替え
    void ChangeState(State newState)
    {
        currentState = newState;
        stateEnter = true;
        stateTime = 0;
    }
#endregion

    private void Update()
    {
        
        //---------------------------------------------------------------
        // テスト
        if(Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(1f, 0, 0) * 5 * Time.deltaTime;
            animator.SetBool("Run", true);
        }
        if(Input.GetKey(KeyCode.A))
        {
            transform.position += new Vector3(-1f, 0, 0) * 5 * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.W))
        {
            transform.position += new Vector3(0, 1f, 0) * 5 * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.S))
        {
            transform.position += new Vector3(0, -1f, 0) * 5 * Time.deltaTime;
        }

        //---------------------------------------------------------------
        hitInfo = Physics2D.OverlapCircle(transform.position, infoRadius, layerMask);

        // プレイヤーが範囲内にいたら
        if(hitInfo != null)
        {
            hitMaterial = true;     // マテリアルが見つかりました
        }
        else
        {
            hitMaterial = false;    // マテリアルが見つかりませんでした
        }
        ///---------------------------------------------------------------
        // ステート管理
        stateTime += Time.deltaTime;
        switch(currentState)
        {
            case State.DoNon:
                if(stateEnter)
                {
                    targetPosition = transform.position;    // 位置を固定
                    isMoving = false;   // 移動を行わない
                }
                break;
            case State.Move:
                if(stateEnter)
                {
                    isMoving = true;    // 移動開始
                }

                // 移動可能なら
                if (isMoving)
                {
                    MoveToTarget();     // 移動処理を実行する
                }
                break;
        }
        
        if(eSlider != null)
        {
            // スライダーの位置を更新
            eSlider.transform.position =
            new Vector3(transform.position.x, transform.position.y + 0.5f);
        }

    }

    /// <summary>
    /// 生成時に行う処理
    /// </summary>
    /// <param name="data"></param>
    public void Initialize(RobotDataSO data)
    {
        rData = data;       // 自身のスクリプトにSOを保存
        bStatus = new BaseStatus();
        animator = GetComponentInChildren<Animator>();
        InitializeRobot();      // 初期化処理を開始
    }
    
    /// <summary>
    /// ロボットの初期化処理
    /// </summary>
    private void InitializeRobot()
    {
        bStatus.RandomStatusProc();                         // ランダムでステータスを決める
        sliderCanvas = GameManager.instance.sliderCanvas;   // スライダーを表示するキャンバスを取得
        currentEnergy = bStatus.maxEnergy;                  // 充電をMaxにする
        rData.robots.Add(this);                             // リストに格納する
        ChangeState(State.DoNon);                           // ステートを何もしない状態にする

        // スライダーの生成
        eSlider = Instantiate(
            energySlider,
            new Vector2(transform.position.x, transform.position.y + 0.5f),
            Quaternion.identity,
            sliderCanvas.transform
        );
    }

    /// <summary>
    /// 目標位置を設定して移動開始
    /// </summary>
    public void SetTargetPosition(Vector3 position)
    {
        targetPosition = position;      // 目標位置の設定
        ChangeState(State.Move);        // ステートを移動処理に移行
    }

    /// <summary>
    /// 目標位置まで移動します
    /// </summary>
    private void MoveToTarget()
    {
        if (currentEnergy <= 0)
        {
            Debug.Log(bStatus.robotName + "のエネルギーが不足しています！");
            animator.SetBool("OutBattery", true);
            isMoving = false;       // 移動停止
            return;
        }

        // ターゲット位置へ移動
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, bStatus.moveSpeed * Time.deltaTime);

        // 距離が近ければ移動終了
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            animator.SetBool("Run", false);
            isMoving = false;       // 移動停止
        }

        // エネルギー消費
        currentEnergy -= Time.deltaTime;
    }

    /// <summary>
    /// エネルギーを充電します
    /// </summary>
    public void RechargeEnergy(float amount)
    {
        // Mathf.Minは最大値である"maxEnergy"を超えないようにしている
        currentEnergy = Mathf.Min(currentEnergy + amount, bStatus.maxEnergy);
    }

    /// <summary>
    /// 資源を収集します
    /// </summary>
    public void GatherResource()
    {
        Debug.Log(bStatus.robotName + "が資源を収集しています！");
        // 実際の収集処理は別途実装
    }
}
