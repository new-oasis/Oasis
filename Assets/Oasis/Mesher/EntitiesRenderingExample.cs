using Oasis.World;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering;
using World = Unity.Entities.World;

public class AddComponentsExample : MonoBehaviour
{
    public Mesh Mesh;
    public Material Material;
    public int EntityCount;

    // Example Burst job that creates many entities
    [GenerateTestsForBurstCompatibility]
    public struct SpawnJob : IJobParallelFor
    {
        public Entity Prototype;
        public int EntityCount;
        public EntityCommandBuffer.ParallelWriter Ecb;

        public void Execute(int index)
        {
            // Clone the Prototype entity to create a new entity.
            var e = Ecb.Instantiate(index, Prototype);
            // Prototype has all correct components up front, can use SetComponent to
            // set values unique to the newly created entity, such as the transform.
            Ecb.SetComponent(index, e, new LocalToWorld {Value = ComputeTransform(index)});
        }

        public float4x4 ComputeTransform(int index)
        {
            return float4x4.Translate(new float3(index, 0, 0));
        }
    }

    void Start()
    {
        var em = World.DefaultGameObjectInjectionWorld.EntityManager;
        var ecb = new EntityCommandBuffer(Allocator.TempJob);
        var desc = new RenderMeshDescription( shadowCastingMode: ShadowCastingMode.Off, receiveShadows: false);
        var renderMeshArray = new RenderMeshArray(new Material[] { Material }, new Mesh[] { Mesh });

        var prototype = em.CreateEntity();
        RenderMeshUtility.AddComponents(
            prototype,
            em,
            desc,
            renderMeshArray,
            MaterialMeshInfo.FromRenderMeshArrayIndices(0, 0));
        em.AddComponentData(prototype, new LocalToWorld());

        // Spawn entities in Burst job with prototype entity. Fastest runtime entity creation.
        var spawnJob = new SpawnJob
        {
            Prototype = prototype,
            Ecb = ecb.AsParallelWriter(),
            EntityCount = EntityCount,
        };
        var spawnHandle = spawnJob.Schedule(EntityCount, 128);
        spawnHandle.Complete();

        ecb.Playback(em);
        ecb.Dispose();
        em.DestroyEntity(prototype);
    }
}