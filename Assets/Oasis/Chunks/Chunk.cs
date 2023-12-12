using Unity.Entities;
using Unity.Mathematics;

namespace Oasis.Chunks
{
    
    public struct Chunk : IComponentData
    {
        public int3 XYZ;
    }

}