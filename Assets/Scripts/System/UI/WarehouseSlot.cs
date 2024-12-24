using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WarehouseSlot : MonoBehaviour
{
    // 確認するためSerializeFieldする
    [SerializeField] MaterialSO mateSO;
    [SerializeField] uint mateAmount;

    [SerializeField] Image icon;
    [SerializeField] Image amotext_parent;
    [SerializeField] TextMeshProUGUI amo_text;

    private void Start() {
        ClearSlot();
    }

    /// <summary>
    /// スロットに素材を追加する
    /// </summary>
    /// <param name="_baseWarehouse_Slot"></param>
    public void AddMaterialToSlot(WarehouseSO.BASE_WAREHOUSE_SLOT _baseWarehouse_Slot)
    {
        mateSO = _baseWarehouse_Slot.mateSO;
        mateAmount = _baseWarehouse_Slot.mateAmount;

        icon.enabled = true;
        amotext_parent.enabled = true;
        icon.sprite = mateSO.icon;
        amo_text.text = $"{mateAmount}";
    }

    /// <summary>
    /// スロットクリアする
    /// </summary>
    public void ClearSlot()
    {
        mateSO = null;
        mateAmount = 0;
        icon.enabled = false;
        amotext_parent.enabled = false;
        icon.sprite = null;
        amo_text.text = null;
    }


    public MaterialSO GetMaterialSO() { return mateSO; }
}