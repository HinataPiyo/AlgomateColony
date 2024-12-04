using UnityEngine;
using UnityEngine.UI;

public class SettingController : MonoBehaviour
{
    [SerializeField] Button backButton;
    FacilityManager fm;
    void Start()
    {
        fm = GetComponent<FacilityManager>();

        backButton.onClick.AddListener(BackButtonOnClick);
    }

    void Update()
    {
        
    }

    /// <summary>
    /// Backボタンを押したときの処理
    /// </summary>
    void BackButtonOnClick()
    {
        fm.CanvasEnabled(CanvasName.Setting, false);
    }
}
