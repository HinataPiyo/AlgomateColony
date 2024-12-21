using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    // ゲームの進行に関するSO
    [SerializeField] SystemControlSO systemControlSO;
    [SerializeField] Transform havematerial_parent;
    [SerializeField] HaveMaterialSlot[] haveMateSlot;
    RobotFactory robotFactory;
    public Canvas sliderCanvas;       // スライダーを表示させるためのキャンバス

    private void Awake()
    {
        if(instance == null) { instance = this; }
        else { Destroy(transform.root.gameObject); }
    }

    private void Start()
    {
        haveMateSlot = havematerial_parent.GetComponentsInChildren<HaveMaterialSlot>();
        robotFactory = GetComponent<RobotFactory>();   
    }

    private void Update()
    {
            
    }

    public SystemControlSO GetSystemControlSO() { return systemControlSO;}
}