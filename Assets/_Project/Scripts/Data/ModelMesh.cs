using System;
using Unity.Entities;
using UnityEngine;

namespace Oasis.Data
{
    
    public struct ModelMesh : ISharedComponentData, IEquatable<ModelMesh>
    {
        public Mesh Value;

        public bool Equals(ModelMesh other)
        {
            return Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            return obj is ModelMesh other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (Value != null ? Value.GetHashCode() : 0);
        }
    }
    
}
