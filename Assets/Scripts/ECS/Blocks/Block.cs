using System;
using Oasis.Common;
using Unity.Collections;
using Unity.Entities;

namespace Oasis.Blocks
{
    
    public struct Block : IComponentData, IEquatable<Block>
    {
        public FixedString64Bytes Name;
        public BlockType Type;
        public TextureType TextureType;
        
        public override bool Equals(object obj)
        {
            if (obj is Block block)
            {
                return Name.Equals(block.Name) && Type.Equals(block.Type) && TextureType.Equals(block.TextureType);
            }

            return false;
        }

        public bool Equals(Block other)
        {
            return Name.Equals(other.Name) && Type == other.Type && TextureType == other.TextureType;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, (int) Type, (int) TextureType);
        }
    }

}