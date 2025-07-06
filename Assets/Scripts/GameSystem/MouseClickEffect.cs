using UnityEngine;

public class MouseClickEffect : MonoBehaviour
{
    [SerializeField] GameObject effect_circle;
    [SerializeField] Transform effectCanvas;

    void Update()
    {
        // 左クリックをしたら
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 pos = Input.mousePosition;
            GameObject obj = Instantiate(effect_circle, pos, Quaternion.identity, effectCanvas);
            Destroy(obj, 1f);
        }
    }
}