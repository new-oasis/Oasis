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

        public event UnityAction<Vector2> Move = delegate { };
        public event UnityAction<Vector2> Look = delegate { };

        public event UnityAction GodMode = delegate { };
        public event UnityAction BobMode = delegate { };

        Scratch.Inputs inputs;

        public Vector2 Direction => inputs.Bob.Move.ReadValue<Vector2>();
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
            Move.Invoke(context.ReadValue<Vector2>());
        }

        void IBobActions.OnLook(InputAction.CallbackContext context)
        {
			var value = context.ReadValue<Vector2>();
			if (value.sqrMagnitude >= _threshold)
                Look.Invoke(context.ReadValue<Vector2>());
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
            throw new System.NotImplementedException();
        }

        void IGodActions.OnBobMode(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            inputs.Bob.Enable();
            inputs.God.Disable();
            BobMode.Invoke();
        }

    }

}