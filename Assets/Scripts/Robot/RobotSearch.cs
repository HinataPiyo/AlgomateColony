using UnityEngine;

/// <summary>
/// Moveコマンドが入力されたらターゲットにヒットするまで範囲を生成しターゲットを取得する
/// </summary>
[RequireComponent(typeof(RobotController))]
[RequireComponent(typeof(RobotMovement))]
public class RobotSearch : MonoBehaviour
{
    RobotController robotCont;
    RobotMovement robotMove;

    [Header("資源を索敵する範囲")]
    [SerializeField] float infoRadius;
    const float INCREASE_RADIUS = 0.6f;
    [SerializeField] LayerMask layerMask;
    Collider2D[] hitInfo;
    bool hitObject;       // 資源が見つかっているか否か

    void Awake()
    {
        robotCont = GetComponent<RobotController>();
        robotMove = GetComponent<RobotMovement>();
    }

    /// <summary>
    /// オブジェクトを見つかるまで探す
    /// </summary>
    public Collider2D Search(string _name)
    {

        // 範囲の生成
        hitInfo = Physics2D.OverlapCircleAll(transform.position, infoRadius, layerMask);

        foreach (Collider2D hit in hitInfo)
        {
            if (hitInfo != null && hitObject == false && hit.CompareTag(_name))
            {

                infoRadius = 0;         // 0に初期化する
                hitObject = true;       // マテリアルが見つかりました

                // オブジェクトの位置を保存しておく
                robotMove.Set_TargetPosition(hit.transform, true);

                // オブジェクトが見つかったらそのオブジェクトに向かって移動
                robotCont.ChangeState(RobotController.State.Move);
                Debug.Log("searchが完了しました");

                return hit;
            }
        }
        infoRadius += INCREASE_RADIUS;  // 半径を広げる
        hitObject = false;    // マテリアルが見つかりませんでした

        return null;
    }

    /// <summary>
    /// Gizmosを表示する
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, infoRadius);
    }
}