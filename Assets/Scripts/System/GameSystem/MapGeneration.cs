using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGeneration : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;        // タイルを配置するTilemapオブジェクト
    [SerializeField] TileBase[] tiles;       // 使用するタイルの配列

    // サイズや範囲に応じてタイルを配置
    const int MAP_SIZE_WID = 300;         // マップの横幅
    const int MAP_SIZE_HIG = 300;        // マップの縦幅

    void Awake()
    {
        // 初期マップの生成
        GenerateMap();
    }

    // マップ生成メソッド
    public void GenerateMap()
    {
        // オフセットを計算してマップの中心が原点になるようにする
        Vector3Int offset = new Vector3Int(-MAP_SIZE_WID / 2, -MAP_SIZE_HIG / 2);

        for (int x = 0; x < MAP_SIZE_WID; x++)
        {
            for (int y = 0; y < MAP_SIZE_HIG; y++)
            {
                // 実際の配置位置を計算
                Vector3Int position = new Vector3Int(x , y) + offset;
                // 配置するタイルをランダムに選択
                TileBase tile = tiles[Random.Range(0, tiles.Length)];
                
                tilemap.SetTile(position, tile);
                SetTileTrandform(position);
            }
        }
    }
    void SetTileTrandform(Vector3Int pos)
    {
        int randomRotation = Random.Range(0,4);
        float angle = 0;
        switch(randomRotation)
        {
            case 0:
            angle = 90;
                break;
            case 1:
            angle = 180;
                break;
            case 2:
            angle = -90;
                break;
            case 3:
                angle = -180;
                break;
        }

        Quaternion rotation = Quaternion.Euler(0,0,angle);
        Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, rotation, Vector3.one);

        // Tilemapの指定位置にタイルを設定
        tilemap.SetTransformMatrix(pos, matrix);
    }


    /// <summary>
    /// ゲッター関数
    /// </summary>
    /// <returns></returns>
    public int GetMapSizeX(){return MAP_SIZE_WID;}
    public int GetMapSizeY(){return MAP_SIZE_HIG;}
    
}
