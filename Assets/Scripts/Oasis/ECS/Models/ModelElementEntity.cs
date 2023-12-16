using Unity.Entities;

namespace Oasis.ECS.Models
{

    [System.Serializable]
    public struct ModelElementEntity : IBufferElementData
    {
        public Entity Value;
    }
}