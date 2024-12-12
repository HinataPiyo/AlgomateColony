using UnityEngine;

[CreateAssetMenu(fileName = "NewMaterial", menuName = "MaterialSO/MaterialData")]
public class MaterialSO : ScriptableObject
{
    public Sprite icon;
    public int serialNum;
    public string materialName;     // 資材の名前
    public int maxHp;               // 最大体力
}
