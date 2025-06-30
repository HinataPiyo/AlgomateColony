using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WarkshopSlot : MonoBehaviour
{
    ProcessingController processingCont;
    public MaterialSO processingData;
    AccessoryController accessoryCont;
    public AccessoryData accessoryData;
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

    public void SetProcessing_NumAndScript(ProcessingController _script, MaterialSO _p_data)
    {
        processingCont = _script;
        processingData = _p_data;

        icon.enabled = true;
        icon.preserveAspect = true;     // 元画像に合わせる
        icon.sprite = processingData.icon;        // アイコンの設定
        name_text.text = processingData.materialName;     // 名前
        exp_text.text = processingData.exp;       // 加工の説明

        statusUp_name.text = "";
        statusUp_value.text = "";
    }

    public void SetAccessory_NumAndScript(AccessoryController _script, AccessoryData _a_data)
    {
        accessoryCont = _script;
        accessoryData = _a_data;

        icon.enabled = true;
        icon.sprite = accessoryData.icon;                // アイコンの設定
        name_text.text = accessoryData._name;     // 名前
        exp_text.text = accessoryData.exp;               // 説明

        statusUp_name.text = $"- {accessoryData.statusup_name}";
        statusUp_value.text = $"+ {accessoryData.statusup_value}";
    }


    public void OnButtonClick()
    {
        SoundManager.instance.PlayAudio("ButtonClick");
        
        if(processingCont != null)
        {
            // スロットを押した瞬間は作成ボタンを効かないようにする
            processingCont.Interactable_CreatButton(false);
            // Infoパネルに自身のスロット番号を渡す
            processingCont.SetProcessing_SelectsButton(processingData);
        }

        if(accessoryCont != null)
        {
            // スロットを押した瞬間は作成ボタンを効かないようにする
            accessoryCont.Interactable_CreatButton(false);
            // Infoパネルに自身のスロット番号を渡す
            accessoryCont.SetAccessory_SelectsButton(accessoryData);
        }
    }
}
