using System.Collections.Generic;
using Oasis.ECS.BlockStates;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Oasis.ECS.World
{
    public class WorldAuthoring : MonoBehaviour
    {
        public WorldType worldType;
        public int3 dims;
        public List<GameObject> blockStates;

        public GameObject dirtBlockState;
        public GameObject grassBlockState;

        private class WorldBaker : Baker<WorldAuthoring>
        {
            public override void Bake(WorldAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new ECS.World.World
                {
                    WorldType = authoring.worldType,
                    Dims = authoring.dims,
                });

                // BlockStates
                var blockStates = AddBuffer<BlockStateElement>(entity);
                foreach (var authoringBlockStateGo in authoring.blockStates)
                {
                    blockStates.Add(new BlockStateElement {Value = GetEntity(authoringBlockStateGo)});
                }

                // Voxels
                var voxels = AddBuffer<Voxel>(entity);
                voxels.ResizeUninitialized(authoring.dims.x * authoring.dims.y * authoring.dims.z);

                
                // TODO Update below with working Comparer
                var dirtEntity = GetEntity(authoring.dirtBlockState, TransformUsageFlags.None);
                var dirtIndex = (byte)blockStates.AsNativeArray().IndexOf(new BlockStateElement {Value = dirtEntity});
                
                var grassEntity = GetEntity(authoring.grassBlockState, TransformUsageFlags.None);
                var grassIndex = (byte)blockStates.AsNativeArray().IndexOf(new BlockStateElement {Value = grassEntity});

                for (var i = 0; i < voxels.Length; i++)
                {
                    var xyz = i.ToInt3(authoring.dims);
                    if (authoring.worldType == WorldType.Flat)
                        voxels[i] = new Voxel{Value = (ushort) (xyz.y < 1 ? grassIndex : 0)};

                    else if (authoring.worldType == WorldType.Hills)
                    {
                        var scale = 0.1f;
                        var amplitude = 5.0f;
                        var xCoord = xyz.x * scale;
                        var zCoord = xyz.z * scale;
                        var height = Mathf.PerlinNoise(xCoord, zCoord) * amplitude;

                        if (xyz.y < height - 1f)
                            voxels[i] = new Voxel{Value = dirtIndex};
                        else if (xyz.y < height)
                            voxels[i] = new Voxel{Value = grassIndex};
                        else
                            voxels[i] = new Voxel{Value = 0};
                    }
                    // voxels[i] = 1;
                    // Debug.Log(xyz + " = " + voxels[i]);
                }
            }
        }
    }
}