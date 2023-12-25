using System.Collections.Generic;
using Oasis.Common;
using Oasis.Data;
using Unity.Entities;
using UnityEditor;
using UnityEngine;

namespace Oasis.Authoring
{
    public class TextureArrayAuthoring : MonoBehaviour
    {
        public int NumElements = 64;
        public TextureType TextureType = TextureType.Opaque;
        public TextureFormat Format = TextureFormat.ARGB32;
        public List<Assets.Texture> Textures;
        public Texture2DArray Array;
        
        private class TextureArrayAuthoringBaker : Baker<TextureArrayAuthoring>
        {
            public override void Bake(TextureArrayAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                
                // Texture2DArray
                var path = $"Assets/Resources/Textures/Texture2DArray_{authoring.Format.ToString()}.asset";
                if (AssetDatabase.LoadAssetAtPath(path, typeof(Texture2DArray)) as Texture2DArray)
                    authoring.Array = AssetDatabase.LoadAssetAtPath<Texture2DArray>(path);
                else
                {
                    authoring.Array = new Texture2DArray(16, 16, authoring.NumElements, authoring.Format, false, false);
                    AssetDatabase.CreateAsset(authoring.Array, path);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
                 
                
                // TextureArray ICD
                AddComponent(entity, new TextureArray
                {
                    TextureType = authoring.TextureType,
                    Format = authoring.Format
                });
                
                // Load Textures
                AddBuffer<TextureArrayElement>(entity);
                authoring.Textures.Clear();
                var textureAuthoringComponents = FindObjectsOfType<TextureAuthoring>();
                for (var i = 0; i < textureAuthoringComponents.Length; i++)
                {
                    // Update authoring
                    textureAuthoringComponents[i].Index = i; // Set index in TextureAuthoring
                    textureAuthoringComponents[i].Texture.Index = i; // Set index in Texture Asset
                    authoring.Textures.Add(textureAuthoringComponents[i].Texture);
                    
                    // Copy texture into array
                    Graphics.CopyTexture(textureAuthoringComponents[i].Texture.Texture2D, 0, 0, authoring.Array, i, 0);
                    
                    // TextureArrayElement
                    var textureArrayElement = new TextureArrayElement
                    {
                        Texture = GetEntity(textureAuthoringComponents[i].gameObject, TransformUsageFlags.None)
                    };
                    AppendToBuffer(entity, textureArrayElement);
                }
                
                
               
                
                
                
                
            }
        }
    }
}
                /*
                    // Create texture entity and data
                    texture.Index = i;
                    var textureEntity = CreateAdditionalEntity(TransformUsageFlags.None, false, texture.name);
                    AddComponent(textureEntity, new Oasis.Data.Texture
                    {
                        TextureName = texture.name,
                        Type = texture.Type,
                        Index = (byte)i
                    });
                    textures.Add( new TextureArrayElement
                    {
                        Texture = textureEntity
                    });
                }
                
                */
                
                