// using System;
// using System.Collections.Generic;
// using Oasis.Data;
// using Unity.Collections;
// using Unity.Entities;
// using Unity.Mathematics;
// using UnityEngine;
//
// namespace Oasis.Game.World
// {
//     public class World : MonoBehaviour
//     {
//     
//         private static World _instance;
//         public static World Instance { get { return _instance; } }
//     
//         public EntityManager em;
//         public Entity worldEntity;
//         public Data.World world;
//     
//         public Action<int, ushort> OnVoxelChanged;  // index, blockIndex
//     
//         [Header("Chunks")]
//         public Transform chunksParent;
//         public GameObject chunkPrefab;
//         public int3 chunkDims;
//         public Dictionary<int3, Chunk.Chunk> chunks = new Dictionary<int3, Chunk.Chunk>();
//     
//         private ushort AirIndex;
//
//         private void Awake()
//         {
//             _instance = this;
//         }
//
//         private void Start()
//         {
//             em = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager;
//             worldEntity = em.CreateEntityQuery(typeof(Data.World)).GetSingletonEntity();
//             world = em.GetComponentData<Data.World>(worldEntity);
//             // var worldBlockStates = em.GetBuffer<PaletteItem>(worldEntity);
//
//             AirIndex = 0;
//
//             for(var x=0; x<world.Dims.x/chunkDims.x; x++)
//             for (var z=0; z < world.Dims.z / chunkDims.z; z++)
//             {
//                 var chunkGO = Instantiate(chunkPrefab, chunksParent);
//                 chunkGO.transform.position = new Vector3(x*chunkDims.x, 0f, z*chunkDims.z);
//                 var chunk = chunkGO.GetComponent<Chunk.Chunk>();
//                 chunk.xyz = new int3(x, 0, z);
//                 chunk.dims = chunkDims;
//                 chunks[chunk.xyz] = chunk;
//                 chunkGO.name = chunk.xyz.ToString();
//             }
//         }
//
//  
//         public void Place(int3 voxel, BlockState blockState)
//         {
//             Debug.Log($"Got voxel {voxel} and blockState {blockState}");
//             var voxelIndex = voxel.ToIndex(world.Dims);
//
//             var query = em.CreateEntityQuery(typeof(BlockState));
//             var blockStates = query.ToComponentDataArray<BlockState>(Allocator.Temp);
//             var blockStateEntities = query.ToEntityArray(Allocator.Temp);
//             var blockStateIndex = blockStates.IndexOf(blockState);
//         
//             // Get paletteIndex
//             var palette = em.GetBuffer<BlockStateElement>(worldEntity);
//             var paletteIndex = (byte)palette.AsNativeArray().IndexOf(new BlockStateElement{Value = blockStateEntities[blockStateIndex]});
//         
//             // Update voxel
//             var voxels = em.GetBuffer<Voxel>(worldEntity);
//             voxels[voxelIndex] = new Voxel {Value = paletteIndex};
//         
//             UpdateChunkMeshes(voxelIndex);
//             OnVoxelChanged?.Invoke(voxelIndex, (ushort)blockStateIndex);
//         }
//
//         public void Remove(int3 voxel)
//         {
//             Debug.Log($"Remove voxel {voxel}");
//             var voxelIndex = voxel.ToIndex(world.Dims);
//             var voxels = em.GetBuffer<Voxel>(worldEntity);
//             voxels[voxelIndex] = new Voxel {Value = AirIndex};
//             UpdateChunkMeshes(voxelIndex);
//             OnVoxelChanged?.Invoke(voxelIndex, AirIndex);
//         }
//     
//         private void UpdateChunkMeshes(int voxelIndex)
//         {
//             var voxelXyz = voxelIndex.ToInt3(world.Dims);
//             var chunkXyz = voxelXyz / chunkDims;
//             chunks[chunkXyz].UpdateChunk();
//             
//             if (voxelXyz.x % chunkDims.x == 0 && chunks.ContainsKey(chunkXyz - new int3(1, 0, 0)))
//                 chunks[chunkXyz - new int3(1, 0, 0)].UpdateChunk();
//             else if (voxelXyz.x % chunkDims.x == chunkDims.x - 1 && chunks.ContainsKey(chunkXyz + new int3(1, 0, 0)))
//                 chunks[chunkXyz + new int3(1, 0, 0)].UpdateChunk();
//             else if (voxelXyz.y % chunkDims.y == 0 && chunks.ContainsKey(chunkXyz - new int3(0, 1, 0)))
//                 chunks[chunkXyz - new int3(0, 1, 0)].UpdateChunk();
//             else if (voxelXyz.y % chunkDims.y == chunkDims.y - 1 && chunks.ContainsKey(chunkXyz + new int3(0, 1, 0)))
//                 chunks[chunkXyz + new int3(0, 1, 0)].UpdateChunk();
//             else if (voxelXyz.z % chunkDims.z == 0 && chunks.ContainsKey(chunkXyz - new int3(0, 0, 1)))
//                 chunks[chunkXyz - new int3(0, 0, 1)].UpdateChunk();
//             else if (voxelXyz.z % chunkDims.z == chunkDims.z - 1 && chunks.ContainsKey(chunkXyz + new int3(0, 0, 1)))
//                 chunks[chunkXyz + new int3(0, 0, 1)].UpdateChunk();
//         }
//     
//     }
// }