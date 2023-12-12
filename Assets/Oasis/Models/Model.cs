using System;
using Oasis.Common;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace Oasis.Models
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




/*
 Problem: Entity references in FixedLists are not resolved
 
Option 1.  
Model GO/Entity with buffer of ModelElementEntity
ModelElement GO/Entity with buffer of ModelFaceEntity
ModelFace GO/Entity with TextureEntity

Option 2.
Custom baking systems to resolve entity refs in FixedList
*/


