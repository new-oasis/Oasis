using System;
using Oasis.ECS.Common;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace Oasis.ECS.Models
{
    
    public struct Model : IComponentData
    {
        public FixedString64Bytes Name;
        public BlockType Type;
        public TextureType TextureType;
    }
    
    [Serializable]
    public struct ModelElement : IComponentData
    {
        public float3 From; // 3 Bytes
        public float3 To; // 3 Bytes
        public bool NoShadows; // 1 Byte
        public ModelElementRotation Rotation;
    }

    [Serializable]
    public struct ModelElementRotation
    {
        public float3 Origin;
        public byte Axis;  // 0 == x, 1 == y, 2 == z
        public float Angle;
    }

    public struct ModelFace : IBufferElementData
    {
        public Side Side;   // 1 byte
        public int4 UV;     // 4 bytes
        public Entity Texture; // 2 bytes
        
        // public TextureObject Texture;  // 2 bytes
        // public bool Cullface; // 1 byte
    }
    
}
