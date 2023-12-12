// using System.Collections.Generic;
// using Oasis.Common;
// using Oasis.ModelFaces;
// using Unity.Collections;
// using Unity.Entities;
// using Unity.Mathematics;
// using UnityEngine;
// using Texture = Oasis.Textures.Texture;
//
// namespace Oasis.Models
// {
//
//     public class ModelElementAuthoring : MonoBehaviour
//     {
//         public float3 From; // 3 Bytes
//         public float3 To; // 3 Bytes
//         public bool NoShadows; // 1 Byte
//         public List<GameObject> Faces;  // ~8 Bytes
//
//         public class ModelElementBaker : Baker<ModelElementAuthoring>
//         {
//             public override void Bake(ModelElementAuthoring authoring)
//             {
//                 // ModelElement component
//                 var entity = GetEntity(TransformUsageFlags.None);
//                 AddComponent(entity, new ModelElement
//                 {
//                     From = authoring.From,
//                     To = authoring.To,
//                     NoShadows = authoring.NoShadows
//                 });
//                 
//                 // Buffer of ModelFaceEntities
//                 var modelFaceEntities = AddBuffer<ModelFaceEntity>(entity);
//                 foreach (var face in authoring.Faces)
//                     modelFaceEntities.Add(new ModelFaceEntity{Value = GetEntity(face)});
//                 
//             }
//         }
//     }
//
// }