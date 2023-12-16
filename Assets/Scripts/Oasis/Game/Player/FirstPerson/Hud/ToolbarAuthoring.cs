using System.Collections.Generic;
using Oasis.ECS.BlockStates;
using UnityEngine;
using Unity.Entities;

public class ToolbarAuthoring : MonoBehaviour
{
    public int selectedItem;
    public List<GameObject> blockStates;
    
    private class ToolbarBaker : Baker<ToolbarAuthoring>
    {
        public override void Bake(ToolbarAuthoring authoring)
        {
            AddComponent<ToolbarData>();
            
            var toolbarBlockStates = AddBuffer<PaletteItem>();
            foreach (var blockState in authoring.blockStates)
               toolbarBlockStates.Add(new PaletteItem{Value = GetEntity(blockState, TransformUsageFlags.None)});
        }
    }
}


public struct ToolbarData : IComponentData
{
    public int selectedItem;
}