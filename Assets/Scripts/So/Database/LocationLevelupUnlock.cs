using UnityEngine;


/// <summary>
/// Lovationのレベルに応じて解放される建物や潜在能力
/// レベルアップに必要な素材など設定するクラス
/// </summary>
[CreateAssetMenu(fileName = "LocationLevelupUnlock", menuName = "Database/LocationLevelupUnlock")]
public class LocationLevelupUnlock : ScriptableObject
{
    public bool batteryFacility;
    [SerializeField] NEXT_UNLOCK[] next_unlock;
    public NEXT_UNLOCK[] NextUnlock => next_unlock;
}

[System.Serializable]
public class NEXT_UNLOCK
{
    public Sprite icon;         // スロットに表示させるアイコン
    public Vector2 objPos;
    public GameObject creatObj; // 生成させるオブジェクト
    public string name_text;    // オブジェクトの名前
    [TextArea(3, 10)]
    public string exp_text;     // オブジェクトの説明

    [Space(10.0f), Header("ステータス上限を決める")]
    public StatusParam[] statusParam;

    [System.Serializable]
    public class StatusParam
    {
        public STATUS_TYPE selectStatus;
        public float statusLimited_value;       // 上限突破するステータスの値
    }

    // Locationのレベルに応じて必要素材を設定できるようにしている
    public DataType.NEED_MATERIAL[] needMaterials;
}