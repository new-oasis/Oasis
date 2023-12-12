using System;
using Unity.Collections;

namespace Oasis.BlockStates
{
    [Serializable]
    public struct State
    {
        public FixedString64Bytes Key; // 61 byte string; 3 byte overhead
        public FixedString64Bytes Value;
    }
}