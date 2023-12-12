using System;
using Unity.Entities;
using Unity.Mathematics;

namespace Oasis.World
{
    [Serializable]
    public struct World : IComponentData
    {
        public WorldType WorldType;
        public int3 Dims;
    }
}