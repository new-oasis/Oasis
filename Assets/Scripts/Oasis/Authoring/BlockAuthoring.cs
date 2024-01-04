using Oasis.Data;
using System.Linq;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using State = Oasis.Data.State;
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
                
                // VARIANTS
                var variants = AddBuffer<BlockState>();
                foreach (var blockVariant in authoring.Block.Variants)
                {
                    var variant = new BlockState
                    {
                        States = new FixedList512Bytes<State>(),
                    };
                    foreach (var state in blockVariant.States)
                        variant.States.Add(new State { Key = state.Key, Value = state.Value, });
                    
                    // Model
                    var modelAuthoringComponent = GetModelAuthoringComponent(authoring, blockVariant);
                    variant.Model = GetEntity(modelAuthoringComponent, TransformUsageFlags.None);
                    variants.Add(variant);
                } 
                
                
                // Apply to entity
                var entity = GetEntity(authoring);
                AddComponent(entity, new Block
                {
                    BlockName = authoring.name,
                    BlockType = authoring.Block.BlockType,
                    TextureType = authoring.Block.TextureType,
                });
                World.All[0].EntityManager.SetName(entity, authoring.Block.name);
            }

            private static ModelAuthoring GetModelAuthoringComponent(BlockAuthoring authoring, Assets.Variant blockVariant)
            {
                var modelAuthoringComponents = FindObjectsOfType<ModelAuthoring>();
                var modelAuthoringComponent = modelAuthoringComponents
                    .FirstOrDefault(component => component.Model == blockVariant.Model);
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
