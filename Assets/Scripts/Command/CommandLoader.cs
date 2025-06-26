
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// jsonからコマンドを読み込む
/// </summary>
public class CommandLoader : MonoBehaviour
{
    public static CommandLoader instance { get; private set; }
    public List<CommandEntry> MoveCommands { get; private set; }
    public List<CommandEntry> GatherCommands { get; private set; }
    public List<CommandEntry> DepositCommands { get; private set; }

    void Awake()
    {
        if (instance == null) instance = this;
    }

    void Start()
    {
        MoveCommands = Load("MoveCommands");
        GatherCommands = Load("GatherCommands");
        DepositCommands = Load("DepositCommands");
    }

    /// <summary>
    /// jsonファイルからコマンドを読み込む
    /// </summary>
    /// <param name="jsonFile"></param>
    List<CommandEntry> Load(string jsonFile)
    {
        TextAsset file = Resources.Load<TextAsset>(jsonFile);
        return JsonUtility.FromJson<CommandEntryList>("{\"entries\":" + file.text + "}").entries;
    }

    [System.Serializable]
    public class CommandEntry
    {
        public string command;
        public string description;
    }

    [System.Serializable]
    private class CommandEntryList
    {
        public List<CommandEntry> entries;
    }
}