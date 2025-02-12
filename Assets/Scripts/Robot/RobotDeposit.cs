using System.Collections.Generic;
using UnityEngine;

public class RobotDeposit : MonoBehaviour
{
    WarehouseController wCont;
    
    RobotController robotCont;
    BaseStatus _base;
    BaseStatus.Slot[] slots;

    [Header("範囲の設定")]
    [SerializeField] float radius;
    [SerializeField] LayerMask layerMask;

    private void Start() {
        wCont = FacilityManager.instance.wController;
    }

    public void GameInit(RobotController _robotCont)
    {
        robotCont = _robotCont;
        _base = robotCont.GetBaseStatus();
        slots = _base.slots;
    }


    /// <summary>
    /// 倉庫に素材を入れる
    /// </summary>
    public void Deposite()
    {
        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, radius, layerMask);
        bool found = false;

        foreach (Collider2D _hit in hit)
        {
            if (_hit.CompareTag("warehouse"))
            {
                found = true;
                break;
            }
        }

        if (!found)
        {
            Debug.Log("倉庫が見つかりませんでした。");
            LogController.instance.SetLog(_base, "倉庫が見つかりませんでした");
            return;
        }

        if (slots == null || slots.Length == 0)
        {
            Debug.Log("インベントリ内には何もありません。");
            LogController.instance.SetLog(_base, "インベントリ内には何もありません");
            return;
        }

        foreach (var slot in slots)
        {
            if (slot.mateSO == null) continue;

            if ($"{slot.mateSO.mateTagName}" == robotCont.DepsiteName[0])
            {
                int quantity;
                if (int.TryParse(robotCont.DepsiteName[1], out quantity))
                {
                    // 所持している数量と照らし合わせる
                    if (slot.itemStackAmount >= quantity)
                    {
                        wCont.SetMaterial_WarehouseSlot(slot.mateSO, quantity);
                        slot.itemStackAmount -= quantity;
                        LogController.instance.SetLog(_base, $"倉庫に{slot.mateSO.materialName}を{quantity}入れました");

                        
                        if(slot.itemStackAmount == 0)
                        {
                            slot.mateSO = null;
                        }
                        

                        // コマンドが終了したことを知らせる
                        robotCont.Get_RobotCommandExecute.StateEndFlag = true;
                        break;
                    }
                    else
                    {
                        Debug.Log("指定した個数は所持数より多いです。");
                        LogController.instance.SetLog(_base, "指定した個数は所持数より多いです");
                        robotCont.ChangeState(RobotController.State.DoNon);
                        break;
                    }
                }
            }
            else
            {
                Debug.Log("素材の名前が一致しません。");
                LogController.instance.SetLog(_base, "素材の名前が一致しません");
                robotCont.ChangeState(RobotController.State.DoNon);
                break;
            }
        }
    }


}