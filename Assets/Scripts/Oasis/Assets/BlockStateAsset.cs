using System.Collections.Generic;
using Oasis.ECS.BlockStates;
using UnityEngine;

namespace Oasis.Assets
{
    [CreateAssetMenu(fileName = "BlockState", menuName = "Oasis/BlockState", order = 1)]
    public class BlockStateAsset : ScriptableObject
    {
        
        public BlockAsset Block;
        public List<State> States;   // 4094 bytes; 2 byte overhead

    }
}