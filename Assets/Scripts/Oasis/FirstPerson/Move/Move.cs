using Oasis.FirstPerson;
using UnityEngine;
using UnityEngine.UIElements;

namespace Oasis.FirstPerson
{
    public class Move : MonoBehaviour
    {
        // References to UI elements
        public VisualElement touchArea;
        public VisualElement knob;

        private Vector2 initialTouchPosition;
        private Vector2 knobOffset;
        private Vector2 lastKnobOffset;
        
        public float smoothingFactor = 10f;
        private Vector2 targetKnobOffset;


        private void Start()
        {
            // Register touch events
            var root = GetComponent<UIDocument>().rootVisualElement;
            touchArea = root.Q<VisualElement>("TouchArea");
            knob = root.Q<VisualElement>("Knob");
            Debug.Log(this.touchArea);
            touchArea.RegisterCallback<PointerDownEvent>(OnPointerDown);
            touchArea.RegisterCallback<PointerMoveEvent>(OnPointerMove);
            touchArea.RegisterCallback<PointerUpEvent>(OnPointerUp);
        }

        void Update()
        {
            knobOffset = Vector2.Lerp(knobOffset, targetKnobOffset, Time.deltaTime * smoothingFactor);
            ConstrainKnobPosition();
            
            knob.style.left = new StyleLength(knobOffset.x);
            knob.style.top = new StyleLength(knobOffset.y);
            
            var normalizationFactor = touchArea.layout.width / 2;
            if (knobOffset != lastKnobOffset)
                Character.Instance.Move(new Vector2(knobOffset.x, -knobOffset.y)/normalizationFactor);

            
            lastKnobOffset = knobOffset;
        }

        private void OnPointerDown(PointerDownEvent e)
        {
            touchArea.CapturePointer(e.pointerId);
            initialTouchPosition = e.position;
        }

        private void OnPointerMove(PointerMoveEvent e)
        {
            targetKnobOffset = new Vector2(e.position.x - initialTouchPosition.x, e.position.y - initialTouchPosition.y);
            ConstrainKnobPosition();
        }

        private void OnPointerUp(PointerUpEvent e)
        {
            touchArea.ReleasePointer(e.pointerId);
            targetKnobOffset = Vector2.zero;
        }

        private void ConstrainKnobPosition()
        {
            // Calculate maximum allowed offset based on knob size and touch area padding
            var maxOffset = (touchArea.layout.width) / 2; // Adjust padding as needed

            // Clamp offset to limits
            knobOffset.x = Mathf.Clamp(knobOffset.x, -maxOffset, maxOffset);
            knobOffset.y = Mathf.Clamp(knobOffset.y, -maxOffset, maxOffset);
        }
        
    }
}