using System;
using System.Collections.Generic;
using Oasis.Common;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Oasis.BlockStates
{
    [CreateAssetMenu(fileName = "Block", menuName = "Oasis/Block", order = 0)]
    public class BlockAsset : ScriptableObject
    {
        public BlockType Type;
        public TextureType TextureType;
    }
}