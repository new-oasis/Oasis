using Unity.Entities.UniversalDelegates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using static Oasis.Inputs;


namespace Oasis
{

    [CreateAssetMenu(fileName = "InputReader", menuName = "Oasis/Input Reader")]
    public class InputReader : ScriptableObject, IBobActions, IGodActions
    {
        Oasis.Inputs inputs;

        public event UnityAction<Vector2> BobMove = delegate { };
        public event UnityAction<Vector2> BobLook = delegate { };
        public event UnityAction BobMode = delegate { };
        public event UnityAction<Vector2> Place = delegate { };
        public event UnityAction<Vector2> RemoveStart = delegate { };
        public event UnityAction RemoveEnd = delegate { };
        public event UnityAction<int> ToolbarSelect = delegate { };
        public Vector2 BobDirection => inputs.Bob.Move.ReadValue<Vector2>();

        public event UnityAction<Vector2> GodMove = delegate { };
        public event UnityAction<Vector2> GodLook = delegate { };
        public event UnityAction GodMode = delegate { };
        public Vector2 GodDirection => inputs.God.Move.ReadValue<Vector2>();

        public bool jump;

		private const float _threshold = 0.01f;

        void OnEnable()
        {
            if (inputs == null)
            {
                inputs = new Oasis.Inputs();
                inputs.Bob.SetCallbacks(this);
                inputs.God.SetCallbacks(this);

                inputs.Bob.Enable();
                inputs.God.Disable();
            }
        }

        // --- Bob Actions ---
        void IBobActions.OnMove(InputAction.CallbackContext context)
        {
            BobMove.Invoke(context.ReadValue<Vector2>());
        }

        void IBobActions.OnLook(InputAction.CallbackContext context)
        {
			var value = context.ReadValue<Vector2>();
			if (value.sqrMagnitude >= _threshold)
                BobLook.Invoke(context.ReadValue<Vector2>());
        }

        void IBobActions.OnJump(InputAction.CallbackContext context)
        {
            jump = (context.phase == InputActionPhase.Started || context.phase == InputActionPhase.Performed);
        }

        void IBobActions.OnGodMode(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            inputs.Bob.Disable();
            inputs.God.Enable();
            GodMode.Invoke();
        }

        void IBobActions.OnToolbar(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            if (!context.started) return;
            if (((KeyControl) context.control).keyCode == Key.Digit1)
                ToolbarSelect.Invoke(0);
            else if (((KeyControl) context.control).keyCode == Key.Digit2)
                ToolbarSelect(1);
            else if (((KeyControl) context.control).keyCode == Key.Digit3)
                ToolbarSelect(2);
            else if (((KeyControl) context.control).keyCode == Key.Digit4)
                ToolbarSelect(3);
            else if (((KeyControl) context.control).keyCode == Key.Digit5)
                ToolbarSelect(4);
            else if (((KeyControl) context.control).keyCode == Key.Digit6)
                ToolbarSelect(5);
            else if (((KeyControl) context.control).keyCode == Key.Digit7)
                ToolbarSelect(6);
            else if (((KeyControl) context.control).keyCode == Key.Digit8)
                ToolbarSelect(7);
            else if (((KeyControl) context.control).keyCode == Key.Digit9)
                ToolbarSelect(8);
            else if (((KeyControl) context.control).keyCode == Key.Digit0)
                ToolbarSelect(9);
        }

        void IBobActions.OnPlace(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            if (!context.started) return;
            var screenPoint = new Vector2(Screen.width / 2, Screen.height / 2); // Assuming crosshair
            Place.Invoke(screenPoint);
        }

        void IBobActions.OnRemove(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            var screenPoint = new Vector2(Screen.width / 2, Screen.height / 2); // Assuming crosshair
            if (context.started)
                RemoveStart.Invoke(screenPoint);
            else if (context.canceled)
                RemoveEnd.Invoke();
        }


        // --- God Actions ---
        void IGodActions.OnMove(InputAction.CallbackContext context)
        {
            GodMove.Invoke(context.ReadValue<Vector2>());
        }

        void IGodActions.OnLook(InputAction.CallbackContext context)
        {
            GodLook.Invoke(context.ReadValue<Vector2>());
        }

        void IGodActions.OnBobMode(InputAction.CallbackContext context)
        {
            inputs.Bob.Enable();
            inputs.God.Disable();
            BobMode.Invoke();
        }
    }

}