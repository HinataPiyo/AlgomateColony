using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 障害物回避のための補助クラス。
/// Rayを使って周囲の空いている方向を判定し、最適な回避方向や脱出方向を決定します。
/// </summary>
public class ObstacleAvoidanceHelper
{
    private readonly List<Vector2> baseDirections = new()
    {
        Vector2.up,
        Vector2.down,
        Vector2.left,
        Vector2.right,
        new Vector2(1, 1).normalized,
        new Vector2(-1, 1).normalized,
        new Vector2(1, -1).normalized,
        new Vector2(-1, -1).normalized
    };

    public Vector2? FindBestFromBlockedDirection(Vector2 origin, Vector2 forward, float length, LayerMask obstacleLayer)
    {
        // 候補方向とその反対側のペアを用意
        var candidatePairs = new List<(Vector2 dir, Vector2 opposite)>
        {
            (Quaternion.Euler(0, 0, 45) * forward, Quaternion.Euler(0, 0, -45) * forward),   // 左斜め前・右斜め前
            (Quaternion.Euler(0, 0, 90) * forward, Quaternion.Euler(0, 0, -90) * forward)    // 左・右
        };

        Vector2? bestDir = null;
        float minAngle = float.MaxValue;

        foreach (var (dir, opposite) in candidatePairs)
        {
            // まずdir側をチェック
            if (!Physics2D.Raycast(origin, dir, length, obstacleLayer))
            {
                float angle = Vector2.Angle(forward, dir);
                if (angle < minAngle)
                {
                    minAngle = angle;
                    bestDir = dir.normalized;
                }
            }
            // dir側が塞がれていた場合、opposite側をチェック
            else if (!Physics2D.Raycast(origin, opposite, length, obstacleLayer))
            {
                float angle = Vector2.Angle(forward, opposite);
                if (angle < minAngle)
                {
                    minAngle = angle;
                    bestDir = opposite.normalized;
                }
            }
        }

        return bestDir;
    }

    /// <summary>
    /// すべての方向に障害物が存在しない場合 false を返します。
    /// </summary>
    public bool HasAnyObstacle(Vector2 origin, float length, LayerMask obstacleLayer)
    {
        foreach (var dir in baseDirections)
        {
            if (Physics2D.Raycast(origin, dir, length, obstacleLayer))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// すべての方向がヒットしている中で最も開けた方向（Rayのヒット距離が最も遠い）を返します。
    /// </summary>
    public Vector2? FallbackEscapeDirection(Vector2 origin, float length, LayerMask obstacleLayer)
    {
        float maxDistance = -1f;
        Vector2 bestDir = Vector2.zero;
        bool found = false;

        foreach (var dir in baseDirections)
        {
            RaycastHit2D hit = Physics2D.Raycast(origin, dir, length, obstacleLayer);
            float hitDistance = hit.collider != null ? hit.distance : length;

            if (hitDistance > maxDistance)
            {
                maxDistance = hitDistance;
                bestDir = dir;
                found = true;
            }
        }

        return found ? bestDir.normalized : (Vector2?)null;
    }

    /// <summary>
    /// Rayのデバッグ描画を行います。
    /// </summary>
    public void DrawDebugRays(Vector2 origin, Vector2 forward, float length, LayerMask obstacleLayer)
    {
        List<Vector2> dirs = new()
        {
            forward,
            Quaternion.Euler(0, 0, 45) * forward,
            Quaternion.Euler(0, 0, -45) * forward,
            Quaternion.Euler(0, 0, 90) * forward,
            Quaternion.Euler(0, 0, -90) * forward
        };

        foreach (var dir in dirs)
        {
            RaycastHit2D hit = Physics2D.Raycast(origin, dir, length, obstacleLayer);
            Gizmos.color = hit.collider != null ? Color.red : Color.cyan;
            Gizmos.DrawLine(origin, origin + dir.normalized * length);
        }
    }
}
