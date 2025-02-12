using UnityEngine;

public class RobotFactory : MonoBehaviour
{
    [SerializeField] SystemControlSO scSO;
    [Header("ロボット系")]
    [SerializeField] GameObject robotPrefab;    // ロボットのPrefab
    [SerializeField] Transform spawnPoint;      // スポーン地点

    [SerializeField] bool flag;

    private void Start() {
        Initialize();
    }
    
    private void Update() {
        if(flag) { CreateRobot(); flag = false; }
    }

    void Initialize()
    {
        // for(int ii = 0; ii < 30; ii++)
        CreateRobot();                 // ロボットを生成するた目の関数に格納
    }

    /// <summary>
    /// ロボットのSOを引数に渡しロボットを生成。
    /// </summary>
    /// <param name="robotData">生成するロボットのデータ</param>
    public void CreateRobot()
    {
        GameObject newRobot = Instantiate(robotPrefab, spawnPoint.position, Quaternion.identity);
        RobotController _robot = newRobot.GetComponent<RobotController>();
        _robot.Initialize();
        scSO.robot_list.Add(_robot.GetBaseStatus());
        _robot.SetRobotName($"アルゴメイト{scSO.robot_list.Count}");
    }
}
