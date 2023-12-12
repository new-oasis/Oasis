using System.Collections.Generic;
using Oasis.BlockStates;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

namespace Oasis.World
{
    public class WorldAuthoring : MonoBehaviour
    {
        public WorldType worldType;
        public int3 dims;
        public List<GameObject> blockStates;

        [FormerlySerializedAs("dirt")] public GameObject dirtBlockState;

        private class WorldBaker : Baker<WorldAuthoring>
        {
            public override void Bake(WorldAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new World
                {
                    WorldType = authoring.worldType,
                    Dims = authoring.dims,
                });

                // BlockStates
                var blockStates = AddBuffer<BlockStateReference>(entity);
                foreach (var authoringBlockStateGo in authoring.blockStates)
                {
                    Debug.Log("Adding blockstate " + authoringBlockStateGo);
                    blockStates.Add(new BlockStateReference {Value = GetEntity(authoringBlockStateGo)});
                }

                // Voxels
                var voxels = AddBuffer<Voxel>(entity);
                voxels.ResizeUninitialized(authoring.dims.x * authoring.dims.y * authoring.dims.z);

                
                var dirtEntity = GetEntity(authoring.dirtBlockState, TransformUsageFlags.None);
                
                // TODO Update below with working Comparer
                var dirtBlockStateIndex = blockStates.AsNativeArray().IndexOf(new BlockStateReference {Value = dirtEntity});
                ComputeVoxels(authoring.worldType, authoring.dims, dirtBlockStateIndex, voxels);
            }

            private void ComputeVoxels(WorldType worldType, int3 dims, int dirtIndex, DynamicBuffer<Voxel> voxels)
            {
                Debug.Log("Compute Voxels");
                for (var i = 0; i < voxels.Length; i++)
                {
                    var xyz = i.ToInt3(dims);
                    if (worldType == WorldType.Flat)
                        voxels[i] = new Voxel{Value = (ushort) (xyz.y < 1 ? dirtIndex : 0)};

                    else if (worldType == WorldType.Hills)
                    {
                        var scale = 0.1f;
                        var amplitude = 5.0f;
                        var xCoord = xyz.x * scale;
                        var zCoord = xyz.z * scale;
                        var height = Mathf.PerlinNoise(xCoord, zCoord) * amplitude;

                        // if (xyz.y < height - 1f)
                        //     voxels[i] = (ushort) BlockStates.FindIndex(b => b.Block.name == "Dirt");
                        // else if (xyz.y < height)
                        //     voxels[i] = (ushort) BlockStates.FindIndex(b => b.Block.name == "Grass Block");
                        // else
                        //     voxels[i] = AirIndex;
                    }
                    // voxels[i] = 1;
                    // Debug.Log(xyz + " = " + voxels[i]);
                }
            }
        }
    }
}