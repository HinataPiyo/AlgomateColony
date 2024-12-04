using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    RobotFactory robotFactory;
    public Canvas sliderCanvas;       // スライダーを表示させるためのキャンバス

    private void Awake()
    {
        if(instance == null) { instance = this; }
        else { Destroy(transform.root.gameObject); }
    }

    private void Start()
    {
        robotFactory = GetComponent<RobotFactory>();
    }
}