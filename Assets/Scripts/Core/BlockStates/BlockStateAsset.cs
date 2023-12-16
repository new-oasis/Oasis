using System;
using System.Collections.Generic;
using Oasis.Common;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Oasis.BlockStates
{
    [CreateAssetMenu(fileName = "BlockState", menuName = "Oasis/BlockState", order = 1)]
    public class BlockStateAsset : ScriptableObject
    {
        
        public BlockAsset Block;
        public List<State> States;   // 4094 bytes; 2 byte overhead

    }
}