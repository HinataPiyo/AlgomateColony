using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "needMate", menuName = "MaterialSO/NeedMaterials")]
public class NeedMaterialSO : ScriptableObject
{
    public List<NEED_MATERIAL_ROOT> need_mate_root = new List<NEED_MATERIAL_ROOT>();

    /// レベルごとの素材の必要量を設定するため、クラスの中にクラスを設定
    [System.Serializable]
    public class NEED_MATERIAL_ROOT
    {
        public NEED_MATEREALS[] need_materials;
        /// <summary>
        /// 必要素材の設定
        /// </summary>
        [System.Serializable]
        public struct NEED_MATEREALS
        {
            public MaterialSO mateSO;
            public int needAmo;
        }
    }
}

