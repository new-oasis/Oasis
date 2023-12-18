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
            // Get blockState
            var em = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager;
            var worldEntity = em.CreateEntityQuery(typeof(Oasis.ECS.World.World)).GetSingletonEntity();
            var worldBlockStates = em.GetBuffer<BlockStateElement>(worldEntity);
            var blockStateEntity = worldBlockStates[BlockStateIndex].Value;
            var blockState = em.GetComponentData<BlockState>(blockStateEntity);
            
            // Update mesh with blockState model
            var mesh = em.GetSharedComponentManaged<ModelMesh>(blockState.Model).Value;
            GetComponent<MeshFilter>().mesh = mesh;
            
            // Update mesh collider
            if (gameObject.TryGetComponent<MeshCollider>(out var meshCollider))
                meshCollider.sharedMesh = mesh;
        }
    }

}

