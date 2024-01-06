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
    
    public struct BlockState : IBufferElementData, IEquatable<BlockState>
    {
        public Entity Block;
        public FixedList512Bytes<State> States;
        public Entity Model;
        public BlockStateRotation Rotation;

        public bool Equals(BlockState other)
        {
            return Block.Equals(other.Block) && States.Equals(other.States) && Model.Equals(other.Model);
        }

        public override bool Equals(object obj)
        {
            return obj is BlockState other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Block, States, Model);
        }
    }
    
    [Serializable]
    public struct State
    {
        public FixedString32Bytes Key;
        public FixedString32Bytes Value;
    }
    
    [Serializable]
    public struct BlockStateRotation
    {
        public byte Axis;  // 0 == x, 1 == y, 2 == z
        public float Angle;
    }
    
    public struct BlockStateRef : IBufferElementData, IEquatable<BlockStateRef>
    {
         public Entity Block;
         public int BlockStatesIndex;
 
         public bool Equals(BlockStateRef other)
         {
             return Block.Equals(other.Block) && BlockStatesIndex == other.BlockStatesIndex;
         }
 
         public override bool Equals(object obj)
         {
             return obj is BlockStateRef other && Equals(other);
         }
 
         public override int GetHashCode()
         {
             return HashCode.Combine(Block, BlockStatesIndex);
         }
     }
}    
   
   
