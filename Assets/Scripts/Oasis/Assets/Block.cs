using System.Collections.Generic;
using Oasis.Common;
using Oasis.Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace Oasis.Assets
{
    [CreateAssetMenu(fileName = "Block", menuName = "Oasis/Block", order = 1)]
    public class Block : ScriptableObject
    {
        public BlockType BlockType;
        public TextureType TextureType;
        public List<Variant> Variants = new List<Variant>();
    }
    
    [System.Serializable]
    public struct Variant
    {
        public List<State> States;
        public Model Model;
    }
}