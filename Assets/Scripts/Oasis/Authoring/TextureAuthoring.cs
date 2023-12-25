using System.Linq;
using Oasis.Common;
using Oasis.Data;
using Unity.Collections;
using Unity.Entities;
using UnityEditor;
using UnityEngine;
using Texture = Oasis.Assets.Texture;
using World = Unity.Entities.World;

namespace Oasis.Authoring
{

    public class TextureAuthoring : MonoBehaviour
    {
        public Texture Texture;
        public int Index;

        public class TextureBaker : Baker<TextureAuthoring>
        {
            public override void Bake(TextureAuthoring authoring)
            {
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
