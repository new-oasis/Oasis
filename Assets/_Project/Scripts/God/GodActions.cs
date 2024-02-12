// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.InputSystem;

// public class GodActions : MonoBehaviour
// {
//     public PlayerInput playerInput;
//     public Transform bobCameraContainer;
//     public Vector3 direction;
//     private Inputs inputs;
//     private CharacterController controller;
//     public float mouseSensitivity = 25f;
//     private float xRotation = 0f;
//     private float yRotation = 0f;
//     public float speed;
    
    
//     private void Awake()
//     {
//         controller = GetComponent<CharacterController>();
//         inputs = new Inputs();
//         inputs.Enable();
//     }
    
//     private void Update()
//     {
//         ApplyMovement();
//         inputs.God.Look.performed += context => {
//             var mouseX = context.ReadValue<Vector2>().x * mouseSensitivity * Time.deltaTime;
//             var mouseY = context.ReadValue<Vector2>().y * mouseSensitivity * Time.deltaTime;
        
//             xRotation -= mouseY;
//             xRotation = Mathf.Clamp(xRotation, 10f, 60f);
//             yRotation += mouseX;
            
//             transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0.0f); // xRotation
//             // transform.rotation = Quaternion.Euler(0.0f, yRotation, 0.0f); // yRotation
//         };
//     }

//     public void Move(InputAction.CallbackContext context)
//     {
//         var input = context.ReadValue<Vector2>();
//         direction = new Vector3(input.x, 0.0f, input.y);
//         direction = Quaternion.Euler(0.0f, yRotation, 0.0f) * direction;
//     }
    
//     private void ApplyMovement()
//     {
//         controller.Move(direction * speed * Time.deltaTime);
//     }

//     public void Bob(InputAction.CallbackContext context)
//     {
//         if (context.performed)
//         {
//             Debug.Log("Switching to Bob");
//             playerInput.SwitchCurrentActionMap("Bob");
//             Camera.main.transform.parent = bobCameraContainer;
//             Camera.main.transform.localPosition = Vector3.zero;
//             Camera.main.transform.localRotation = Quaternion.identity;
//         }
//     }    
    
//     void OnEnable()
//     {
//         Cursor.lockState = CursorLockMode.Locked;
//     }

//     void OnDisable()
//     {
//         Cursor.lockState = CursorLockMode.None;
//     }

// }