using Oasis.Common;
using Unity.Collections;
using Unity.Entities;

namespace Oasis.BlockStates
{
    public struct BlockState : IComponentData
    {
        public Entity Block;
        public BlockType Type;
        public TextureType TextureType;
        public FixedList4096Bytes<State> States;   // 4094 bytes; 2 byte overhead
        public Entity Model;
    }
}