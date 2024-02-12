using System;
using Oasis.Common;
using Unity.Entities;
using UnityEngine;

namespace Oasis.Data
{
    public struct TextureArray : IComponentData, IEquatable<TextureArray>
    {
        public TextureType TextureType;
        public TextureFormat Format;

        // Just TextureType
        public bool Equals(TextureArray other)
        {
            return TextureType == other.TextureType;
        }

        public override bool Equals(object obj)
        {
            return obj is TextureArray other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (int) TextureType;
        }
    }
    
    public struct TextureArrayElement : IBufferElementData
    {
        public Entity Texture;
    }
}