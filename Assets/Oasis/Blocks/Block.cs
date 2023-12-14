using Oasis.Common;
using Unity.Collections;
using Unity.Entities;

namespace Oasis.Blocks
{
    
    public struct Block : IComponentData
    {
        public FixedString64Bytes Name;
        public BlockType Type;
        public TextureType TextureType;
    }

}