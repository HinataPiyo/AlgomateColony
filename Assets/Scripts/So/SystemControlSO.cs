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