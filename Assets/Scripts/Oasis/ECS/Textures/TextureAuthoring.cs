using Oasis.ECS.Common;
using Oasis.Game.Textures;
using Unity.Entities;
using UnityEngine;

namespace Oasis.ECS.Textures
{

    public class TextureAuthoring : MonoBehaviour
    {
        public TextureType Type;
        public Texture2D Tex;
        public int Index;

        public class TextureBaker : Baker<TextureAuthoring>
        {
            public override void Bake(TextureAuthoring authoring)
            {
                Debug.Log("Baking model " + authoring.name);
                
                if (TextureManager.Instance == null) return;
                authoring.Index = TextureManager.Instance.LoadTexture(authoring.Tex, authoring.Type);
                
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new Texture
                {
                    TextureName = authoring.gameObject.name,
                    Type = authoring.Type,
                    Index = (byte)authoring.Index,
                });

            }
        }
    }
    
    


}