using Oasis.Data;
using Unity.Entities;
using UnityEngine;

namespace Oasis.Authoring
{

    public class TextureAuthoring : MonoBehaviour
    {
        public Assets.Texture Texture;
        public int Index;

        private int _previousTextureInstanceID;

        public class TextureBaker : Baker<TextureAuthoring>
        {
            public override void Bake(TextureAuthoring authoring)
            {
                DependsOn(authoring.Texture);

                var entity = GetEntity(TransformUsageFlags.None);
                
                // Texture
                AddComponent(entity, new Oasis.Data.Texture
                {
                    TextureName = authoring.Texture.name,
                    Type = authoring.Texture.Type,
                    Index = (byte)authoring.Index  // Assume TextureArrayAuthoring is first in hierarchy
                });
            }
        }

    }
}
