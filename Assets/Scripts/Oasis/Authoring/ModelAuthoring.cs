using System;
using System.Collections.Generic;
using System.Linq;
using Oasis.Common;
using Oasis.Data;
using Oasis.Mesher;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using static Unity.Mathematics.math;
using Texture = Oasis.Data.Texture;
using World = Unity.Entities.World;

namespace Oasis.Authoring
{
    public class ModelAuthoring : MonoBehaviour
    {

        public Assets.Model Model;

        public class ModelBaker : Baker<ModelAuthoring>
        {
            public override void Bake(ModelAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                CreateComponentData(authoring, entity);

                // Mesh TODO Lazy load this
                var mesh = ComputeModelMesh(authoring);
                AddSharedComponentManaged(entity, new ModelMesh {Value = mesh});
            }

            private void CreateComponentData(ModelAuthoring authoring, Entity entity)
            {
                AddComponent(entity, new Model
                {
                    Name = authoring.gameObject.name,
                    Type = authoring.Model.Type,
                    TextureType = authoring.Model.TextureType
                });

                // ModelElements
                var modelElementEntities = AddBuffer<ModelElementReference>(entity);
                for (var i = 0; i < authoring.Model.ModelElements.Count; i++)
                {
                    var authoringModelElement = authoring.Model.ModelElements[i];
                    var modelElementEntity = CreateAdditionalEntity(TransformUsageFlags.None, false, $"{authoring.name}/{i}");
                    AppendToBuffer(entity, new ModelElementReference {Value = modelElementEntity});
                    AddComponent(modelElementEntity, new ModelElement
                    {
                        From = authoringModelElement.From,
                        To = authoringModelElement.To,
                        NoShadows = authoringModelElement.NoShadows,
                        Rotation = new ModelElementRotation
                        {
                            Angle = authoringModelElement.Rotation.Angle,
                            Axis = authoringModelElement.Rotation.Axis,
                            Origin = authoringModelElement.Rotation.Origin
                        }
                    });
                    
                    // Add ModelFaces to ModelElement
                    AddBuffer<ModelFace>(modelElementEntity);
                    foreach (var modelFace in authoringModelElement.ModelFaces)
                    {
                        var textureAuthoringComponent = GetTextureAuthoringComponent(modelFace);
                        AppendToBuffer(modelElementEntity, new ModelFace
                        {
                            Side = modelFace.Side,
                            UV = modelFace.UV,
                            Texture = GetEntity(textureAuthoringComponent, TransformUsageFlags.None),
                        });
                    }
                }
            }

            private static TextureAuthoring GetTextureAuthoringComponent(Assets.ModelFace modelFace)
            {
                var textureAuthoringComponents = FindObjectsOfType<TextureAuthoring>();
                var textureAuthoringComponent = textureAuthoringComponents
                    .FirstOrDefault(component => component.Texture == modelFace.Texture);
                if (textureAuthoringComponent == null)
                {
                    Debug.LogError("No texture authoring component found for model face texture ");
                    return textureAuthoringComponent;
                }

                return textureAuthoringComponent;
            }

