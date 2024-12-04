using UnityEngine;

public class CenterCollider : MonoBehaviour
{
    [SerializeField] Collider2D[] colInfo;        // コライダーを格納
    [SerializeField] LayerMask centerColLayer;  // レイヤー
    [SerializeField] float radius;              // 範囲

    private void Update()
    {
        // 最初に拠点の周りに生成される資源を破棄するため
        // 範囲の生成
        colInfo = Physics2D.OverlapCircleAll(transform.position, radius, centerColLayer);

        // 範囲内に入っていたら
        if(colInfo != null)
        {
            foreach(var _col in colInfo)
            {
                Debug.Log("破棄しました");
                // 範囲内に入った資源オブジェクトを破棄
                Destroy(_col.gameObject);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
