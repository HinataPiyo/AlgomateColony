using UnityEngine;

[CreateAssetMenu(menuName = "CommandList", fileName = "CommandSO")]
public class CommandSO : ScriptableObject
{
    [Header("コマンド一覧で表示するテキスト")] public ComanndsDetail[] cmdsDetail;
}

[System.Serializable]
public class ComanndsDetail
{
    public string commandName;
    [TextArea(5,10)] public string exp;     // 説明
    public string[] canUseCommads;     // 使用できる引数
}