using UnityEngine;
using Unity.Mathematics;
using UnityEngine.Events;

namespace Oasis
{

    [CreateAssetMenu(fileName = "Target", menuName = "Oasis/Target")]
    public class Target : ScriptableObject
    {
        public int3 Value;
        public int3 Adjacent;
        public event UnityAction<int3> Changed = delegate { };


        public void InvokeChangedEvent()
        {
            Changed.Invoke(Value);
        }

    }
}