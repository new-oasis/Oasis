using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static Oasis.Inputs;


namespace Oasis
{

    [CreateAssetMenu(fileName = "InputReader", menuName = "Oasis/Input Reader")]
    public class InputReader : ScriptableObject, IBobActions
    {

        public event UnityAction<Vector2> Move = delegate { };
        public event UnityAction<Vector2, bool> Look = delegate { };

        public event UnityAction EnableMouseControlsCamera = delegate { };
        public event UnityAction DisableMouseControlsCamera = delegate { };

        Inputs inputs;

        public bool IsMouse;
        public Vector3 Direction => inputs.Bob.Move.ReadValue<Vector2>();


        void OnEnable()
        {
            Debug.Log("ENABLING INPUT READER");
            // Instantiate the input system and set callbacks
            if (inputs == null)
            {
                // inputs = new Inputs();
                // inputs.Bob.SetCallbacks(this);
                // inputs.God.SetCallbacks(this);
            }

            // inputs.Bob.Enable();
        Debug.Log("enabled bob");
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            Move.Invoke(context.ReadValue<Vector2>());
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            Debug.Log("ass");
            // Look.Invoke(context.ReadValue<Vector2>(), IsMouse(context));
            Look.Invoke(context.ReadValue<Vector2>(), true);
        }
        // bool IsMouse(InputAction.CallbackContext context) => context.control.device == Mouse.current;


        public void OnMouseLook(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    EnableMouseControlsCamera.Invoke();
                    break;
                case InputActionPhase.Canceled:
                    DisableMouseControlsCamera.Invoke();
                    break;
            }
        }










        public void OnBlueprint(InputAction.CallbackContext context)
        {
        }

        public void OnGod(InputAction.CallbackContext context)
        {
        }

        public void OnJump(InputAction.CallbackContext context)
        {
        }

        public void OnPlace(InputAction.CallbackContext context)
        {
        }

        public void OnRemove(InputAction.CallbackContext context)
        {
        }

        public void OnRotateClockwise(InputAction.CallbackContext context)
        {
        }

        public void OnRotateCounterClockwise(InputAction.CallbackContext context)
        {
        }

        public void OnToolbar(InputAction.CallbackContext context)
        {
        }



        // God Actions
        public void OnFire(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnBob(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }
    }

}