using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static Scratch.Inputs;


namespace Scratch
{

    [CreateAssetMenu(fileName = "InputReader", menuName = "Oasis/Scratch/Input Reader")]
    public class InputReader : ScriptableObject, IPlayerActions
    {

        public event UnityAction<Vector2> Move = delegate { };
        public event UnityAction<Vector2> Look = delegate { };

        Scratch.Inputs inputs;

        public Vector2 Direction => inputs.Player.Move.ReadValue<Vector2>();
        public bool jump;

		private const float _threshold = 0.01f;

        void OnEnable()
        {
            if (inputs == null)
            {
                inputs = new Scratch.Inputs();
                inputs.Player.SetCallbacks(this);
                inputs.Player.Enable();
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            Move.Invoke(context.ReadValue<Vector2>());
        }

        public void OnLook(InputAction.CallbackContext context)
        {
			var value = context.ReadValue<Vector2>();
			if (value.sqrMagnitude >= _threshold)
                Look.Invoke(context.ReadValue<Vector2>());
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            jump = (context.phase == InputActionPhase.Started || context.phase == InputActionPhase.Performed);
        }

    }

}