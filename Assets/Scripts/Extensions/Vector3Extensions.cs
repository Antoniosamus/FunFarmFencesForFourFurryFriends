using UnityEngine;
using System.Collections;

public static class Vector3Extensions 
{
    public static Vector3 GetRamdomAtDistance(this Vector3 v3, float distance)
    {
        return new Vector3(
            Vector3Extensions.GetRandomFloat(v3.x, distance),
            Vector3Extensions.GetRandomFloat(v3.y, distance),
            Vector3Extensions.GetRandomFloat(v3.z, distance));
    }

    public static float GetRandomFloat(float value, float variation)
    {
        return ((float)Random.Range(0, 100) / 100 - 0.5f) * variation + value;
    }
}
