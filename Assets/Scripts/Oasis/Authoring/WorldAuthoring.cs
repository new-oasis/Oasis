using System;
using System.Collections.Generic;
using System.Linq;
using Oasis.Common;
using Oasis.Data;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Oasis.Authoring
{
    
    // Initial BlockStates
    [Serializable]
    public struct WorldBlockStateAuthoring
    {
        public Assets.Block Block;
        public int BlockStatesIndex;
    }

    public class WorldAuthoring : MonoBehaviour
    {
        public WorldType WorldType;
        public int3 Dims;
        public List<WorldBlockStateAuthoring> BlockStates;

        public ushort Dirt;
        public ushort Grass;

        private class WorldBaker : Baker<WorldAuthoring>
        {
            public override void Bake(WorldAuthoring authoring)
            {
                // World
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new Data.World
                {
                    WorldType = authoring.WorldType,
                    Dims = authoring.Dims
                });

                // WorldBlockVariants
                var worldBlockVariants = AddBuffer<Data.WorldBlockState>(entity);
                var blockAuthoringComponents = FindObjectsOfType<BlockAuthoring>();
                foreach (var blockState in authoring.BlockStates)
                {
                    // Find Block
                    var blockAuthoringComponent = blockAuthoringComponents
                        .FirstOrDefault(component => component.Block == blockState.Block);
                    if (blockAuthoringComponent == null)
                    {
                        Debug.LogError("No block authoring component found for " + blockState.Block.name);
                        return;
                    }

                    worldBlockVariants.Add(new Data.WorldBlockState
                    {
                        Block = GetEntity(blockAuthoringComponent, TransformUsageFlags.None),
                        BlockStatesIndex = blockState.BlockStatesIndex
                    });
                }

                // Voxels
                var voxels = AddBuffer<Voxel>(entity);
                voxels.ResizeUninitialized(authoring.Dims.x * authoring.Dims.y * authoring.Dims.z);

                for (var i = 0; i < voxels.Length; i++)
                {
                    var xyz = i.ToInt3(authoring.Dims);
                    if (authoring.WorldType == WorldType.Flat)
                    {
                        voxels[i] = new Voxel {Value = (ushort) (xyz.y < 1 ? authoring.Grass : 0)};
                    }

                    else if (authoring.WorldType == WorldType.Hills)
                    {
                        var scale = 0.1f;
                        var amplitude = 5.0f;
                        var xCoord = xyz.x * scale;
                        var zCoord = xyz.z * scale;
                        var height = Mathf.PerlinNoise(xCoord, zCoord) * amplitude;

                        if (xyz.y < height - 1f)
                            voxels[i] = new Voxel {Value = authoring.Dirt};
                        else if (xyz.y < height)
                            voxels[i] = new Voxel {Value = authoring.Grass};
                        else
                            voxels[i] = new Voxel {Value = 0};
                    }
                }
            }

        }
    }
}


// Q. Should World have buffer of blockState refs or actual blockStates?
// A. Refs.
// Unable to block.blockStates buffer from this world authoring anyway.
// Can denormalize in Mesher if needed

