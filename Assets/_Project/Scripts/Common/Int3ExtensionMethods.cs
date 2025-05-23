using System;
using Unity.Mathematics;
using UnityEngine;
using System.Collections;


public static class Int3ExtensionMethods
{
    public static int ToYZXIndex(this int3 i, int3 Dims)
    {
        return (i.y * Dims.z * Dims.x) + (i.z * Dims.x) + (i.x);
    }
    public static int ToIndex(this int3 i, int size = 16)
    {
        return i.ToIndex(new int3(size));
    }

    public static int ToIndex(this int3 i, int3 dims)
    {
        if (dims.Equals(default(int3)))
            dims = new int3(16);
        
        // YZX 
        // return i.y * dims.x * dims.z + i.z * dims.x + i.x; 
        
        // XYZ
        return i.x + i.y * dims.x + i.z * dims.x * dims.y;
        // return i.x * dims.y * dims.z + i.y * dims.z + i.z; // 
    }
    

        
    public static int3 Chunk(this int3 i)
    {
        return new int3((int) Math.Floor(i.x / 16.0f), (int) Math.Floor(i.y / 16.0f),
            (int) Math.Floor(i.z / 16.0f));
    }

    public static int3 ChunkVoxel(this int3 xyz)
    {
        return new int3(((xyz.x % 16 + 16) % 16), ((xyz.y % 16 + 16) % 16), ((xyz.z % 16 + 16) % 16)); // xyz
    }

    public static int Volume(this int3 xyz)
    {
        return xyz.x * xyz.y * xyz.z;
    }
    
    public static int3 Increment(this int3 i3, int i)
    {
        if (i == 0)
            return new int3(i3.x + 1, i3.y, i3.z);
        if (i == 1)
            return new int3(i3.x, i3.y + 1, i3.z);
        if (i == 2)
            return new int3(i3.x, i3.y, i3.z + 1);
        return i3;
    }

    public static bool OOB(this int3 i3, int3 i)
    {
        return i.x >= i3.x ||
               i.y >= i3.y ||
               i.z >= i3.z ||
               i.x < 0 ||
               i.y < 0 ||
               i.z < 0;
    }

    public static int3 North(this int3 i)
    {
        return i + new int3(0, 0, 1);
    }
    public static int3 East(this int3 i)
    {
        return i + new int3(1, 0, 0);
    }
    public static int3 South(this int3 i)
    {
        return i + new int3(0, 0, -1);
    }
    public static int3 West(this int3 i)
    {
        return i + new int3(-1, 0, 0);
    }
    
    
    public static int Magnitude(this int3 i)
    {
        // return (i.x * i.x + i.y * i.y + i.z * i.z);
        return (i.x * i.y * i.z);
    }
    
    public static string ToStr(this int3 i)
    {
        return i.x + ", " + i.y + ", " + i.z;
    }

    // public static Int3 ToInt3(this int3 i)
    // {
    //     return new Int3() {X = i.x, Y = i.y, Z = i.z};
    // }
    public static int3 FlipZ(this int3 id)
    {
        return new int3(id.x, id.y, 0-id.z);
        
    }
    
    public static Int32[] ToArr(this int3 i)
    {
        return new int[] {i.x, i.y, i.z};
    }

    public static Vector3 ToVector3(this int3 i)
    {
        return new Vector3(i.x, i.y, i.z);
    }

    public static float3 ToFloat3(this int3 i)
    {
        return new float3(i.x, i.y, i.z);
    }
    
    
    // static method to compute euclidean distance between two points
    public static float Distance(this int3 a, int3 b)
    {
        return math.sqrt(math.pow(a.x - b.x, 2) + math.pow(a.y - b.y, 2) + math.pow(a.z - b.z, 2));
    }
    
}
