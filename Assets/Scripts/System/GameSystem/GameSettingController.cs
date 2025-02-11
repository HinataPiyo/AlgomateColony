using UnityEngine;
using UnityEngine.UI;

public class GameSettingController : MonoBehaviour
{
    [SerializeField] Button backButton;
    [SerializeField] Transform cangePanel_parent;
    [SerializeField] GameObject gameSetting_obj;
    [SerializeField] GameObject audioSetting_obj;
    [SerializeField] GameObject commandList_obj;

    ButtonSlotVarticalHorizontal[] changePanel_slot;
    
    void Start()
    {
        backButton.onClick.AddListener(BackButtonOnClick);
        changePanel_slot = cangePanel_parent.GetComponentsInChildren<ButtonSlotVarticalHorizontal>();

        // パネルを変える際に押すボタンの設定
        for(int pp = 0; pp < changePanel_slot.Length; pp++)
        {
            changePanel_slot[pp].slotNo = pp;
            changePanel_slot[pp].Initialize_GameSetting(this);

            switch(pp)
            {
                case 0:
                    changePanel_slot[pp].button_name.text = "ゲーム設定";
                    break;
                case 1:
                    changePanel_slot[pp].button_name.text = "音量設定";
                    break;
                case 2:
                    changePanel_slot[pp].button_name.text = "コマンド一覧";
                    break;
            }
        }

        gameSetting_obj.SetActive(true);
        audioSetting_obj.SetActive(false);
        commandList_obj.SetActive(false);
    }

    void Update()
    {
        
    }

    /// <summary>
    /// 選択されたボタンによって変える
    /// </summary>
    /// <param name="_num"></param>
    public void ChangePanel(int _num)
    {
        switch(_num)
        {
            case 0:
                gameSetting_obj.SetActive(true);

                audioSetting_obj.SetActive(false);

                commandList_obj.SetActive(false);
                break;
            case 1:
                gameSetting_obj.SetActive(false);

                audioSetting_obj.SetActive(true);

                commandList_obj.SetActive(false);
                break;
            case 2:
                gameSetting_obj.SetActive(false);

                audioSetting_obj.SetActive(false);

                commandList_obj.SetActive(true);
                break;
        }
    }



    /// <summary>
    /// Backボタンを押したときの処理
    /// </summary>
    void BackButtonOnClick()
    {
        FacilityManager.instance.CanvasEnabled(CanvasName.Setting, false);
    }
}
