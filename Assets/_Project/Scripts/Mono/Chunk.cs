using System.Collections.Generic;
using Oasis.Common;
using Oasis.Data;
using TMPro;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem.UI;
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
        
        public Dictionary<int3, BlockStateRef> Models;
        public Dictionary<int3, GameObject> ModelGameObjects;
        public GameObject ModelPrefab;
        
        private void Start()
        {
            em = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager;
            worldEntity = em.CreateEntityQuery(typeof(Data.World)).GetSingletonEntity();
            world = em.GetComponentData<Data.World>(worldEntity);

            ModelGameObjects = new Dictionary<int3,GameObject>();
            Models = new Dictionary<int3, BlockStateRef>();
            
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
            var worldBlockStates = em.GetBuffer<Data.BlockStateRef>(worldEntity);
            
            // Remove any models that are no longer in the chunk
            var indicesToRemove = new List<int3>();
            foreach (var modelXYZ in Models.Keys)
            {
                var voxelXYZ = modelXYZ + XYZ * Dims;
                var voxelIndex = voxelXYZ.ToIndex(world.Dims);
                var voxel = voxels[voxelIndex];
                var worldBlockState = worldBlockStates[voxel];
                
                if (em.GetComponentData<Block>(worldBlockState.Block).BlockType != BlockType.Model)
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
        
                var blockStateRef = worldBlockStates[voxel]; // TODO optimize by getting blockIds of models and avoid 16^3 block lookups
                if (em.GetComponentData<Block>(blockStateRef.Block).BlockType == BlockType.Model)
                {
                    // Debug.Log($"Found model block at {chunkVoxelXYZ}");
                    
                    // Continue if modelBlock already exists
                    if (Models.ContainsKey(chunkVoxelXYZ) && Models[chunkVoxelXYZ].Equals(blockStateRef))
                        continue;
                    
                    // Remove any existing model at xyz
                    if (Models.ContainsKey(chunkVoxelXYZ))
                    {
                        Destroy(ModelGameObjects[chunkVoxelXYZ]);
                        ModelGameObjects.Remove(chunkVoxelXYZ);
                        Models.Remove(chunkVoxelXYZ);
                    }
                    
                    // Add new model at xyz
                    var modelGO = Instantiate(ModelPrefab);
                    
                    // Get the blockState for the model
                    var blockStates = em.GetBuffer<Data.BlockState>(blockStateRef.Block);
                    var modelEntity = blockStates[blockStateRef.BlockStatesIndex].Model;
                    var modelMesh = em.GetSharedComponentManaged<ModelMesh>(modelEntity).Value;
                    var modelData = em.GetComponentData<Data.Model>(modelEntity);
                    modelGO.name = $"{chunkVoxelXYZ.ToString()} {modelData.Name}";

                    modelGO.GetComponent<MeshFilter>().sharedMesh = modelMesh;
                    ModelGameObjects.Add(chunkVoxelXYZ, modelGO);
                    Models.Add(chunkVoxelXYZ, blockStateRef);

                    // Rotate model;  Assumes pivot in voxel center
                    var rotationAngle = blockStates[blockStateRef.BlockStatesIndex].Rotation.Angle;
                    var rotationAxis = blockStates[blockStateRef.BlockStatesIndex].Rotation.Axis;
                    Quaternion rotation = Quaternion.identity;
                    if (rotationAxis == 0)
                        rotation = Quaternion.Euler(rotationAngle, 0, 0);
                    else if (rotationAxis == 1)
                        rotation = Quaternion.Euler(0, rotationAngle, 0);
                    else if (rotationAxis == 2)
                        rotation = Quaternion.Euler(0, 0, rotationAngle);
                    modelGO.transform.localRotation = rotation;


                    modelGO.transform.parent = transform;
                    modelGO.transform.localPosition = new Vector3(x, y, z) + new Vector3(0.5f, 0.5f, 0.5f);


                    // Collider for playerTarget and playerMovement collider
                    var from = modelData.NonSolidHitBoxFrom;
                    var to = modelData.NonSolidHitBoxTo;
                    var boxCollider = modelGO.AddComponent<BoxCollider>();
                    boxCollider.center = (from + to) / 2;
                    boxCollider.size = to - from;
                    boxCollider.isTrigger = false;
                    // Debug.Log("from: " + from + " \tto: " + to + " \tcenter: " + boxCollider.center + " \tsize: " + boxCollider.size);
                    if (!modelData.NonSolidBlocksMovement)
                        modelGO.layer = LayerMask.NameToLayer("Ignore Player Movement");


                    // Add point light component to game object and set light intensity
                    if (modelData.Light)
                    {
                        var lightGO = new GameObject("Light");
                        lightGO.transform.parent = modelGO.transform;
                        var light = lightGO.AddComponent<Light>();
                        light.type = LightType.Point;
                        // light.intensity = modelData.Light;
                        light.range = 10;
                        light.color = Color.white;
                        lightGO.transform.localPosition = new Vector3(0f, 0f, 0f);
                        // lightGO.transform.localPosition = new Vector3(modelData.LightPosition.x, modelData.LightPosition.y, modelData.LightPosition.z);
                    
                        // Disable castShadows
                        // modelGO.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                    }

                }
            }
        }
        
    }
}

