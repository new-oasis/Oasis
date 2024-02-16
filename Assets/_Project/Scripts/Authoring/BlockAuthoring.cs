using Oasis.Data;
using System.Linq;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using World = Unity.Entities.World;

namespace Oasis.Authoring
{
    public class BlockAuthoring : MonoBehaviour
    {
        public Assets.Block Block; 
        
        private class BlockAuthoringBaker : Baker<BlockAuthoring>
        {
            public override void Bake(BlockAuthoring authoring)
            {
                if (authoring.Block == null)
                {
                    Debug.LogError($"{authoring.name} has no block asset assigned");
                    return;
                }
                
                var entity = GetEntity(authoring, TransformUsageFlags.None);
                
                // BLOCK STATES
                var blockStates = AddBuffer<Data.BlockState>(entity);
                foreach (var authoringBlockState in authoring.Block.BlockStates)
                {
                    var blockState = new BlockState
                    {
                        States = new FixedList512Bytes<State>(),
                        Block = entity,
                    };
                    foreach (var state in authoringBlockState.States)
                        blockState.States.Add(new State { Key = state.Key, Value = state.Value, });
                    
                    // Model
                    var modelAuthoringComponent = GetModelAuthoringComponent(authoring, authoringBlockState);
                    blockState.Model = GetEntity(modelAuthoringComponent, TransformUsageFlags.None);
                    blockState.Rotation = new BlockStateRotation{
                        Axis = authoringBlockState.Rotation.Axis,
                        Angle = authoringBlockState.Rotation.Angle,
                    };
                    Debug.Log($"Block {authoring.name} has rotation {blockState.Rotation.Axis} {blockState.Rotation.Angle}");
                    blockStates.Add(blockState);
                } 
                
                
                // Apply to entity
                AddComponent(entity, new Block
                {
                    BlockName = authoring.name,
                    BlockType = authoring.Block.BlockType,
                    TextureType = authoring.Block.TextureType,
                });
                Unity.Entities.World.All[0].EntityManager.SetName(entity, authoring.Block.name);
            }

            private static ModelAuthoring GetModelAuthoringComponent(BlockAuthoring authoring, Assets.BlockState blockState)
            {
                var modelAuthoringComponents = FindObjectsOfType<ModelAuthoring>();
                var modelAuthoringComponent = modelAuthoringComponents
                    .FirstOrDefault(component => component.Model == blockState.Model);
                if (modelAuthoringComponent == null)
                {
                    Debug.LogError("No model authoring component found for variant in block " + authoring.name);
                    return modelAuthoringComponent;
                }

                return modelAuthoringComponent;
            }
        }
    }
}
