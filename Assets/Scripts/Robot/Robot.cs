using System.Collections;
using UnityEngine;

using UnityEngine.UI;
// ロボット自身にアタッチするスクリプト
public class Robot : MonoBehaviour
{
    enum State
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
    [SerializeField] GameObject energySlider;
    GameObject eSlider;     // 位置を調整するため
    Slider _eslider;        // スライダーを更新するため
    [SerializeField] GameObject gatherSlider;
    GameObject gSlider;     // 位置を調整するため
    Slider _gslider;        // スライダーを更新するため

    [Header("ステータス")]
    Vector3 targetPos;      // 移動先の目標座標
    [SerializeField] float currentEnergy;       // 現在のエネルギー
    [SerializeField] bool chargeEnergy;
    [SerializeField] int currentRecharged;      // 現在の充電回数
    [SerializeField] bool needChangeBattery;    // 充電回数が最大値に到達したらバッテリー交換が必要ということを知らせる
    bool isMoving;          // 移動中かどうか
    bool hitMaterial;       // 資源が見つかっているか否か

    [Header("コンポーネント")]
    [SerializeField] Transform charaPos;
    [SerializeField] BaseStatus _base;
    Canvas sliderCanvas;
    Animator animator;

    [Header("資源を索敵する範囲")]
    [SerializeField] float infoRadius;
    const float INCREASE_RADIUS = 3f;
    [SerializeField] LayerMask layerMask;
    Collider2D hitInfo;
    [Header("移動先にオブジェクトがないか確認する")]
    float destinationRadius = 0.3f;
    [SerializeField] LayerMask destinationObjLayer;

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
    void ChangeState(State newState)
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
        //---------------------------------------------------------------
        // テスト(移動処理)
        // if(Input.GetKey(KeyCode.D))
        // {
        //     transform.position += new Vector3(1f, 0, 0) * 5 * Time.deltaTime;
        //     animator.SetBool("Run", true);
        // }
        // if(Input.GetKey(KeyCode.A))
        // {
        //     transform.position += new Vector3(-1f, 0, 0) * 5 * Time.deltaTime;
        // }
        // if(Input.GetKey(KeyCode.W))
        // {
        //     transform.position += new Vector3(0, 1f, 0) * 5 * Time.deltaTime;
        // }
        // if(Input.GetKey(KeyCode.S))
        // {
        //     transform.position += new Vector3(0, -1f, 0) * 5 * Time.deltaTime;
        // }

        ///---------------------------------------------------------------
        if(currentEnergy > 0)
        {
            if(currentState != State.DoNon)
            {
                // エネルギー消費
                currentEnergy -= Time.deltaTime;
                _eslider.value = currentEnergy;
            }
        }
        else
        {
            // 充電がなくなった場合のステートに移行
            ChangeState(State.NonEnergy);
        }
        

        // ステート管理
        stateTime += Time.deltaTime;
        switch(currentState)
        {
            case State.DoNon:
                if(stateEnter)
                {
                    Debug.Log(_base.robotName + "が待機状態になりました");
                    targetPos = transform.position;    // 位置を固定
                    isMoving = false;   // 移動を行わない
                    gSlider.SetActive(false);   // 非アクティブ状態にする
                }
                break;
            case State.Search:
                Search();               // 資源を探す
                break;
            case State.Move:
                // 移動可能なら かつ ターゲットが見つかっているなら
                if (isMoving && targetPos != null) MoveToTarget();                  // 移動処理を実行する
                else Debug.Log("ターゲットが見つかりませんでした。移動できません。");   // ターゲットが見つからなかった場合
                break;
            case State.GatherResource:      // 収集する
                if(stateEnter) StartCoroutine(GatherResource());       // 収集開始
                break;
            case State.Recharge:        // 充電する
                if(stateEnter)
                {
                    // 最大充電回数より低ければ
                    if(currentRecharged < _base.recharge_MAX)
                    {
                        // 充電回数を加算する
                        currentEnergy ++;
                        needChangeBattery = false;
                    }
                    else    // 充電回数が最大値になっていれば
                    {
                        // 充電回数が最大値に到達したらバッテリー交換が必要ということを知らせる
                        needChangeBattery = true;
                    }
                }

                // バッテリー交換が必要なければ充電する
                if(needChangeBattery == false) RechargeEnergy();
                break;
            case State.NonEnergy:
                if(stateEnter)
                {
                    currentEnergy = 0;
                    chargeEnergy = true;
                    gSlider.SetActive(false);   // 非アクティブ状態にする

                    Debug.Log(_base.robotName + "のエネルギーが不足しています！");
                    animator.SetBool("OutBattery", true);       // 充電不足のアニメーションを開始
                }
                break;
        }
        
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

