using Oasis.Data;
using Unity.Collections;
using UnityEngine;

namespace Oasis.Mono
{

    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class BlockState : MonoBehaviour
    {
        public Assets.Block Block;
        public int BlockStateIndex;
        
        private void Start()
        {
            // Block Index
            var em = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager;
            var blockQuery = em.CreateEntityQuery(typeof(Block));
            var blocks = blockQuery.ToComponentDataArray<Block>(Allocator.TempJob);
            var blockIndex = blocks.IndexOf(new Block {BlockName = Block.name, BlockType = Block.BlockType, TextureType = Block.TextureType});
            
            // Block Entity
            var blockEntities = blockQuery.ToEntityArray(Unity.Collections.Allocator.TempJob);
            var blockEntity = blockEntities[blockIndex];

            // BlockStates
            var blockStates = em.GetBuffer<Data.BlockState>(blockEntity);
            var blockState = blockStates[BlockStateIndex];
            
            // Variant Model Mesh
            var mesh = em.GetSharedComponentManaged<ModelMesh>(blockState.Model).Value;
            GetComponent<MeshFilter>().mesh = mesh;
            
            // Update mesh collider
            if (gameObject.TryGetComponent<MeshCollider>(out var meshCollider))
                meshCollider.sharedMesh = mesh;
        }
    }

}

