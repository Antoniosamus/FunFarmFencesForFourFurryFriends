using UnityEngine;
using System.Collections;

public static class Vector2Extensions 
{
    public static Vector2 GetRamdomAtDistance(this Vector2 v3, float distance)
    {
        return new Vector3(
            Vector2Extensions.GetRandomFloat(v3.x, distance),
            Vector2Extensions.GetRandomFloat(v3.y, distance));
    }

    public static float GetRandomFloat(float value, float variation)
    {
        return ((float)Random.Range(0, 100) / 100 - 0.5f) * variation + value;
    }
}
