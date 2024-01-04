using System;
using Oasis.Common;
using Unity.Entities;
using Unity.Mathematics;

namespace Oasis.Data
{
    public struct World : IComponentData
    {
        public WorldType WorldType;
        public int3 Dims;
    }
    
    public struct WorldBlockState : IBufferElementData, IEquatable<WorldBlockState>
    {
        public Entity Block;
        public int VariantIndex;

        public bool Equals(WorldBlockState other)
        {
            return Block.Equals(other.Block) && VariantIndex == other.VariantIndex;
        }

        public override bool Equals(object obj)
        {
            return obj is WorldBlockState other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Block, VariantIndex);
        }
    }
}