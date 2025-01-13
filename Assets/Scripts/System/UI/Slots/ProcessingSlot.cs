using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProcessingSlot : MonoBehaviour
{
    ProcessingController processingCont;
    public AccessorySO.PROCESSING_STATUS processing_status;
    public Image icon;
    public TextMeshProUGUI name_text;
    public TextMeshProUGUI exp_text;
    public Button button;

    private void Start()
    {
        button.onClick.AddListener(OnButtonClick);

        // icon.enabled = false;
        // name_text.text = "";
        // exp_text.text = "";
    }

    public void Set_NumAndScript(ProcessingController _script, AccessorySO.PROCESSING_STATUS p_status)
    {
        processingCont = _script;
        processing_status = p_status;

        icon.enabled = true;
        icon.sprite = processing_status.mateSO.icon;        // アイコンの設定
        name_text.text = processing_status.mateSO.materialName;     // 名前
        exp_text.text = processing_status.mateSO.exp;       // 加工の説明

    }

    public void OnButtonClick()
    {
        // Infoパネルに自身のスロット番号を渡す
        processingCont.SetProcessing_SelectsButton(processing_status);
    }
}
