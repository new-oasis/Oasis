using Oasis.ECS.Common;
using Unity.Collections;
using Unity.Entities;

namespace Oasis.ECS.Textures
{
    public struct Texture : IComponentData
    {
        public FixedString64Bytes TextureName; // 64 bytes
        public TextureType Type;
        public byte Index;
        public byte Metallic; // 1 byte
        public byte Smoothness; // 1 byte
        
    }
}