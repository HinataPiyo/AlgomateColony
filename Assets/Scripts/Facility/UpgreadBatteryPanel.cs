using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgreadBatteryPanel : MonoBehaviour
{
    UpdateTime_Class updateTime = new UpdateTime_Class();

    [Header("Text")]
    [SerializeField] TextMeshProUGUI robotAmo_text;

    [Header("ロボットを表示させるPrefab")]
    [SerializeField] Transform slot_parent;
    [SerializeField] GameObject robotSlot_prefab;

    [Header("現在のスロット数")]
    [SerializeField] List<GameObject> slot_lists = new List<GameObject>();


    private void Start()
    {
        Check_RobotAmount();        // ロボットの出現数をテキストに反映させるs
        Creat_Slot();               // スロットを生成させる
    }

    private void Update()
    {
        // 数秒に一回更新されるようにする
        if(updateTime.UpdateTime() == true)
        {
            Check_RobotAmount();        // ロボットの出現数をテキストに反映させる
            Creat_Slot();               // スロットを生成させる
        }
    }

    /// <summary>
    /// ロボットの出現数をテキストに反映させる
    /// </summary>
    void Check_RobotAmount()
    {
        robotAmo_text.text = $"{GameManager.instance.RobotList.Count}";
    }

    /// <summary>
    /// スロットを生成させる
    /// </summary>
    void Creat_Slot()
    {
        // ロボットの出現数と生成しているスロット数の差異を計算する
        int diff = GameManager.instance.RobotList.Count - slot_lists.Count;

        // スロットの数がロボットの出現数より小さかったら
        if(diff > 0)
        {
            for(int ii = 0; ii < diff; ii++)
            {
                // Groupの下にスロットを生成させる
                GameObject _slot = Instantiate(robotSlot_prefab, slot_parent);
                slot_lists.Add(_slot);
            }
        }
        // スロット数の方がロボットの出現数より大きかった場合
        else if(diff < 0)
        {
            for(int ii = slot_lists.Count - 1; ii >= GameManager.instance.RobotList.Count; ii--)
            {
                // 大きかった分だけ破棄
                Destroy(slot_lists[ii]);
                slot_lists.Remove(slot_lists[ii]);
            }
        }

        SetSlot();
    }

    void SetSlot()
    {
        for(int ii = 0; ii < GameManager.instance.RobotList.Count; ii++)
        {
            UpgreadBatteryRobotSlot slot_cs = slot_lists[ii].GetComponent<UpgreadBatteryRobotSlot>();
            slot_cs.ClearSlot();
            if(slot_cs.RobotBase == null)
            {
                slot_cs.InSlot(GameManager.instance.RobotList[ii]);
            }
        }
    }
}   