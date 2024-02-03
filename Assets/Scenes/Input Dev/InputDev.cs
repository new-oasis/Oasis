using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputDev : MonoBehaviour
{

    void Start()
    {
        Debug.Log("InputDev#Start");
    }


    void OnAction(InputValue value)
    {
        Debug.Log("InputDev#OnJump");
    }
  

}
