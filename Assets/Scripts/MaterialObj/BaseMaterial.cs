using UnityEngine;

public class BaseMaterial : MonoBehaviour
{
    [SerializeField] MaterialSO stoneSO;

    int hp;

    void Start()
    {
        hp = stoneSO.maxHp;     // SOで設定した最大体力を変数に設定する
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;

        if(hp <= 0)
        {
            hp = 0;
            Destroy(gameObject);
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
