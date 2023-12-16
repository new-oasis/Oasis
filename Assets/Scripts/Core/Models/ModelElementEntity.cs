using Oasis.Common;
using Unity.Entities;
using Unity.Mathematics;

namespace Oasis.Models
{

    [System.Serializable]
    public struct ModelElementEntity : IBufferElementData
    {
        public Entity Value;
    }
}