using System.Collections.Generic;
using Oasis.Common;
using Oasis.Models;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Texture = Oasis.Textures.Texture;

namespace Oasis.ModelFaces
{

    public class ModelFaceAuthoring : MonoBehaviour
    {
        public Side Side;   // 1 byte
        public int4 UV;     // 4 bytes
        public GameObject Texture; // 2 bytes

        public class ModelFaceBaker : Baker<ModelFaceAuthoring>
        {
            public override void Bake(ModelFaceAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                
                
                // ModelFace component
                var modelFace = new ModelFace
                {
                    Side = authoring.Side,
                    UV = authoring.UV,
                    Texture = GetEntity(authoring.Texture)
                };
                
            }
        }
    }

}