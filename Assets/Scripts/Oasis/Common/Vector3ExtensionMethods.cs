using UnityEngine;
using Unity.Mathematics;


public static class Vector3ExtensionMethods
{

    public static int3 ToInt3(this Vector3 v)
    {
        return new int3 ((int)Mathf.Floor(v.x), (int)Mathf.Floor(v.y), (int)Mathf.Floor(v.z));
    }
    
    public static float3 ToFloat3(this Vector3 v)
    {
        return new float3 (v.x, v.y, v.z);
    }
    public static float4 ToFloat4(this Vector3 v)
    {
        return new float4 (v.x, v.y, (float)(v.z), 1f);
    }


    // Convert vector3 normal to a string representation of the side
    public static string ToSide(this Vector3 v)
    {
        if (v == Vector3.up) return "up";
        if (v == Vector3.down) return "down";
        if (v == Vector3.left) return "west";
        if (v == Vector3.right) return "east";
        if (v == Vector3.forward) return "south";
        if (v == Vector3.back) return "north";
        Debug.LogError("Vector3 is not a side");
        return "error";
    }


}
