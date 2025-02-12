using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NextLevelUnlockedSO", menuName = "CreatScriptableObject/NextLevelUnlockedSO", order = 0)]
public class NextLevelUnlockedSO : ScriptableObject
{
    public bool batteryFacility;
    [SerializeField] List<BASE_NEXT_UNLOCK> next_unlocks = new List<BASE_NEXT_UNLOCK>();

    public List<BASE_NEXT_UNLOCK> GetBaseNextUnlocks_List() { return next_unlocks; }
}

[System.Serializable]
public class BASE_NEXT_UNLOCK
{
    public Sprite icon;         // スロットに表示させるアイコン
    public Vector2 objPos;
    public GameObject creatObj; // 生成させるオブジェクト
    public string name_text;    // オブジェクトの名前
    [TextArea(3, 10)]
    public string exp_text;     // オブジェクトの説明

    [Space(10.0f),Header("ステータス上限を決める")]
    public StatusParam[] statusParam;

    [System.Serializable]
    public struct StatusParam
    {
        public STATUS_SELECT selectStatus;
        public float statusLimited_value;       // 上限突破するステータスの値
    }
}