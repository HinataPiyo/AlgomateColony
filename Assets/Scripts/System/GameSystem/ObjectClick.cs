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
            
            if(gameObject.CompareTag("Location"))
            {
                Debug.Log($"オブジェクト {name} がクリックされました。");
                FacilityManager.instance.CanvasEnabled(CanvasName.Location, true);
            }
            else if(gameObject.CompareTag("Warehouse"))
            {
                Debug.Log($"オブジェクト {name} がクリックされました。");
                FacilityManager.instance.CanvasEnabled(CanvasName.Warehouse, true);
            }
            else if(gameObject.CompareTag("ChargingBattery"))
            {
                Debug.Log($"オブジェクト {name} がクリックされました。");
                FacilityManager.instance.CanvasEnabled(CanvasName.BatteryRoom, true);
            }
        }

        if(gameObject.CompareTag("Robot"))
        {
            Debug.Log($"オブジェクト {name} がクリックされました。");
            RobotController _robot = GetComponent<RobotController>();
            RobotStatusPanelManager.instance.SetRobotStatus(_robot);
            FacilityManager.instance.CanvasEnabled(CanvasName.RobotStatus, true);
        }
    }
}