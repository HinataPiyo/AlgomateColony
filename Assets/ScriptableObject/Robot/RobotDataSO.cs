using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRobotData", menuName = "RobotSO/Robot Data")]
public class RobotDataSO : ScriptableObject
{
    public List<Robot> robots = new List<Robot>();
}

[System.Serializable]
public class BaseStatus
{
    public string robotName;        // ロボットの名前
    public float moveSpeed;         // 移動速度
    public float maxDurability;     // 耐久値
    public float maxEnergy;         // 最大エネルギー
    public float gatherRate;        // 資源収集速度

    public void RandomStatusProc()
    {
        robotName = "RobotName";
        moveSpeed = Mathf.Round(Random.Range(2f,8f) * 10 / 10);
        maxDurability = Mathf.Round(Random.Range(50f,100f) * 10 / 10);
        maxEnergy = Mathf.Round(Random.Range(50f,100f) * 10 / 10);
        gatherRate = Mathf.Round(Random.Range(3f,10f) * 10 / 10);
    }
}