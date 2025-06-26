using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetailPanel : MonoBehaviour
{
    [SerializeField] SystemControlSO scSO;
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI objectName_text;
    [SerializeField] TextMeshProUGUI useCommand_text;
    [SerializeField] GameObject detailPanel;
    [SerializeField] Animator detail_anim;
    [SerializeField] AnimEndFlag_AnimStat animStatFlag;

    bool playClosePanel;
    float waitTime = 2f;
    float progressTime;

    private void Start()
    {
        playClosePanel = true;
        animStatFlag.panelCloseFlag = true;
        detailPanel.SetActive(false);
    }

    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("location"))
            {
                SetDetailSlot(scSO.settingDetails[0].icon, scSO.settingDetails[0]._name, scSO.settingDetails[0].cmd);
                OpenPanel();
            }
            else if (hit.collider.CompareTag("warehouse"))
            {
                SetDetailSlot(scSO.settingDetails[1].icon, scSO.settingDetails[1]._name, scSO.settingDetails[1].cmd);
                OpenPanel();
            }
            else if (hit.collider.CompareTag("chargingroom"))
            {
                SetDetailSlot(scSO.settingDetails[2].icon, scSO.settingDetails[2]._name, scSO.settingDetails[2].cmd);
                OpenPanel();
            }
            else if (hit.collider.CompareTag("processingroom"))
            {
                SetDetailSlot(scSO.settingDetails[3].icon, scSO.settingDetails[3]._name, scSO.settingDetails[3].cmd);
                OpenPanel();
            }
            else if (hit.collider.CompareTag("rock"))
            {
                SetDetailSlot(scSO.settingDetails[4].icon, scSO.settingDetails[4]._name, scSO.settingDetails[4].cmd);
                OpenPanel();
            }
            else if (hit.collider.CompareTag("tree"))
            {
                SetDetailSlot(scSO.settingDetails[5].icon, scSO.settingDetails[5]._name, scSO.settingDetails[5].cmd);
                OpenPanel();
            }
            else if (hit.collider.CompareTag("ironore"))
            {
                SetDetailSlot(scSO.settingDetails[6].icon, scSO.settingDetails[6]._name, scSO.settingDetails[6].cmd);
                OpenPanel();
            }
            else if(hit.collider.CompareTag("NightLight"))
            {
                SetDetailSlot(scSO.settingDetails[7].icon, scSO.settingDetails[7]._name, scSO.settingDetails[7].cmd);
                OpenPanel();
            }

            progressTime = 0;
        }
        else
        {
            NonHitTimer();
        }
    }

    /// <summary>
    /// 一定時間後にクローズアニメーションが発火するようにする関数
    /// </summary>
    void NonHitTimer()
    {
        progressTime += Time.deltaTime;
        if (progressTime > waitTime)
        {
            if (animStatFlag.panelCloseFlag == false)
            {
                detail_anim.SetTrigger("Close");
                playClosePanel = true;
            }
        }
    }

    /// <summary>
    /// パネルを開くアニメーション
    /// </summary>
    void OpenPanel()
    {
        if (playClosePanel == true)
        {
            detailPanel.SetActive(true);
        }

        if (animStatFlag.panelCloseFlag == true)
        {
            detail_anim.SetTrigger("Open");
            animStatFlag.panelCloseFlag = false;
            playClosePanel = false;
        }
    }


    void SetDetailSlot(Sprite _icon, string _objName, string _useCmd)
    {
        icon.sprite = _icon;
        objectName_text.text = _objName;
        useCommand_text.text = _useCmd;
    }
}
