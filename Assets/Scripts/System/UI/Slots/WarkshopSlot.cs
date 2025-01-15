using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WarkshopSlot : MonoBehaviour
{
    ProcessingController processingCont;
    public AccessorySO.PROCESSING_STATUS processing_status;
    AccessoryController accessoryCont;
    public AccessorySO.ACCESSORY_STATUS accessory_status;
    public Image icon;
    public TextMeshProUGUI name_text;
    public TextMeshProUGUI exp_text;
    public Button button;

    private void Start()
    {
        button.onClick.AddListener(OnButtonClick);
    }

    public void SetProcessing_NumAndScript(ProcessingController _script, AccessorySO.PROCESSING_STATUS p_status)
    {
        processingCont = _script;
        processing_status = p_status;

        icon.enabled = true;
        icon.sprite = processing_status.mateSO.icon;        // アイコンの設定
        name_text.text = processing_status.mateSO.materialName;     // 名前
        exp_text.text = processing_status.mateSO.exp;       // 加工の説明
    }

    public void SetAccessory_NumAndScript(AccessoryController _script, AccessorySO.ACCESSORY_STATUS a_status)
    {
        accessoryCont = _script;
        accessory_status = a_status;

        icon.enabled = true;
        icon.sprite = accessory_status.icon;                // アイコンの設定
        name_text.text = accessory_status.statusup_name;    // 名前
        exp_text.text = accessory_status.exp;               // 説明
    }


    public void OnButtonClick()
    {
        SoundManager.instance.PlayAudio("ButtonClick");
        
        if(processingCont != null)
        {
            // スロットを押した瞬間は作成ボタンを効かないようにする
            processingCont.Interactable_CreatButton(false);
            // Infoパネルに自身のスロット番号を渡す
            processingCont.SetProcessing_SelectsButton(processing_status);
        }

        if(accessoryCont != null)
        {
            // スロットを押した瞬間は作成ボタンを効かないようにする
            accessoryCont.Interactable_CreatButton(false);
            // Infoパネルに自身のスロット番号を渡す
            accessoryCont.SetAccessory_SelectsButton(accessory_status);
        }
    }
}
