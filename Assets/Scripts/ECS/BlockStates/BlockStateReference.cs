using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;

namespace Oasis.BlockStates
{
    
    public struct BlockStateReference : IBufferElementData, IEquatable<BlockStateReference>
    {
        public Entity Value;

        public bool Equals(BlockStateReference other)
        {
            return Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            return obj is BlockStateReference other && Equals(other);
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