            private Mesh ComputeModelMesh(ModelAuthoring authoring)
            {
                var numFaces = authoring.Model.ModelElements.Sum(element => element.ModelFaces.Count);
                Mesh.MeshDataArray meshDataArray;
                meshDataArray = Mesh.AllocateWritableMeshData(1);
                var meshData = meshDataArray[0];
                meshData.SetVertexBufferParams(numFaces * 4,
                    new VertexAttributeDescriptor(VertexAttribute.Position, dimension: 3, stream: 0),
                    new VertexAttributeDescriptor(VertexAttribute.Normal, dimension: 3, stream: 0),
                    new VertexAttributeDescriptor(VertexAttribute.Tangent, VertexAttributeFormat.Float16, 4),
                    new VertexAttributeDescriptor(VertexAttribute.TexCoord0, VertexAttributeFormat.Float16, 2, 1),
                    new VertexAttributeDescriptor(VertexAttribute.TexCoord1, VertexAttributeFormat.UNorm8, 4, 2),
                    new VertexAttributeDescriptor(VertexAttribute.Color, VertexAttributeFormat.UNorm8, 4, 3));
                meshData.SetIndexBufferParams(numFaces * 6, IndexFormat.UInt16);

                var verts = meshData.GetVertexData<VertexStream0>();
                var triangles = meshData.GetIndexData<ushort>();
                var texCoord0 = meshData.GetVertexData<half2>(1);
                var texCoord1 = meshData.GetVertexData<float>(2);
                var colors = meshData.GetVertexData<Color32>(3);
                var opaqueTriangles = new NativeList<ushort>(Allocator.Temp);
                var transparentTriangles = new NativeList<ushort>(Allocator.Temp);
                var alphaClipTriangles = new NativeList<ushort>(Allocator.Temp);

                NativeList<ushort> tris;
                if (authoring.Model.TextureType == TextureType.Cutout)
                    tris = alphaClipTriangles;
                else if (authoring.Model.TextureType == TextureType.Transparent)
                    tris = transparentTriangles;
                else
                    tris = opaqueTriangles;

                var f = 0;
                var v1 = new float3();
                var v2 = new float3();
                var v3 = new float3();
                var v4 = new float3();
                foreach (var element in authoring.Model.ModelElements)
                foreach (var face in element.ModelFaces)
                {
                    switch (face.Side)
                    {
                        case Side.East:
                            v1 = new float3(element.To.x / 16f, element.From.y / 16f, element.From.z / 16f);
                            v2 = new float3(element.To.x / 16f, element.To.y / 16f, element.From.z / 16f);
                            v3 = new float3(element.To.x / 16f, element.To.y / 16f, element.To.z / 16f);
                            v4 = new float3(element.To.x / 16f, element.From.y / 16f, element.To.z / 16f);
                            break;
                        case Side.Up:
                            v1 = new float3(element.From.x / 16f, element.To.y / 16f, element.From.z / 16f);
                            v2 = new float3(element.From.x / 16f, element.To.y / 16f, element.To.z / 16f);
                            v3 = new float3(element.To.x / 16f, element.To.y / 16f, element.To.z / 16f);
                            v4 = new float3(element.To.x / 16f, element.To.y / 16f, element.From.z / 16f);
                            break;
                        case Side.North:
                            v1 = new float3(element.To.x / 16f, element.From.y / 16f, element.To.z / 16f);
                            v2 = new float3(element.To.x / 16f, element.To.y / 16f, element.To.z / 16f);
                            v3 = new float3(element.From.x / 16f, element.To.y / 16f, element.To.z / 16f);
                            v4 = new float3(element.From.x / 16f, element.From.y / 16f, element.To.z / 16f);
                            break;
                        case Side.West:
                            v1 = new float3(element.From.x / 16f, element.From.y / 16f, element.To.z / 16f);
                            v2 = new float3(element.From.x / 16f, element.To.y / 16f, element.To.z / 16f);
                            v3 = new float3(element.From.x / 16f, element.To.y / 16f, element.From.z / 16f);
                            v4 = new float3(element.From.x / 16f, element.From.y / 16f, element.From.z / 16f);
                            break;
                        case Side.Down:
                            v1 = new float3(element.From.x / 16f, element.From.y / 16f, element.From.z / 16f);
                            v2 = new float3(element.To.x / 16f, element.From.y / 16f, element.From.z / 16f);
                            v3 = new float3(element.To.x / 16f, element.From.y / 16f, element.To.z / 16f);
                            v4 = new float3(element.From.x / 16f, element.From.y / 16f, element.To.z / 16f);
                            break;
                        case Side.South:
                            v1 = new float3(element.From.x / 16f, element.From.y / 16f, element.From.z / 16f);
                            v2 = new float3(element.From.x / 16f, element.To.y / 16f, element.From.z / 16f);
                            v3 = new float3(element.To.x / 16f, element.To.y / 16f, element.From.z / 16f);
                            v4 = new float3(element.To.x / 16f, element.From.y / 16f, element.From.z / 16f);
                            break;
                    }

                    // Update vertices if element has rotation
                    if (!element.Rotation.Equals(default(ModelElementRotation)))
                    {
                        float3x3 rotationMatrix;
                        var angle = element.Rotation.Angle * Mathf.Deg2Rad;
                        switch (element.Rotation.Axis)
                        {
                            case 0:
                                rotationMatrix = math.float3x3(1f, 0f, 0f, 0f, math.cos(angle), math.sin(angle), 0f, -math.sin(angle), math.cos(angle));
                                break;
                            case 1:
                                rotationMatrix = math.float3x3(math.cos(angle), 0f, -math.sin(angle), 0f, 1f, 0f, math.sin(angle), 0f, math.cos(angle));
                                break;
                            case 2:
                                rotationMatrix = math.float3x3(math.cos(angle), math.sin(angle), 0f, -math.sin(angle), math.cos(angle), 0f, 0f, 0f, 1f);
                                break;
                            default:
                                rotationMatrix = math.float3x3(math.cos(angle), 0f, -math.sin(angle), 0f, 1f, 0f, math.sin(angle), 0f, math.cos(angle));
                                break;
                        }

                        v1 -= element.Rotation.Origin;
                        v1 = math.mul(rotationMatrix, v1);
                        v1 += element.Rotation.Origin;
                        
                        v2 -= element.Rotation.Origin;
                        v2 = math.mul(rotationMatrix, v2);
                        v2 += element.Rotation.Origin;
                        
                        v3 -= element.Rotation.Origin;
                        v3 = math.mul(rotationMatrix, v3);
                        v3 += element.Rotation.Origin;
                        
                        v4 -= element.Rotation.Origin;
                        v4 = math.mul(rotationMatrix, v4);
                        v4 += element.Rotation.Origin;
                        
                    }
                        
                    verts[f * 4 + 0] = new VertexStream0 {Position = v1};
                    verts[f * 4 + 1] = new VertexStream0 {Position = v2};
                    verts[f * 4 + 2] = new VertexStream0 {Position = v3};
                    verts[f * 4 + 3] = new VertexStream0 {Position = v4};

                    
                    AddTris(tris, f);
                    AddTexCoord0(texCoord0, f, face.UV);
                    AddColors(f, (byte) face.Texture.Index, ref colors); // Can we get this value at bake time.?
                    f++;
                }


                // Concat triangles
                for (var i = 0; i < opaqueTriangles.Length; i++)
                    triangles[i] = opaqueTriangles[i];
                for (var i = 0; i < alphaClipTriangles.Length; i++)
                    triangles[opaqueTriangles.Length + i] = alphaClipTriangles[i];
                for (var i = 0; i < transparentTriangles.Length; i++)
                    triangles[opaqueTriangles.Length + alphaClipTriangles.Length + i] = transparentTriangles[i];

                meshData.subMeshCount = 3;
                meshData.SetSubMesh(0, new SubMeshDescriptor(0, opaqueTriangles.Length));
                meshData.SetSubMesh(1, new SubMeshDescriptor(opaqueTriangles.Length, alphaClipTriangles.Length));
                meshData.SetSubMesh(2, new SubMeshDescriptor(opaqueTriangles.Length + alphaClipTriangles.Length, transparentTriangles.Length));

                var mesh = new Mesh();
                Mesh.ApplyAndDisposeWritableMeshData(meshDataArray, mesh);
                mesh.RecalculateNormals();
                mesh.RecalculateBounds();
                return mesh;
            }


