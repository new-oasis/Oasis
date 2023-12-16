// Use ChunkMono until ready for Unity.Entities.Graphics


// using Unity.Entities;
// using Unity.Mathematics;
// using Unity.Rendering;
// using Unity.Transforms;
// using UnityEngine;
// using UnityEngine;
// using UnityEngine.Rendering;
// using World = Unity.Entities.World;
//
// namespace Oasis.Chunks
// {
//
//     public class ChunkAuthoring : MonoBehaviour
//     {
//         public int3 xyz;
//
//         public class ChunkBaker : Baker<ChunkAuthoring>
//         {
//             public override void Bake(ChunkAuthoring authoring)
//             {
//                 var entity = GetEntity(TransformUsageFlags.Renderable);
//                 AddComponent(entity, new Chunk
//                 {
//                     XYZ = authoring.xyz
//                 });
//                 
//                 // Compute mesh
//                 
//                 
//                 
//                 // Render mesh
//                 var em = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager;
//                 var desc = new RenderMeshDescription( shadowCastingMode: ShadowCastingMode.Off, receiveShadows: false);
//                 var renderMeshArray = new RenderMeshArray(new Material[] { }, new Mesh[] { });
//                 RenderMeshUtility.AddComponents(
//                     entity,
//                     em,
//                     desc,
//                     renderMeshArray,
//                     MaterialMeshInfo.FromRenderMeshArrayIndices(0, 0));
//                 em.AddComponentData(entity, new LocalToWorld());
//             }
//         }
//     }
//
// }