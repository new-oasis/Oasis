using System.Collections.Generic;
using Oasis.BlockStates;
using UnityEngine;
using Unity.Entities;

public class ToolbarData : MonoBehaviour
{
    public int selectedItem;
    public List<GameObject> blockStates;
    
    private class ToolbarBaker : Baker<ToolbarData>
    {
        public override void Bake(ToolbarData authoring)
        {
            AddComponent<Toolbar>();
            
            var toolbarBlockStates = AddBuffer<BlockStateReference>();
            foreach (var blockState in authoring.blockStates)
               toolbarBlockStates.Add(new BlockStateReference{Value = GetEntity(blockState, TransformUsageFlags.None)});
        }
    }
}


public struct Toolbar : IComponentData
{
    public int selectedItem;
}