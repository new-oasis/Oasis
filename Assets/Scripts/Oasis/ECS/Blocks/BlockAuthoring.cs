using System.Collections.Generic;
using Oasis.ECS.Blocks;
using Oasis.ECS.BlockStates;
using Oasis.ECS.Common;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Oasis.ECS.Blocks
{
    public class BlockAuthoring : MonoBehaviour
    {
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
                    Name = new FixedString64Bytes(authoring.gameObject.name),
                    Type = authoring.Type,
                    TextureType = authoring.TextureType
                });
                
                
                var paletteItems = AddBuffer<PaletteItem>();
                foreach (var blockState in authoring.BlockStates)
                {
                    var paletteItem = new PaletteItem { Value = GetEntity(blockState) };
                    paletteItems.Add(paletteItem);
                }
            }
        }
    }
}

public struct Jim : IBufferElementData
{
    public int Value;
}

