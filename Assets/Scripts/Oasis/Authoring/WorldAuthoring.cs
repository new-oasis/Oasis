using System;
using System.Collections.Generic;
using System.Linq;
using Oasis.Common;
using Oasis.Data;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Block = Oasis.Assets.Block;
using World = Oasis.Data.World;

namespace Oasis.Authoring
{
    [Serializable]
    public struct WorldBlockVariant
    {
        public Block Block;
        public int VariantIndex;
    }


    public class WorldAuthoring : MonoBehaviour
    {
        public WorldType WorldType;
        public int3 Dims;
        public List<WorldBlockVariant> BlockVariants;

        public ushort Dirt;
        public ushort Grass;

        private class WorldBaker : Baker<WorldAuthoring>
        {
            public override void Bake(WorldAuthoring authoring)
            {
                // World
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new World
                {
                    WorldType = authoring.WorldType,
                    Dims = authoring.Dims
                });

                // WorldBlockVariants
                var worldBlockVariants = AddBuffer<Data.WorldBlockState>(entity);
                var blockAuthoringComponents = FindObjectsOfType<BlockAuthoring>();
                foreach (var blockVariant in authoring.BlockVariants)
                {
                    var blockAuthoringComponent = blockAuthoringComponents
                        .FirstOrDefault(component => component.Block == blockVariant.Block);
                    if (blockAuthoringComponent == null)
                    {
                        Debug.LogError("No block authoring component found for " + blockVariant.Block.name);
                        return;
                    }

                    worldBlockVariants.Add(new Data.WorldBlockState
                    {
                        Block = GetEntity(blockAuthoringComponent, TransformUsageFlags.None),
                        VariantIndex = blockVariant.VariantIndex
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

                    // voxels[i] = new Voxel {Value = 0};
                    // Debug.Log(xyz + " = " + voxels[i].Value);
                }
            }
        }
    }
}