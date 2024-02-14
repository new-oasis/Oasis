using Cinemachine;
using Oasis.Bob;
using UnityEngine;

namespace Scratch
{
	public class GodController : MonoBehaviour
	{
		public Scratch.InputReader	inputReader;

		[Header("God")]
		public float MoveSpeed = 4.0f;
		public float RotationSpeed = 1.0f;
		public float SpeedChangeRate = 10.0f;

		[Header("Cinemachine")]
		public GameObject CinemachineCameraTarget;
		[Tooltip("How far in degrees can you move the camera up")]
		public float TopClamp = 90.0f;
		[Tooltip("How far in degrees can you move the camera down")]
		public float BottomClamp = -90.0f;

		private float pitch;
		private float rotation;



		private void Start()
		{
			inputReader.GodLook += OnLook;
		}

        void Update()
        {
            Move();
        }

		private void OnLook(Vector2 _input)
		{
			var isMouse = true; // TODO
			float deltaTimeMultiplier = isMouse ? 1.0f : Time.deltaTime;
			
			pitch -= _input.y * RotationSpeed * deltaTimeMultiplier;
			pitch = ClampAngle(pitch, BottomClamp, TopClamp);
			rotation += _input.x * RotationSpeed * deltaTimeMultiplier;

			CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(pitch, rotation, 0.0f);
        }

		private void Move()
		{
			Vector3 movement = new Vector3(inputReader.GodDirection.x, 0, inputReader.GodDirection.y);
			movement = Quaternion.Euler(0, rotation, 0) * movement;
			transform.position += movement * MoveSpeed * Time.deltaTime;
		}


        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
		{
			if (lfAngle < -360f) lfAngle += 360f;
			if (lfAngle > 360f) lfAngle -= 360f;
			return Mathf.Clamp(lfAngle, lfMin, lfMax);
		}

	}
}