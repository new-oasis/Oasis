using Oasis.Data;
using Unity.Entities.UniversalDelegates;
using Unity.Mathematics;
using UnityEngine;

public static class RaycastHitExtensions
{

    public static int3 ToVoxel(this RaycastHit hit)
    {
        int3 voxel = new int3(0, 0, 0);

        if (hit.normal.x == 1.0f && IsCloseToRoundNumber(hit.point.x)) // east
            voxel.x = (int)Mathf.Round(hit.point.x) -1;
        else if (hit.normal.x == -1.0f && IsCloseToRoundNumber(hit.point.x)) // west
            voxel.x = (int)Mathf.Round(hit.point.x);
        else if (hit.normal.x == 1.0f) 
        {
        Debug.Log($"EAST Hit point: {hit.point} \t Hit normal: {hit.normal} \t Voxel: {voxel} \t Voxel int3: {voxel}");
            voxel.x = (int)Mathf.Floor(hit.point.x);
        }
        else if (hit.normal.x == -1.0f) 
        {
        Debug.Log($"WEST Hit point: {hit.point} \t Hit normal: {hit.normal} \t Voxel: {voxel} \t Voxel int3: {voxel}");
            voxel.x = (int)Mathf.Floor(hit.point.x);
        }
        else
        {
            voxel.x = (int)Mathf.Floor(hit.point.x);
        }




        if (hit.normal.y == 1.0f && IsCloseToRoundNumber(hit.point.y)) // top
            voxel.y = (int)Mathf.Round(hit.point.y) -1;
        else if (hit.normal.y == -1.0f && IsCloseToRoundNumber(hit.point.y)) // bottom
            voxel.y = (int)Mathf.Round(hit.point.y) +1;
        else if (hit.normal.y == 1.0f) 
        {
        Debug.Log($"UP Hit point: {hit.point} \t Hit normal: {hit.normal} \t Voxel: {voxel} \t Voxel int3: {voxel}");
            voxel.y = (int)Mathf.Floor(hit.point.y);
        }
        else if (hit.normal.y == -1.0f) 
        {
        Debug.Log($"DOWN Hit point: {hit.point} \t Hit normal: {hit.normal} \t Voxel: {voxel} \t Voxel int3: {voxel}");
            voxel.y = (int)Mathf.Floor(hit.point.y);
        }
        else
            voxel.y = (int)Mathf.Floor(hit.point.y);




        if (hit.normal.z == 1.0f && IsCloseToRoundNumber(hit.point.z)) // north
            voxel.z = (int)Mathf.Round(hit.point.z) -1;
        else if (hit.normal.z == -1.0f && IsCloseToRoundNumber(hit.point.z)) // south
            voxel.z = (int)Mathf.Round(hit.point.z);
        else if (hit.normal.z == -1.0f) 
        {
        Debug.Log($"SOUTH Hit point: {hit.point} \t Hit normal: {hit.normal} \t Voxel: {voxel} \t Voxel int3: {voxel}");
            voxel.z = (int)Mathf.Floor(hit.point.z);
        }
        else if (hit.normal.z == 1.0f) 
        {
        Debug.Log($"NORTH Hit point: {hit.point} \t Hit normal: {hit.normal} \t Voxel: {voxel} \t Voxel int3: {voxel}");
            voxel.z = (int)Mathf.Floor(hit.point.z);
        }
        else
            voxel.z = (int)Mathf.Floor(hit.point.z);


        // Debug.Log($"Hit point: {hit.point} \t Hit normal: {hit.normal} \t Voxel: {voxel} \t Voxel int3: {voxel}");
        return voxel;
    }

    public static int3 ToAdjacentVoxel(this RaycastHit hit)
    {
        int3 voxel = new int3(0, 0, 0);

        if (hit.normal.x == 1.0f && IsCloseToRoundNumber(hit.point.x)) // east
            voxel.x = (int)Mathf.Round(hit.point.x);
        else if (hit.normal.x == -1.0f && IsCloseToRoundNumber(hit.point.x)) // west
            voxel.x = (int)Mathf.Round(hit.point.x) -1;
        else
            voxel.x = (int)Mathf.Floor(hit.point.x);


        if (hit.normal.y == 1.0f && IsCloseToRoundNumber(hit.point.y)) // top
            voxel.y = (int)Mathf.Round(hit.point.y);
        else if (hit.normal.y == -1.0f && IsCloseToRoundNumber(hit.point.y)) // bottom
            voxel.y = (int)Mathf.Round(hit.point.y);
        else
            voxel.y = (int)Mathf.Floor(hit.point.y);


        if (hit.normal.z == 1.0f && IsCloseToRoundNumber(hit.point.z)) // north
            voxel.z = (int)Mathf.Round(hit.point.z);
        else if (hit.normal.z == -1.0f && IsCloseToRoundNumber(hit.point.z)) // south
            voxel.z = (int)Mathf.Round(hit.point.z) -1;
        else
            voxel.z = (int)Mathf.Floor(hit.point.z);

        // Debug.Log($"Hit point: {hit.point} \t Hit normal: {hit.normal} \t Voxel: {voxel} \t Voxel int3: {voxel}");
        return voxel;
    }



    private static bool IsCloseToRoundNumber(float value)
    {
        float threshold = 0.1f;
        return Mathf.Abs(value - Mathf.Round(value)) < threshold;
    }
}
