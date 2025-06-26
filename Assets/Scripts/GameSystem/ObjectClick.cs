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
            
            if(gameObject.CompareTag("location"))
            {
                Debug.Log($"オブジェクト {name} がクリックされました。");
                FacilityManager.instance.CanvasEnabled(CanvasName.Location, true);
                if(TutorialController.insrance.BigTaskNumber == 2)
                {
                    TutorialController.insrance.TutorialCheck(2, 0);
                }
            }
            else if(gameObject.CompareTag("warehouse"))
            {
                Debug.Log($"オブジェクト {name} がクリックされました。");
                FacilityManager.instance.CanvasEnabled(CanvasName.Warehouse, true);
            }
            else if(gameObject.CompareTag("chargingroom"))
            {
                Debug.Log($"オブジェクト {name} がクリックされました。");
                FacilityManager.instance.CanvasEnabled(CanvasName.BatteryRoom, true);
            }
            else if(gameObject.CompareTag("processingroom"))
            {
                Debug.Log($"オブジェクト {name} がクリックされました。");
                FacilityManager.instance.CanvasEnabled(CanvasName.Warkshop, true);
            }
        }

        if(gameObject.CompareTag("Robot"))
        {
            Debug.Log($"オブジェクト {name} がクリックされました。");
            RobotController _robot = GetComponent<RobotController>();
            RobotStatusPanelManager.instance.SetRobotStatus(_robot);
            FacilityManager.instance.CanvasEnabled(CanvasName.RobotStatus, true);
            TutorialController.insrance.TutorialCheck(0, 0);
        }
    }
}