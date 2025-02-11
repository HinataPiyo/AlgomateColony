using UnityEngine;

[CreateAssetMenu(fileName = "NewMaterial", menuName = "MaterialSO/MaterialData")]
public class MaterialSO : ScriptableObject
{
    public Sprite icon;
    public int serialNum;
    public string materialName;     // 資材の名前
    public int maxHp;               // 最大体力
    public float gatherTime;        // 収集時間
    public EQUIPMENT_NAME EquipmentToMatch;
    [TextArea(3, 10)] public string exp;              // アイテムの説明
    [Header("アイテムのタグ名")]
    public MaterialNameTag mateTagName;
}

[System.Flags]
public enum MaterialNameTag
{
    rock,
    tree,
    ironore,
}
