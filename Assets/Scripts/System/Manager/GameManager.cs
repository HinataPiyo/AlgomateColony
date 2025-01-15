using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("ゲームの進行に関するSO")]
    [SerializeField] SystemControlSO systemControlSO;
    [Header("左上のUIの設定")]
    [SerializeField] TextMeshProUGUI playername_text;
    [SerializeField] TextMeshProUGUI locationlevel_text;
    [Header("素材の所持数")]
    [SerializeField] Transform havematerial_parent;
    [SerializeField] HaveMaterialSlot[] haveMateSlot;
    RobotFactory robotFactory;
    public Canvas sliderCanvas;       // スライダーを表示させるためのキャンバス

    private void Awake()
    {
        if(instance == null) { instance = this; }
        else { Destroy(transform.root.gameObject); }
    }

    private void Start()
    {
        haveMateSlot = havematerial_parent.GetComponentsInChildren<HaveMaterialSlot>();
        robotFactory = GetComponent<RobotFactory>();
    }

    private void Update()
    {
            
    }

    public SystemControlSO GetSystemControlSO() { return systemControlSO;}
    public void Set_PlayerName_LocationLevel(string _playerName, int _level)
    {
        // 名前が設定されていなければ"unknown"と表示する
        if(_playerName == "") { playername_text.text = "unknown"; }
        else { playername_text.text = _playerName; }        // 名前を設定する（SystemSOで管理）

        locationlevel_text.text = "" + _level;              // LocationLevelを設定する（SystemSOで管理）
    }

    
}