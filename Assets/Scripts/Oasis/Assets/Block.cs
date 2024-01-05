using System;
using System.Collections.Generic;
using Oasis.Common;
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
        public List<State> States;
        public Model Model;
        public BlockStateRotation Rotation;
    }

    [Serializable]
    public struct State
    {
        public string Key;
        public string Value;
    }
    
    [Serializable]
    public struct BlockStateRotation
    {
        public byte Axis;  // 0 == x, 1 == y, 2 == z
        public float Angle;
    }
}