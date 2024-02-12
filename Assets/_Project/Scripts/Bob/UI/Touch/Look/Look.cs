using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine;


namespace Oasis.Bob
{
    public class Look : MonoBehaviour
    {
        public float Sensitivity = 50f;
        private float touchStartTime;
        private bool isFirstMoveEvent = true; // First touch fires a move event.  This flag helps ignore
        private bool isTouching;
        private bool hasLongPress;
        private bool hasMoved;
        private Vector3 touchPosition;
        private VisualElement root;
        private PanelSettings panelSettings;
        private VisualElement touchIndicator;

        void OnEnable()
        {
            root = GetComponent<UIDocument>().rootVisualElement;
            panelSettings = GetComponent<UIDocument>().panelSettings;
            Debug.Log($"Screen size: {Screen.width} x {Screen.height}");
            Debug.Log($"Reference resolution: {panelSettings.referenceResolution}");

            touchIndicator = root.Q<VisualElement>("TouchIndicator");
            touchIndicator.pickingMode = PickingMode.Ignore;

            var lookArea = root.Q<VisualElement>("LookArea");
            lookArea.RegisterCallback<PointerUpEvent>((ev) => OnPointerUp(ev));
            lookArea.RegisterCallback<PointerDownEvent>((ev) => OnPointerDown(ev));
            lookArea.RegisterCallback<PointerMoveEvent>((ev) => OnPointerMove(ev));
        }

        void OnPointerDown(PointerDownEvent ev)
        {
            // Debug.Log("OnPointerDown");
            ShowTouchIndicator(ev.position);
            touchPosition = ev.position;
            touchStartTime = Time.time;
            hasLongPress = false;
            hasMoved = false;
        }

        void OnPointerUp(PointerUpEvent ev)
        {
            // Debug.Log("OnPointerUp");
            HideTouchIndicator();
            if (!hasLongPress && !hasMoved)
            {
                var screenTouchPosition = GetScreenTouchPosition(ev.position);
                var ray = Camera.main.ScreenPointToRay(screenTouchPosition);
                Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 200f);
                // BobActions.Instance.Place(screenTouchPosition);
            }
            isFirstMoveEvent = true;
        }

        void Update()
        {
            // Detect long press
            if (isTouching && !hasMoved && Time.time - touchStartTime > 1f) // 1 second threshold for long press
                hasLongPress = true;

            // Remove if long press
            if (hasLongPress && isTouching)
            {
                var screenTouchPosition = GetScreenTouchPosition(touchPosition);
                var ray = Camera.main.ScreenPointToRay(screenTouchPosition);
                RaycastHit hit;
                if (!Physics.Raycast(ray, out hit)) return;
                var removeXYZ = (hit.point - hit.normal * 0.05f).ToInt3();
                Mono.World.Instance.Remove(removeXYZ);
            }
        }

        void OnPointerMove(PointerMoveEvent ev)
        {
            // Debug.Log("OnPointerMove");
            ShowTouchIndicator(ev.position);

            // Ignore first move event on touching screen
            if (isFirstMoveEvent)
            {
                isFirstMoveEvent = false;
                return;
            }

            // Normalize look
            var deltaMax = 40f;
            var deltaX = Mathf.Clamp(ev.deltaPosition.x, -deltaMax, deltaMax) / deltaMax;
            var deltaY = Mathf.Clamp(ev.deltaPosition.y, -deltaMax, deltaMax) / deltaMax;
            Bob.Instance.Look(new Vector2(deltaX, -deltaY), Sensitivity);        
            hasMoved = true;
            touchPosition = ev.position; // Update touchPosition here
        }

        private void ShowTouchIndicator(Vector2 position)
        {
            isTouching = true;
            touchIndicator.style.left = position.x - touchIndicator.resolvedStyle.width / 2;
            touchIndicator.style.top = position.y - touchIndicator.resolvedStyle.height / 2;
            touchIndicator.visible = true;
        }

        private void HideTouchIndicator()
        {
            isTouching = false;
            touchIndicator.visible = false;
        }


        private Vector2 GetScreenTouchPosition(Vector2 position)
        {
            return new Vector2(
                (position.x / panelSettings.referenceResolution.x) * Screen.width,
                (1 - position.y / panelSettings.referenceResolution.y) * Screen.height
            );
        }

    }
}