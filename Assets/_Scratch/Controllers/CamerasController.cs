using UnityEngine;
using Cinemachine;


public class CamerasController : MonoBehaviour
{
    public Scratch.InputReader inputReader;
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