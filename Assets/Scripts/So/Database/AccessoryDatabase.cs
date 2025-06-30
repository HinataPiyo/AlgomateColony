using UnityEngine;


/// <summary>
/// アクセサリーのデータを管理するクラス
/// </summary>
[CreateAssetMenu(fileName = "AccessoryDatabase", menuName = "Database/AccessoryDatabase")]
public class AccessoryDatabase : ScriptableObject
{
    [Header("アクセサリーが装着できないスロットだった時に表示する画像")]
    [SerializeField] Sprite stop_sprite;
    [Header("アクセサリーの一覧"), SerializeField] AccessoryData[] accessoryDatabase;


    public AccessoryData[] DB => accessoryDatabase;
    public Sprite StopSprite => stop_sprite;
}