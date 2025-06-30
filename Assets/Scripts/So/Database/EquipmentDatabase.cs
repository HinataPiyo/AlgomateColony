using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentDatabase", menuName = "Database/EquipmentDatabase")]
public class EquipmentDatabase : ScriptableObject
{
    //【装備の設定】
    // ----------------------------------------------------------------------------------------------
    // 初期の段階では１ロボットにつき装備は一つしか装備できないが
    // 進んでいくと装備スロットが解放され２つ以上装備できるようにする...とか
    // 装備していないロボットが例えば木を切りに行くとすると、採取速度がかなり落ちる様にする...とか
    // 装備は共通して、一つしか存在しないものとする。他のロボットが装着しても能力が引き継がれる。
    // ----------------------------------------------------------------------------------------------

    [SerializeField] Sprite stop_sprite;
    [SerializeField] EquipmentType.DATA[] equipment_values;
    public EquipmentType.DATA[] DB => equipment_values;
    public Sprite StopSprite => stop_sprite;
}