using System.Collections.Generic;
using UnityEngine;

public enum MATERIAL_NAME
{
    stone,
    tree,
}

public class MaterialObjSpawnController : MonoBehaviour
{
    [SerializeField] Transform objectStorage;
    [SerializeField] GameObject[] material;
    public List<GameObject> materials = new List<GameObject>();
    
    int MATERIAL_SPAWN_MAX = 100;
    int currentSpawnObj;
    int SPAWN_WID = 100;
    int SPAWN_HIG = 100;

    void Start()
    {
        SpawnProc();
    }
    void SpawnProc()
    {
        for(int ii = 0; ii < MATERIAL_SPAWN_MAX; ii++)
        {
            int rPos_wid = Random.Range(-SPAWN_WID / 2, SPAWN_WID / 2);
            int rPos_hig = Random.Range(-SPAWN_HIG / 2, SPAWN_HIG / 2);
            Vector2 pos = new Vector2(rPos_wid, rPos_hig);
            int rMate = Random.Range(0, material.Length);

            GameObject mateObj = Instantiate(material[rMate], pos, Quaternion.identity, objectStorage);
            materials.Add(mateObj);
        }
    }
}
