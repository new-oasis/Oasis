using System.Collections;
using System.Collections.Generic;
using Oasis.Blocks;
using Oasis.BlockStates;
using Oasis.World;
using Unity.Collections;
using UnityEngine;

public class GetBlockStateEntity : MonoBehaviour
{

    public BlockStateAuthoring BlockStateAuthoring;
    
    void Start()
    {
        var em = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager;
        // var world = em.CreateEntityQuery(typeof(World)).GetSingleton<World>();


        var blockAuthoring = BlockStateAuthoring.block.GetComponent<BlockAuthoring>();
        
        // Find Block
        var targetBlock = new Block
        {
            Name = BlockStateAuthoring.block.name,
            Type = blockAuthoring.Type,
            TextureType = blockAuthoring.TextureType
        };
        var blocks = em.CreateEntityQuery(typeof(Block)).ToComponentDataArray<Block>(Allocator.TempJob);
        var blockIndex = blocks.IndexOf(targetBlock);
        var blockEntities = em.CreateEntityQuery(typeof(Block)).ToEntityArray(Allocator.TempJob);
        var blockEntity = blockEntities[blockIndex];
        Debug.Log("blockIndex = " + blockIndex);
        

        // Find BlockState
        var states = new FixedList4096Bytes<State>();
        foreach (var state in BlockStateAuthoring.states)
            states.Add(state);
        var targetBlockState = new BlockState
        {
            Block = blockEntity,
            States = states,
        };
        var blockStates = em.CreateEntityQuery(typeof(BlockState)).ToComponentDataArray<BlockState>(Allocator.TempJob);
        var blockStateEntities = em.CreateEntityQuery(typeof(BlockState)).ToEntityArray(Allocator.TempJob);
        var blockStateIndex = blockStates.IndexOf(targetBlockState);
        Debug.Log("blockStateIndex = " + blockStateIndex);
    }

}
