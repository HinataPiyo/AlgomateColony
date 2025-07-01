using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WarkshopNeedMaterialSlot : MonoBehaviour
{
    [SerializeField] MaterialSO mateSO;
    [SerializeField] Image icon;                            // アイコンの素材
    [SerializeField] TextMeshProUGUI materialName_text;     // 素材の名前
    [SerializeField] TextMeshProUGUI stockAmount_text;      // 所持数
    [SerializeField] TextMeshProUGUI needAmount_text;       // 必要個数

    // 所持数,必要個数(RangeはDebug用)
    [SerializeField] int stockAmo, needAmo;

    private void Update() {
        // テスト
        SetStockAmount(stockAmo);
    }

    /// <summary>
    /// スロットの要素を一気に設定
    /// </summary>
    /// <param name="_mateSO"> 素材のデータ </param>
    /// <param name="_stockAmo"> 現在所持している素材の数 </param>
    /// <param name="_needAmo"> 必要個数 </param>
    public void SetSlotMaterial(MaterialSO _mateSO, int _needAmo)
    {
        if(_mateSO != null)
        {
            // スロットに設定
            icon.sprite = _mateSO.icon;
            icon.preserveAspect = true;
            materialName_text.text = "- " + _mateSO.materialName;
            needAmount_text.text = " / " + _needAmo;
        
            // 数値を取得
            mateSO = _mateSO;
            needAmo = _needAmo;
        }
        else
        {
            // 数値を取得
            mateSO = null;
            needAmo = 0;
            
            // スロットに設定
            icon.sprite = null;
            materialName_text.text = "";
            stockAmount_text.text = "";
            needAmount_text.text = "";

            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 現在所持している素材の数が
    /// 必要個数より大きければ / true
    /// 必要個数より少なければ / false
    /// </summary>
    /// <returns></returns>
    public bool Check_OverNeedAmo()
    {
        if(stockAmo < needAmo)
        {
            // 必要個数より小さければ赤いテキストにする
            stockAmount_text.color = Color.red;
            needAmount_text.color = Color.red;
            return false;
        }

        // 必要個数より大きければ緑テキストにする
        stockAmount_text.color = Color.green;
        needAmount_text.color = Color.green;
        return true;
    }

    /// <summary>
    /// 現在所持している素材の数を取得
    /// </summary>
    /// <param name="amo"></param>
    public void SetStockAmount(int amo)
    {
        stockAmo = amo;
        stockAmount_text.text = "" + stockAmo;
    }

    public MaterialSO GetMaterialSO() { return mateSO; }
}
