using Oasis.ECS.Models;
using Oasis.ECS.World;
using Oasis.Game.Mesher;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Oasis.ECS.BlockStates
{

    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class BlockStateMono : MonoBehaviour
    {
        public ushort BlockStateIndex;
        
        private void Start()
        {
            // var voxels = new NativeArray<ushort>(1, Allocator.Temp);
            // voxels[0] = BlockStateIndex;
            // var mesh = Mesher.Compute(new int3(1), new int3(0), new int3(1), voxels);
            
            
            // Get blockState.model.mesh
            var em = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager;
            var worldEntity = em.CreateEntityQuery(typeof(Oasis.ECS.World.World)).GetSingletonEntity();
            var worldBlockStates = em.GetBuffer<BlockStateElement>(worldEntity);
            var blockStateEntity = worldBlockStates[BlockStateIndex].Value;
            var blockState = em.GetComponentData<BlockState>(blockStateEntity);
            
            GetComponent<MeshFilter>().mesh = em.GetSharedComponentManaged<ModelMesh>(blockState.Model).Value;
        }
    }

}

