using System.Collections;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.UI;

public class RobotGather : MonoBehaviour
{
    [SerializeField] EquipmentSO equipmentSO;
    [SerializeField] SpriteRenderer equipment_sprite;
    
    RobotController robotCont;
    BaseStatus _base;
    Slider _gslider;        // 収集スライダー

    public void GameInit(RobotController _robotCont)
    {
        robotCont = _robotCont;
        _base = robotCont.GetBaseStatus();
        _gslider = robotCont.GetGatherSliderObject().GetComponent<Slider>();
        ChangeEquipment(null);
    }

    /// <summary>
    /// コルーチンを実行する為の関数
    /// </summary>
    public void StartCoroutine_GatherResource()
    {
        if(robotCont.GetHitInfo() == null)
        {
            Debug.Log("対象のオブジェクトを発見できませんでした。");
            LogController.instance.SetLog(_base, "対象のオブジェクトを発見できませんでした");
            return;
        }

        StartCoroutine(GatherResource());
    }

    /// <summary>
    /// 資源を収集します
    /// </summary>
    IEnumerator GatherResource()
    {
        bool checkHitInfo = true;

        if(robotCont.ObjectName != robotCont.GetHitInfo().tag)
        {
            Debug.Log("設定した名前は対象の名前と異なります。");
            LogController.instance.SetLog(_base, "設定した名前は対象の名前と異なります");
            yield break;
        }

        Debug.Log(_base.robotName + "が資源を収集しています。");
        
        
        // 収集処理
        while(checkHitInfo == true && _base.recharge_battery == false)
        {
            // 収集時間を表すスライダーを表示する
            if(robotCont.GetHitInfo() != null)     // 資源が存在していれば
            {
                // 全てのスロットがスタックMaxだった場合
                if(_base.CheckAllStackMax() == BaseStatus.SLOT_STACK.ALL_STACK_MAX)
                {
                    Debug.Log(_base.robotName + "のインベントリがいっぱいです。");
                    LogController.instance.SetLog(_base, "インベントリがいっぱいです");
                    robotCont.ChangeState(RobotController.State.DoNon);   // 何もしない状態に遷移
                    yield break;                // コルーチンを抜ける
                }

                robotCont.GetGatherSliderObject().SetActive(true);    // 収集ゲージを表示する・アクティブ状態にする

                // 一度スクリプトを取得する
                BaseMaterial _baseMate = robotCont.GetHitInfo()?.GetComponent<BaseMaterial>();
                LogController.instance.SetLog(_base, $"{_baseMate.mateSO.materialName}を収集しています");

                // 収集するオブジェクトによって装備を変える
                ChangeEquipment(_baseMate);

                float time = 0;
                _gslider.maxValue = _baseMate.mateSO.gatherTime;           // マックススライダーに反映する
                _gslider.value = _gslider.maxValue;

                // 資源に設定されてある時間より経過時間が小さければ
                while(time < _baseMate.mateSO.gatherTime)
                {
                    // ! 収集速度を上昇させるか確認する
                    UpGread_GatharRate(_baseMate.mateSO);

                    time += _base.gatherRate * Time.deltaTime;         // 経過時間を更新する
                    _gslider.value = _gslider.maxValue - time;         // 減少させるようにスライダーの値を設定

                    // 時間経過中に収集中のモノがなくなったら
                    if(robotCont.GetHitInfo() == null) 
                    {
                        robotCont.ChangeState(RobotController.State.DoNon);
                        yield break;                // コルーチンを抜ける
                    }
                    
                    yield return null;                      // 次のフレームまで待機
                }

                // 資源にダメージを与える
                _baseMate.TakeDamage(_base.gatherSterngth);
                
                // 収集している資源のシリアル番号が同一か確かめる
                foreach(var _slot in _base.slots)
                {
                    // スタック数がMaxではなかったら
                    if(_base.CheckStackMax() == BaseStatus.SLOT_STACK.STACK_TRUE)
                    {
                        // スロット内に同一のシリアル番号がなければ
                        if(_slot.mateSO?.serialNum != _baseMate.mateSO.serialNum)
                        {
                            // 空のスロットを見つける
                            if(_slot.mateSO == null)
                            {
                                // スロット(インベントリ)に格納する
                                _slot.mateSO = _baseMate.mateSO;
                                // スタック数を増やす
                                _slot.itemStackAmount += _baseMate.GetAmo();
                                break;      // foreachを抜ける
                            }
                        }
                        else    // 同一のシリアル番号が存在したら
                        {
                            // スタック数を増やす
                            _slot.itemStackAmount += _baseMate.GetAmo();
                            break;      // foreachを抜ける
                        }
                    }
                    // スタックがMaxだった場合
                    else if(_base.CheckStackMax() == BaseStatus.SLOT_STACK.STACK_MAX)
                    {

                        // 空のスロットを探す
                        if(_slot.mateSO == null)
                        {
                            // スロット(インベントリ)に格納する
                            _slot.mateSO = _baseMate.mateSO;
                            // スタック数を増やす
                            _slot.itemStackAmount += _baseMate.GetAmo();
                            break;      // foreachを抜ける
                        }             
                    }
                }
            }
            else
            {
                // コマンドが終了したことを知らせる
                Debug.Log($"{_base.robotName}が資源を収集完了しました。");
                LogController.instance.SetLog(_base, "資源を収集完了しました");
                robotCont.ChangeState(RobotController.State.DoNon);   // 何もしない状態に遷移
                checkHitInfo = false;       // hitInfoが存在しているか否か
                ChangeEquipment(null);      // 収集が終わったので装備を外す
                robotCont.Get_RobotCommandExecute.StateEndFlag = true;
            }

            yield return null;              // 次のフレームまで待機
        }

        yield break;        // コルーチンを抜ける
    }

    
    /// <summary>
    /// 収集速度ステータスの上昇
    /// 収集速度は値が増えるほど速くなる
    /// </summary>
    /// <param name="_equipmentSO"></param>
    public void UpGread_GatharRate(MaterialSO _mateSO)
    {
        // 装備している装備の名前を引数で渡し、名前に合った値を返してくる
        _base.gatherRate = _base.GetGatherRate_Min() + 
            equipmentSO.GetEquipmentTotalValue(_base.equipment_value, _mateSO);
    }

    /// <summary>
    /// 収集するオブジェクトによって装備を変更する
    /// </summary>
    /// <param name="_baseMate"></param>
    void ChangeEquipment(BaseMaterial _baseMate)
    {
        if(_baseMate == null)
        {
            equipment_sprite.sprite = null;
            equipment_sprite.enabled = false;
            return;
        }

        MaterialSO mateSo = _baseMate.mateSO;

        switch(mateSo.EquipmentToMatch)
        {
            case EQUIPMENT_NAME.NONE:
                equipment_sprite.sprite = null;
                equipment_sprite.enabled = false;
                break;
            case EQUIPMENT_NAME.DRIL:
                _base.equipment_value = equipmentSO.equipment_values[0];
                equipment_sprite.sprite = _base.equipment_value.icon;
                equipment_sprite.enabled = true;
                break;
            case EQUIPMENT_NAME.ARM:
                _base.equipment_value = equipmentSO.equipment_values[1];
                equipment_sprite.sprite = _base.equipment_value.icon;
                equipment_sprite.enabled = true;
                break;
            case EQUIPMENT_NAME.CHAINSAW:
                _base.equipment_value = equipmentSO.equipment_values[2];
                equipment_sprite.sprite = _base.equipment_value.icon;
                equipment_sprite.enabled = true;
                break;
        }
    }
}