using System;
using Oasis.Common;
using Unity.Collections;
using Unity.Entities;

namespace Oasis.Data
{
    public struct Texture : IComponentData, IEquatable<Texture>
    {
        public FixedString64Bytes TextureName; // 64 bytes
        public TextureType Type;
        public byte Index;
        public byte Metallic; // 1 byte
        public byte Smoothness; // 1 byte

        // Equals considers only the texture name
        public bool Equals(Texture other)
        {
            return TextureName.Equals(other.TextureName);
        }

        public override bool Equals(object obj)
        {
            return obj is Texture other && Equals(other);
        }

        public override int GetHashCode()
        {
            return TextureName.GetHashCode();
        }
    }
}