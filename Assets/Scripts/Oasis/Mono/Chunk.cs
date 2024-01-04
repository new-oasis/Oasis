using System.Collections.Generic;
using Oasis.Common;
using Oasis.Data;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using World = Oasis.Data.World;

namespace Oasis.Mono
{

    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class Chunk : MonoBehaviour
    {
        public int3 XYZ;
        public int3 Dims;
        private EntityManager em;
        Entity worldEntity;
        private Data.World world;
        
        public Dictionary<int3, WorldBlockVariant> Models;
        public Dictionary<int3, GameObject> ModelGameObjects;
        public GameObject ModelPrefab;
        
        private void Start()
        {
            em = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager;
            worldEntity = em.CreateEntityQuery(typeof(World)).GetSingletonEntity();
            world = em.GetComponentData<World>(worldEntity);

            ModelGameObjects = new Dictionary<int3,GameObject>();
            Models = new Dictionary<int3, WorldBlockVariant>();
            
            UpdateChunk();
        }

        public void UpdateChunk()
        {
            var voxels = em.GetBuffer<Voxel>(worldEntity).AsNativeArray().Reinterpret<ushort>();
            UpdateMesh(voxels);
            UpdateModels(voxels);
            
            // TODO Resolve these World methods...
            // GetComponent<NavMeshSurface>().BuildNavMesh();
            // ComputeHeightMap();    // TODO still used?
            // ComputeBuildingSites();  // TODO What to do about Bobs enroute?
        }

        public void UpdateMesh(NativeArray<ushort> voxels)
        {
            var mesh = Mesher.Mesher.Compute(Dims, XYZ * Dims, world.Dims, voxels);
            GetComponent<MeshFilter>().mesh = mesh;
            GetComponent<MeshCollider>().sharedMesh = mesh;
        }

        private void UpdateModels(NativeArray<ushort> voxels)
        {
            var worldBlockVariants = em.GetBuffer<WorldBlockVariant>(worldEntity);
            
            // Remove any models that are no longer in the chunk
            var indicesToRemove = new List<int3>();
            foreach (var modelXYZ in Models.Keys)
            {
                var voxelXYZ = modelXYZ + XYZ * Dims;
                var voxelIndex = voxelXYZ.ToIndex(world.Dims);
                var voxel = voxels[voxelIndex];
                var blockVariant = worldBlockVariants[voxel];
                
                if (em.GetComponentData<Block>(blockVariant.Block).BlockType != BlockType.Model)
                    indicesToRemove.Add(modelXYZ);
            }
            for (var i = indicesToRemove.Count - 1; i >= 0; i--)
            {
                var indexToRemove = indicesToRemove[i];
                Destroy(ModelGameObjects[indexToRemove]);
                Models.Remove(indexToRemove);
                ModelGameObjects.Remove(indexToRemove);
            }

            // Ensure all models that should be in the chunk are there
            for (var x = 0; x < Dims.x; x++)
            for (var y = 0; y < Dims.y; y++)
            for (var z = 0; z < Dims.z; z++)
            {
                var chunkVoxelXYZ = new int3(x, y, z);
                var voxelXyz = chunkVoxelXYZ + XYZ * Dims;
                var voxelIndex = voxelXyz.ToIndex(world.Dims);
                var voxel = voxels[voxelIndex];
        
                var blockVariant = worldBlockVariants[voxel]; // TODO optimize by getting blockIds of models and avoid 16^3 block lookups
                if (em.GetComponentData<Block>(blockVariant.Block).BlockType == BlockType.Model)
                {
                    // Continue if modelBlock already exists
                    if (Models.ContainsKey(chunkVoxelXYZ) && Models[chunkVoxelXYZ].Equals(blockVariant))
                        continue;
                    
                    // Remove any existing model at xyz
                    if (Models.ContainsKey(chunkVoxelXYZ))
                    {
                        Destroy(ModelGameObjects[chunkVoxelXYZ]);
                        ModelGameObjects.Remove(chunkVoxelXYZ);
                        Models.Remove(chunkVoxelXYZ);
                    }
                    
                    // Add new model at xyz
                    var model = Instantiate(ModelPrefab);
                    var blockVariants = em.GetBuffer<Variant>(blockVariant.Block);
                    var variant = blockVariants[blockVariant.VariantIndex];
                    var modelMesh = em.GetSharedComponentManaged<ModelMesh>(variant.Model).Value;
                    model.GetComponent<MeshFilter>().sharedMesh = modelMesh;
                    model.GetComponent<MeshCollider>().sharedMesh = modelMesh;
                    model.transform.parent = transform;
                    model.transform.localPosition = new Vector3(x, y, z);
                    ModelGameObjects.Add(chunkVoxelXYZ, model);
                    Models.Add(chunkVoxelXYZ, blockVariant);
                }
            }
        }
        
    }
}

