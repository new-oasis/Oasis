using Oasis.Blocks;
using Unity.Collections;
using Oasis.Models;
using Unity.Entities;
using UnityEngine;

namespace Oasis.BlockStates
{

    public class BlockStateAuthoring : MonoBehaviour
    {
        public GameObject block;
        public GameObject model;
        public State[] states;

        public class BlockStateBaker : Baker<BlockStateAuthoring>
        {
            public override void Bake(BlockStateAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                
                var blockState = new BlockState
                {
                    Block = GetEntity(authoring.block, TransformUsageFlags.None), 
                    States = new FixedList4096Bytes<State>(),
                    Model = GetEntity(authoring.model, TransformUsageFlags.None),
                    
                    // denormalized
                    Type = authoring.block.GetComponent<BlockAuthoring>().Type, 
                    TextureType = authoring.block.GetComponent<BlockAuthoring>().TextureType,
                };
                foreach (var state in authoring.states)
                    blockState.States.Add(state);
                AddComponent(entity, blockState);
            }
        }
    }

}

