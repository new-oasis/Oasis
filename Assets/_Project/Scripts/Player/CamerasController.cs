using UnityEngine;
using Cinemachine;

namespace Oasis
{
    public class CamerasController : MonoBehaviour
    {
        public Oasis.InputReader inputReader;
        public CinemachineVirtualCamera BobCamera;
        public CinemachineVirtualCamera GodCamera;

        private void Awake()
        {
        }

        private void OnEnable()
        {
            inputReader.GodMode += OnGodMode;
            inputReader.BobMode += OnBobMode;
        }

        private void OnDisable()
        {
            inputReader.GodMode -= OnGodMode;
            inputReader.BobMode -= OnBobMode;
        }

        void OnBobMode()
        {
            GodCamera.Priority -= 1;
            BobCamera.Priority += 1;
        }

        void OnGodMode()
        {
            GodCamera.Priority += 1;
            BobCamera.Priority -= 1;
        }

    }
}