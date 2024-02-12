using System.Collections.Generic;
using Oasis.Common;
using Oasis.Data;
using Unity.Collections;
using Unity.Entities;
using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Oasis.Authoring
{
    public class TextureArrayAuthoring : MonoBehaviour
    {
        public int NumElements = 64;
        public TextureType TextureType;
        public Texture2DArray Array;
        public Material Lit;
        public Material Unlit;
        [LabelText("Textures (Automatically Populated)")] public List<Assets.Texture> Textures;

        private class TextureArrayAuthoringBaker : Baker<TextureArrayAuthoring>
        {
            public override void Bake(TextureArrayAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);

                // Compute texture format
                var buildTarget = EditorUserBuildSettings.activeBuildTarget;
                TextureFormat textureFormat = TextureFormat.DXT5;
                if (buildTarget == BuildTarget.StandaloneOSX) 
                    if (authoring.TextureType == TextureType.Transparent || authoring.TextureType == TextureType.Cutout)
                        textureFormat = TextureFormat.DXT5;
                    else 
                        textureFormat = TextureFormat.DXT1;
                else if (buildTarget == BuildTarget.iOS) 
                    textureFormat = TextureFormat.ASTC_6x6;
                // Debug.Log($"Build target: {buildTarget}.   Texture format: {textureFormat}");


                // Create Texture2DArray
                var path = $"Assets/Resources/Textures/Texture2DArray_{authoring.TextureType}_{textureFormat}.asset";
                if (AssetDatabase.LoadAssetAtPath(path, typeof(Texture2DArray)) as Texture2DArray)
                    authoring.Array = AssetDatabase.LoadAssetAtPath<Texture2DArray>(path);
                else
                {
                    authoring.Array = new Texture2DArray(16, 16, authoring.NumElements, textureFormat, false, false);
                    AssetDatabase.CreateAsset(authoring.Array, path);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }

                // Update shaders with textureArray
                authoring.Unlit.SetTexture("_TextureArray", authoring.Array);
                authoring.Lit.SetTexture("_TextureArray", authoring.Array);

                // TextureArray ICD
                AddComponent(entity, new TextureArray
                {
                    TextureType = authoring.TextureType,
                    Format = textureFormat
                });

                // Load Textures
                AddBuffer<TextureArrayElement>(entity);
                authoring.Textures.Clear();
                var textureAuthoringComponents = FindObjectsOfType<TextureAuthoring>();
                var index = 0;
                foreach (var textureAuthoringComponent in textureAuthoringComponents)
                {
                    if (textureAuthoringComponent.Texture.Type != authoring.TextureType)
                        continue;

                    // Update authoring
                    textureAuthoringComponent.Index = index; // Set index in TextureAuthoring
                    textureAuthoringComponent.Texture.Index = index; // Set index in Texture Asset
                    authoring.Textures.Add(textureAuthoringComponent.Texture);

                    // Copy texture into array
                    // Debug.Log($"Copying texture {textureAuthoringComponent.Texture.Texture2D.name} into array {textureFormat} at index {index}");
                    Graphics.CopyTexture(textureAuthoringComponent.Texture.Texture2D, 0, 0, authoring.Array, index, 0);

                    // TextureArrayElement
                    var textureArrayElement = new TextureArrayElement
                    {
                        Texture = GetEntity(textureAuthoringComponent.gameObject, TransformUsageFlags.None)
                    };
                    AppendToBuffer(entity, textureArrayElement);
                    index++;
                }

            }
        }
    }
}


// https://docs.unity3d.com/Manual/class-TextureImporterOverride.html  <== Recommended TextureTypes for platforms