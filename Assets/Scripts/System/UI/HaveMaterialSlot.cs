using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HaveMaterialSlot : MonoBehaviour
{
    [SerializeField] MaterialSO mateSO;
    [SerializeField] Image icon;
    [SerializeField] uint haveAmo;
    [SerializeField] TextMeshProUGUI haveAmo_text;

    private void Start()
    {
        icon.sprite = mateSO?.icon;
        haveAmo_text.text = "" + haveAmo;
    }

    private void Update() {
        haveAmo_text.text = "" + haveAmo;
    }
    
    /// <summary>
    /// 自身のオブジェクトに素材のSOと現在の所持数を設定する
    /// </summary>
    /// <param name="_mateSO"></param>
    /// <param name="amo"></param>
    public void SetHaveMaterial(uint amo)
    {
        haveAmo = amo;
        haveAmo_text.text = "" + haveAmo;
    }

    public MaterialSO GetMaterialSO() { return mateSO; }
}