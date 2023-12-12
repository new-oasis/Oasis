using System.Collections.Generic;
using Oasis.BlockStates;
using Oasis.Common;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Oasis.Blocks
{
    public class BlockAuthoring : MonoBehaviour
    {
        public string BlockName;
        public BlockType Type;
        public TextureType TextureType;
        public List<GameObject> BlockStates;

        public class BlockBaker : Baker<BlockAuthoring>
        {
            public override void Bake(BlockAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new Block
                {
                    Name = new FixedString64Bytes(authoring.BlockName),
                    Type = authoring.Type,
                    TextureType = authoring.TextureType
                });
                
                
                var blockStateReferences = AddBuffer<BlockStateReference>();
                foreach (var blockState in authoring.BlockStates)
                {
                    var blockStateReference = new BlockStateReference { Value = GetEntity(blockState) };
                    blockStateReferences.Add(blockStateReference);
                }
            }
        }
    }
}

public struct Jim : IBufferElementData
{
    public int Value;
}

