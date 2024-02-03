using System;
using Unity.Entities;
using UnityEngine;

namespace Oasis.Data
{
    
    public struct ModelHitBox : ISharedComponentData, IEquatable<ModelHitBox>
    {
        public Mesh Value;

        public bool Equals(ModelHitBox other)
        {
            return Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            return obj is ModelHitBox other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (Value != null ? Value.GetHashCode() : 0);
        }
    }
    
}
