using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 単純な直線補間による経路生成クラス。
/// 経路探索アルゴリズムを後から差し替えられるように独立構造を採用。
/// </summary>
public class PathfindingSystem
{
    private readonly int stepCount;

    /// <summary>
    /// コンストラクタ。分割数（経由点数）を指定。
    /// </summary>
    /// <param name="steps">経路を何分割するか（最低1）</param>
    public PathfindingSystem(int steps = 10)
    {
        stepCount = Mathf.Max(1, steps);
    }

    /// <summary>
    /// 開始地点から終了地点までを補間し、ウェイポイントをキューで返す。
    /// </summary>
    /// <param name="start">開始位置</param>
    /// <param name="end">終了位置</param>
    /// <returns>Waypoint のキュー</returns>
    public Queue<Vector2> GeneratePath(Vector2 start, Vector2 end)
    {
        Queue<Vector2> waypoints = new();

        for (int i = 1; i <= stepCount; i++)
        {
            Vector2 point = Vector2.Lerp(start, end, i / (float)stepCount);
            waypoints.Enqueue(point);
        }

        return waypoints;
    }
}
