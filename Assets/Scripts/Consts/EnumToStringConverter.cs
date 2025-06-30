using System.Collections.Generic;

public static class EnumToStringConverter
{
    /// <summary>
    /// STATUS_TYPEの名前を日本語に変換
    /// </summary>
    public static Dictionary<STATUS_TYPE, string> StatusNameConvert = new()
    {
        {STATUS_TYPE.NONE, null},
        {STATUS_TYPE.MoveSpeedMax, "移動速度"},
        {STATUS_TYPE.RechargeMax, "充電回数"},
        {STATUS_TYPE.EnergyMax, "バッテリー容量"},
        {STATUS_TYPE.GatherStrengthMax, "収集力"},
        {STATUS_TYPE.GatherRateMax, "収集速度"},
    };
}