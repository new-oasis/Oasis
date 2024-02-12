using System;
using Oasis.Common;
using UnityEngine;

namespace Oasis.Assets
{
    [CreateAssetMenu(fileName = "Texture", menuName = "Oasis/Texture", order = 3)]
    public class Texture : ScriptableObject, IEquatable<Texture>
    {
        public TextureType Type;
        public Texture2D Texture2D;
        public int Index;

        // Asset name only
        public bool Equals(Texture other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && name == other.name;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Texture) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), name);
        }
    
    }
}
