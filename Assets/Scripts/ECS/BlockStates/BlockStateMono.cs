using System;
using Oasis.Blocks;
using Unity.Collections;
using Oasis.Models;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Oasis.BlockStates
{

    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class BlockStateMono : MonoBehaviour
    {
        public ushort BlockStateIndex;
        
        private void Start()
        {
            var voxels = new NativeArray<ushort>(1, Allocator.Temp);
            voxels[0] = BlockStateIndex;
            var mesh = Mesher.Mesher.Compute(new int3(1), new int3(0), new int3(1), voxels);
            GetComponent<MeshFilter>().mesh = mesh;
        }
        
    }

}

