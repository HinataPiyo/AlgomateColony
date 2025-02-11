using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PcsCommandSlot : MonoBehaviour
{
    CommandList commandList;
    [SerializeField] Button button;
    [SerializeField] TextMeshProUGUI commandName_text;

    [Header("コマンドの詳細"), SerializeField] ComanndsDetail cmd_Detail;

    private void Start()
    {
        button.onClick.AddListener(DetailButtonClick);
    }


    public void SetSlot(int ii, ComanndsDetail _cmd_detail, CommandList cmdList)
    {
        if(commandList == null)
        {
            commandList = cmdList;
        }

        cmd_Detail = _cmd_detail;
        commandName_text.text = $"{ii}. {_cmd_detail.commandName}";
    }

    /// <summary>
    /// 詳細ボタンが押されたら
    /// </summary>
    void DetailButtonClick()
    {
        commandList.SetDetail(cmd_Detail);
    }

}

[System.Serializable]
public class ComanndsDetail
{
    public string commandName;
    [TextArea(5,10)] public string exp;     // 説明
    public string[] canUseCommads;     // 使用できる引数
}