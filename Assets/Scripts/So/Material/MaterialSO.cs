using UnityEngine;

[CreateAssetMenu(fileName = "NewMaterial", menuName = "MaterialSO/MaterialData")]
public class MaterialSO : ScriptableObject
{
    public Sprite icon;
    public int serialNum;
    public string materialName;     // 資材の名前
    public string tagName;          // タグの名前
    public int maxHp;               // 最大体力
    public float gatherTime;        // 収集時間
    public EquipmentType.TYPE equipmentToMatch;             // 収集される装備によって収集速度が変わる
    [TextArea(3, 10)] public string exp;                // アイテムの説明
    [Header("素材のデータ/必要個数"), Tooltip("加工品以外は設定しなくていい")]
    public DataType.NEED_MATERIAL[] need_mate_list;
}
