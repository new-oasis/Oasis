using Cinemachine;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;


namespace Oasis
{
    public class PlayerController : MonoBehaviour
    {

        [Header("References")]
        [SerializeField] CharacterController controller;
        [SerializeField] Animator animator;
        [SerializeField] CinemachineFreeLook freeLookCamera;
        [SerializeField] InputReader input;

        [Header("Settings")]
        [SerializeField] float moveSpeed = 6f;
        [SerializeField] float rotationSpeed = 15f;
        [SerializeField] float smoothTime = 0.2f;

        Transform mainCamera;
        float velocity;
        float currentSpeed;

        void Awake()
        {
            mainCamera = Camera.main.transform;
            freeLookCamera.Follow = transform;
            freeLookCamera.LookAt = transform;
            freeLookCamera.OnTargetObjectWarped(transform, transform.position - freeLookCamera.transform.position - Vector3.forward);
        }


        void Update()
        {
            HandleMovement();
            // UpdateAnimator();
        }

        void HandleMovement()
        {
            var movementDirection = new Vector3(input.Direction.x, 0f, input.Direction.y).normalized;

            // Adjust direction for input and camera rotation
            var adjustedDirection = Quaternion.AngleAxis(mainCamera.eulerAngles.y, Vector3.up) * movementDirection;

            if (adjustedDirection.magnitude > 0.1f)
            {
                HandleRotation(adjustedDirection);
                HandleCharacterController(adjustedDirection);
                SmoothSpeed(adjustedDirection.magnitude);
            }
            else
            {
                SmoothSpeed(0f);
            }

        }

        void HandleCharacterController(Vector3 adjustedDirection)
        {
            var adjustedMovement = adjustedDirection * (moveSpeed * Time.deltaTime);
            controller.Move(adjustedMovement);
            SmoothSpeed(adjustedMovement.magnitude);
        }

        void SmoothSpeed(float value)
        {
            currentSpeed = Mathf.SmoothDamp(currentSpeed, value, ref velocity, smoothTime);
        }

        private void HandleRotation(Vector3 adjustedDirection)
        {
            var targetRotation = Quaternion.LookRotation(adjustedDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            // transform.LookAt(transform.position + adjustedDirection);
        }
    }

}
// https://www.youtube.com/watch?v=--_CH5DYz0M&t=794s