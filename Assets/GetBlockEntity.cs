using System.Collections;
using System.Collections.Generic;
using Oasis.Blocks;
using Oasis.BlockStates;
using Oasis.World;
using Unity.Collections;
using UnityEngine;

public class GetBlockEntity : MonoBehaviour
{

    public BlockStateAuthoring BlockStateAuthoring;
    
    public BlockAuthoring Block;
    public List<State> States;
    
    void Start()
    {
        var em = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager;
        // var world = em.CreateEntityQuery(typeof(World)).GetSingleton<World>();

        // Find Block
        var targetBlock = new Block
        {
            Name = Block.name,
            Type = Block.Type,
            TextureType = Block.TextureType
        };
        var blocks = em.CreateEntityQuery(typeof(Block)).ToComponentDataArray<Block>(Allocator.TempJob);
        var block = blocks.IndexOf(targetBlock);
        var blockEntities = em.CreateEntityQuery(typeof(Block)).ToEntityArray(Allocator.TempJob);
        var blockEntity = blockEntities[block];
        

        // Find BlockState
        var targetBlockState = new BlockState
        {
            Block = blockEntity,
            States = new FixedList4096Bytes<State>()
        };
        var blockStates = em.CreateEntityQuery(typeof(BlockState)).ToComponentDataArray<BlockState>(Allocator.TempJob);
        var blockStateEntities = em.CreateEntityQuery(typeof(BlockState)).ToEntityArray(Allocator.TempJob);
        var blockStateIndex = blockStates.IndexOf(targetBlockState);
        Debug.Log("blockStateIndex = " + blockStateIndex);
        

        
        
    }

}