    /// <summary>
    /// 生成時に行う処理(コンストラクタ)
    /// </summary>
    public void Initialize()
    {
        _base = new BaseStatus();   // 自身にクラスを生成
        _base.RandomStatusProc();   // ランダムでステータスを決める
        _base.GeneratSlots();       // インベントリを生成
        _base.TotalStatus();        // 総合ステータスを生成

        sliderCanvas = GameManager.instance.sliderCanvas;   // スライダーを表示するキャンバスを取得
        // スライダーの生成
        eSlider = Instantiate(      // バッテリーを示すスライダー
            energySlider,
            new Vector2(transform.position.x, transform.position.y + 0.5f),
            Quaternion.identity,
            sliderCanvas.transform
        );

        gSlider = Instantiate(      // 資源収集を行うときのスライダー
            gatherSlider,
            new Vector2(transform.position.x, transform.position.y + 0.6f),
            Quaternion.identity,
            sliderCanvas.transform
        );

        // コンポーネントの取得
        _eslider = eSlider.GetComponent<Slider>();
        _gslider = gSlider.GetComponent<Slider>();
        animator = GetComponentInChildren<Animator>();

        gSlider.SetActive(false);   // 非アクティブ状態にする


         // 初期化処理を開始
        currentEnergy = _base.maxEnergy;        // 充電をMaxにする
        _eslider.maxValue = _base.maxEnergy;    // スライダーの最大値を個々の充電量を反映
        _eslider.value = _eslider.maxValue;     // スライダーのvalueをMaxに設定
        ChangeState(State.DoNon);               // ステートを何もしない状態にする
    }

    /// <summary>
    /// 目標位置まで移動します
    /// </summary>
    private void MoveToTarget()
    {
        // 現在の充電量が0以下なら
        if (currentEnergy <= 0 && chargeEnergy == false)
        {
            isMoving = false;       // 移動停止
            return;     // ここで終了
        }
        // 範囲の生成
        Collider2D _hit = Physics2D.OverlapCircle(transform.position, destinationRadius, destinationObjLayer);
        if(_hit != null)    // 自身の周りにオブジェクトが存在していたら
        {
            // 中心座標
            Vector3 hitCenter = _hit.transform.position;
            Vector3 dir = (hitCenter - transform.position).normalized;
            
            // 一旦保留-------------------------------------------
            transform.position += -dir * 10f * Time.deltaTime;
            // ---------------------------------------------------
            animator.SetBool("Run", true);
        }
        else    // 自身の周りにオブジェクトが存在していなければ
        {
            // ターゲット位置へ移動
            transform.position = Vector3.MoveTowards(transform.position, targetPos, _base.moveSpeed * Time.deltaTime);

            if(transform.position.x >= targetPos.x) charaPos.localScale = new Vector3(-1,1);
            else charaPos.localScale = new Vector3(1,1);

            animator.SetBool("Run", true);
        }


        // 距離が近ければ移動終了
        if (Vector3.Distance(transform.position, targetPos) < 1f)
        {
            // 収集ステートに移行
            ChangeState(State.GatherResource);

            animator.SetBool("Run", false);
            isMoving = false;       // 移動停止
        }
    }

    /// <summary>
    /// 資源を見つかるまで探す
    /// </summary>
    void Search()
    {
        // 範囲の生成
        hitInfo = Physics2D.OverlapCircle(transform.position, infoRadius, layerMask);

        // 資源が範囲内にいたら
        if(hitInfo != null && hitMaterial == false)
        {
            infoRadius = 0;                         // 0に初期化する
            isMoving = true;                        // 移動開始
            hitMaterial = true;                     // マテリアルが見つかりました
            targetPos = hitInfo.transform.position; // 資源の位置を保存しておく
            ChangeState(State.Move);                // 資源が見つかったらその資源に向かって移動
        }
        else
        {
            infoRadius += INCREASE_RADIUS;  // 半径を広げる
            hitMaterial = false;    // マテリアルが見つかりませんでした
        }
    }

