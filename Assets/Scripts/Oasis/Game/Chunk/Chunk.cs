using System.Collections.Generic;
using System.Diagnostics;
using Oasis.ECS.BlockStates;
using Oasis.ECS.Common;
using Oasis.ECS.World;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Oasis.Game.Chunk
{

    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class Chunk : MonoBehaviour
    {
        public int3 xyz;
        public int3 dims;
        private EntityManager em;
        Entity worldEntity;
        private Oasis.ECS.World.World world;
        
        public Dictionary<int3, BlockState> modelBlockStates;
        public Dictionary<int3, GameObject> modelGameObjects;
        public GameObject modelPrefab;
        
        private void Start()
        {
            em = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager;
            worldEntity = em.CreateEntityQuery(typeof(ECS.World.World)).GetSingletonEntity();
            world = em.GetComponentData<ECS.World.World>(worldEntity);

            modelGameObjects = new Dictionary<int3,GameObject>();
            modelBlockStates = new Dictionary<int3, BlockState>();
            
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
            var mesh = Mesher.Mesher.Compute(dims, xyz * dims, world.Dims, voxels);
            GetComponent<MeshFilter>().mesh = mesh;
            GetComponent<MeshCollider>().sharedMesh = mesh;
        }

        private void UpdateModels(NativeArray<ushort> voxels)
        {
            var query = em.CreateEntityQuery(typeof(BlockState));
            var blockStates = query.ToComponentDataArray<BlockState>(Allocator.Temp);
            
            // Remove any models that are no longer in the chunk
            var indicesToRemove = new List<int3>();
            foreach (var modelXYZ in modelBlockStates.Keys)
            {
                var voxelXYZ = modelXYZ + xyz * dims;
                var voxelIndex = voxelXYZ.ToIndex(world.Dims);
                var voxel = voxels[voxelIndex];
                var blockState = blockStates[voxel];
                
                if (blockState.Type != BlockType.Model)
                    indicesToRemove.Add(modelXYZ);
            }
            for (var i = indicesToRemove.Count - 1; i >= 0; i--)
            {
                var indexToRemove = indicesToRemove[i];
                Destroy(modelGameObjects[indexToRemove]);
                modelBlockStates.Remove(indexToRemove);
                modelGameObjects.Remove(indexToRemove);
            }

            // Ensure all models that should be in the chunk are there
            for (var x = 0; x < dims.x; x++)
            for (var y = 0; y < dims.y; y++)
            for (var z = 0; z < dims.z; z++)
            {
                var chunkVoxelXYZ = new int3(x, y, z);
                var voxelXyz = chunkVoxelXYZ + xyz * dims;
                var voxelIndex = voxelXyz.ToIndex(world.Dims);
                var voxel = voxels[voxelIndex];
        
                var blockState = blockStates[voxel]; // TODO optimize by getting blockStateIds of models and avoid 16^3 blockState lookups
                if (blockState.Type == BlockType.Model)
                {
                    // Continue if modelBlockState already exists
                    if (modelBlockStates.ContainsKey(chunkVoxelXYZ) && modelBlockStates[chunkVoxelXYZ].Equals(blockState))
                        continue;
                    
                    // Remove any existing model at xyz
                    if (modelBlockStates.ContainsKey(chunkVoxelXYZ))
                    {
                        Destroy( modelGameObjects[chunkVoxelXYZ]);
                        modelGameObjects.Remove(chunkVoxelXYZ);
                        modelBlockStates.Remove(chunkVoxelXYZ);
                    }
                    
                    // Add new model at xyz
                    var model = Instantiate(modelPrefab);
                    model.GetComponent<BlockStateMono>().BlockStateIndex = voxel;
                    // model.GetComponent<MeshFilter>().sharedMesh = blockState.Model.ComputeMesh();
                    // model.GetComponent<MeshCollider>().sharedMesh = blockState.Model.ComputeMesh();
                    // model.GetComponent<MeshRenderer>().materials = Textures.Instance.LitMaterials;
                    model.transform.parent = transform;
                    model.transform.localPosition = new Vector3(x, y, z);
                    modelGameObjects.Add(chunkVoxelXYZ, model);
                    modelBlockStates.Add(chunkVoxelXYZ, blockState);
                }
            }
        }
        
    }
}

