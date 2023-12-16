using Unity.Mathematics;
using UnityEngine;
using Oasis.World;
using UnityEngine.Serialization;

namespace Oasis.Chunks
{

    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class ChunkMono : MonoBehaviour
    {
        public int3 xyz;
        public int3 dims;
        
        private void Start()
        {
            // Get World Entity
            var em = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager;
            var worldEntity = em.CreateEntityQuery(typeof(Oasis.World.World)).GetSingletonEntity();
            var world = em.GetComponentData<Oasis.World.World>(worldEntity);
            
            var voxels = em.GetBuffer<Voxel>(worldEntity).AsNativeArray().Reinterpret<ushort>();
            var mesh = Mesher.Mesher.Compute(dims, xyz * dims, world.Dims, voxels);
            GetComponent<MeshFilter>().mesh = mesh;
        }
        
    }

}