    /// <summary>
    /// エネルギーを充電します
    /// </summary>
    void RechargeEnergy()
    {
        // Mathf.Minは最大値である"maxEnergy"を超えないようにしている
        currentEnergy = Mathf.Min(currentEnergy + 1, _base.maxEnergy);
    }

    /// <summary>
    /// 資源を収集します
    /// </summary>
    IEnumerator GatherResource()
    {
        bool checkHitInfo = true;
        Debug.Log(_base.robotName + "が資源を収集しています。");
        // 収集処理
        while(checkHitInfo == true && chargeEnergy == false)
        {
            // 収集時間を表すスライダーを表示する
            if(hitInfo != null)     // 資源が存在していれば
            {
                // 全てのスロットがスタックMaxだった場合
                if(_base.CheckAllStackMax() == BaseStatus.SLOT_STACK.ALL_STACK_MAX)
                {
                    Debug.Log(_base.robotName + "のインベントリがいっぱいです。");
                    ChangeState(State.DoNon);   // 何もしない状態に遷移
                    gSlider.SetActive(false);   // 非アクティブ状態にする
                    yield break;                // コルーチンを抜ける
                }

                gSlider.SetActive(true);                    // アクティブ状態にする
                float progressTime = _base.gatherRate;      // 個々の収集速度を設定する
                _gslider.maxValue = progressTime;           // マックススライダーに反映する
                while(progressTime > 0f)                    // 経過時間が0より大きかったら
                {
                    progressTime -= Time.deltaTime;         // 経過時間を更新する
                    _gslider.value = progressTime;          // 経過時間をスライダーに反映する

                    if(hitInfo == null) 
                    {
                        ChangeState(State.DoNon);
                        yield break;                // コルーチンを抜ける
                    }
                    
                    yield return null;                      // 次のフレームまで待機
                }

                // 一度スクリプトを取得する
                BaseMaterial _baseMate = hitInfo?.GetComponent<BaseMaterial>();
                // 資源にダメージを与える
                _baseMate.TakeDamage(_base.gatherSterngth);
                
                // 収集している資源のシリアル番号が同一か確かめる
                foreach(var _slot in _base.slots)
                {
                    // スタック数がMaxではなかったら
                    if(_base.CheckStackMax() == BaseStatus.SLOT_STACK.STACK_TRUE)
                    {
                        // スロット内に同一のシリアル番号がなければ
                        if(_slot.mateSO?.serialNum != _baseMate.mateSO.serialNum)
                        {
                            // 空のスロットを見つける
                            if(_slot.mateSO == null)
                            {
                                // スロット(インベントリ)に格納する
                                _slot.mateSO = _baseMate.mateSO;
                                // スタック数を増やす
                                _slot.itemStackAmount += _baseMate.GetAmo();
                                break;      // foreachを抜ける
                            }
                        }
                        else    // 同一のシリアル番号が存在したら
                        {
                            // スタック数を増やす
                            _slot.itemStackAmount += _baseMate.GetAmo();
                            break;      // foreachを抜ける
                        }
                    }
                    // スタックがMaxだった場合
                    else if(_base.CheckStackMax() == BaseStatus.SLOT_STACK.STACK_MAX)
                    {

                        // 空のスロットを探す
                        if(_slot.mateSO == null)
                        {
                            // スロット(インベントリ)に格納する
                            _slot.mateSO = _baseMate.mateSO;
                            // スタック数を増やす
                            _slot.itemStackAmount += _baseMate.GetAmo();
                            break;      // foreachを抜ける
                        }             
                    }
                }
            }
            else
            {
                Debug.Log($"{_base.robotName}が資源を収集完了しました。");
                gSlider.SetActive(false);   // 非アクティブ状態にする
                ChangeState(State.DoNon);   // 何もしない状態に遷移
                checkHitInfo = false;       // hitInfoが存在しているか否か
            }

            yield return null;              // 次のフレームまで待機
        }

        yield break;        // コルーチンを抜ける
    }


    /// <summary>
    /// Gizmosを表示する
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, infoRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, destinationRadius);
    }

    public BaseStatus GetBaseStatus() { return _base; }
    public BaseStatus.Slot[] GetSlot() { return _base.slots; }
    public float GetCurrentEnergy() { return currentEnergy; }
    public int GetCurrentRecharge() { return currentRecharged; }
}
