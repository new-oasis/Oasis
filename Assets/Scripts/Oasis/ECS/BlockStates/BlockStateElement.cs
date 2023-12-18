using System;
using Unity.Entities;

namespace Oasis.ECS.BlockStates
{
    
    public struct BlockStateElement : IBufferElementData, IEquatable<BlockStateElement>
    {
        public Entity Value;

        public bool Equals(BlockStateElement other)
        {
            return Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            return obj is BlockStateElement other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
    

    // public class BlockStateComparer : IEqualityComparer<BlockStateElement>
    // {
    //     public bool Equals(BlockStateElement x, BlockStateElement y)
    //     {
    //         return x.Value == y.Value;
    //     }
    //
    //     public int GetHashCode(BlockStateElement obj)
    //     {
    //         return obj.Value.GetHashCode();
    //     }
    // }
}