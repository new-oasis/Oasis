using System.Collections.Generic;
using Oasis.Common;
using Unity.Entities;
using UnityEngine;

namespace Oasis.Textures
{

    public class TextureAuthoring : MonoBehaviour
    {
        public string TextureName;
        public TextureType Type;
        public Texture2D Tex;

        public class TextureBaker : Baker<TextureAuthoring>
        {
            public override void Bake(TextureAuthoring authoring)
            {
                // var index = (byte)TextureManager.Instance.LoadTexture(authoring.Tex, authoring.Type);
                var index = TextureManager.Instance.LoadTexture(authoring.Tex, authoring.Type);
                
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new Texture
                {
                    TextureName = authoring.TextureName,
                    Type = authoring.Type,
                    Index = (byte)index,
                });

            }
        }
    }
    
    


}