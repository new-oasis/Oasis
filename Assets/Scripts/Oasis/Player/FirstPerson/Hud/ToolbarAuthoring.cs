// using System.Collections.Generic;
// using Oasis.Data;
// using UnityEngine;
// using Unity.Entities;
//
// public class ToolbarAuthoring : MonoBehaviour
// {
//     public int selectedItem;
//     public List<GameObject> blockStates;
//     
//     private class ToolbarBaker : Baker<ToolbarAuthoring>
//     {
//         public override void Bake(ToolbarAuthoring authoring)
//         {
//             AddComponent<ToolbarData>();
//             
//             var toolbarBlockStates = AddBuffer<BlockStateElement>();
//             foreach (var blockState in authoring.blockStates)
//                toolbarBlockStates.Add(new BlockStateElement{Value = GetEntity(blockState, TransformUsageFlags.None)});
//         }
//     }
// }
//
//
// public struct ToolbarData : IComponentData
// {
//     public int selectedItem;
// }