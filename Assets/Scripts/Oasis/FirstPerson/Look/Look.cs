using Oasis.Game.Player.FirstPerson.Character;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Oasis.FirstPerson.Look
{
    public class Look : MonoBehaviour
    {
        public float Sensitivity = 50f;
        
        
        void OnEnable()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            var lookArea = root.Q<VisualElement>("LookArea");
            lookArea.RegisterCallback<PointerMoveEvent>((ev) => OnPointerMove(ev));
        }
        
        void OnPointerMove(PointerMoveEvent ev)
        {
            // normalize look 
            var deltaMax = 40f;
            var deltaX = Mathf.Clamp(ev.deltaPosition.x, -deltaMax, deltaMax) / deltaMax;
            var deltaY = Mathf.Clamp(ev.deltaPosition.y, -deltaMax, deltaMax) / deltaMax;

            Debug.Log("deltaX: " + deltaX + ", deltaY: " + deltaY);

            Character.Instance.Look(new Vector2(deltaX, -deltaY), Sensitivity);
        }

    }
    
}
