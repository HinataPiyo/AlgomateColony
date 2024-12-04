using UnityEngine;
using UnityEngine.UI;

public class LocationCanvasController : MonoBehaviour
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
        fm.CanvasEnabled(CanvasName.Location, false);
    }
}
