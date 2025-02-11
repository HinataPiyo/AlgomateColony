using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WarkshopSlot : MonoBehaviour
{
    ProcessingController processingCont;
    public AccessorySO.PROCESSING_STATUS processing_status;
    AccessoryController accessoryCont;
    public AccessorySO.NEED_ACCESSORY_STATUS accessory_status;
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI name_text;
    [SerializeField] TextMeshProUGUI exp_text;
    [SerializeField] TextMeshProUGUI statusUp_value;
    [SerializeField] TextMeshProUGUI statusUp_name;
    [SerializeField] Button button;

    private void Start()
    {
        button.onClick.AddListener(OnButtonClick);
    }

    public void SetProcessing_NumAndScript(ProcessingController _script, AccessorySO.PROCESSING_STATUS p_status)
    {
        processingCont = _script;
        processing_status = p_status;

        icon.enabled = true;
        icon.preserveAspect = true;     // 元画像に合わせる
        icon.sprite = processing_status.mateSO.icon;        // アイコンの設定
        name_text.text = processing_status.mateSO.materialName;     // 名前
        exp_text.text = processing_status.mateSO.exp;       // 加工の説明

        statusUp_name.text = "";
        statusUp_value.text = "";
    }

    public void SetAccessory_NumAndScript(AccessoryController _script, AccessorySO.NEED_ACCESSORY_STATUS a_status)
    {
        accessoryCont = _script;
        accessory_status = a_status;

        icon.enabled = true;
        icon.sprite = accessory_status.acceData.icon;                // アイコンの設定
        name_text.text = accessory_status.acceData._name;            // 名前
        exp_text.text = accessory_status.acceData.exp;               // 説明

        statusUp_name.text = $"- {accessory_status.acceData.statusup_name}";
        statusUp_value.text = $"+ {accessory_status.acceData.statusup_value}";
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
