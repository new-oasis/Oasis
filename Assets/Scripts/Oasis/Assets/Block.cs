using System;
using System.Collections.Generic;
using Oasis.Common;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Oasis.Assets
{
    [CreateAssetMenu(fileName = "Block", menuName = "Oasis/Block", order = 1)]
    public class Block : ScriptableObject
    {
        public BlockType BlockType;
        public TextureType TextureType;
        public List<BlockState> BlockStates = new();
    }
    
    [Serializable]
    public struct BlockState
    {
        [HorizontalGroup("Split"), HideLabel]
        public List<State> States;

        [HorizontalGroup("Split")]
        [VerticalGroup("Split/Right")]
        public Model Model;
        [VerticalGroup("Split/Right"), HideLabel]
        public BlockStateRotation Rotation;
    }

    [Serializable]
    public struct State
    {
        [HorizontalGroup("Split"), HideLabel]
        public string Key;

        [HorizontalGroup("Split"), HideLabel]
        public string Value;
    }
    
    [Serializable]
    public struct BlockStateRotation
    {
        public byte Axis;  // 0 == x, 1 == y, 2 == z
        public float Angle;
    }
}