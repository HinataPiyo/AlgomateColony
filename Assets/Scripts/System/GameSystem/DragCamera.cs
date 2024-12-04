using UnityEngine;

public class DragCamera : MonoBehaviour
{
    [SerializeField] Transform cameraTransform;
    private Vector3 dragOrigin; // ドラッグ開始位置
    private bool isDragging = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)    // 左クリック
        && FacilityManager.instance.GetIsOpenCanvas() == false)     // キャンバスが開いていないとき
        {
            // クリックした位置をスクリーン座標からワールド座標に変換
            dragOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            isDragging = true;      // ドラッグ開始の合図
        }

        // ドラッグ中
        if (Input.GetMouseButton(0) && isDragging)
        {
            // クリックした位置を"isDragging"が"true"の時、スクリーン座標からワールド座標に変換
            Vector3 currentPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // 最初にクリックした位置と現在のカーソルの位置の距離を計算
            Vector3 difference = dragOrigin - currentPoint;

            // ターゲットオブジェクトを移動
            cameraTransform.position += difference;

            dragOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0)) // マウスボタンを離したとき
        {
            isDragging = false;
        }
    }
}
