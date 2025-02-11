using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HaveMaterialSlot : MonoBehaviour
{
    [SerializeField] MaterialSO mateSO;
    [SerializeField] Image icon;
    [SerializeField] int haveAmo;
    [SerializeField] TextMeshProUGUI haveAmo_text;

    /// <summary>
    /// 自身のオブジェクトに素材のSOと現在の所持数を設定する
    /// </summary>
    /// <param name="_mateSO"></param>
    /// <param name="amo"></param>
    public void SetHaveMaterial(MaterialSO _mateSO, int _amo)
    {
        mateSO = _mateSO;
        icon.sprite = mateSO.icon;
        haveAmo = _amo;
        haveAmo_text.text = "" + haveAmo;
    }

    public void ClearSlot()
    {
        mateSO = null;
        icon.sprite = null;
        haveAmo = 0;
        haveAmo_text.text = "" + haveAmo;
    }

    public MaterialSO GetMaterialSO() { return mateSO; }
}