using System;
using Oasis.ECS.Common;
using Unity.Collections;
using Unity.Entities;

namespace Oasis.ECS.BlockStates
{
    public struct BlockState : IComponentData, IEquatable<BlockState>
    {
        public Entity Block;
        public BlockType Type;
        public TextureType TextureType;
        public FixedList4096Bytes<State> States;   // 4094 bytes; 2 byte overhead
        public Entity Model;


        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public bool Equals(BlockState other)
        {
            return Block.Equals(other.Block); // && States.Equals(other.States);
        }

        public override int GetHashCode()
        {
            // return HashCode.Combine(Block, States);
            return HashCode.Combine(Block);
        }
    }
}