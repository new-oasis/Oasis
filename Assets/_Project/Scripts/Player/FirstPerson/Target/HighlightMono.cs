using System.Collections;
using System.Collections.Generic;
using Oasis;
using Unity.Mathematics;
using UnityEngine;

public class HighlightMono : MonoBehaviour
{

    public Target target;

    void Start()
    {
        target.Changed += MoveHighlight;
    }

    void MoveHighlight(int3 voxel)
    {
        var offset = new float3(0.5f, 0.5f, 0.5f);
        if (!transform.position.Equals(voxel + offset))
            transform.position = voxel + offset;
    }
}
