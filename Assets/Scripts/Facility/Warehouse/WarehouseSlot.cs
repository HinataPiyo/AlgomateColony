using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WarehouseSlot : MonoBehaviour
{
    // 確認するためSerializeFieldする
    MaterialSO mateSO;
    int mateAmount;

    AccessoryData acceData;

    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI amo_text;



    private void Start() {
        ClearSlot();
    }

    /// <summary>
    /// アクセサリー用スロット
    /// </summary>
    /// <param name="_data"></param>
    public void AddAccessorySlot(AccessoryData _data)
    {
        acceData = _data;

        icon.enabled = true;
        icon.sprite = acceData.icon;
        amo_text.text = "";
    }

    /// <summary>
    /// スロットに素材を追加する
    /// </summary>
    /// <param name="_baseWarehouse_Slot"></param>
    public void AddMaterialToSlot(DataType.WAREHOUSE_SLOT _baseWarehouse_Slot)
    {
        mateSO = _baseWarehouse_Slot.mateSO;
        mateAmount = _baseWarehouse_Slot.hasAmount;

        icon.enabled = true;
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
        icon.sprite = null;
        amo_text.text = null;
    }


    public MaterialSO GetMaterialSO() { return mateSO; }
    public AccessoryData GetAccessoryData() { return acceData; }
    public int GetHaveAmount() { return mateAmount; }
}