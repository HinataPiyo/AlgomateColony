using UnityEngine;

// パネルのドラッグを管理するクラス
public class PanelDrag : MonoBehaviour
{
    private RectTransform rectTransform;
    private Vector2 dragOffset; // ドラッグ開始時のオフセット
    private bool isDragging = false;

    [SerializeField] private float dragSpeed = 1f; // ドラッグ速度の調整

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        // ドラッグ開始
        if (Input.GetMouseButtonDown(0)     // マウス左クリック
        && FacilityManager.instance.GetIsOpenCanvas() == true)      // キャンバスが表示されていたら
        {
            // マウス位置がパネル上にあるか確認
            // RectangleContainsScreenPoint(RectTransform, screenPoint, Camera)
            if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition))
            {
                Vector2 localMousePos;
                // 親RectTransformのローカル座標系でのマウス位置を取得
                // UIオブジェクトのローカル座標計算を行う際には、親RectTransformが必要になるため
                RectTransform parentRect = rectTransform.parent as RectTransform;
                // ScreenPointToLocalPointInRectangle(RectTransform, screenPoin, Camera, localPoint)
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    parentRect,             // RectTransform
                    Input.mousePosition,    // スクリーン空間位置
                    null, // Screen Space - Overlayの場合はnull。Screen Space - Cameraの場合はカメラを指定。
                    out localMousePos))     // RectTransform のローカル空間でのポイント。(out引数として変換後のローカル座標を格納)
                {
                    // ドラッグ開始時のオフセットを計算
                    dragOffset = rectTransform.anchoredPosition - localMousePos;
                    isDragging = true;
                }
            }
        }

        // ドラッグ中
        if (Input.GetMouseButton(0) && isDragging && FacilityManager.instance.GetIsOpenCanvas())
        {
            Vector2 localMousePos;
            RectTransform parentRect = rectTransform.parent as RectTransform;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentRect,
                Input.mousePosition,
                null, // Screen Space - Overlayの場合はnull。Screen Space - Cameraの場合はカメラを指定。
                out localMousePos))
            {
                // 新しいアンカー位置を計算
                Vector2 newAnchoredPos = localMousePos + dragOffset;

                // 滑らかにドラッグ速度を適用
                rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, newAnchoredPos, dragSpeed * Time.deltaTime);
            }
        }

        // ドラッグ終了
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }
}
