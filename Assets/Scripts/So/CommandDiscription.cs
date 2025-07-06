using UnityEngine;

public static class CommandDiscription
{
    public static readonly CommandDetail[] CommandDetails = new CommandDetail[]
    {
        new CommandDetail
        {
            commandName = "Move();",
            exp = "指定した位置に移動します。\n例: Move(location); ,Move(rock)",
            canUseCommads = new string[] { "location, warehouse, rock, tree, ironOre" }
        },
        new CommandDetail
        {
            commandName = "Gather();",
            exp = "指定した資源を収集します。\n例: Gather(rock);",
            canUseCommads = new string[] { "rock, tree, ironOre" }
        },
        new CommandDetail
        {
            commandName = "Deposit(,);",
            exp = "収集した資源を指定した数だけ倉庫に預けます。\n※このコマンドは倉庫に移動していないと使用できません。\n例: deposit(rock, 20);",
            canUseCommads = new string[] { "rock, tree, ironOre" }
        }
    };
}

[System.Serializable]
public class CommandDetail
{
    public string commandName;
    [TextArea(5,10)] public string exp;     // 説明
    public string[] canUseCommads;     // 使用できる引数
}