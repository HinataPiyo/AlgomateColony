using UnityEngine;

public class RobotBattery : MonoBehaviour
{
    RobotController robotCont;
    BaseStatus _base;
    Animator robot_anim;

    public void GameInit(RobotController _robotCont)
    {
        robotCont = _robotCont;
        _base = robotCont.GetBaseStatus();
        robot_anim = robotCont.GetRobotAnim();
    }

    /// <summary>
    /// ロボットの充電がなくなったか調べる
    /// </summary>
    public void Check_CurrentEnergy()
    {
        if(_base.currentEnergy > 0)
        {
            if(robotCont.GetCurrentStat() != RobotController.State.DoNon)
            {
                // エネルギー消費
                _base.currentEnergy -= Time.deltaTime;
                robotCont.GetEnergySlider().value = _base.currentEnergy;
            }
        }
        else
        {
            // 充電がなくなった場合のステートに移行
            robotCont.ChangeState(RobotController.State.NonEnergy);
        }
    }

    /// <summary>
    /// ロボットにバッテリー交換が必要か否か確認する
    /// </summary>
    public void Check_NeedRecharge()
    {
        // 最大充電回数より低ければ
        if(_base.currentRecharged < _base.recharge_MAX)
        {
            // 充電可能にする
            _base.needchange_battery = false;
        }
        else    // 充電回数が最大値になっていれば
        {
            // 充電回数が最大値に到達したらバッテリー交換が必要ということを知らせる
            _base.needchange_battery = true;
        }
    }
    

    /// <summary>
    /// バッテリーを充電します
    /// </summary>
    public void RechargeBattery()
    {
        // バッテリー交換が必要なければ充電する
        if(_base.needchange_battery == false)
        {
            // Mathf.Minは最大値である"maxEnergy"を超えないようにしている
            _base.currentEnergy = Mathf.Min(_base.currentEnergy + 1, _base.maxEnergy);
        }
    }

    /// <summary>
    /// 充電がなくなったら時の処理
    /// </summary>
    public void NonEnergy()
    {
        _base.currentEnergy = 0;
        _base.recharge_battery = true;      // バッテリーの充電をしなければならない

        // 収集用スライダーを非アクティブ状態にする
        robotCont.GetGatherSliderObject().SetActive(false);

        Debug.Log(_base.robotName + "のエネルギーが不足しています！");
        robot_anim.SetBool("OutBattery", true);       // 充電不足のアニメーションを開始
    }

}