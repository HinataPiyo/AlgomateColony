using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SystemControlSO", menuName = "System/SystemControlSO")]
public class SystemControlSO : ScriptableObject 
{
    public string playerName;
    [SerializeField] int locationLevel;                 // 拠点のレベルを設定
    [SerializeField] float upgread_chargingTime;        // ロボットの充電を早くする為のUpgread要素
    
    public List<SettingDetail> settingDetails = new List<SettingDetail>();

    public int LocationLevel => locationLevel;
    public float GetBatteryChargingTime() { return upgread_chargingTime; }


    /// <summary>
    /// Locationのレベルを上げたときに処理される関数
    /// </summary>
    public void LocationLevelUp()
    {
        NEXT_UNLOCK.StatusParam[] _statusParams = null;
        NEXT_UNLOCK[] _nextUnlock = DataManager.instance.levelupUnlockTB.NextUnlock;
        if (locationLevel < _nextUnlock.Length)
        {
            // TODO レベルアップしたとき建物を生成する処理だが、いずれどうにかしたい。
            if (locationLevel == 1)
            {
                Instantiate(_nextUnlock[1].creatObj, _nextUnlock[1].objPos, Quaternion.identity);
            }

            // 現在のレベルに合わせたStatusParamを取得する
            _statusParams = _nextUnlock[locationLevel].statusParam;
        }

        // 潜在能力の上昇
        foreach (var param in _statusParams)
        {
            DataManager.instance.SetPotential(param.selectStatus, param.statusLimited_value);
        }

        // アンロックさせる内容の処理が終わったら
        locationLevel++;        // 拠点のレベルを上げる
    }

    // TODO 右下に表示されるパネルの設定だが、これはどうにかしたい。
    [System.Serializable]
    public class SettingDetail
    {
        public Sprite icon;
        public string _name;
        public string cmd;
    }
}




/// <summary>
/// UIの更新を1フレームごとに行わないため
/// </summary>
public class UpdateTime_Class
{
    const float update_AbsTime = 1.0f;      // UIの更新を1フレームごとに行わないための時間
    float processTime = 0f;                 // 経過時間

    /// <summary>
    /// UIの更新を1フレームごとに行わないための処理
    /// </summary>
    public bool UpdateTime()
    {
        // 経過時間を更新
        processTime += Time.deltaTime;

        // 設定時間より経過時間の方が大きくなったら
        if (processTime > update_AbsTime)
        {
            processTime = 0f; // 条件を満たしたらリセット
            return true;
        }

        return false;
    }
}