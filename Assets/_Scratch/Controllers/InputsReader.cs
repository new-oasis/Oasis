using Unity.Entities.UniversalDelegates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static Scratch.Inputs;


namespace Scratch
{

    [CreateAssetMenu(fileName = "InputReader", menuName = "Oasis/Scratch/Input Reader")]
    public class InputReader : ScriptableObject, IBobActions, IGodActions
    {

        public event UnityAction<Vector2> BobMove = delegate { };
        public event UnityAction<Vector2> BobLook = delegate { };

        public event UnityAction<Vector2> GodMove = delegate { };
        public event UnityAction<Vector2> GodLook = delegate { };

        public event UnityAction GodMode = delegate { };
        public event UnityAction BobMode = delegate { };

        Scratch.Inputs inputs;

        public Vector2 BobDirection => inputs.Bob.Move.ReadValue<Vector2>();
        public Vector2 GodDirection => inputs.God.Move.ReadValue<Vector2>();
        public bool jump;

		private const float _threshold = 0.01f;

        void OnEnable()
        {
            if (inputs == null)
            {
                inputs = new Scratch.Inputs();
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