using UnityEngine;

public class RobotFactory : MonoBehaviour
{
    [Header("ロボット系")]
    [SerializeField] GameObject robotPrefab;    // ロボットのPrefab
    [SerializeField] Transform spawnPoint;      // スポーン地点

    // テスト生成
    [SerializeField] bool CreatFlag;

    void Update()
    {
        if (CreatFlag)
        {
            CreateRobot();
            CreatFlag = false;
        }
    }

    /// <summary>
    /// ロボットのSOを引数に渡しロボットを生成。
    /// </summary>
    /// <param name="robotData">生成するロボットのデータ</param>
    public void CreateRobot()
    {
        GameObject newRobot = Instantiate(robotPrefab, spawnPoint.position, Quaternion.identity);
        RobotController _robot = newRobot.GetComponent<RobotController>();
        _robot.SpawnInit();
        GameManager.instance.RobotList.Add(_robot.GetBaseStatus());
        _robot.SetRobotName($"アルゴメイト{GameManager.instance.RobotList.Count}");
    }
}
