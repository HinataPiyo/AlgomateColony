using UnityEngine;

public class RobotMovement : MonoBehaviour
{
    RobotController robotCont;
    BaseStatus _base;
    Animator robot_anim;
    
    Transform targetPos;      // 移動先の目標座標
    bool isMoving;          // 移動中かどうか
    [Header("ロボットの画像"), SerializeField] SpriteRenderer charImg;

    [Header("移動先にオブジェクトがないか確認する")]
    [SerializeField] float destinationRadius = 0.3f;
    [SerializeField] LayerMask destinationObjLayer;



    public void GameInit(RobotController _robotCont)
    {
        robotCont = _robotCont;
        _base = robotCont.GetBaseStatus();
        robot_anim = robotCont.GetRobotAnim();
    }

    public void Set_TargetPosition(Transform _target, bool flag)
    {
        targetPos = _target;
        isMoving = flag;
    }

    /// <summary>
    /// 目標位置まで移動します
    /// </summary>
    public void MoveToTarget()
    {
        // 現在の充電量が0以下なら
        if (_base.currentEnergy <= 0 && _base.recharge_battery == false)
        {
            isMoving = false;       // 移動停止
            return;     // ここで終了
        }
        // 範囲の生成
        Collider2D _hit = Physics2D.OverlapCircle(transform.position, destinationRadius, destinationObjLayer);
        if(_hit != null)    // 自身の周りにオブジェクトが存在していたら
        {
            // 距離が近ければ移動終了
            if (_hit.CompareTag(robotCont.ObjectName)) // Vector3.Distance(transform.position, targetPos) < 2.5f)
            {
                // 収集ステートに移行
                robotCont.ChangeState(RobotController.State.DoNon);
                
                robot_anim.SetBool("Run", false);
                isMoving = false;       // 移動停止

                robotCont.Get_RobotCommandExecute.StateEndFlag = true;      // 移動処理が終了したときにフラグを立てる
                return;
            }

            // 中心座標
            Vector3 hitCenter = _hit.transform.position;
            Vector3 dir = (hitCenter - transform.position).normalized;
            
            // 一旦保留-------------------------------------------
            transform.position += -dir * 1f * Time.deltaTime;
            // ---------------------------------------------------
            robot_anim.SetBool("Run", true);

            return;
        }
        else    // 自身の周りにオブジェクトが存在していなければ
        {
            // ターゲット位置へ移動
            transform.position = Vector3.MoveTowards(transform.position, targetPos.position, _base.moveSpeed * Time.deltaTime);

            // 反転処理
            if(transform.position.x >= targetPos.position.x) charImg.flipX = true;
            else charImg.flipX = false;

            robot_anim.SetBool("Run", true);
        }
    }

    /// <summary>
    /// 何もしないステートの時に処理
    /// </summary>
    public void DoNonPosition()
    {
        // 移動を行わない
        isMoving = false;
    }

    /// <summary>
    /// ロボットの移動処理
    /// </summary>
    public void Moveing()
    {
        // 移動可能なら かつ ターゲットが見つかっているなら
        if (isMoving && targetPos != null)
        {
            // 移動処理を実行する
            MoveToTarget();
        }
        else
        {
            Debug.Log("ターゲットが見つかりませんでした。移動できません。");   // ターゲットが見つからなかった場合
            LogController.instance.SetLog(_base, "ターゲットが見つかりませんでした, 移動できません");
        }
    }

    /// <summary>
    /// Gizmosを表示する
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, destinationRadius);
    }
}