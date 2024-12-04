using UnityEngine;
using UnityEngine.EventSystems;

// UI ではなく、オブジェクトをクリックしたときに呼ばれるスクリプト
// クリックされた時にイベントを起こしたいオブジェクトにアタッチ
public class ObjectClick : MonoBehaviour, IPointerClickHandler
{
    // クリックされたときに呼び出されるメソッド
    public void OnPointerClick(PointerEventData eventData)
    {
        // キャンバスが開いていないとき
        if(FacilityManager.instance.GetIsOpenCanvas() == false)
        {
            if(eventData.pointerPress.GetComponent<LocationController>() != null)
            {
                Debug.Log($"オブジェクト {name} がクリックされました。");
                FacilityManager.instance.CanvasEnabled(CanvasName.Location, true);
            }
        }
    }
}