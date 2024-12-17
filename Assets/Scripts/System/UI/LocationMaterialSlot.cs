using NUnit.Framework.Constraints;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LocationMaterialSlot : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI materialName_text;
    [SerializeField] TextMeshProUGUI stockAmount_text;
    [SerializeField] TextMeshProUGUI needAmount_text;

    // 所持数,必要個数
    [SerializeField, Range(0, 20)] int stockAmo, needAmo;

    private void Start() {
        // テスト
        SetSlotMaterial(null, "- stone", 15);
    }

    private void Update() {
        // テスト
        GetStockAmount(stockAmo);



        Check_OverNeedAmo();        // 現在所持している素材の数が必要個数より大きいか小さいか判断する
    }

    /// <summary>
    /// スロットの要素を一気に設定
    /// </summary>
    /// <param name="_sprite"> スロットのアイコンを設定 </param>
    /// <param name="_name"> 素材の名前を設定</param>
    /// <param name="_stockAmo"> 現在所持している素材の数 </param>
    /// <param name="_needAmo"> 必要個数 </param>
    public void SetSlotMaterial(Sprite _sprite, string _name, int _needAmo)
    {
        icon.sprite = _sprite;
        materialName_text.text = _name;
        needAmount_text.text = " / " + _needAmo;
        
        needAmo = _needAmo;
    }

    /// <summary>
    /// 現在所持している素材の数が
    /// 必要個数より大きければ / true
    /// 必要個数より少なければ / false
    /// </summary>
    /// <returns></returns>
    public bool Check_OverNeedAmo()
    {
        int yellowAmo = (int)Mathf.Floor(needAmo * 0.8f);

        if(stockAmo < needAmo)
        {
            if(stockAmo >= yellowAmo)
            {
                Debug.Log(yellowAmo);
                // 必要個数より少し小さければ黄色テキストにする
                stockAmount_text.color = Color.yellow;
                needAmount_text.color = Color.yellow;
                return false;
            }
            else
            {
                // 必要個数より小さければ赤いテキストにする
                stockAmount_text.color = Color.red;
                needAmount_text.color = Color.red;
                return false;
            }
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
    public void GetStockAmount(int amo)
    {
        stockAmo = amo;
        stockAmount_text.text = "" + stockAmo;
    }
}
