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
    
    public struct WorldBlockVariant : IBufferElementData
    {
        public Entity Block;
        public int VariantIndex;
    }
}