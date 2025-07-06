using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// RobotMovement はロボットの移動処理を管理するクラス
/// PathfindingSystem によって経路生成を行い、ObstacleAvoidanceHelper によって障害物回避を行う
/// </summary>
[RequireComponent(typeof(RobotController))]
public class RobotMovement : MonoBehaviour, IRobotInitializable
{
    [Header("ロボットの画像"), SerializeField] SpriteRenderer charImg;

    [Header("移動先にオブジェクトがないか確認する")]
    [SerializeField] float destinationRadius = 0.3f;
    [SerializeField] LayerMask destinationObjLayer;
    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] float rayLength = 0.5f;
    [SerializeField] float avoidDuration = 1.0f;

    RobotController robotCont;
    BaseStatus _base;
    Animator robot_anim;

    Transform targetPos;
    bool isMoving;

    // 経路点のキュー
    Queue<Vector2> _waypoints = new();
    // 最後に計算したターゲット位置
    Vector2 _calcedTargetPos = Vector2.positiveInfinity;
    float _elapsed;
    float _reCalcTime = 0.5f;
    // 最後の移動方向
    Vector2 lastMoveDir = Vector2.right;

    // 障害物回避フラグ・タイマー・方向
    bool isAvoiding = false;
    float avoidTimer = 0f;
    private Vector2 avoidDirection;

    ObstacleAvoidanceHelper avoidanceHelper;
    PathfindingSystem pathfinder;

    void Awake()
    {
        robotCont = GetComponent<RobotController>();

        _waypoints.Clear();
        avoidanceHelper = new ObstacleAvoidanceHelper();
        pathfinder = new PathfindingSystem(10);
    }

    public void Initialize()
    {
        _base = robotCont.GetBaseStatus();
        robot_anim = robotCont.GetRobotAnim();
    }

    /// <summary>
    /// ターゲット位置と移動フラグの設定
    /// </summary>
    public void Set_TargetPosition(Transform _target, bool flag)
    {
        targetPos = _target;
        isMoving = flag;
        _calcedTargetPos = Vector2.positiveInfinity;
    }

    /// <summary>
    /// ターゲットへの移動処理
    /// </summary>
    public void MoveToTarget()
    {
        // エネルギー切れやターゲット未設定時は移動しない
        if (_base.currentEnergy <= 0 && !_base.recharge_battery) { isMoving = false; return; }
        if (targetPos == null) { isMoving = false; return; }

        UpdatePathIfNeeded();
        if (ReachedDestination()) { StopAtDestination(); return; }

        Vector2 currentPos = transform.position;
        // 経路点に到達したら次の点へ
        if (_waypoints.Count > 0 && Vector2.Distance(_waypoints.Peek(), currentPos) < 0.05f)
            _waypoints.Dequeue();

        Vector2 moveDir;

        // 障害物回避中
        if (isAvoiding)
        {
            // 障害物があれば回避方向を再決定
            bool hit = Physics2D.Raycast(currentPos, avoidDirection, rayLength, obstacleLayer);
            if (hit)
            {
                avoidTimer = avoidDuration;
                Vector2? newDir = avoidanceHelper.FindBestFromBlockedDirection(currentPos, avoidDirection, rayLength, obstacleLayer);
                avoidDirection = newDir ?? avoidDirection;
            }

            moveDir = avoidDirection;
            avoidTimer -= Time.deltaTime;

            if (avoidTimer <= 0f || !avoidanceHelper.HasAnyObstacle(currentPos, rayLength, obstacleLayer))
            {
                isAvoiding = false;
                CalculatePath();
            }
        }
        else if (_waypoints.Count > 0)
        {
            // 経路に沿って移動
            Vector2 next = _waypoints.Peek();
            Vector2 forward = (next - currentPos).normalized;
            // 障害物があれば回避開始
            if (Physics2D.Raycast(currentPos, forward, rayLength, obstacleLayer))
            {
                isAvoiding = true;
                avoidTimer = avoidDuration;
                Vector2? newDir = avoidanceHelper.FindBestFromBlockedDirection(currentPos, forward, rayLength, obstacleLayer);
                avoidDirection = newDir ?? forward;
                moveDir = avoidDirection;
            }
            else moveDir = forward;
        }
        else return;

        // 実際の移動処理
        transform.position += (Vector3)(moveDir * _base.moveSpeed * Time.deltaTime);
        lastMoveDir = moveDir;

        // アニメーション・画像反転
        robot_anim.SetBool("Run", true);
        charImg.flipX = (moveDir.x < 0);
    }

    /// <summary>
    /// ターゲット位置が変化した場合や一定時間ごとに経路を再計算
    /// </summary>
    private void UpdatePathIfNeeded()
    {
        if ((Vector2)targetPos.position != _calcedTargetPos)
        {
            _elapsed += Time.deltaTime;
            if (_elapsed > _reCalcTime)
            {
                _elapsed = 0;
                CalculatePath();
                _calcedTargetPos = targetPos.position;
            }
        }
    }

    /// <summary>
    /// 目的地到達判定
    /// </summary>
    private bool ReachedDestination()
    {
        Collider2D _hit = Physics2D.OverlapCircle(transform.position, destinationRadius, destinationObjLayer);
        return _hit != null && _hit.transform == targetPos;
    }

    /// <summary>
    /// 目的地到達時の処理
    /// </summary>
    private void StopAtDestination()
    {
        robotCont.ChangeState(RobotController.State.DoNon);
        robot_anim.SetBool("Run", false);
        isMoving = false;

        // チュートリアル進行チェック
        if (targetPos.CompareTag("location"))
        {
            TutorialController.insrance.TutorialCheck(0, 3);
            TutorialController.insrance.BigTaskCheck(0);
        }

        robotCont.RobotCommandExecute.StateEndFlag = true;
    }

    /// <summary>
    /// 経路計算
    /// </summary>
    private void CalculatePath()
    {
        _waypoints = pathfinder.GeneratePath(transform.position, targetPos.position);
    }

    /// <summary>
    /// 移動停止
    /// </summary>
    public void DoNonPosition() => isMoving = false;

    /// <summary>
    /// 移動実行
    /// </summary>
    public void Moveing()
    {
        if (isMoving && targetPos != null) MoveToTarget();
        else
        {
            Debug.Log("ターゲットが見つかりませんでした。移動できません。");
            LogController.instance.SetLog(_base, "ターゲットが見つかりませんでした, 移動できません");
        }
    }

    /// <summary>
    /// デバッグ用Gizmos描画
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, destinationRadius);

        if (_waypoints != null)
        {
            Gizmos.color = Color.yellow;
            foreach (var point in _waypoints)
                Gizmos.DrawSphere(point, 0.05f);
        }

        if (avoidanceHelper != null)
            avoidanceHelper.DrawDebugRays(transform.position, lastMoveDir.normalized, rayLength, obstacleLayer);
    }
}
