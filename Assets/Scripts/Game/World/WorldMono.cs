using System;
using System.Collections.Generic;
using Oasis.Chunks;
using Oasis.World;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using World = Unity.Entities.World;

public class WorldMono : MonoBehaviour
{
    public EntityManager em;
    public Entity worldEntity;
    public Oasis.World.World world;
    
    public Action<int, ushort> OnVoxelChanged;  // index, blockIndex
    
    [Header("Chunks")]
    public Transform chunksParent;
    public GameObject chunkPrefab;
    public int3 chunkDims;
    public Dictionary<int3, ChunkMono> chunks = new Dictionary<int3, ChunkMono>();
    
    private ushort AirIndex;
    
    
    private void Start()
    {
        em = World.DefaultGameObjectInjectionWorld.EntityManager;
        worldEntity = em.CreateEntityQuery(typeof(Oasis.World.World)).GetSingletonEntity();
        world = em.GetComponentData<Oasis.World.World>(worldEntity);
        // var worldBlockStates = em.GetBuffer<BlockStateReference>(worldEntity);

        AirIndex = 0;

        for(var x=0; x<world.Dims.x/chunkDims.x; x++)
        for (var z=0; z < world.Dims.z / chunkDims.z; z++)
        {
            var chunkGO = Instantiate(chunkPrefab, chunksParent);
            chunkGO.transform.position = new Vector3(x*chunkDims.x, 0f, z*chunkDims.z);
            var chunk = chunkGO.GetComponent<ChunkMono>();
            chunk.xyz = new int3(x, 0, z);
            chunk.dims = chunkDims;
            chunks[chunk.xyz] = chunk;
            chunkGO.name = chunk.xyz.ToString();
        }
    }

 
    public void Place(int voxelIndex, ushort blockIndex)
    {
        var voxels = em.GetBuffer<Voxel>(worldEntity);
        voxels[voxelIndex] = new Voxel {Value = blockIndex};
        UpdateChunkMeshes(voxelIndex);
        OnVoxelChanged?.Invoke(voxelIndex, blockIndex);
    }

    public void Remove(int voxelIndex)
    {
        var voxels = em.GetBuffer<Voxel>(worldEntity);
        voxels[voxelIndex] = new Voxel {Value = AirIndex};
        UpdateChunkMeshes(voxelIndex);
        OnVoxelChanged?.Invoke(voxelIndex, AirIndex);
    }
    
    private void UpdateChunkMeshes(int voxelIndex)
    {
        // var voxelXyz = voxelIndex.ToInt3(dims);
        // var chunkXyz = voxelXyz / chunkDims;
        // chunks[chunkXyz].UpdateChunk();
        //
        // if (voxelXyz.x % chunkDims.x == 0 && chunks.ContainsKey(chunkXyz - new int3(1, 0, 0)))
        //     chunks[chunkXyz - new int3(1, 0, 0)].UpdateChunk();
        // else if (voxelXyz.x % chunkDims.x == chunkDims.x - 1 && chunks.ContainsKey(chunkXyz + new int3(1, 0, 0)))
        //     chunks[chunkXyz + new int3(1, 0, 0)].UpdateChunk();
        // else if (voxelXyz.y % chunkDims.y == 0 && chunks.ContainsKey(chunkXyz - new int3(0, 1, 0)))
        //     chunks[chunkXyz - new int3(0, 1, 0)].UpdateChunk();
        // else if (voxelXyz.y % chunkDims.y == chunkDims.y - 1 && chunks.ContainsKey(chunkXyz + new int3(0, 1, 0)))
        //     chunks[chunkXyz + new int3(0, 1, 0)].UpdateChunk();
        // else if (voxelXyz.z % chunkDims.z == 0 && chunks.ContainsKey(chunkXyz - new int3(0, 0, 1)))
        //     chunks[chunkXyz - new int3(0, 0, 1)].UpdateChunk();
        // else if (voxelXyz.z % chunkDims.z == chunkDims.z - 1 && chunks.ContainsKey(chunkXyz + new int3(0, 0, 1)))
        //     chunks[chunkXyz + new int3(0, 0, 1)].UpdateChunk();
    }
    
}