using System.Collections.Generic;
using Oasis.Common;
using Oasis.Data;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

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
        
        public Dictionary<int3, Block> ModelBlocks;
        public Dictionary<int3, GameObject> ModelGameObjects;
        public GameObject ModelPrefab;
        
        private void Start()
        {
            em = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager;
            worldEntity = em.CreateEntityQuery(typeof(Data.World)).GetSingletonEntity();
            world = em.GetComponentData<Data.World>(worldEntity);

            ModelGameObjects = new Dictionary<int3,GameObject>();
            ModelBlocks = new Dictionary<int3, Block>();
            
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
            var query = em.CreateEntityQuery(typeof(Block));
            var blocks = query.ToComponentDataArray<Block>(Allocator.Temp);
            
            // Remove any models that are no longer in the chunk
            var indicesToRemove = new List<int3>();
            foreach (var modelXYZ in ModelBlocks.Keys)
            {
                var voxelXYZ = modelXYZ + XYZ * Dims;
                var voxelIndex = voxelXYZ.ToIndex(world.Dims);
                var voxel = voxels[voxelIndex];
                var block = blocks[voxel];
                
                if (block.BlockType != BlockType.Model)
                    indicesToRemove.Add(modelXYZ);
            }
            for (var i = indicesToRemove.Count - 1; i >= 0; i--)
            {
                var indexToRemove = indicesToRemove[i];
                Destroy(ModelGameObjects[indexToRemove]);
                ModelBlocks.Remove(indexToRemove);
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
        
                var block = blocks[voxel]; // TODO optimize by getting blockIds of models and avoid 16^3 block lookups
                if (block.BlockType == BlockType.Model)
                {
                    // Continue if modelBlock already exists
                    if (ModelBlocks.ContainsKey(chunkVoxelXYZ) && ModelBlocks[chunkVoxelXYZ].Equals(block))
                        continue;
                    
                    // Remove any existing model at xyz
                    if (ModelBlocks.ContainsKey(chunkVoxelXYZ))
                    {
                        Destroy(ModelGameObjects[chunkVoxelXYZ]);
                        ModelGameObjects.Remove(chunkVoxelXYZ);
                        ModelBlocks.Remove(chunkVoxelXYZ);
                    }
                    
                    // Add new model at xyz
                    var model = Instantiate(ModelPrefab);
                    // model.GetComponent<Block>().BlockIndex = voxel;
                    
                    // model.GetComponent<MeshFilter>().sharedMesh = block.Model.ComputeMesh();
                    // model.GetComponent<MeshCollider>().sharedMesh = block.Model.ComputeMesh();
                    // model.GetComponent<MeshRenderer>().materials = Textures.Instance.LitMaterials;
                    model.transform.parent = transform;
                    model.transform.localPosition = new Vector3(x, y, z);
                    ModelGameObjects.Add(chunkVoxelXYZ, model);
                    ModelBlocks.Add(chunkVoxelXYZ, block);
                }
            }
        }
        
    }
}

