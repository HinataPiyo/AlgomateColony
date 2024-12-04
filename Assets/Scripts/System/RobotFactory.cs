using UnityEngine;

public class RobotFactory : MonoBehaviour
{
    [Header("ロボット系")]
    [SerializeField] GameObject robotPrefab;    // ロボットのPrefab
    [SerializeField] Transform spawnPoint;      // スポーン地点

    [SerializeField] RobotDataSO rDataSO;   // テスト

    private void Start() {
        Initialize();
    }

    void Initialize()
    {
        CreateRobot(rDataSO);                 // ロボットを生成するた目の関数に格納
    }

    /// <summary>
    /// ロボットのSOを引数に渡しロボットを生成。
    /// </summary>
    /// <param name="robotData">生成するロボットのデータ</param>
    public Robot CreateRobot(RobotDataSO rData)
    {
        GameObject newRobot = Instantiate(robotPrefab, spawnPoint.position, Quaternion.identity);
        Robot _robot = newRobot.GetComponent<Robot>();
        _robot.Initialize(rData);
        return _robot;
    }
}
