using System;
using System.Collections.Generic;
using Oasis.Data;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Oasis.Mono
{
    public class World : MonoBehaviour
    {
        private static World _instance;
        public static World Instance { get { return _instance; } }

        public Action<int, ushort> OnVoxelChanged;  // index, blockIndex
    
        [Header("Chunks")]
        public Transform ChunksParent;
        public GameObject ChunkPrefab;
        public int3 ChunkDims;
        private readonly Dictionary<int3, Chunk> _chunks = new();
    
        
        private EntityManager _em;
        private Entity _worldEntity;
        private Data.World _worldData;
        private ushort _airIndex;

        private void Awake()
        {
            _instance = this;
        }

        private void Start()
        {
            _em = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager;
            _worldEntity = _em.CreateEntityQuery(typeof(Data.World)).GetSingletonEntity();
            _worldData = _em.GetComponentData<Data.World>(_worldEntity);

            _airIndex = 0;

            for(var x=0; x<_worldData.Dims.x/ChunkDims.x; x++)
            for (var z=0; z < _worldData.Dims.z / ChunkDims.z; z++)
            {
                var chunkGameObject = Instantiate(ChunkPrefab, ChunksParent);
                chunkGameObject.transform.position = new Vector3(x*ChunkDims.x, 0f, z*ChunkDims.z);
                var chunk = chunkGameObject.GetComponent<Chunk>();
                chunk.XYZ = new int3(x, 0, z);
                chunk.Dims = ChunkDims;
                _chunks[chunk.XYZ] = chunk;
                chunkGameObject.name = chunk.XYZ.ToString();
            }
        }

        public void Place(int3 voxel, BlockStateRef blockStateRef)
        {
            Debug.Log($"Got voxel {voxel} and blockStateRef {_em.GetName(blockStateRef.Block)} blockStateIndex {blockStateRef.BlockStatesIndex}");
            var voxelIndex = voxel.ToIndex(_worldData.Dims);

            // Get worldBlockStateIndex
            var worldBlockStates = _em.GetBuffer<BlockStateRef>(_worldEntity);
            var worldBlockStateIndex = worldBlockStates.AsNativeArray().IndexOf(blockStateRef);
            if (worldBlockStateIndex == -1)
                Debug.LogError($"Could not find blockStateRef {blockStateRef} in worldBlockStates");
            Debug.Log($"worldBlockStateIndex is : {worldBlockStateIndex}");
            
            // Update voxel
            var voxels = _em.GetBuffer<Voxel>(_worldEntity);
            voxels[voxelIndex] = new Voxel {Value = (ushort)worldBlockStateIndex};
        
            UpdateChunkMeshes(voxelIndex);
            OnVoxelChanged?.Invoke(voxelIndex, (ushort)worldBlockStateIndex);
        }

        public void Remove(int3 voxel)
        {
            Debug.Log($"Remove voxel {voxel}");
            var voxelIndex = voxel.ToIndex(_worldData.Dims);
            var voxels = _em.GetBuffer<Voxel>(_worldEntity);
            voxels[voxelIndex] = new Voxel {Value = _airIndex};
            UpdateChunkMeshes(voxelIndex);
            OnVoxelChanged?.Invoke(voxelIndex, _airIndex);
        }
    
        private void UpdateChunkMeshes(int voxelIndex)
        {
            var voxelXyz = voxelIndex.ToInt3(_worldData.Dims);
            var chunkXyz = voxelXyz / ChunkDims;
            _chunks[chunkXyz].UpdateChunk();
            
            if (voxelXyz.x % ChunkDims.x == 0 && _chunks.ContainsKey(chunkXyz - new int3(1, 0, 0)))
                _chunks[chunkXyz - new int3(1, 0, 0)].UpdateChunk();
            else if (voxelXyz.x % ChunkDims.x == ChunkDims.x - 1 && _chunks.ContainsKey(chunkXyz + new int3(1, 0, 0)))
                _chunks[chunkXyz + new int3(1, 0, 0)].UpdateChunk();
            else if (voxelXyz.y % ChunkDims.y == 0 && _chunks.ContainsKey(chunkXyz - new int3(0, 1, 0)))
                _chunks[chunkXyz - new int3(0, 1, 0)].UpdateChunk();
            else if (voxelXyz.y % ChunkDims.y == ChunkDims.y - 1 && _chunks.ContainsKey(chunkXyz + new int3(0, 1, 0)))
                _chunks[chunkXyz + new int3(0, 1, 0)].UpdateChunk();
            else if (voxelXyz.z % ChunkDims.z == 0 && _chunks.ContainsKey(chunkXyz - new int3(0, 0, 1)))
                _chunks[chunkXyz - new int3(0, 0, 1)].UpdateChunk();
            else if (voxelXyz.z % ChunkDims.z == ChunkDims.z - 1 && _chunks.ContainsKey(chunkXyz + new int3(0, 0, 1)))
                _chunks[chunkXyz + new int3(0, 0, 1)].UpdateChunk();
        }
    
    }
}