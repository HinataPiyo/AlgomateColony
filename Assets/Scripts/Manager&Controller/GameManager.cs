using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// シングルトンの頂点
/// ゲームの進行を管理しているはずのクラス
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    [Header("左上のUIの設定")]
    [SerializeField] TextMeshProUGUI playername_text;
    [SerializeField] TextMeshProUGUI locationlevel_text;

    [Header("スライダーを表示させるためのキャンバス")]
    [SerializeField] Canvas sliderCanvas;

    [Header("設定キャンバス")]
    [SerializeField] Button setting_button;

    [Header("出現しているロボット")]
    [SerializeField] List<BaseStatus> robot_list = new List<BaseStatus>();

    // ゲッター
    public List<BaseStatus> RobotList => robot_list;
    public Canvas SliderCanvas => sliderCanvas;

    // コンポーネント
    RobotFactory robotFactory;

    private void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(transform.root.gameObject); }

        setting_button.onClick.AddListener(SettingButtonOnClick);
        robotFactory = GetComponent<RobotFactory>();
    }

    void Start()
    {
        robotFactory.CreateRobot();     // ロボットを生成
    }

    public void Set_PlayerName_LocationLevel(string _playerName, int _level)
    {
        // 名前が設定されていなければ"unknown"と表示する
        if (_playerName == "") { playername_text.text = "unknown"; }
        else { playername_text.text = _playerName; }        // 名前を設定する（SystemSOで管理）

        locationlevel_text.text = "" + _level;              // LocationLevelを設定する（SystemSOで管理）
    }

    void SettingButtonOnClick()
    {
        FacilityManager.instance.CanvasEnabled(CanvasName.Setting, true);
    }

}