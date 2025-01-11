using UnityEngine;
using UnityEngine.UI;

public class RobotSearch : MonoBehaviour
{
    RobotController robotCont;
    RobotMovement robotMove;

    [Header("資源を索敵する範囲")]
    [SerializeField] float infoRadius;
    const float INCREASE_RADIUS = 3f;
    [SerializeField] LayerMask layerMask;
    Collider2D hitInfo;
    bool hitMaterial;       // 資源が見つかっているか否か


    public void GameInit(RobotController _robotCont, RobotMovement _robotMove)
    {
        robotCont = _robotCont;
        robotMove = _robotMove;
    }

    /// <summary>
    /// 資源を見つかるまで探す
    /// </summary>
    public Collider2D Material_Search()
    {
        // 範囲の生成
        hitInfo = Physics2D.OverlapCircle(transform.position, infoRadius, layerMask);

        // 資源が範囲内にいたら
        if(hitInfo != null && hitMaterial == false)
        {
            infoRadius = 0;         // 0に初期化する
            hitMaterial = true;     // マテリアルが見つかりました

            // 資源の位置を保存しておく
            robotMove.Set_TargetPosition(hitInfo.transform.position, true);

            // 資源が見つかったらその資源に向かって移動
            robotCont.ChangeState(RobotController.State.Move);
        }
        else    // 資源が見つからなかったら
        {
            infoRadius += INCREASE_RADIUS;  // 半径を広げる
            hitMaterial = false;    // マテリアルが見つかりませんでした
        }

        return hitInfo;
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