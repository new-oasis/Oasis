using System.Collections.Generic;
using Oasis.Common;
using Unity.Entities;
using UnityEngine;

namespace Oasis.Textures
{

    public class TextureAuthoring : MonoBehaviour
    {
        public TextureType Type;
        public Texture2D Tex;

        public class TextureBaker : Baker<TextureAuthoring>
        {
            public override void Bake(TextureAuthoring authoring)
            {
                // var index = (byte)TextureManager.Instance.LoadTexture(authoring.Tex, authoring.Type);
                if (TextureManager.Instance == null) return;
                var index = TextureManager.Instance.LoadTexture(authoring.Tex, authoring.Type);
                
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new Texture
                {
                    TextureName = authoring.gameObject.name,
                    Type = authoring.Type,
                    Index = (byte)index,
                });

            }
        }
    }
    
    


}