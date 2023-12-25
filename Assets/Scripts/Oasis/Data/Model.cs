using System;
using Oasis.Common;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine.Serialization;

namespace Oasis.Data
{
    
    public struct Model : IComponentData
    {
        public FixedString64Bytes Name;
        public BlockType Type;
        public TextureType TextureType;
    }
    
    public struct ModelElement : IComponentData
    {
        public float3 From; // 3 Bytes
        public float3 To; // 3 Bytes
        public bool NoShadows; // 1 Byte
        public ModelElementRotation Rotation;
    }

    public struct ModelElementRotation
    {
        public float3 Origin;
        public byte Axis;  // 0 == x, 1 == y, 2 == z
        public float Angle;
    }

    [Serializable]
    public struct ModelFace : IBufferElementData
    {
        public Side Side;   // 1 byte
        public int4 UV;     // 4 bytes
        public Entity Texture; // 2 bytes
        
        // public TextureObject Texture;  // 2 bytes
        // public bool Cullface; // 1 byte
    }
    
    public struct ModelElementReference : IBufferElementData
    {
        public Entity Value;
    }
}
