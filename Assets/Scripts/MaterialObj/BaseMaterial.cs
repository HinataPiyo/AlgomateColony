using System.Collections;
using UnityEngine;

public class BaseMaterial : MonoBehaviour
{
    public MaterialSO mateSO;
    float hp;
    int amo = 1;    // 収集一回で得られる個数
    bool deathFlag;

    void Start()
    {
        hp = mateSO.maxHp;     // SOで設定した最大体力を変数に設定する
    }

    // ダメージ処理
    public void TakeDamage(float damage)
    {
        hp -= damage;

        if(hp <= 0)
        {
            hp = 0;
            deathFlag = true;
            Destroy(gameObject);
        }

        deathFlag = false;
    }

    public int GetAmo() 
    {
        switch(deathFlag)
        {
            case false:
                return amo;     // 収集一回で得られる個数
            case true:
                return amo + 10;    // ここが課題
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("CenterCollider"))
        {
            Debug.Log("資材を破棄しました");
            Destroy(gameObject);
        }
    }
}
