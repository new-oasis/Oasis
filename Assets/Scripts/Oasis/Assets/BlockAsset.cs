using Oasis.ECS.Common;
using UnityEngine;

namespace Oasis.Assets
{
    [CreateAssetMenu(fileName = "Block", menuName = "Oasis/Block", order = 0)]
    public class BlockAsset : ScriptableObject
    {
        public BlockType Type;
        public TextureType TextureType;
    }
}