using System;
using System.Collections.Generic;
using Oasis.Common;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Oasis.Models
{
    public class ModelAuthoring : MonoBehaviour
    {
        public string ModelName;
        public BlockType Type;
        public TextureType TextureType;
        public List<ModelElementAuthoring> ModelElements;
        
        public class ModelBaker : Baker<ModelAuthoring>
        {
            public override void Bake(ModelAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new Model
                {
                    Name = authoring.ModelName,
                    Type = authoring.Type,
                    TextureType = authoring.TextureType,
                });
                
                // ModelElements
                var modelElementEntities = AddBuffer<ModelElementEntity>(entity);
                foreach (var authoringModelElement in authoring.ModelElements)
                {
                    var modelElementEntity = CreateAdditionalEntity(TransformUsageFlags.None);
                    AddComponent(modelElementEntity, new ModelElement
                    {
                        From = authoringModelElement.From,
                        To = authoringModelElement.To,
                        NoShadows = authoringModelElement.NoShadows
                    });
                    modelElementEntities.Add(new ModelElementEntity { Value = modelElementEntity });
                    
                    var modelFaceEntities = AddBuffer<ModelFace>(modelElementEntity);
                    foreach (var modelFaceAuthoring in authoringModelElement.Faces)
                    {
                        modelFaceEntities.Add(new ModelFace
                        {
                            Side = modelFaceAuthoring.Side,
                            UV = modelFaceAuthoring.UV,
                            Texture = GetEntity(modelFaceAuthoring.Texture)
                        });
                    }
                    
                }
            }
        }
    }

    
    [Serializable]
    public class ModelElementAuthoring
    {
        public float3 From; // 3 Bytes
        public float3 To; // 3 Bytes
        public bool NoShadows; // 1 Byte
        public List<ModelFaceAuthoring> Faces;  // ~8 Bytes
    }
    
    [Serializable]
    public class ModelFaceAuthoring
    {
        public Side Side;   // 1 byte
        public int4 UV;     // 4 bytes
        public GameObject Texture; // 2 bytes
    }
    
    
}