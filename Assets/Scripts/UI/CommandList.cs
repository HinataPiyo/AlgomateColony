using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class CommandList : MonoBehaviour
{
    [SerializeField] GameObject pcsCmdSlot_prefab;
    [SerializeField] Transform slotParent;
    [SerializeField] Button back_button;
    

    [Header("詳細パネル")]
    [SerializeField] GameObject cmdDetailPanel;
    [SerializeField] TextMeshProUGUI commandExp_text;
    [SerializeField] TextMeshProUGUI canUseCommands_text;
    [SerializeField] TextMeshProUGUI commandName_text;

    void Start()
    {
        cmdDetailPanel.SetActive(false);
        back_button.onClick.AddListener(BackButtonOnClick);

        for (int ii = 0; ii < CommandDiscription.CommandDetails.Length; ii++)
        {
            GameObject obj = Instantiate(pcsCmdSlot_prefab, slotParent);
            obj.GetComponent<PcsCommandSlot>().SetSlot(ii + 1, CommandDiscription.CommandDetails[ii], this);
        }
    }

    /// <summary>
    /// 詳細ボタンがされた時の処理
    /// </summary>
    /// <param name="_detail"></param>
    public void SetDetail(CommandDetail _detail)
    {
        cmdDetailPanel.SetActive(true);
        // コマンドの説明
        commandExp_text.text = _detail.exp;
        // カンマ区切りで表示
        canUseCommands_text.text = string.Join(",", _detail.canUseCommads);
        commandName_text.text = _detail.commandName;
    }

    void BackButtonOnClick()
    {
        SoundManager.instance.PlayAudio("SelectObject");
        cmdDetailPanel.SetActive(false);
    }
}
