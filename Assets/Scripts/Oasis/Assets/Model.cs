using System;
using System.Collections.Generic;
using Oasis.Common;
using Oasis.Data;
using Unity.Mathematics;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Oasis.Assets
{
    [CreateAssetMenu(fileName = "Model", menuName = "Oasis/Model", order = 2)]
    public class Model : SerializedScriptableObject
    {
        public BlockType Type;
        public TextureType TextureType;

        public List<ModelElement> ModelElements;

        public ushort Light;
        public float3 LightPosition;

        public bool BlocksMovement;
        public float3 NonSolidHitBoxFrom;  // TODO compute automagically
        public float3 NonSolidHitBoxTo;
    }

    [Serializable]
    public class ModelElement
    {
        public float3 From;
        public float3 To;
        [TableList] public List<ModelFace> ModelFaces;
        public bool NoShadows;
        public ModelElementRotation Rotation;
    }

    [Serializable]
    public class ModelFace
    {
        public Side Side;
        public Texture Texture;
        public int4 UV;
    }
}