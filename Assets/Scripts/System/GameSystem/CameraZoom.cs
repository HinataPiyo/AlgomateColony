using UnityEngine;
using Unity.Cinemachine;

public class CameraZoom : MonoBehaviour
{
    public CinemachineCamera virtualCamera;     // Cinemachine Virtual Cameraの参照
    public float zoomSpeed = 5f;                // ズーム速度
    float minZoom = 1f;                  // 最小ズーム
    float maxZoom = 15f;                 // 最大ズーム

    void Update()
    {
        // キャンバスが開いていないとき
        if(FacilityManager.instance.GetIsOpenCanvas() == false)
        {
            // マウスホイールの取得
            float scroll = Input.GetAxis("Mouse ScrollWheel");

            // マウスホイールを動かしていたら
            if (scroll != 0)
            {
                // ChinemachineのlensValueの取得
                var lens = virtualCamera.Lens;

                // Orthographic Size（2Dカメラ）
                if (lens.Orthographic)
                {
                    // ホイールに合わせて、拡大縮小する
                    lens.OrthographicSize -= scroll * zoomSpeed;
                    // 最小値と最大値を超えないように設定
                    lens.OrthographicSize = Mathf.Clamp(lens.OrthographicSize, minZoom, maxZoom);
                }

                virtualCamera.Lens = lens;      // 更新を反映\
            }
        }
    }
}
