using System;
using Oasis.Common;
using Unity.Collections;
using Unity.Entities;

namespace Oasis.Data
{
    public struct Block : IComponentData, IEquatable<Block>
    {
        public BlockType BlockType;
        public TextureType TextureType;
        public FixedString32Bytes BlockName;

        public bool Equals(Block other)
        {
            return BlockType == other.BlockType && TextureType == other.TextureType && BlockName.Equals(other.BlockName);
        }

        public override bool Equals(object obj)
        {
            return obj is Block other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int) BlockType, (int) TextureType, BlockName);
        }
    }
    
    public struct BlockState : IBufferElementData
    {
        public FixedList512Bytes<State> States;
        public Entity Model;
    }
    
    [Serializable]
    public struct State
    {
        public FixedString32Bytes Key;
        public FixedString32Bytes Value;
    }
    
}