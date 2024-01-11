using System.Collections.Generic;
using System.Linq;
using Oasis.Data;
using Unity.Entities;
using UnityEngine;

namespace Oasis.FirstPerson
{
    public class ToolbarAuthoring : MonoBehaviour
    {
        public List<Oasis.Assets.Block> Blocks;
    
        private class ToolbarBaker : Baker<ToolbarAuthoring>
        {
            public override void Bake(ToolbarAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent<ToolbarData>(entity);
            
                var toolbarBlockStates = AddBuffer<BlockStateRef>(entity);
                var blockAuthoringComponents = FindObjectsOfType<Authoring.BlockAuthoring>();
            
                foreach (var block in authoring.Blocks)
                {
                    var blockAuthoringComponent = blockAuthoringComponents
                        .FirstOrDefault(component => component.Block == block);
                    if (blockAuthoringComponent == null)
                        Debug.LogError("No block authoring component found for toolbar block");
                
                    toolbarBlockStates.Add(new BlockStateRef
                    {
                        Block = GetEntity(blockAuthoringComponent, TransformUsageFlags.None)
                    });
                }
            }
        }
    }


    public struct ToolbarData : IComponentData
    {
        public int SelectedItem;
    }
}