            // “My suggestion is not to pack the floats at all, *just use 12 floats spread across different UV sets*.” - bgolus
            private void AddColors(int i, byte textureIndex, ref NativeArray<Color32> colors)
            {
                const byte metallic = 0;
                const byte smoothness = 0;
                colors[i * 4 + 0] = new Color32(0, metallic, smoothness, textureIndex);
                colors[i * 4 + 1] = new Color32(0, metallic, smoothness, textureIndex);
                colors[i * 4 + 2] = new Color32(0, metallic, smoothness, textureIndex);
                colors[i * 4 + 3] = new Color32(0, metallic, smoothness, textureIndex);
            }

            // MC UVs start in top left, so y = 16-y
            private static void AddTexCoord0(NativeArray<half2> texCoord0, int i, int4 ii)
            {
                texCoord0[i * 4 + 0] = half2((half) (ii.x / 16f), (half) (ii.y / 16f));
                texCoord0[i * 4 + 1] = half2((half) (ii.x / 16f), (half) (ii.w / 16f));
                texCoord0[i * 4 + 2] = half2((half) (ii.z / 16f), (half) (ii.w / 16f));
                texCoord0[i * 4 + 3] = half2((half) (ii.z / 16f), (half) (ii.y / 16f));
            }

            private static void AddTris(NativeList<ushort> tris, int i)
            {
                tris.Add((ushort) (i * 4 + 0));
                tris.Add((ushort) (i * 4 + 1));
                tris.Add((ushort) (i * 4 + 2));
                tris.Add((ushort) (i * 4 + 2));
                tris.Add((ushort) (i * 4 + 3));
                tris.Add((ushort) (i * 4 + 0));
            }
        }

       
    }
}